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

namespace LD.FormsX.Views.InventarioAleatorio
{
    /// <summary>
    /// Lógica de interacción para InventarioCiclicoView.xaml
    /// </summary>
    public partial class InventarioCiclicoView : UserControl
    {
        public InventarioCiclicoView()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e) { }
        private void BtnActualizar_Click(object sender, RoutedEventArgs e) { }
        private void BtnNuevoInventario_Click(object sender, RoutedEventArgs e) { }
        private void BtnEditar_Click(object sender, RoutedEventArgs e) { }
        private void dgFechas_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void dgAuditores_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
    }
}
