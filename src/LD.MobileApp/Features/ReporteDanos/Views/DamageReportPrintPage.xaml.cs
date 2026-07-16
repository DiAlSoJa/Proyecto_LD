using LD.Contracts.DamageReports;
using LD.Contracts.Requests;
using MauiAppLogin.Controls;
using MauiAppLogin.Services;
using System.Globalization;
using System.Text;

namespace MauiAppLogin;

public partial class DamageReportPrintPage : ContentPage, IQueryAttributable
{
    private readonly List<string> _photoPaths = new();
    private readonly IDialogService _dialogService;
    private readonly IBluetoothPrinterService _bluetoothPrinterService;
    private DamageReportRequest? _report;

    public DamageReportPrintPage(
        IDialogService dialogService,
        IBluetoothPrinterService bluetoothPrinterService)
    {
        InitializeComponent();
        _dialogService = dialogService;
        _bluetoothPrinterService = bluetoothPrinterService;
        BindingContext = this;
    }

    public string StandardIdText { get; private set; } = "Estandar ID: -";
    public string DamageReportCodeText { get; private set; } = "Codigo QR: -";
    public string PartNumberText { get; private set; } = "Numero de Parte: -";
    public string UbicacionText { get; private set; } = "Ubicacion: -";
    public string StatusText { get; private set; } = "Status: -";
    public string CantidadRecibidaText { get; private set; } = "Cantidad Recibida: -";
    public string AlmacenText { get; private set; } = "Almacen: -";
    public string ProyectoText { get; private set; } = "Proyecto: -";
    public string ClienteText { get; private set; } = "Cliente: -";
    public string DescripcionText { get; private set; } = "Descripcion: -";
    public string AsnText { get; private set; } = "ASN: -";
    public string FechaRecepcionText { get; private set; } = "Fecha de Recepcion: -";
    public string DisponibleText { get; private set; } = "Disponible: -";
    public string EstadoText { get; private set; } = "Estado: -";
    public string TipoDanoText { get; private set; } = "Tipo de Dano: -";
    public string CategoriaText { get; private set; } = "Categoria: -";
    public string NuevoEstatusText { get; private set; } = "Nuevo Estatus: -";
    public string FechaReporteText { get; private set; } = "Fecha de Reporte: -";
    public string Comments { get; private set; } = string.Empty;
    public ImageSource? Photo1Source { get; private set; }
    public ImageSource? Photo2Source { get; private set; }
    public ImageSource? Photo3Source { get; private set; }
    public ImageSource? Photo4Source { get; private set; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("report", out var reportValue) && reportValue is DamageReportRequest report)
            ApplyReport(report);

        Photo1Source = BuildImageSource(GetQueryString(query, "photo1Path"));
        Photo2Source = BuildImageSource(GetQueryString(query, "photo2Path"));
        Photo3Source = BuildImageSource(GetQueryString(query, "photo3Path"));
        Photo4Source = BuildImageSource(GetQueryString(query, "photo4Path"));
        _photoPaths.Clear();
        AddPhotoPath(GetQueryString(query, "photo1Path"));
        AddPhotoPath(GetQueryString(query, "photo2Path"));
        AddPhotoPath(GetQueryString(query, "photo3Path"));
        AddPhotoPath(GetQueryString(query, "photo4Path"));

        RefreshBindings();
    }

    private void ApplyReport(DamageReportRequest report)
    {
        _report = report;

        StandardIdText = BuildText("Estandar ID", FirstNotEmpty(report.StandardIdCode, report.StandardId?.ToString()));

        var reportCode = DamageReportCodeGenerator.Normalize(report.DamageReportCode);
        if (string.IsNullOrWhiteSpace(reportCode))
        {
            reportCode = DamageReportCodeGenerator.Generate(
                report.ReportDate == default ? DateTime.Now : report.ReportDate);
            report.DamageReportCode = reportCode;
        }

        DamageReportCodeText = BuildText("Codigo QR", reportCode);
        PartNumberText = BuildText("Numero de Parte", report.PartNumber);
        UbicacionText = BuildText("Ubicacion", report.Location);
        StatusText = BuildText("Status", report.CurrentStatus);
        CantidadRecibidaText = BuildText("Cantidad Recibida", FormatDecimal(report.ReceivedQuantity));
        AlmacenText = BuildText("Almacen", report.Warehouse);
        ProyectoText = BuildText("Proyecto", report.Project);
        ClienteText = BuildText("Cliente", report.Client);
        DescripcionText = BuildText("Descripcion", report.Description);
        AsnText = BuildText("ASN", report.Asn);
        FechaRecepcionText = BuildText("Fecha de Recepcion", FormatDate(report.ReceptionDate));
        DisponibleText = BuildText("Disponible", FormatDecimal(report.AvailableQuantity));
        EstadoText = BuildText("Estado", report.InventoryState);
        TipoDanoText = BuildText("Tipo de Dano", report.DamageType);
        CategoriaText = BuildText("Categoria", report.Category);
        NuevoEstatusText = BuildText("Nuevo Estatus", report.NewStatus);
        FechaReporteText = BuildText("Fecha de Reporte", FormatDate(report.ReportDate));
        Comments = report.Comments ?? string.Empty;
    }

    private static string BuildText(string label, string? value)
    {
        return $"{label}: {FirstNotEmpty(value, "-")}";
    }

    private static string FirstNotEmpty(params string?[] values)
    {
        return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))?.Trim() ?? string.Empty;
    }

    private static string FormatDecimal(decimal? value)
    {
        return value.HasValue ? value.Value.ToString("0.##") : string.Empty;
    }

    private static string FormatDate(DateTime? value)
    {
        return value.HasValue && value.Value != default
            ? value.Value.ToString("dd-MM-yyyy HH:mm")
            : string.Empty;
    }

    private static string GetQueryString(IDictionary<string, object> query, string key)
    {
        return query.TryGetValue(key, out var value) ? value?.ToString() ?? string.Empty : string.Empty;
    }

    private static ImageSource? BuildImageSource(string? path)
    {
        return !string.IsNullOrWhiteSpace(path) && File.Exists(path)
            ? ImageSource.FromFile(path)
            : null;
    }

    private void AddPhotoPath(string? path)
    {
        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            _photoPaths.Add(path);
    }

    private void RefreshBindings()
    {
        OnPropertyChanged(nameof(StandardIdText));
        OnPropertyChanged(nameof(DamageReportCodeText));
        OnPropertyChanged(nameof(PartNumberText));
        OnPropertyChanged(nameof(UbicacionText));
        OnPropertyChanged(nameof(StatusText));
        OnPropertyChanged(nameof(CantidadRecibidaText));
        OnPropertyChanged(nameof(AlmacenText));
        OnPropertyChanged(nameof(ProyectoText));
        OnPropertyChanged(nameof(ClienteText));
        OnPropertyChanged(nameof(DescripcionText));
        OnPropertyChanged(nameof(AsnText));
        OnPropertyChanged(nameof(FechaRecepcionText));
        OnPropertyChanged(nameof(DisponibleText));
        OnPropertyChanged(nameof(EstadoText));
        OnPropertyChanged(nameof(TipoDanoText));
        OnPropertyChanged(nameof(CategoriaText));
        OnPropertyChanged(nameof(NuevoEstatusText));
        OnPropertyChanged(nameof(FechaReporteText));
        OnPropertyChanged(nameof(Comments));
        OnPropertyChanged(nameof(Photo1Source));
        OnPropertyChanged(nameof(Photo2Source));
        OnPropertyChanged(nameof(Photo3Source));
        OnPropertyChanged(nameof(Photo4Source));
    }

    private async void OnPrintClicked(object sender, EventArgs e)
    {
        if (_report is null)
        {
            await _dialogService.ShowErrorAsync("Impresion", "No se encontro el reporte para imprimir.");
            return;
        }

        var savedPrinter = await _bluetoothPrinterService.GetSavedPrinterAsync();
        if (savedPrinter is null)
        {
            var goToSettings = await _dialogService.ShowWarningAsync(
                "Impresion",
                "No hay una impresora Bluetooth guardada. Deseas configurarla ahora?");

            if (goToSettings)
                await Shell.Current.GoToAsync(nameof(BluetoothPrinterSettingsPage));

            return;
        }

        try
        {
            await _bluetoothPrinterService.PrintDamageReportAsync(_report);
            await _dialogService.ShowSuccessAsync("Impresion", "El reporte se envio a la impresora Bluetooth.");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Impresion", ex.Message);
        }
    }

    private async void OnConfigurePrinterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(BluetoothPrinterSettingsPage));
    }

    private async void OnPDFClicked(object sender, EventArgs e)
    {
        try
        {
            var pdfPath = await CreatePdfAsync();
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Reporte de danos",
                File = new ShareFile(pdfPath)
            });
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("PDF", ex.Message);
        }
    }

    private async void OnExcelClicked(object sender, EventArgs e)
    {
        try
        {
            var excelPath = await CreateExcelAsync();
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Reporte de danos",
                File = new ShareFile(excelPath)
            });
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("Excel", ex.Message);
        }
    }

    private async Task<string> CreatePdfAsync()
    {
        if (_report is null)
            throw new InvalidOperationException("No se encontro el reporte para imprimir.");

        return await DamageReportPdfExporter.CreateAsync(_report, _photoPaths);
    }

    private async Task<string> CreateExcelAsync()
    {
        if (_report is null)
            throw new InvalidOperationException("No se encontro el reporte para exportar.");

        return await DamageReportExcelExporter.CreateAsync(_report, _photoPaths);
    }

    private IReadOnlyList<string> GetSummaryLines()
    {
        return new[]
        {
            StandardIdText,
            DamageReportCodeText,
            PartNumberText,
            UbicacionText,
            StatusText,
            CantidadRecibidaText,
            AlmacenText,
            ProyectoText,
            ClienteText,
            DescripcionText,
            AsnText,
            FechaRecepcionText,
            DisponibleText,
            EstadoText,
            TipoDanoText,
            CategoriaText,
            NuevoEstatusText,
            FechaReporteText,
            $"Comentarios: {Comments}"
        };
    }

    private sealed class SimplePdfBuilder
    {
        private const double PageWidth = 595;
        private const double PageHeight = 842;
        private readonly List<PdfPage> _pages = new();

        public void AddTextPage(string title, IReadOnlyList<string> lines)
        {
            var content = new StringBuilder();
            content.AppendLine("BT /F1 18 Tf 50 792 Td");
            content.AppendLine($"{ToPdfText(title)} Tj");
            content.AppendLine("ET");

            var y = 758;
            foreach (var line in lines.SelectMany(x => WrapLine(x, 86)))
            {
                content.AppendLine($"BT /F1 11 Tf 50 {y} Td {ToPdfText(line)} Tj ET");
                y -= 18;

                if (y < 60)
                    break;
            }

            _pages.Add(new PdfPage(content.ToString(), Array.Empty<PdfImage>()));
        }

        public void AddImagePages(IReadOnlyList<string> imagePaths)
        {
            if (imagePaths.Count == 0)
                return;

            var currentImages = new List<PdfImage>();
            var content = new StringBuilder();
            var y = 700d;
            var imageIndex = 1;

            foreach (var path in imagePaths)
            {
                if (!TryCreateImage(path, imageIndex, out var image))
                    continue;

                var target = FitImage(image.Width, image.Height, 495, 260);
                if (y - target.Height < 60 && currentImages.Count > 0)
                {
                    _pages.Add(new PdfPage(content.ToString(), currentImages));
                    currentImages = new List<PdfImage>();
                    content = new StringBuilder();
                    y = 700;
                }

                currentImages.Add(image);
                content.AppendLine($"BT /F1 13 Tf 50 {y + 20:0.##} Td {ToPdfText($"Foto {imageIndex}")} Tj ET");
                content.AppendLine($"q {target.Width:0.##} 0 0 {target.Height:0.##} 50 {y - target.Height:0.##} cm /{image.Name} Do Q");

                y -= target.Height + 55;
                imageIndex++;
            }

            if (currentImages.Count > 0)
                _pages.Add(new PdfPage(content.ToString(), currentImages));
        }

        public byte[] Build()
        {
            var objects = new List<byte[]>();
            AddObject(objects, "<< /Type /Catalog /Pages 2 0 R >>");

            var fontObjectNumber = 3;
            var pageObjectNumbers = new List<int>();
            var pendingPageObjects = new List<(PdfPage Page, int ContentObject, int PageObject)>();
            var imageObjectNumbers = new Dictionary<string, int>();

            objects.Add(Array.Empty<byte>());
            AddObject(objects, "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>");

            foreach (var page in _pages)
            {
                foreach (var image in page.Images)
                {
                    imageObjectNumbers[image.Name] = objects.Count + 1;
                    AddStreamObject(objects, image.Dictionary, image.Bytes);
                }

                var contentObject = objects.Count + 1;
                AddStreamObject(objects, "<< /Length {0} >>", Encoding.ASCII.GetBytes(page.Content));

                var pageObject = objects.Count + 1;
                pageObjectNumbers.Add(pageObject);
                pendingPageObjects.Add((page, contentObject, pageObject));
                objects.Add(Array.Empty<byte>());
            }

            objects[1] = Encoding.ASCII.GetBytes($"<< /Type /Pages /Kids [{string.Join(" ", pageObjectNumbers.Select(x => $"{x} 0 R"))}] /Count {pageObjectNumbers.Count} >>");

            foreach (var item in pendingPageObjects)
            {
                var imageResources = item.Page.Images.Count == 0
                    ? string.Empty
                    : $" /XObject << {string.Join(" ", item.Page.Images.Select(x => $"/{x.Name} {imageObjectNumbers[x.Name]} 0 R"))} >>";

                var pageDictionary = $"<< /Type /Page /Parent 2 0 R /MediaBox [0 0 {PageWidth} {PageHeight}] /Resources << /Font << /F1 {fontObjectNumber} 0 R >>{imageResources} >> /Contents {item.ContentObject} 0 R >>";
                objects[item.PageObject - 1] = Encoding.ASCII.GetBytes(pageDictionary);
            }

            return WritePdf(objects);
        }

        private static void AddObject(List<byte[]> objects, string value)
        {
            objects.Add(Encoding.ASCII.GetBytes(value));
        }

        private static void AddStreamObject(List<byte[]> objects, string dictionary, byte[] stream)
        {
            var header = string.Format(dictionary, stream.Length);
            var bytes = new List<byte>();
            bytes.AddRange(Encoding.ASCII.GetBytes(header));
            bytes.AddRange(Encoding.ASCII.GetBytes("\nstream\n"));
            bytes.AddRange(stream);
            bytes.AddRange(Encoding.ASCII.GetBytes("\nendstream"));
            objects.Add(bytes.ToArray());
        }

        private static byte[] WritePdf(IReadOnlyList<byte[]> objects)
        {
            using var output = new MemoryStream();
            WriteAscii(output, "%PDF-1.4\n");
            var offsets = new List<long> { 0 };

            for (var i = 0; i < objects.Count; i++)
            {
                offsets.Add(output.Position);
                WriteAscii(output, $"{i + 1} 0 obj\n");
                output.Write(objects[i], 0, objects[i].Length);
                WriteAscii(output, "\nendobj\n");
            }

            var xref = output.Position;
            WriteAscii(output, $"xref\n0 {objects.Count + 1}\n");
            WriteAscii(output, "0000000000 65535 f \n");
            foreach (var offset in offsets.Skip(1))
                WriteAscii(output, $"{offset:0000000000} 00000 n \n");

            WriteAscii(output, $"trailer\n<< /Size {objects.Count + 1} /Root 1 0 R >>\nstartxref\n{xref}\n%%EOF");
            return output.ToArray();
        }

        private static void WriteAscii(Stream stream, string value)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }

        private static bool TryCreateImage(string path, int index, out PdfImage image)
        {
            image = default;
            var bytes = File.ReadAllBytes(path);
            if (!TryGetJpegSize(bytes, out var width, out var height))
                return false;

            image = new PdfImage(
                $"Im{index}",
                width,
                height,
                bytes,
                $"<< /Type /XObject /Subtype /Image /Width {width} /Height {height} /ColorSpace /DeviceRGB /BitsPerComponent 8 /Filter /DCTDecode /Length {{0}} >>");

            return true;
        }

        private static (double Width, double Height) FitImage(double imageWidth, double imageHeight, double maxWidth, double maxHeight)
        {
            var scale = Math.Min(maxWidth / imageWidth, maxHeight / imageHeight);
            return (imageWidth * scale, imageHeight * scale);
        }

        private static bool TryGetJpegSize(byte[] bytes, out int width, out int height)
        {
            width = height = 0;
            if (bytes.Length < 4 || bytes[0] != 0xFF || bytes[1] != 0xD8)
                return false;

            var index = 2;
            while (index + 9 < bytes.Length)
            {
                if (bytes[index] != 0xFF)
                {
                    index++;
                    continue;
                }

                var marker = bytes[index + 1];
                var length = (bytes[index + 2] << 8) + bytes[index + 3];
                if (length < 2 || index + length >= bytes.Length)
                    return false;

                if (marker is 0xC0 or 0xC2)
                {
                    height = (bytes[index + 5] << 8) + bytes[index + 6];
                    width = (bytes[index + 7] << 8) + bytes[index + 8];
                    return width > 0 && height > 0;
                }

                index += length + 2;
            }

            return false;
        }

        private static IEnumerable<string> WrapLine(string value, int maxLength)
        {
            value = Sanitize(value);
            while (value.Length > maxLength)
            {
                var split = value.LastIndexOf(' ', Math.Min(maxLength, value.Length - 1));
                if (split <= 0)
                    split = maxLength;

                yield return value[..split].Trim();
                value = value[split..].Trim();
            }

            if (!string.IsNullOrWhiteSpace(value))
                yield return value;
        }

        private static string ToPdfText(string value)
        {
            return $"({EscapePdfText(Sanitize(value))})";
        }

        private static string EscapePdfText(string value)
        {
            return value
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }

        private static string Sanitize(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var normalized = value.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(normalized.Length);

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                    continue;

                builder.Append(c is >= ' ' and <= '~' ? c : '?');
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }

        #if false
        private static char RemoveAccent(char value)
        {
            return value switch
            {
                'Ã¡' or 'Ã ' or 'Ã¤' or 'Ã¢' or 'Ã' or 'Ã€' or 'Ã„' or 'Ã‚' => 'a',
                'Ã©' or 'Ã¨' or 'Ã«' or 'Ãª' or 'Ã‰' or 'Ãˆ' or 'Ã‹' or 'ÃŠ' => 'e',
                'Ã­' or 'Ã¬' or 'Ã¯' or 'Ã®' or 'Ã' or 'ÃŒ' or 'Ã' or 'ÃŽ' => 'i',
                'Ã³' or 'Ã²' or 'Ã¶' or 'Ã´' or 'Ã“' or 'Ã’' or 'Ã–' or 'Ã”' => 'o',
                'Ãº' or 'Ã¹' or 'Ã¼' or 'Ã»' or 'Ãš' or 'Ã™' or 'Ãœ' or 'Ã›' => 'u',
                'Ã±' or 'Ã‘' => 'n',
                _ => '?'
            };
        }

        #endif

        private readonly record struct PdfPage(string Content, IReadOnlyList<PdfImage> Images);

        private readonly record struct PdfImage(
            string Name,
            int Width,
            int Height,
            byte[] Bytes,
            string Dictionary);
    }
}
