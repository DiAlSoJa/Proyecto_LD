using Plugin.Maui.OCR;
using SkiaSharp;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MauiAppLogin.Features.Seguridad.Ocr;

public sealed record LicenseOcrData(string? Nombre, string? Licencia, DateTime? Vencimiento)
{
    public bool HasAny =>
        !string.IsNullOrWhiteSpace(Nombre) ||
        !string.IsNullOrWhiteSpace(Licencia) ||
        Vencimiento.HasValue;
}

public static class LicenseOcrReader
{
    private static readonly string[] NameLabels =
    {
        "APELLIDO PATERNO",
        "APELLIDO MATERNO",
        "NOMBRE S",
        "NOMBRES",
        "NOMBRE",
        "NAME"
    };

    private static readonly string[] LicenseLabels =
    {
        "NUMERO DE LICENCIA",
        "NO DE LICENCIA",
        "NRO DE LICENCIA",
        "NUM LICENCIA",
        "NUMERO LICENCIA"
    };

    private static readonly string[] ExpirationLabels =
    {
        "FECHA DE EXPIRACION",
        "FECHA DE VENCIMIENTO",
        "VENCIMIENTO",
        "VIGENCIA",
        "VIGENTE",
        "VALIDA HASTA",
        "HASTA",
        "VENCE",
        "CADUCIDAD",
        "EXPIRA",
        "EXPIRACION"
    };

    private static readonly string[] FieldStopWords =
    {
        "APELLIDO",
        "NOMBRE",
        "CURP",
        "RFC",
        "LICENCIA",
        "NUMERO",
        "FECHA",
        "VIGENCIA",
        "VENCIMIENTO",
        "EXPEDICION",
        "CADUCIDAD",
        "DOMICILIO",
        "GOBIERNO",
        "ESTADO",
        "ESTADOS",
        "MEXICANO",
        "MEXICANOS",
        "SECRETARIA",
        "TRANSPORTE",
        "FEDERAL",
        "CONDUCTOR",
        "CHOFER",
        "CELULAR",
        "TELEFONO",
        "TEL",
        "CALLE",
        "COLONIA",
        "MUNICIPIO",
        "NACIMIENTO",
        "ANTIGUEDAD",
        "TIPO",
        "CATEGORIA",
        "PLACA",
        "VEHICULO",
        "FOLIO",
        "DIRECCION",
        "MOVIL",
        "PHONE"
    };

    public static async Task<LicenseOcrData> ReadAsync(byte[] bytes)
    {
        var best = ParseText(await ReadTextAsync(bytes));

        if (NeedsSecondPass(best))
        {
            var enhancedBytes = await CreateEnhancedImageAsync(bytes);
            if (enhancedBytes is not null)
            {
                var enhanced = ParseText(await ReadTextAsync(enhancedBytes));
                best = best.MergeWith(enhanced);
            }
        }

        return new LicenseOcrData(best.Nombre, best.Licencia, best.Vencimiento);
    }

    private static async Task<string> ReadTextAsync(byte[] bytes)
    {
        try
        {
            var result = await OcrPlugin.Default.RecognizeTextAsync(bytes, tryHard: false);
            return result?.AllText?.Trim() ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static bool NeedsSecondPass(ParsedLicenseResult result)
    {
        return string.IsNullOrWhiteSpace(result.Nombre) ||
               string.IsNullOrWhiteSpace(result.Licencia) ||
               !result.Vencimiento.HasValue ||
               result.NombreScore < 70 ||
               result.LicenciaScore < 70 ||
               result.VencimientoScore < 70;
    }

    private static ParsedLicenseResult ParseText(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return ParsedLicenseResult.Empty;

        var lines = BuildLines(text);

        var name = TryExtractName(lines);
        var license = TryExtractLicense(lines);
        var expiration = TryExtractExpiration(lines);

        return new ParsedLicenseResult(
            name.Value,
            name.Score,
            license.Value,
            license.Score,
            expiration.Value,
            expiration.Score);
    }

    private static ParsedLicenseResult MergeWith(
        this ParsedLicenseResult current,
        ParsedLicenseResult other)
    {
        return new ParsedLicenseResult(
            ChooseBetterValue(current.Nombre, current.NombreScore, other.Nombre, other.NombreScore),
            Math.Max(current.NombreScore, other.NombreScore),
            ChooseBetterValue(current.Licencia, current.LicenciaScore, other.Licencia, other.LicenciaScore),
            Math.Max(current.LicenciaScore, other.LicenciaScore),
            other.VencimientoScore > current.VencimientoScore ? other.Vencimiento : current.Vencimiento,
            Math.Max(current.VencimientoScore, other.VencimientoScore));
    }

    private static string? ChooseBetterValue(string? currentValue, int currentScore, string? candidateValue, int candidateScore)
    {
        if (string.IsNullOrWhiteSpace(candidateValue))
            return currentValue;

        if (string.IsNullOrWhiteSpace(currentValue))
            return candidateValue;

        if (candidateScore > currentScore)
            return candidateValue;

        if (candidateScore == currentScore && candidateValue.Length > currentValue.Length)
            return candidateValue;

        return currentValue;
    }

    private static (string? Value, int Score) TryExtractName(IReadOnlyList<OcrLine> lines)
    {
        var structured = TryExtractStructuredName(lines);
        var fallback = TryExtractContiguousName(lines);
        return PreferBetterCandidate(structured, fallback);
    }

    private static (string? Value, int Score) TryExtractStructuredName(IReadOnlyList<OcrLine> lines)
    {
        var apellidoPaterno = ExtractNameLabelValue(lines, new[] { "APELLIDO PATERNO" }, 2);
        var apellidoMaterno = ExtractNameLabelValue(lines, new[] { "APELLIDO MATERNO" }, 2);
        var nombres = ExtractNameLabelValue(lines, new[] { "NOMBRE S", "NOMBRES", "NOMBRE", "NAME" }, 3);

        var parts = new List<string>();
        var score = 0;

        if (!string.IsNullOrWhiteSpace(apellidoPaterno.Value))
        {
            parts.Add(apellidoPaterno.Value);
            score += apellidoPaterno.Score;
        }

        if (!string.IsNullOrWhiteSpace(apellidoMaterno.Value))
        {
            parts.Add(apellidoMaterno.Value);
            score += apellidoMaterno.Score;
        }

        if (!string.IsNullOrWhiteSpace(nombres.Value))
        {
            parts.Add(nombres.Value);
            score += nombres.Score;
        }

        if (parts.Count == 0)
            return (null, 0);

        var combined = CleanReadableText(string.Join(" ", parts));
        var words = combined.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        score += 50;
        score += Math.Min(20, words.Length * 4);

        if (parts.Count == 1 && words.Length <= 2)
            score -= 10;

        return (combined, score);
    }

    private static (string? Value, int Score) TryExtractContiguousName(IReadOnlyList<OcrLine> lines)
    {
        string? best = null;
        var bestScore = 0;

        for (var i = 0; i < lines.Count; i++)
        {
            if (!IsLikelyNameFragment(lines[i].Normalized))
                continue;

            var block = new List<string>();
            for (var j = i; j < lines.Count && j < i + 4; j++)
            {
                if (!IsLikelyNameFragment(lines[j].Normalized))
                    break;

                block.Add(lines[j].Normalized);
            }

            var combined = CleanReadableText(string.Join(" ", block));
            var words = combined.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 2 || words.Length > 6)
                continue;

            if (ContainsAnyKeyword(combined, FieldStopWords))
                continue;

            var score = 55 + words.Length * 5 + block.Count * 3 - Math.Min(i, 10);
            if (score > bestScore)
            {
                best = combined;
                bestScore = score;
            }
        }

        return (best, bestScore);
    }

    private static (string? Value, int Score) ExtractNameLabelValue(
        IReadOnlyList<OcrLine> lines,
        IReadOnlyList<string> labels,
        int maxLookAheadLines)
    {
        foreach (var label in labels)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                var normalizedLine = lines[i].Normalized;
                var labelIndex = normalizedLine.IndexOf(label, StringComparison.Ordinal);
                if (labelIndex < 0)
                    continue;

                var parts = new List<string>();
                var tail = CleanReadableText(normalizedLine[(labelIndex + label.Length)..]);
                if (IsLikelyNameFragment(tail))
                    parts.Add(tail);

                for (var offset = 1; offset <= maxLookAheadLines && i + offset < lines.Count; offset++)
                {
                    var candidate = lines[i + offset].Normalized;
                    if (!IsLikelyNameFragment(candidate))
                        break;

                    parts.Add(candidate);
                }

                var combined = CleanReadableText(string.Join(" ", parts));
                if (!string.IsNullOrWhiteSpace(combined))
                {
                    var words = combined.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var score = 70 + words.Length * 4 + Math.Max(0, 10 - i);
                    if (label == "NOMBRE S" || label == "NOMBRES" || label == "NOMBRE" || label == "NAME")
                        score += 5;
                    return (combined, score);
                }
            }
        }

        return (null, 0);
    }

    private static bool IsLikelyNameFragment(string value)
    {
        var normalized = NormalizeComparisonText(value);
        if (normalized.Length is < 2 or > 40)
            return false;

        if (normalized.Any(char.IsDigit))
            return false;

        if (ContainsAnyKeyword(normalized, FieldStopWords))
            return false;

        var words = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length is < 1 or > 4)
            return false;

        return Regex.IsMatch(normalized, @"^[A-Z ]+$", RegexOptions.CultureInvariant);
    }

    private static (string? Value, int Score) TryExtractLicense(IReadOnlyList<OcrLine> lines)
    {
        var byLabel = TryExtractLicenseByLabel(lines);
        var fallback = TryExtractLicenseFallback(lines);
        return PreferBetterCandidate(byLabel, fallback);
    }

    private static (string? Value, int Score) TryExtractLicenseByLabel(IReadOnlyList<OcrLine> lines)
    {
        foreach (var label in LicenseLabels)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].Normalized.IndexOf(label, StringComparison.Ordinal) < 0)
                    continue;

                var sameLine = ExtractBestLicenseCandidateFromText(lines[i].Raw);
                if (!string.IsNullOrWhiteSpace(sameLine.Value))
                    return (NormalizeLicenseValue(sameLine.Value), 95);

                for (var offset = 1; offset <= 2 && i + offset < lines.Count; offset++)
                {
                    var nextLine = ExtractBestLicenseCandidateFromText(lines[i + offset].Raw);
                    if (!string.IsNullOrWhiteSpace(nextLine.Value))
                        return (NormalizeLicenseValue(nextLine.Value), 90 - offset * 5);
                }
            }
        }

        return (null, 0);
    }

    private static (string? Value, int Score) TryExtractLicenseFallback(IReadOnlyList<OcrLine> lines)
    {
        string? best = null;
        var bestScore = 0;

        for (var i = 0; i < lines.Count; i++)
        {
            var windowText = BuildWindowText(lines, i, 2);
            var normalizedWindow = NormalizeComparisonText(windowText);
            if (ContainsAnyKeyword(normalizedWindow, FieldStopWords) &&
                !normalizedWindow.Contains("NUMERO", StringComparison.Ordinal) &&
                !normalizedWindow.Contains("LICENCIA", StringComparison.Ordinal))
            {
                continue;
            }

            var candidate = ExtractBestLicenseCandidateFromText(windowText);
            if (string.IsNullOrWhiteSpace(candidate.Value))
                continue;

            var normalizedCandidate = NormalizeLicenseValue(candidate.Value);
            if (normalizedCandidate.Length < 6 || normalizedCandidate.Length > 20)
                continue;

            var score = candidate.Score;
            if (ContainsAnyKeyword(normalizedWindow, LicenseLabels))
                score += 20;
            if (i > 0 && ContainsAnyKeyword(lines[i - 1].Normalized, LicenseLabels))
                score += 10;
            if (i + 1 < lines.Count && ContainsAnyKeyword(lines[i + 1].Normalized, LicenseLabels))
                score += 10;

            if (score > bestScore)
            {
                best = normalizedCandidate;
                bestScore = score;
            }
        }

        return (best, bestScore);
    }

    private static (string? Value, int Score) ExtractBestLicenseCandidateFromText(string text)
    {
        string? best = null;
        var bestScore = 0;

        foreach (var candidate in ExtractLicenseCandidates(text))
        {
            var normalized = NormalizeLicenseValue(candidate);
            if (normalized.Length is < 6 or > 20)
                continue;

            if (!normalized.Any(char.IsDigit))
                continue;

            if (LooksLikeDate(normalized))
                continue;

            var score = normalized.Length;
            if (Regex.IsMatch(normalized, @"[A-Z]", RegexOptions.CultureInvariant) &&
                Regex.IsMatch(normalized, @"\d", RegexOptions.CultureInvariant))
            {
                score += 20;
            }
            else
            {
                score += 8;
            }

            if (normalized.Length is >= 8 and <= 12)
                score += 5;

            if (score > bestScore)
            {
                best = normalized;
                bestScore = score;
            }
        }

        return (best, bestScore);
    }

    private static IEnumerable<string> ExtractLicenseCandidates(string text)
    {
        var tokens = Regex.Matches(text.ToUpperInvariant(), @"[A-Z0-9]+")
            .Select(match => match.Value)
            .Where(token => token.Length > 0)
            .ToList();

        var yielded = new HashSet<string>(StringComparer.Ordinal);

        for (var i = 0; i < tokens.Count; i++)
        {
            var builder = new StringBuilder();

            for (var length = 1; length <= 3 && i + length <= tokens.Count; length++)
            {
                builder.Clear();
                for (var j = i; j < i + length; j++)
                    builder.Append(tokens[j]);

                var candidate = builder.ToString();
                if (candidate.Length is < 4 or > 20)
                    continue;

                if (!candidate.Any(char.IsDigit))
                    continue;

                if (yielded.Add(candidate))
                    yield return candidate;
            }
        }
    }

    private static bool LooksLikeDate(string value)
    {
        return Regex.IsMatch(value, @"^\d{1,2}[\/\.\-]\d{1,2}[\/\.\-]\d{2,4}$", RegexOptions.CultureInvariant) ||
               Regex.IsMatch(value, @"^\d{4}[\/\.\-]\d{1,2}[\/\.\-]\d{1,2}$", RegexOptions.CultureInvariant);
    }

    private static (DateTime? Value, int Score) TryExtractExpiration(IReadOnlyList<OcrLine> lines)
    {
        var bestValue = default(DateTime?);
        var bestScore = 0;

        for (var i = 0; i < lines.Count; i++)
        {
            var windowText = BuildWindowText(lines, i, 2);
            var normalizedWindow = NormalizeComparisonText(windowText);

            if (!ContainsAnyKeyword(normalizedWindow, ExpirationLabels) &&
                !normalizedWindow.Contains("HASTA", StringComparison.Ordinal))
            {
                continue;
            }

            if (normalizedWindow.Contains("EXPEDICION", StringComparison.Ordinal))
                continue;

            var dates = ExtractDateCandidates(windowText).ToList();
            if (dates.Count == 0)
                continue;

            var chosenDate = dates.Last();
            if (!TryParseOcrDate(chosenDate, out var parsedDate))
                continue;

            var score = 70;
            if (normalizedWindow.Contains("HASTA", StringComparison.Ordinal))
                score += 20;
            if (normalizedWindow.Contains("VIGENTE", StringComparison.Ordinal))
                score += 10;
            if (normalizedWindow.Contains("VENCIMIENTO", StringComparison.Ordinal) ||
                normalizedWindow.Contains("EXPIRACION", StringComparison.Ordinal) ||
                normalizedWindow.Contains("CADUCIDAD", StringComparison.Ordinal) ||
                normalizedWindow.Contains("VENCE", StringComparison.Ordinal))
            {
                score += 15;
            }

            if (score > bestScore)
            {
                bestValue = parsedDate;
                bestScore = score;
            }
        }

        if (bestValue.HasValue)
            return (bestValue, bestScore);

        var allText = string.Join(" ", lines.Select(line => line.Raw));
        var allDates = ExtractDateCandidates(allText).ToList();
        if (allDates.Count == 1 && TryParseOcrDate(allDates[0], out var singleDate))
            return (singleDate, 25);

        return (null, 0);
    }

    private static IEnumerable<string> ExtractDateCandidates(string text)
    {
        const string datePattern = @"(?:\d{1,2}[\/\.\-]\d{1,2}[\/\.\-]\d{2,4}|\d{4}[\/\.\-]\d{1,2}[\/\.\-]\d{1,2})";

        foreach (Match match in Regex.Matches(text, datePattern, RegexOptions.CultureInvariant))
        {
            if (match.Success)
                yield return match.Value;
        }
    }

    private static bool TryParseOcrDate(string value, out DateTime date)
    {
        var normalized = Regex.Replace(value.Trim(), @"[.\-]", "/");

        if (DateTime.TryParseExact(
                normalized,
                new[]
                {
                    "d/M/yyyy",
                    "dd/MM/yyyy",
                    "d/M/yy",
                    "dd/MM/yy",
                    "yyyy/M/d",
                    "yyyy/MM/dd",
                    "yyyy/M/dd",
                    "yyyy/MM/d"
                },
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out date))
        {
            date = date.Date;
            return true;
        }

        if (DateTime.TryParse(
                normalized,
                new CultureInfo("es-MX"),
                DateTimeStyles.AllowWhiteSpaces,
                out date))
        {
            date = date.Date;
            return true;
        }

        date = default;
        return false;
    }

    private static IReadOnlyList<OcrLine> BuildLines(string text)
    {
        return text.Replace("\r", "\n")
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(line => new OcrLine(line, NormalizeComparisonText(line)))
            .ToList();
    }

    private static string BuildWindowText(IReadOnlyList<OcrLine> lines, int startIndex, int count)
    {
        return string.Join(" ", lines.Skip(startIndex).Take(count).Select(line => line.Raw));
    }

    private static string NormalizeComparisonText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var formD = value.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(formD.Length);

        foreach (var c in formD)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category == UnicodeCategory.NonSpacingMark)
                continue;

            if (char.IsLetterOrDigit(c))
                builder.Append(char.ToUpperInvariant(c));
            else
                builder.Append(' ');
        }

        return Regex.Replace(builder.ToString(), @"\s+", " ").Trim();
    }

    private static string CleanReadableText(string value)
    {
        return Regex.Replace(value, @"\s+", " ").Trim(' ', ':', ';', ',', '.', '-', '|');
    }

    private static string NormalizeLicenseValue(string value)
    {
        return Regex.Replace(value, @"\s+", string.Empty).Trim().ToUpperInvariant();
    }

    private static bool ContainsAnyKeyword(string value, IEnumerable<string> keywords)
    {
        foreach (var keyword in keywords)
        {
            if (value.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    private static (string? Value, int Score) PreferBetterCandidate(
        (string? Value, int Score) current,
        (string? Value, int Score) candidate)
    {
        if (string.IsNullOrWhiteSpace(candidate.Value))
            return current;

        if (string.IsNullOrWhiteSpace(current.Value))
            return candidate;

        if (candidate.Score > current.Score)
            return candidate;

        if (candidate.Score == current.Score && candidate.Value.Length > current.Value.Length)
            return candidate;

        return current;
    }

    private static Task<byte[]?> CreateEnhancedImageAsync(byte[] bytes)
    {
        return Task.Run(() =>
        {
            using var source = SKBitmap.Decode(bytes);
            if (source is null)
                return (byte[]?)null;

            var maxDimension = Math.Max(source.Width, source.Height);
            var targetMax = Math.Clamp(Math.Max(maxDimension, 1600), 1600, 2400);
            var scale = (double)targetMax / maxDimension;
            var targetWidth = Math.Max(1, (int)Math.Round(source.Width * scale));
            var targetHeight = Math.Max(1, (int)Math.Round(source.Height * scale));

            using var resized = source.Resize(new SKImageInfo(targetWidth, targetHeight), SKFilterQuality.High);
            if (resized is null)
                return (byte[]?)null;

            using var output = new SKBitmap(targetWidth, targetHeight, SKColorType.Bgra8888, SKAlphaType.Opaque);
            using (var canvas = new SKCanvas(output))
            {
                canvas.Clear(SKColors.White);
                using var paint = new SKPaint
                {
                    IsAntialias = true,
                    FilterQuality = SKFilterQuality.High,
                    ColorFilter = SKColorFilter.CreateColorMatrix(CreateContrastMatrix(1.35f, 8f)),
                };

                canvas.DrawBitmap(resized, new SKRect(0, 0, targetWidth, targetHeight), paint);
                canvas.Flush();
            }

            using var image = SKImage.FromBitmap(output);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 95);
            return data.ToArray();
        });
    }

    private static float[] CreateContrastMatrix(float contrast, float brightness)
    {
        var translation = 128f * (1f - contrast) + brightness;

        return new[]
        {
            0.299f * contrast, 0.587f * contrast, 0.114f * contrast, 0, translation,
            0.299f * contrast, 0.587f * contrast, 0.114f * contrast, 0, translation,
            0.299f * contrast, 0.587f * contrast, 0.114f * contrast, 0, translation,
            0, 0, 0, 1, 0
        };
    }

    private sealed record OcrLine(string Raw, string Normalized);

    private sealed record ParsedLicenseResult(
        string? Nombre,
        int NombreScore,
        string? Licencia,
        int LicenciaScore,
        DateTime? Vencimiento,
        int VencimientoScore)
    {
        public static readonly ParsedLicenseResult Empty = new(null, 0, null, 0, null, 0);

        public ParsedLicenseResult MergeWith(ParsedLicenseResult other)
        {
            return new ParsedLicenseResult(
                ChooseBetterValue(Nombre, NombreScore, other.Nombre, other.NombreScore),
                Math.Max(NombreScore, other.NombreScore),
                ChooseBetterValue(Licencia, LicenciaScore, other.Licencia, other.LicenciaScore),
                Math.Max(LicenciaScore, other.LicenciaScore),
                other.VencimientoScore > VencimientoScore ? other.Vencimiento : Vencimiento,
                Math.Max(VencimientoScore, other.VencimientoScore));
        }
    }
}
