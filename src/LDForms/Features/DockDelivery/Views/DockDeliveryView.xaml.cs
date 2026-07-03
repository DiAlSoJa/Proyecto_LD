using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LDForms.Features.DockDelivery.Views;

public partial class DockDeliveryView : UserControl
{
    private readonly IServiceProvider _serviceProvider;

    public DockDeliveryView(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void BtnVehiculos_Click(object sender, RoutedEventArgs e)
    {
        var window = _serviceProvider.GetRequiredService<VehiculosDockDeliveryView>();
        LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(
            window,
            LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        window.ShowDialog();
    }

    private void BtnChoferes_Click(object sender, RoutedEventArgs e)
    {
        var window = _serviceProvider.GetRequiredService<ChoferesDockDeliveryView>();
        LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(
            window,
            LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
        window.ShowDialog();
    }
}
