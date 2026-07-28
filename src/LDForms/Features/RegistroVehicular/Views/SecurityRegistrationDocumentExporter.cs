using LD.Contracts.DTOs.Security;
using LD.Contracts.Enums;
using LD.FormsX.Features.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LD.FormsX.Views.RegistroVehicular;

internal static class SecurityRegistrationDocumentExporter
{
    private const double Dpi = 96d;
    private const double PageWidth = 8.5d * Dpi;
    private const double PageHeight = 11d * Dpi;
    private const double MarginSize = 38d;
    private const double ContentWidth = PageWidth - (MarginSize * 2);
    private const double RootHeight = PageHeight - (MarginSize * 2);
    private const double LabelWidth = 160d;
    private const double DetailWidth = 500d;

    private static readonly FontFamily Arial = new("Arial");
    private static readonly Brush MutedText = new SolidColorBrush(Color.FromRgb(90, 90, 90));

    private sealed record PhotoCard(string? Path);

    public static async Task PreviewAsync(
        SecurityRegistrationDto registration,
        IReadOnlyList<SecurityTaskDto> tasks,
        Func<string?, Task<byte[]>> imageBytesProvider)
    {
        var orderedTasks = tasks
            .OrderBy(t => t.FechaIniciada)
            .ThenBy(t => t.SecurityTaskId)
            .ToList();

        var imageCache = await LoadImageCacheAsync(
            CollectPhotoPaths(registration),
            imageBytesProvider);

        var document = BuildDocument(registration, orderedTasks, imageCache);
        var preview = new PrintPreviewWindow(
            document,
            $"Vista previa - Registro vehicular {registration.SecurityRegistrationId}",
            $"Registro vehicular {registration.SecurityRegistrationId}");

        WindowOwnerHelper.AttachOwnerOrCenter(preview, Application.Current?.MainWindow);
        preview.ShowDialog();
    }

    private static FixedDocument BuildDocument(
        SecurityRegistrationDto registration,
        IReadOnlyList<SecurityTaskDto> tasks,
        IReadOnlyDictionary<string, ImageSource?> imageCache)
    {
        var document = new FixedDocument
        {
            DocumentPaginator = { PageSize = new Size(PageWidth, PageHeight) }
        };

        var printedAt = DateTime.Now;
        var totalPages = 2 + tasks.Count;
        var headerSource = BuildHeaderBitmap(registration);

        AddPage(document, BuildPage(
            BuildRegistrationPage(registration, imageCache),
            headerSource,
            printedAt,
            1,
            totalPages));

        AddPage(document, BuildPage(
            BuildLicenseAndBoxPage(registration, imageCache),
            headerSource,
            printedAt,
            2,
            totalPages));

        for (var index = 0; index < tasks.Count; index++)
        {
            AddPage(document, BuildPage(
                BuildMovementPage(registration, tasks[index], imageCache),
                headerSource,
                printedAt,
                index + 3,
                totalPages));
        }

        return document;
    }

    private static FixedPage BuildPage(
        FrameworkElement body,
        ImageSource headerSource,
        DateTime printedAt,
        int pageNumber,
        int totalPages)
    {
        var page = new FixedPage
        {
            Width = PageWidth,
            Height = PageHeight,
            Background = Brushes.White
        };

        var root = new Grid
        {
            Width = ContentWidth,
            Height = RootHeight,
            Margin = new Thickness(MarginSize)
        };

        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        AddToGrid(root, BuildHeader(headerSource), 0);
        AddToGrid(root, body, 1);
        AddToGrid(root, BuildFooter(printedAt, pageNumber, totalPages), 2);

        page.Children.Add(root);
        return page;
    }

    private static FrameworkElement BuildHeader(ImageSource headerSource)
    {
        return new Image
        {
            Width = ContentWidth,
            Height = 132,
            Margin = new Thickness(0, 0, 0, 20),
            Source = headerSource,
            Stretch = Stretch.Fill
        };
    }

    private static ImageSource BuildHeaderBitmap(SecurityRegistrationDto registration)
    {
        var visual = BuildHeaderVisual(registration);
        visual.Measure(new Size(ContentWidth, 132));
        visual.Arrange(new Rect(0, 0, ContentWidth, 132));
        visual.UpdateLayout();
        visual.Dispatcher.Invoke(
            () => { },
            DispatcherPriority.Render);

        const double scale = 2d;
        RenderTargetBitmap? lastBitmap = null;

        for (var attempt = 0; attempt < 4; attempt++)
        {
            var bitmap = new RenderTargetBitmap(
                (int)Math.Ceiling(ContentWidth * scale),
                (int)Math.Ceiling(132 * scale),
                Dpi * scale,
                Dpi * scale,
                PixelFormats.Pbgra32);
            bitmap.Render(visual);
            bitmap.Freeze();
            lastBitmap = bitmap;

            if (HasHeaderContent(bitmap))
            {
                return CreatePortableBitmap(bitmap);
            }

            visual.Dispatcher.Invoke(
                () => { },
                DispatcherPriority.ApplicationIdle);
        }

        return CreatePortableBitmap(lastBitmap!);
    }

    private static BitmapSource CreatePortableBitmap(BitmapSource source)
    {
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(source));

        using var stream = new MemoryStream();
        encoder.Save(stream);
        stream.Position = 0;

        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = stream;
        image.EndInit();
        image.Freeze();
        return image;
    }

    private static bool HasHeaderContent(BitmapSource bitmap)
    {
        var stride = bitmap.PixelWidth * 4;
        var pixels = new byte[stride * bitmap.PixelHeight];
        bitmap.CopyPixels(pixels, stride, 0);

        var darkPixels = 0;
        for (var index = 0; index < pixels.Length; index += 4)
        {
            if (pixels[index] < 240 ||
                pixels[index + 1] < 240 ||
                pixels[index + 2] < 240)
            {
                darkPixels++;
            }
        }

        return darkPixels > 20_000;
    }

    private static FrameworkElement BuildHeaderVisual(SecurityRegistrationDto registration)
    {
        var header = new Grid
        {
            Width = ContentWidth,
            Height = 132,
            Background = Brushes.White
        };

        header.RowDefinitions.Add(new RowDefinition { Height = new GridLength(112) });
        header.RowDefinitions.Add(new RowDefinition { Height = new GridLength(3) });
        header.RowDefinitions.Add(new RowDefinition { Height = new GridLength(17) });

        var top = new Grid();
        top.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(136) });
        top.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
        top.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        var logo = new Image
        {
            Width = 108,
            Height = 78,
            Margin = new Thickness(10, 0, 0, 0),
            Source = LoadLogo(),
            Opacity = 0.35,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top
        };
        Grid.SetColumn(logo, 0);
        top.Children.Add(logo);

        var company = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Top
        };
        company.Children.Add(Text("LOGISTICA FLEXIBLE", 18, FontWeights.Bold));

        var warehouse = Text("ALMACEN B5", 10.5, FontWeights.Bold);
        warehouse.Margin = new Thickness(0, 17, 0, 0);
        company.Children.Add(warehouse);
        company.Children.Add(Text("CARRETERA LA VENTA NEXTIPAC NO.", 9.7));
        company.Children.Add(Text("3020", 9.7));
        company.Children.Add(Text("LA VENTA DEL ASTILLERO", 9.7));
        company.Children.Add(Text("ZAPOPAN JALISCO", 9.7));
        Grid.SetColumn(company, 1);
        top.Children.Add(company);

        var report = new Grid
        {
            Margin = new Thickness(10, 10, 0, 0)
        };
        report.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
        report.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
        report.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var outerTitle = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1.3),
            Padding = new Thickness(2),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top
        };
        outerTitle.Child = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Child = BuildCenteredText("Registro Vehicular", 20, FontWeights.Bold)
        };
        Grid.SetRow(outerTitle, 0);
        report.Children.Add(outerTitle);

        var status = BuildCenteredText(
            registration.IsActive ? "ACTIVO" : "INACTIVO",
            25,
            FontWeights.Black);
        status.Foreground = registration.IsActive
            ? Brushes.Red
            : new SolidColorBrush(Color.FromRgb(100, 100, 100));
        Grid.SetRow(status, 1);
        report.Children.Add(status);

        var recordNumber = Text(
            $"No. Registro: {registration.SecurityRegistrationId}",
            9.5,
            FontWeights.Normal,
            TextAlignment.Right);
        recordNumber.VerticalAlignment = VerticalAlignment.Bottom;
        Grid.SetRow(recordNumber, 2);
        report.Children.Add(recordNumber);

        Grid.SetColumn(report, 2);
        top.Children.Add(report);

        Grid.SetRow(top, 0);
        header.Children.Add(top);

        var rule = new Border
        {
            Height = 3,
            Background = Brushes.Black
        };
        Grid.SetRow(rule, 1);
        header.Children.Add(rule);

        return header;
    }

    private static FrameworkElement BuildRegistrationPage(
        SecurityRegistrationDto registration,
        IReadOnlyDictionary<string, ImageSource?> imageCache)
    {
        var body = new StackPanel
        {
            Width = ContentWidth,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        body.Children.Add(BuildFieldTable(
        [
            ("No. Registro:", registration.SecurityRegistrationId.ToString(CultureInfo.InvariantCulture)),
            ("Estado:", FormatStatus(registration.Estado)),
            ("Hora Ingreso:", FormatDateTime(registration.CreatedAt)),
            ("Tipo:", FormatValue(registration.Tipo)),
            ("Cortina:", FormatValue(registration.CortinaNumero)),
            ("Activo:", registration.IsActive ? "SI" : "NO")
        ],
        DetailWidth,
        24));

        var vehiclePhotos = GetRegistrationVehiclePhotos(registration);
        var vehiclePanel = BuildVehiclePanel(registration, vehiclePhotos, imageCache);
        vehiclePanel.Margin = new Thickness(0, 22, 0, 0);
        body.Children.Add(vehiclePanel);

        var driverPanel = BuildDriverPanel(registration, imageCache);
        driverPanel.Margin = new Thickness(0, 22, 0, 0);
        body.Children.Add(driverPanel);

        return body;
    }

    private static FrameworkElement BuildVehiclePanel(
        SecurityRegistrationDto registration,
        IReadOnlyList<PhotoCard> photos,
        IReadOnlyDictionary<string, ImageSource?> imageCache)
    {
        var panel = new Grid
        {
            Width = ContentWidth,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(LabelWidth) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(DetailWidth - LabelWidth) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ContentWidth - DetailWidth) });

        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(24) });
        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(24) });
        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(112) });
        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(112) });
        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(24) });

        AddTableCell(panel, "Linea:", 0, 0, true);
        AddTableCell(panel, FormatValue(registration.Linea), 0, 1, false);
        AddTableCell(panel, "Origen:", 1, 0, true);
        AddTableCell(panel, FormatValue(registration.Origen), 1, 1, false);
        AddTableCell(panel, "Numero:", 2, 0, true, VerticalAlignment.Top);
        AddTableCell(panel, FormatValue(registration.Numero), 2, 1, false, VerticalAlignment.Top);
        AddTableCell(panel, "Placa:", 3, 0, true, VerticalAlignment.Top);
        AddTableCell(panel, FormatValue(registration.Placa), 3, 1, false, VerticalAlignment.Top);
        AddTableCell(panel, "Descripcion:", 4, 0, true);
        AddTableCell(panel, FormatValue(registration.TipoVehiculo), 4, 1, false);

        AddPhotoCell(panel, GetPhoto(photos, 0), imageCache, 2, 2);
        AddPhotoCell(panel, GetPhoto(photos, 1), imageCache, 3, 2);

        return panel;
    }

    private static FrameworkElement BuildDriverPanel(
        SecurityRegistrationDto registration,
        IReadOnlyDictionary<string, ImageSource?> imageCache)
    {
        var panel = new Grid
        {
            Width = ContentWidth,
            Height = 126
        };

        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(LabelWidth) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(DetailWidth - LabelWidth) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ContentWidth - DetailWidth) });
        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(126) });

        AddTableCell(panel, "Conductor:", 0, 0, true, VerticalAlignment.Top);
        AddTableCell(panel, FormatValue(registration.Nombre), 0, 1, false, VerticalAlignment.Top);
        AddPhotoCell(
            panel,
            new PhotoCard(registration.Firma?.FilePath),
            imageCache,
            0,
            2);

        return panel;
    }

    private static FrameworkElement BuildLicenseAndBoxPage(
        SecurityRegistrationDto registration,
        IReadOnlyDictionary<string, ImageSource?> imageCache)
    {
        var body = new StackPanel
        {
            Width = ContentWidth,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        var licensePanel = new Grid
        {
            Width = ContentWidth,
            Height = 146
        };
        licensePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(LabelWidth) });
        licensePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(DetailWidth - LabelWidth) });
        licensePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ContentWidth - DetailWidth) });
        licensePanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(146) });

        AddTableCell(licensePanel, "Licencia:", 0, 0, true, VerticalAlignment.Top);
        AddTableCell(licensePanel, FormatValue(registration.Licencia), 0, 1, false, VerticalAlignment.Top);
        AddPhotoCell(
            licensePanel,
            GetLicensePhoto(registration),
            imageCache,
            0,
            2);
        body.Children.Add(licensePanel);

        var boxPanel = BuildBoxPanel(registration, GetRegistrationVehiclePhotos(registration), imageCache);
        boxPanel.Margin = new Thickness(0, 22, 0, 0);
        body.Children.Add(boxPanel);

        var additional = BuildFieldTable(
        [
            ("Celular:", FormatValue(registration.Celular)),
            ("Vencimiento:", FormatDate(registration.Vencimiento)),
            ("Estado Registro:", FormatStatus(registration.Estado))
        ],
        DetailWidth,
        24);
        additional.Margin = new Thickness(0, 22, 0, 0);
        body.Children.Add(additional);

        return body;
    }

    private static FrameworkElement BuildBoxPanel(
        SecurityRegistrationDto registration,
        IReadOnlyList<PhotoCard> photos,
        IReadOnlyDictionary<string, ImageSource?> imageCache)
    {
        var panel = new Grid
        {
            Width = ContentWidth,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(LabelWidth) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(DetailWidth - LabelWidth) });
        panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ContentWidth - DetailWidth) });

        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(24) });
        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(24) });
        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(105) });
        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(105) });
        panel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(105) });

        AddTableCell(panel, "ID Caja:", 0, 0, true);
        AddTableCell(panel, FormatValue(registration.NumeroCaja), 0, 1, false);
        AddTableCell(panel, "Descripcion:", 1, 0, true);
        AddTableCell(panel, registration.TieneCaja ? "CONTENEDOR / CAJA" : "SIN CAJA", 1, 1, false);
        AddTableCell(panel, "Numero:", 2, 0, true, VerticalAlignment.Top);
        AddTableCell(panel, FormatValue(registration.NumeroCaja), 2, 1, false, VerticalAlignment.Top);
        AddTableCell(panel, "Placa:", 3, 0, true, VerticalAlignment.Top);
        AddTableCell(panel, FormatValue(registration.PlacaCaja), 3, 1, false, VerticalAlignment.Top);
        AddTableCell(panel, "Sello Ingreso:", 4, 0, true, VerticalAlignment.Top);
        AddTableCell(panel, FormatValue(registration.Sello), 4, 1, false, VerticalAlignment.Top);

        AddPhotoCell(panel, GetPhoto(photos, 2), imageCache, 2, 2);
        AddPhotoCell(panel, GetPhoto(photos, 3), imageCache, 3, 2);
        AddPhotoCell(panel, GetPhoto(photos, 4), imageCache, 4, 2);

        return panel;
    }

    private static FrameworkElement BuildMovementPage(
        SecurityRegistrationDto registration,
        SecurityTaskDto task,
        IReadOnlyDictionary<string, ImageSource?> imageCache)
    {
        var body = new StackPanel
        {
            Width = ContentWidth,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        body.Children.Add(BuildSectionTitle(FormatTaskTitle(task.TipoAccion)));

        var role = IsSecurityTask(task.TipoAccion)
            ? "Seguridad"
            : "Operaciones";

        body.Children.Add(BuildFieldTable(
        [
            ("Usuario Asignacion:", FormatValue(task.RealizadaPor)),
            ("Hora Asignacion:", FormatDateTime(task.FechaIniciada)),
            ($"Usuario {role}:", FormatValue(task.RealizadaPor)),
            ($"Hora {role}:", FormatDateTime(task.FechaCompletada)),
            ($"Obs. {role}:", "SIN OBSERVACIONES"),
            ("Cortina:", FormatValue(task.CortinaNumero ?? registration.CortinaNumero)),
            ("Estado:", task.Completada ? "COMPLETADO" : "PENDIENTE")
        ],
        DetailWidth,
        24));

        var evidence = BuildEvidenceGrid(
            GetTaskPhotos(registration, task),
            imageCache);
        evidence.Margin = new Thickness(0, 18, 0, 0);
        body.Children.Add(evidence);

        return body;
    }

    private static FrameworkElement BuildSectionTitle(string title)
    {
        return new Border
        {
            Width = ContentWidth,
            Height = 34,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1.5),
            Child = BuildCenteredText(title, 16, FontWeights.Bold)
        };
    }

    private static FrameworkElement BuildFieldTable(
        IReadOnlyList<(string Label, string Value)> rows,
        double width,
        double rowHeight)
    {
        var table = new Grid
        {
            Width = width,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(LabelWidth) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width - LabelWidth) });

        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            table.RowDefinitions.Add(new RowDefinition { Height = new GridLength(rowHeight) });
            AddTableCell(table, rows[rowIndex].Label, rowIndex, 0, true);
            AddTableCell(table, rows[rowIndex].Value, rowIndex, 1, false);
        }

        return table;
    }

    private static FrameworkElement BuildEvidenceGrid(
        IReadOnlyList<PhotoCard> photos,
        IReadOnlyDictionary<string, ImageSource?> imageCache)
    {
        if (photos.Count == 0)
        {
            return new Border
            {
                Width = ContentWidth,
                Height = 300,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Child = BuildMutedCenteredText("SIN EVIDENCIA FOTOGRAFICA")
            };
        }

        var displayedPhotos = photos.Take(4).ToList();
        var rows = displayedPhotos.Count <= 2 ? 1 : 2;
        var grid = new UniformGrid
        {
            Width = ContentWidth,
            Height = rows == 1 ? 330 : 440,
            Columns = 2,
            Rows = rows
        };

        foreach (var photo in displayedPhotos)
        {
            grid.Children.Add(BuildPhotoBorder(photo, imageCache));
        }

        while (grid.Children.Count < rows * 2)
        {
            grid.Children.Add(BuildPhotoBorder(new PhotoCard(null), imageCache));
        }

        return grid;
    }

    private static FrameworkElement BuildPhotoBorder(
        PhotoCard photo,
        IReadOnlyDictionary<string, ImageSource?> imageCache)
    {
        var border = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0.8),
            Background = Brushes.White,
            ClipToBounds = true
        };

        var source = LoadImage(photo.Path, imageCache);
        border.Child = source is null
            ? BuildMutedCenteredText("SIN FOTO")
            : new Image
            {
                Source = source,
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

        return border;
    }

    private static void AddPhotoCell(
        Grid grid,
        PhotoCard photo,
        IReadOnlyDictionary<string, ImageSource?> imageCache,
        int row,
        int column)
    {
        var border = BuildPhotoBorder(photo, imageCache);
        Grid.SetRow(border, row);
        Grid.SetColumn(border, column);
        grid.Children.Add(border);
    }

    private static FrameworkElement BuildFooter(DateTime printedAt, int pageNumber, int totalPages)
    {
        var footer = new Grid
        {
            Margin = new Thickness(40, 8, 40, 0)
        };

        footer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        footer.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        var left = Text(
            $"Impresion: {printedAt:dd/MM/yyyy hh:mm tt}",
            9.5);
        footer.Children.Add(left);

        var right = Text(
            $"Pagina {pageNumber} de {totalPages}",
            9.5,
            FontWeights.Normal,
            TextAlignment.Right);
        Grid.SetColumn(right, 1);
        footer.Children.Add(right);

        return footer;
    }

    private static void AddTableCell(
        Grid grid,
        string value,
        int row,
        int column,
        bool isLabel,
        VerticalAlignment verticalAlignment = VerticalAlignment.Center)
    {
        var text = Text(
            value,
            12,
            isLabel ? FontWeights.Bold : FontWeights.Normal);
        text.VerticalAlignment = verticalAlignment;

        var border = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0.8),
            Padding = new Thickness(3, 1, 3, 1),
            Child = text
        };

        Grid.SetRow(border, row);
        Grid.SetColumn(border, column);
        grid.Children.Add(border);
    }

    private static IReadOnlyList<PhotoCard> GetRegistrationVehiclePhotos(SecurityRegistrationDto registration)
    {
        return registration.Fotos
            .Where(photo => photo.SecurityTaskId is null && photo.Categoria == PhotoCategoria_e.Vehiculo)
            .OrderBy(photo => photo.Orden)
            .Select(photo => new PhotoCard(photo.FilePath))
            .ToList();
    }

    private static PhotoCard GetLicensePhoto(SecurityRegistrationDto registration)
    {
        var path = registration.Fotos
            .Where(photo => photo.SecurityTaskId is null && photo.Categoria == PhotoCategoria_e.Licencia)
            .OrderBy(photo => photo.Orden)
            .Select(photo => photo.FilePath)
            .FirstOrDefault();

        return new PhotoCard(path);
    }

    private static IReadOnlyList<PhotoCard> GetTaskPhotos(
        SecurityRegistrationDto registration,
        SecurityTaskDto task)
    {
        return registration.Fotos
            .Where(photo => photo.SecurityTaskId == task.SecurityTaskId)
            .OrderBy(photo => photo.Orden)
            .Select(photo => new PhotoCard(photo.FilePath))
            .ToList();
    }

    private static PhotoCard GetPhoto(IReadOnlyList<PhotoCard> photos, int index)
    {
        return index >= 0 && index < photos.Count
            ? photos[index]
            : new PhotoCard(null);
    }

    private static IEnumerable<string> CollectPhotoPaths(SecurityRegistrationDto registration)
    {
        var paths = registration.Fotos
            .Select(photo => photo.FilePath)
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Cast<string>()
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (!string.IsNullOrWhiteSpace(registration.Firma?.FilePath))
        {
            paths.Add(registration.Firma.FilePath);
        }

        return paths;
    }

    private static async Task<IReadOnlyDictionary<string, ImageSource?>> LoadImageCacheAsync(
        IEnumerable<string> paths,
        Func<string?, Task<byte[]>> imageBytesProvider)
    {
        var cache = new Dictionary<string, ImageSource?>(StringComparer.OrdinalIgnoreCase);

        foreach (var path in paths.OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
        {
            cache[path] = await LoadImageAsync(path, imageBytesProvider);
        }

        return cache;
    }

    private static async Task<ImageSource?> LoadImageAsync(
        string? path,
        Func<string?, Task<byte[]>> imageBytesProvider)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        try
        {
            var bytes = await imageBytesProvider(path);
            if (bytes.Length == 0)
            {
                return null;
            }

            using var stream = new MemoryStream(bytes);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            return image;
        }
        catch
        {
            return null;
        }
    }

    private static ImageSource? LoadImage(
        string? path,
        IReadOnlyDictionary<string, ImageSource?> cache)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        return cache.TryGetValue(path, out var image)
            ? image
            : null;
    }

    private static ImageSource? LoadLogo()
    {
        try
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.UriSource = new Uri(
                "pack://application:,,,/Resources/Images/logo.png");
            image.EndInit();
            image.Freeze();
            return image;
        }
        catch
        {
            return null;
        }
    }

    private static bool IsSecurityTask(string? taskType)
    {
        return taskType is "AbrirCortina" or "CerrarRegistro";
    }

    private static string FormatValue(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? "-"
            : value.Trim().ToUpperInvariant();
    }

    private static string FormatDate(DateTime value)
    {
        return value == default
            ? "-"
            : value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
    }

    private static string FormatDateTime(DateTime value)
    {
        return value == default
            ? "-"
            : value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
    }

    private static string FormatDateTime(DateTime? value)
    {
        return value.HasValue
            ? FormatDateTime(value.Value)
            : "-";
    }

    private static string FormatStatus(RegistroEstado_e value)
    {
        return value switch
        {
            RegistroEstado_e.Registrado => "REGISTRADO",
            RegistroEstado_e.CortinaAsignada => "CORTINA ASIGNADA",
            RegistroEstado_e.PendienteDeCerrar => "PENDIENTE DE CERRAR",
            RegistroEstado_e.Cerrado => "CERRADO",
            _ => value.ToString().ToUpperInvariant()
        };
    }

    private static string FormatTaskTitle(string? value)
    {
        return value switch
        {
            "AbrirCortina" => "Apertura de Cortina",
            "CerrarRegistro" => "Orden de Salida / Cierre de Registro",
            "IniciarOperacion" => "Inicio de Operacion",
            "ComenzarOperacion" => "Inicio de Operacion",
            "FinalizarOperacion" => "Fin de Operacion",
            "TerminarOperacion" => "Fin de Operacion",
            _ => FormatValue(value)
        };
    }

    private static TextBlock Text(
        string text,
        double size,
        FontWeight? weight = null,
        TextAlignment alignment = TextAlignment.Left)
    {
        return new TextBlock
        {
            Text = text,
            FontFamily = Arial,
            FontSize = size,
            FontWeight = weight ?? FontWeights.Normal,
            TextAlignment = alignment,
            TextWrapping = TextWrapping.Wrap
        };
    }

    private static TextBlock BuildCenteredText(
        string text,
        double size,
        FontWeight weight)
    {
        var block = Text(text, size, weight, TextAlignment.Center);
        block.HorizontalAlignment = HorizontalAlignment.Center;
        block.VerticalAlignment = VerticalAlignment.Center;
        return block;
    }

    private static TextBlock BuildMutedCenteredText(string text)
    {
        var block = BuildCenteredText(text, 10, FontWeights.Normal);
        block.Foreground = MutedText;
        return block;
    }

    private static void AddToGrid(Grid grid, UIElement element, int row)
    {
        Grid.SetRow(element, row);
        grid.Children.Add(element);
    }

    private static void AddPage(FixedDocument document, FixedPage page)
    {
        page.Measure(new Size(PageWidth, PageHeight));
        page.Arrange(new Rect(0, 0, PageWidth, PageHeight));
        page.UpdateLayout();

        var pageContent = new PageContent();
        ((IAddChild)pageContent).AddChild(page);
        document.Pages.Add(pageContent);
    }
}
