using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LD.FormsX.Features.Common;

public partial class PrintPreviewWindow : Window
{
    private readonly IDocumentPaginatorSource _document;
    private readonly string _jobName;

    public PrintPreviewWindow(IDocumentPaginatorSource document, string title, string jobName)
    {
        InitializeComponent();

        _document = document ?? throw new ArgumentNullException(nameof(document));
        _jobName = string.IsNullOrWhiteSpace(jobName) ? title : jobName;

        Title = title;
        txtTitle.Text = title;
        documentViewer.Document = document;
    }

    private void BtnPrint_Click(object sender, RoutedEventArgs e)
    {
        var printDialog = new PrintDialog();
        if (printDialog.ShowDialog() != true)
            return;

        printDialog.PrintDocument(_document.DocumentPaginator, _jobName);
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
