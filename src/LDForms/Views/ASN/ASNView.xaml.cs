using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LD.Client.Services;
using LD.Contracts.Location;
using LD.FormsX.Helpers;
using LD.FormsX.Views.Dialogs;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.ASN
{
    /// <summary>
    /// Lógica de interacción para ASNView.xaml
    /// </summary>
    public partial class ASNView : UserControl
    {
        private readonly LocationService _service;
        private readonly IServiceProvider _serviceProvider;

        private readonly WpfGridFilter<LocationDto> _gridFilter;

        private LocationDto? _selectedX;
        private bool _loaded;
        public ASNView(LocationService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e) { }
        private void BtnActualizar_Click(object sender, RoutedEventArgs e) { }
        private void BtnNuevoASN_Click(object sender, RoutedEventArgs e) {
            var dialog = _serviceProvider.GetRequiredService<NuevoASNView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();
            /*
            if (result == true)
            {
                await CargarDatosAsync();
            }*/
        }
        private void BtnRegistrarArribo_Click(object sender, RoutedEventArgs e) { }
        private void BtnEscanear_Click(object sender, RoutedEventArgs e) { }
        private void dgASN_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void dgDetalleASN_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

    }
}
