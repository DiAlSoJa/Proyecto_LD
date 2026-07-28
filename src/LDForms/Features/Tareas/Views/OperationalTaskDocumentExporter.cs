using LD.Contracts.DTOs.OperationalTasks;
using LD.FormsX.Features.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LD.FormsX.Views.Tareas;

internal static class OperationalTaskDocumentExporter
{
    private const double Dpi = 96d;
    private const double PageWidth = 8.5d * Dpi;
    private const double PageHeight = 11d * Dpi;
    private const double MarginSize = 32d;
    private const double ContentWidth = PageWidth - (MarginSize * 2);

    private static readonly FontFamily Arial = new("Arial");
    private static readonly FontFamily Segoe = new("Segoe UI");

    private sealed record TaskPhoto(string Title, string? Path);

    public static void Preview(OperationalTaskDto task, Func<string?, string?> imageUrlFactory)
    {
        var document = BuildDocument(task, imageUrlFactory);
        var preview = new PrintPreviewWindow(document, $"Vista previa - Tarea {task.OperationalTaskId}", $"Reporte de tarea {task.OperationalTaskId}");

        WindowOwnerHelper.AttachOwnerOrCenter(preview, Application.Current?.MainWindow);
        preview.ShowDialog();
    }

    private static FixedDocument BuildDocument(OperationalTaskDto task, Func<string?, string?> imageUrlFactory)
    {
        var document = new FixedDocument
        {
            DocumentPaginator = { PageSize = new Size(PageWidth, PageHeight) }
        };

        var printedAt = DateTime.Now;
        AddPage(document, BuildMainPage(task, printedAt, imageUrlFactory));

        if (HasResolutionContent(task))
            AddPage(document, BuildResolutionPage(task, printedAt, imageUrlFactory));

        return document;
    }

    private static FixedPage BuildMainPage(
        OperationalTaskDto task,
        DateTime printedAt,
        Func<string?, string?> imageUrlFactory)
    {
        return BuildPage(
            "REPORTE DE TAREA",
            task,
            printedAt,
            BuildGeneralRows(task),
            BuildInitialPhotos(task),
            "Evidencia inicial",
            imageUrlFactory);
    }

    private static FixedPage BuildResolutionPage(
        OperationalTaskDto task,
        DateTime printedAt,
        Func<string?, string?> imageUrlFactory)
    {
        return BuildPage(
            "RESOLUCION DE TAREA",
            task,
            printedAt,
            BuildResolutionRows(task),
            BuildResolvedPhotos(task),
            "Fotos de resolucion",
            imageUrlFactory);
    }

    private static FixedPage BuildPage(
        string title,
        OperationalTaskDto task,
        DateTime printedAt,
        IReadOnlyList<(string Label, string Value)> rows,
        IReadOnlyList<TaskPhoto> photos,
        string photoSectionTitle,
        Func<string?, string?> imageUrlFactory)
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
            Margin = new Thickness(MarginSize)
        };

        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(14) });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        AddToGrid(root, BuildHeader(title, task, printedAt), 0);
        AddToGrid(root, new Border
        {
            Height = 2.5,
            Background = Brushes.Black,
            Margin = new Thickness(0, 2, 0, 0)
        }, 1);
        AddToGrid(root, BuildFieldTable(rows), 2);
        AddToGrid(root, BuildPhotoSection(photoSectionTitle, photos, imageUrlFactory), 4);
        AddToGrid(root, BuildFooter(printedAt), 5);

        page.Children.Add(root);
        return page;
    }

    private static FrameworkElement BuildHeader(string title, OperationalTaskDto task, DateTime printedAt)
    {
        var header = new Grid
        {
            Margin = new Thickness(0, 0, 0, 4)
        };

        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(106) });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        header.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(180) });

        var logo = new Image
        {
            Width = 92,
            Height = 56,
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Source = LoadLogo()
        };
        Grid.SetColumn(logo, 0);
        header.Children.Add(logo);

        var center = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top
        };
        center.Children.Add(Text("LOGISTICA FLEXIBLE", 14, FontWeights.Bold, TextAlignment.Center, Arial));
        center.Children.Add(new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1.5),
            Margin = new Thickness(14, 6, 14, 0),
            Padding = new Thickness(8, 5, 8, 5),
            Child = Text(title, 16, FontWeights.Black, TextAlignment.Center, Arial)
        });
        Grid.SetColumn(center, 1);
        header.Children.Add(center);

        var right = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Top
        };

        right.Children.Add(Text($"Folio: {task.OperationalTaskId}", 10.5, FontWeights.Bold, TextAlignment.Right, Segoe));
        right.Children.Add(Text($"Usuario: {FormatValue(task.CreatedByUserName)}", 10.5, FontWeights.Normal, TextAlignment.Right, Segoe));
        right.Children.Add(Text($"Generado: {printedAt:dd/MM/yyyy HH:mm}", 10.5, FontWeights.Normal, TextAlignment.Right, Segoe));
        Grid.SetColumn(right, 2);
        header.Children.Add(right);

        return header;
    }

    private static FrameworkElement BuildFieldTable(IReadOnlyList<(string Label, string Value)> rows)
    {
        var table = new Grid
        {
            Width = ContentWidth,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
        table.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(ContentWidth - 160) });

        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            table.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            AddCell(table, rows[rowIndex].Label, rowIndex, 0, true);
            AddCell(table, rows[rowIndex].Value, rowIndex, 1, false);
        }

        return table;
    }

    private static FrameworkElement BuildPhotoSection(
        string title,
        IReadOnlyList<TaskPhoto> photos,
        Func<string?, string?> imageUrlFactory)
    {
        var section = new StackPanel
        {
            Width = ContentWidth,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 10, 0, 0)
        };

        section.Children.Add(Text(title, 13.5, FontWeights.Bold, TextAlignment.Left, Arial));

        var frame = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            Margin = new Thickness(0, 8, 0, 0),
            Padding = new Thickness(8),
            Width = ContentWidth,
            Height = 340
        };

        var grid = new UniformGrid
        {
            Columns = 2,
            Rows = 2
        };

        foreach (var photo in photos)
            grid.Children.Add(BuildPhotoTile(photo, imageUrlFactory));

        frame.Child = grid;
        section.Children.Add(frame);

        return section;
    }

    private static FrameworkElement BuildPhotoTile(TaskPhoto photo, Func<string?, string?> imageUrlFactory)
    {
        var imageSource = LoadImage(photo.Path, imageUrlFactory);

        var tile = new Border
        {
            Margin = new Thickness(6),
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(4),
            Background = Brushes.White,
            ClipToBounds = true
        };

        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        var titleBlock = new Border
        {
            Background = new SolidColorBrush(Color.FromRgb(241, 245, 249)),
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0, 0, 0, 1),
            Padding = new Thickness(6, 4, 6, 4),
            Child = Text(photo.Title, 10.5, FontWeights.Bold, TextAlignment.Left, Segoe)
        };
        Grid.SetRow(titleBlock, 0);
        grid.Children.Add(titleBlock);

        var imageBorder = new Border
        {
            Background = Brushes.WhiteSmoke,
            Margin = new Thickness(6),
            BorderBrush = Brushes.LightGray,
            BorderThickness = new Thickness(1),
            ClipToBounds = true
        };

        if (imageSource is not null)
        {
            imageBorder.Child = new Image
            {
                Source = imageSource,
                Stretch = Stretch.Uniform,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
        }
        else
        {
            imageBorder.Child = Text("Sin foto", 11, FontWeights.Normal, TextAlignment.Center, Segoe);
        }

        Grid.SetRow(imageBorder, 1);
        grid.Children.Add(imageBorder);

        tile.Child = grid;
        return tile;
    }

    private static FrameworkElement BuildFooter(DateTime printedAt)
    {
        var footer = new StackPanel
        {
            Margin = new Thickness(0, 10, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Right
        };

        footer.Children.Add(Text($"Generado el {printedAt:dd/MM/yyyy HH:mm}", 9.5, FontWeights.Normal, TextAlignment.Right, Segoe));
        return footer;
    }

    private static IReadOnlyList<(string Label, string Value)> BuildGeneralRows(OperationalTaskDto task) =>
    [
        ("Folio", FormatValue(task.OperationalTaskId.ToString(CultureInfo.InvariantCulture))),
        ("Almacen", FormatValue(task.WarehouseName)),
        ("Prioridad", FormatValue(task.Priority)),
        ("Actividad", FormatValue(task.Activity)),
        ("Nombre", FormatValue(task.Name)),
        ("Descripcion", FormatValue(task.Description)),
        ("Creado el", FormatDateTime(task.CreatedAt)),
        ("Estatus", task.Completed ? "Finalizada" : "Pendiente")
    ];

    private static IReadOnlyList<(string Label, string Value)> BuildResolutionRows(OperationalTaskDto task) =>
    [
        ("Folio", FormatValue(task.OperationalTaskId.ToString(CultureInfo.InvariantCulture))),
        ("Finalizado", task.Completed ? "Si" : "No"),
        ("Finalizo", FormatValue(task.CompletedByName)),
        ("Fecha finalizacion", FormatDateTime(task.CompletedAt)),
        ("Observaciones", FormatValue(task.ResolutionObservations)),
        ("Estatus", task.Completed ? "Finalizada" : "Pendiente")
    ];

    private static IReadOnlyList<TaskPhoto> BuildInitialPhotos(OperationalTaskDto task) =>
    [
        new("Foto inicial 1", task.Photo1Path),
        new("Foto inicial 2", task.Photo2Path),
        new("Foto inicial 3", task.Photo3Path),
        new("Foto inicial 4", task.Photo4Path)
    ];

    private static IReadOnlyList<TaskPhoto> BuildResolvedPhotos(OperationalTaskDto task) =>
    [
        new("Foto final 1", task.ResolvedPhoto1Path),
        new("Foto final 2", task.ResolvedPhoto2Path),
        new("Foto final 3", task.ResolvedPhoto3Path),
        new("Foto final 4", task.ResolvedPhoto4Path)
    ];

    private static bool HasResolutionContent(OperationalTaskDto task)
    {
        return task.Completed
            || task.CompletedAt.HasValue
            || !string.IsNullOrWhiteSpace(task.CompletedByName)
            || !string.IsNullOrWhiteSpace(task.ResolutionObservations)
            || BuildResolvedPhotos(task).Any(photo => !string.IsNullOrWhiteSpace(photo.Path));
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

    private static void AddToGrid(Grid grid, UIElement element, int row)
    {
        Grid.SetRow(element, row);
        grid.Children.Add(element);
    }

    private static void AddCell(Grid table, string value, int row, int column, bool isLabel)
    {
        var border = new Border
        {
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(0.8),
            Padding = new Thickness(6, 4, 6, 4),
            Child = Text(value, 11, isLabel ? FontWeights.Bold : FontWeights.Normal, TextAlignment.Left, Segoe)
        };

        Grid.SetRow(border, row);
        Grid.SetColumn(border, column);
        table.Children.Add(border);
    }

    private static TextBlock Text(string text, double size, FontWeight weight, TextAlignment alignment, FontFamily family)
    {
        return new TextBlock
        {
            Text = text,
            FontSize = size,
            FontWeight = weight,
            TextAlignment = alignment,
            TextWrapping = TextWrapping.Wrap,
            FontFamily = family
        };
    }

    private static string FormatValue(string? value) => string.IsNullOrWhiteSpace(value) ? "-" : value.Trim();

    private static string FormatDateTime(DateTime? value) => value?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) ?? "-";

    private static ImageSource? LoadLogo()
    {
        try
        {
            return new BitmapImage(new Uri("pack://application:,,,/Resources/Images/logo.png"));
        }
        catch
        {
            return null;
        }
    }

    private static ImageSource? LoadImage(string? path, Func<string?, string?> imageUrlFactory)
    {
        if (string.IsNullOrWhiteSpace(path))
            return null;

        var imageUrl = imageUrlFactory(path);
        if (string.IsNullOrWhiteSpace(imageUrl))
            return null;

        try
        {
            return new BitmapImage(new Uri(imageUrl, UriKind.RelativeOrAbsolute));
        }
        catch
        {
            return null;
        }
    }
}
