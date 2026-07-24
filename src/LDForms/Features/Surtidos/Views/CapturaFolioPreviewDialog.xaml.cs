using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using LD.Contracts.DTOs.KittingFolioCapture;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Surtidos.Views;

public partial class CapturaFolioPreviewDialog : Window, INotifyPropertyChanged
{
    private string _sourceFileName = string.Empty;
    private string _guideNumber = string.Empty;
    private string _invoiceNumber = string.Empty;
    private string _summaryMessage = string.Empty;
    private string _warningMessage = string.Empty;
    private string _invalidWarningMessage = string.Empty;
    private string _rowsSummary = string.Empty;
    private bool _canAccept;
    private bool _hasInvalidRows;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<KittingFolioCapturePreviewRowDto> PreviewRows { get; } = new();

    public string SourceFileName
    {
        get => _sourceFileName;
        private set => SetField(ref _sourceFileName, value);
    }

    public string GuideNumber
    {
        get => _guideNumber;
        private set => SetField(ref _guideNumber, value);
    }

    public string InvoiceNumber
    {
        get => _invoiceNumber;
        private set => SetField(ref _invoiceNumber, value);
    }

    public string SummaryMessage
    {
        get => _summaryMessage;
        private set => SetField(ref _summaryMessage, value);
    }

    public string WarningMessage
    {
        get => _warningMessage;
        private set => SetField(ref _warningMessage, value);
    }

    public string InvalidWarningMessage
    {
        get => _invalidWarningMessage;
        private set => SetField(ref _invalidWarningMessage, value);
    }

    public string RowsSummary
    {
        get => _rowsSummary;
        private set => SetField(ref _rowsSummary, value);
    }

    public bool CanAccept
    {
        get => _canAccept;
        private set => SetField(ref _canAccept, value);
    }

    public bool HasInvalidRows
    {
        get => _hasInvalidRows;
        private set => SetField(ref _hasInvalidRows, value);
    }

    public CapturaFolioPreviewDialog()
    {
        InitializeComponent();
        DataContext = this;
    }

    public void SetPreview(KittingFolioCapturePreviewDto preview)
    {
        SourceFileName = preview.SourceFileName;
        GuideNumber = preview.GuideNumber;
        InvoiceNumber = preview.InvoiceNumber;
        RowsSummary = $"Total: {preview.TotalRows} | Validos: {preview.ValidRows} | Con error: {preview.InvalidRows}";
        if (preview.ValidRows > 0)
        {
            SummaryMessage = preview.InvalidRows > 0
                ? $"Estas seguro de cargar {preview.ValidRows} registro(s)? {preview.InvalidRows} registro(s) con error no se cargaran."
                : $"Estas seguro de cargar {preview.ValidRows} registro(s)?";
            WarningMessage = preview.InvalidRows > 0
                ? "Los registros marcados en rojo no se cargaran."
                : "Todos los registros estan listos para cargarse.";
            InvalidWarningMessage = preview.InvalidRows > 0
                ? "Revisa los motivos antes de confirmar."
                : string.Empty;
        }
        else
        {
            SummaryMessage = "No hay registros validos para cargar.";
            WarningMessage = "Todos los registros tienen errores y no se cargara ninguno.";
            InvalidWarningMessage = "Corrige los errores marcados en rojo antes de continuar.";
        }
        CanAccept = preview.ValidRows > 0;
        HasInvalidRows = preview.InvalidRows > 0;

        PreviewRows.Clear();
        foreach (var row in preview.Rows)
            PreviewRows.Add(row);
    }

    private void BtnAccept_Click(object sender, RoutedEventArgs e)
    {
        if (!CanAccept)
        {
            DialogHelper.ShowWarning("No hay registros validos para cargar.");
            return;
        }

        DialogResult = true;
        Close();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
            DragMove();
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
