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

namespace LD.FormsX.Views.ASN
{
    /// <summary>
    /// Lógica de interacción para ASNView.xaml
    /// </summary>
    public partial class ASNView : UserControl
    {
        public ASNView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e) { }
        private void BtnActualizar_Click(object sender, RoutedEventArgs e) { }
        private void BtnNuevoASN_Click(object sender, RoutedEventArgs e) { }
        private void BtnRegistrarArribo_Click(object sender, RoutedEventArgs e) { }
        private void BtnEscanear_Click(object sender, RoutedEventArgs e) { }
        private void dgASN_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void dgDetalleASN_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
