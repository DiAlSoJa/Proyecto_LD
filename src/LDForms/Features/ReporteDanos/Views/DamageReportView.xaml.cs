using LD.FormsX.Features.ReporteDanos.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.ReporteDanos;

public partial class DamageReportView : UserControl
{
    private bool _loaded;
    private DamageReportViewModel ViewModel => (DamageReportViewModel)DataContext;

    public DamageReportView(DamageReportViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded) return;
        _loaded = true;

        await ViewModel.InicializarAsync();
    }
}
