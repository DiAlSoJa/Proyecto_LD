using LD.FormsX.Features.CheckList.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LD.FormsX.Views.CheckList.Tabs
{
    public partial class ResumenBateriasTab : UserControl
    {
        private ResumenBateriasTabViewModel ViewModel => (ResumenBateriasTabViewModel)DataContext;

        public ResumenBateriasTab(ResumenBateriasTabViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        // Pendiente: implementar cuando exista el endpoint de checklists de baterías.
        private void BtnBuscar_Click(object sender, RoutedEventArgs e) { }
        private void dgBaterias_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
    }
}
