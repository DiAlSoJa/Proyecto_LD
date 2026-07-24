using System.Globalization;
using LD.Application.Common.Interfaces.KittingFolioImport;
using LD.Contracts.DTOs.KittingFolioCapture;
using LD.Contracts.Requests;
using LD.Domain.Entities;
using LD.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Services;

public class KittingFolioImportService : IKittingFolioImportService
{
    private const string DefaultKittingStatus = "Creado";
    private readonly LdProyectDbContext _context;

    public KittingFolioImportService(LdProyectDbContext context)
    {
        _context = context;
    }

    public async Task<KittingFolioCapturePreviewDto> PreviewAsync(
        GenerateKittingFolioCaptureRequest request,
        CancellationToken cancellationToken = default)
    {
        return await BuildPreviewAsync(request, cancellationToken);
    }

    public async Task<(int KittingId, string KittingCode, int CaptureCount)> GenerateAsync(
        GenerateKittingFolioCaptureRequest request,
        CancellationToken cancellationToken = default)
    {
        var preview = await BuildPreviewAsync(request, cancellationToken);
        var validRows = preview.Rows.Where(x => x.IsValid).ToList();

        if (validRows.Count == 0)
            throw new Exception("No se encontraron registros validos para cargar.");

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var project = await LoadProjectAsync(request.ProjectId, cancellationToken);

            var currentNumber = 1;
            if (!string.IsNullOrWhiteSpace(project.KittingNumber)
                && int.TryParse(project.KittingNumber, out var parsedNumber)
                && parsedNumber > 0)
            {
                currentNumber = parsedNumber;
            }

            var kitting = new Kitting
            {
                ClientId = request.ClientId,
                ProjectId = request.ProjectId,
                GuideNumber = preview.GuideNumber,
                InvoiceNumber = preview.InvoiceNumber,
                Status = DefaultKittingStatus
            };

            kitting.KittingCode = $"{project.KittingPrefix}{currentNumber:D5}";
            kitting.PreKittingCode = kitting.KittingCode;

            _context.Kittings.Add(kitting);

            project.KittingNumber = (currentNumber + 1).ToString(CultureInfo.InvariantCulture);
            _context.Projects.Update(project);

            await _context.SaveChangesAsync(cancellationToken);

            var detailEntities = validRows.Select(row => new KittingDetail
            {
                KittingId = kitting.KittingId,
                PartNumber = row.PartNumber,
                Description = row.Description,
                Quantity = row.Quantity,
                CantidadSurtida = 0m,
                LotNumber = row.LotNumber,
                Status = DefaultKittingStatus
            }).ToList();

            _context.KittingDetails.AddRange(detailEntities);
            await _context.SaveChangesAsync(cancellationToken);

            var captureEntities = validRows.Select((row, index) => new KittingFolioCapture
            {
                KittingId = kitting.KittingId,
                KittingDetailId = detailEntities[index].KittingDetailId,
                SourceFileName = request.SourceFileName,
                SourceLineNumber = row.SourceLineNumber,
                GuideNumber = preview.GuideNumber,
                InvoiceNumber = preview.InvoiceNumber,
                PartNumber = row.PartNumber,
                Description = row.Description,
                Quantity = row.Quantity,
                LotNumber = row.LotNumber,
                SourceStatus = DefaultKittingStatus
            }).ToList();

            _context.KittingFolioCaptures.AddRange(captureEntities);
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return (kitting.KittingId, kitting.KittingCode ?? string.Empty, captureEntities.Count);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<KittingFolioCapturePreviewDto> BuildPreviewAsync(
        GenerateKittingFolioCaptureRequest request,
        CancellationToken cancellationToken)
    {
        var lines = ReadLines(request.FileContent);
        var guideNumber = NormalizeValue(RequireHeaderValue(lines, "Deliver To"));
        var invoiceNumber = NormalizeValue(RequireHeaderValue(lines, "Shipper Ref"));
        var detailHeaderIndex = FindDetailHeaderIndex(lines);

        if (detailHeaderIndex < 0)
            throw new Exception("No se encontro el encabezado de detalle del archivo.");

        var detailRows = ParseDetailRows(lines, detailHeaderIndex + 1);
        if (detailRows.Count == 0)
            throw new Exception("No se encontraron partidas en el archivo.");

        await LoadProjectAsync(request.ProjectId, cancellationToken);
        var validParts = await LoadValidPartNumbersAsync(request.ClientId, request.ProjectId, cancellationToken);
        var lotsByPart = await LoadAvailableLotsByPartAsync(request.ClientId, request.ProjectId, cancellationToken);

        var previewRows = detailRows
            .Select(row => ValidateRow(row, validParts, lotsByPart))
            .ToList();

        return new KittingFolioCapturePreviewDto
        {
            SourceFileName = request.SourceFileName,
            GuideNumber = guideNumber,
            InvoiceNumber = invoiceNumber,
            TotalRows = previewRows.Count,
            ValidRows = previewRows.Count(x => x.IsValid),
            InvalidRows = previewRows.Count(x => !x.IsValid),
            Rows = previewRows
        };
    }

    private async Task<HashSet<string>> LoadValidPartNumbersAsync(
        int clientId,
        int projectId,
        CancellationToken cancellationToken)
    {
        var parts = await _context.items
            .AsNoTracking()
            .Where(x => x.ClientId == clientId && x.ProjectId == projectId)
            .Where(x => x.PartNumber != null)
            .Select(x => x.PartNumber!)
            .ToListAsync(cancellationToken);

        return new HashSet<string>(
            parts
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(NormalizeValue),
            StringComparer.OrdinalIgnoreCase);
    }

    private async Task<Dictionary<string, HashSet<string>>> LoadAvailableLotsByPartAsync(
        int clientId,
        int projectId,
        CancellationToken cancellationToken)
    {
        var inventoryLots = await _context.AvailableInventories
            .AsNoTracking()
            .Where(x => x.ClientId == clientId && x.ProjectId == projectId)
            .Where(x => x.PartNumber != null)
            .Select(x => new
            {
                x.PartNumber,
                x.LotNumber
            })
            .ToListAsync(cancellationToken);

        return inventoryLots
            .Where(x => !string.IsNullOrWhiteSpace(x.PartNumber))
            .GroupBy(x => NormalizeValue(x.PartNumber), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => new HashSet<string>(
                    group.Select(x => NormalizeValue(x.LotNumber))
                        .Where(x => !string.IsNullOrWhiteSpace(x)),
                    StringComparer.OrdinalIgnoreCase),
                StringComparer.OrdinalIgnoreCase);
    }

    private async Task<Project> LoadProjectAsync(int projectId, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(x => x.ProjectId == projectId, cancellationToken);

        if (project is null)
            throw new Exception("No se encontro el proyecto.");

        if (string.IsNullOrWhiteSpace(project.KittingPrefix))
            throw new Exception("El proyecto no tiene configurado el prefijo de Kitting.");

        return project;
    }

    private static KittingFolioCapturePreviewRowDto ValidateRow(
        ParsedDetailRow row,
        HashSet<string> validParts,
        IReadOnlyDictionary<string, HashSet<string>> lotsByPart)
    {
        var partNumber = NormalizeValue(row.PartNumber);
        var lotNumber = NormalizeValue(row.LotNumber);
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(partNumber))
        {
            errors.Add("El numero de parte esta vacio.");
        }
        else if (!validParts.Contains(partNumber))
        {
            errors.Add("El numero de parte no existe para el cliente y proyecto seleccionados.");
        }

        if (string.IsNullOrWhiteSpace(lotNumber))
        {
            errors.Add("El lote es obligatorio.");
        }
        else if (string.IsNullOrWhiteSpace(partNumber) ||
                 !lotsByPart.TryGetValue(partNumber, out var lots) ||
                 !lots.Contains(lotNumber))
        {
            errors.Add("El lote no existe para este numero de parte.");
        }

        return new KittingFolioCapturePreviewRowDto
        {
            SourceLineNumber = row.SourceLineNumber,
            PartNumber = partNumber,
            Description = row.Description,
            Quantity = row.Quantity,
            LotNumber = lotNumber,
            IsValid = errors.Count == 0,
            ValidationMessage = string.Join(" ", errors)
        };
    }

    private static List<string> ReadLines(string content)
    {
        var result = new List<string>();
        using var reader = new StringReader(content ?? string.Empty);

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            result.Add(line);
        }

        return result;
    }

    private static int FindDetailHeaderIndex(IReadOnlyList<string> lines)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var fields = SplitFields(lines[i]);
            if (fields.Count == 0)
                continue;

            if (!string.Equals(fields[0], "Product", StringComparison.OrdinalIgnoreCase))
                continue;

            return i;
        }

        return -1;
    }

    private static List<ParsedDetailRow> ParseDetailRows(IReadOnlyList<string> lines, int startIndex)
    {
        var rows = new List<ParsedDetailRow>();

        for (var i = startIndex; i < lines.Count; i++)
        {
            var rawLine = lines[i];
            if (string.IsNullOrWhiteSpace(rawLine))
                continue;

            var fields = SplitFields(rawLine);
            if (fields.Count < 3)
                continue;

            var firstToken = fields[0];
            if (IsControlLine(firstToken))
                break;

            var partNumber = NormalizeValue(fields.ElementAtOrDefault(0));
            var description = NormalizeValue(fields.ElementAtOrDefault(1));
            var quantity = ParseDecimal(fields.ElementAtOrDefault(2));
            var lotNumber = NormalizeValue(fields.ElementAtOrDefault(11));

            if (string.IsNullOrWhiteSpace(partNumber))
                continue;

            rows.Add(new ParsedDetailRow(
                i + 1,
                partNumber,
                description,
                quantity,
                lotNumber));
        }

        return rows;
    }

    private static bool IsControlLine(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        return token.StartsWith("Total", StringComparison.OrdinalIgnoreCase)
            || token.StartsWith("Form-", StringComparison.OrdinalIgnoreCase)
            || token.StartsWith("Supplier", StringComparison.OrdinalIgnoreCase)
            || token.StartsWith("Consignor", StringComparison.OrdinalIgnoreCase)
            || token.StartsWith("Consignee", StringComparison.OrdinalIgnoreCase)
            || token.StartsWith("Deliver To", StringComparison.OrdinalIgnoreCase)
            || token.StartsWith("Shipper Ref", StringComparison.OrdinalIgnoreCase);
    }

    private static List<string> SplitFields(string line) =>
        line.Split('\t')
            .Select(field => field?.Trim() ?? string.Empty)
            .ToList();

    private static string RequireHeaderValue(IReadOnlyList<string> lines, string label)
    {
        var value = TryGetHeaderValue(lines, label);
        if (string.IsNullOrWhiteSpace(value))
            throw new Exception($"No se encontro el campo {label} en el archivo.");

        return value;
    }

    private static string? TryGetHeaderValue(IReadOnlyList<string> lines, string label)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var fields = SplitFields(lines[i]);
            if (fields.Count == 0)
                continue;

            for (var j = 0; j < fields.Count; j++)
            {
                if (!MatchesLabel(fields[j], label))
                    continue;

                for (var k = j + 1; k < fields.Count; k++)
                {
                    if (!string.IsNullOrWhiteSpace(fields[k]))
                        return fields[k];
                }
            }
        }

        return null;
    }

    private static bool MatchesLabel(string value, string label) =>
        string.Equals(value?.Trim().TrimEnd(':'), label, StringComparison.OrdinalIgnoreCase);

    private static decimal ParseDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0m;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
            return result;

        return 0m;
    }

    private static string NormalizeValue(string? value) =>
        string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();

    private sealed record ParsedDetailRow(
        int SourceLineNumber,
        string PartNumber,
        string Description,
        decimal Quantity,
        string LotNumber);
}
