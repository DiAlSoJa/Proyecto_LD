using LD.Contracts.DTOs.ReportQueries;
using LD.FormsX.Features.Reportes.ViewModels;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Reportes;

public partial class ReportQueryParametersView : Window
{
    private ReportQueryParametersViewModel ViewModel => (ReportQueryParametersViewModel)DataContext;

    public Dictionary<string, string?> ResultParameters => ViewModel.ResultParameters;

    public ReportQueryParametersView(ReportQueryParametersViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        viewModel.RequestClose = () =>
        {
            DialogResult = true;
            Close();
        };
    }

    public void SetReportQuery(ReportQueryDto query)
    {
        ViewModel.SetReportQuery(query);
    }

    public void SetParameters(IEnumerable<ReportQueryParameterDto> parameters)
    {
        ViewModel.SetParameters(parameters);
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            await ViewModel.LoadAsync();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    private void BtnEjecutar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ViewModel.Confirm();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
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
