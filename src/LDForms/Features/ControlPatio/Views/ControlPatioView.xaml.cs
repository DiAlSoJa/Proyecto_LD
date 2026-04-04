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

namespace LD.FormsX.Views.ControlPatio
{
    /// <summary>
    /// Lógica de interacción para ControlPatioView.xaml
    /// </summary>
    public partial class ControlPatioView : UserControl
    {
        public ControlPatioView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e) { }
        private void BtnActualizar_Click(object sender, RoutedEventArgs e) { }

        private void dgPendientesOperaciones_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void dgPendientesTransportes_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void dgOperaciones_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void dgTransportes_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void BtnEditarOperacion_Click(object sender, RoutedEventArgs e) { }
        private void BtnCancelarOperacion_Click(object sender, RoutedEventArgs e) { }

        private void BtnEditarTransporte_Click(object sender, RoutedEventArgs e) { }
        private void BtnCancelarTransporte_Click(object sender, RoutedEventArgs e) { }
    }
}
