using LD.FormsX.Features.Common;
using LD.FormsX.Features.Reportes.ViewModels;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Reportes;

public partial class ReportQueryEditorView : Window
{
    private ReportQueryEditorViewModel ViewModel => (ReportQueryEditorViewModel)DataContext;

    public ReportQueryEditorView(ReportQueryEditorViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        viewModel.RequestClose = () =>
        {
            DialogResult = true;
            Close();
        };
    }

    public void SetQuery(LD.Contracts.DTOs.ReportQueries.ReportQueryDto? query)
    {
        ViewModel.SetQuery(query);
    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            btnSave.IsEnabled = false;
            await ViewModel.SaveAsync();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            btnSave.IsEnabled = true;
        }
    }

    private void BtnCerrar_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
            DragMove();
    }
}
