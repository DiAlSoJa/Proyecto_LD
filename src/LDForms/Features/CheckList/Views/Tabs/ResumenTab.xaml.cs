using LD.FormsX.Features.CheckList.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.CheckList.Tabs
{
    public partial class ResumenTab : UserControl
    {
        private ResumenTabViewModel ViewModel => (ResumenTabViewModel)DataContext;

        public ResumenTab(ResumenTabViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        // Pendiente: implementar cuando exista el endpoint de checklists enviados.
        private void BtnBuscar_Click(object sender, RoutedEventArgs e) { }
        private void dgResumen_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void BtnVerImagenes_Click(object sender, RoutedEventArgs e) { }
    }
}
