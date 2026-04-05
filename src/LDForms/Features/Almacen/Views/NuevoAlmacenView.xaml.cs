using LD.Contracts.Warehouse;
using LD.FormsX.Features.Almacen.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Almacen
{
    public partial class NuevoAlmacenView : Window
    {
        private NuevoAlmacenViewModel ViewModel => (NuevoAlmacenViewModel)DataContext;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevoAlmacenView(NuevoAlmacenViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetWarehouse(WarehouseDto warehouse)
        {
            ViewModel.SetWarehouse(warehouse);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAsync();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e) => Close();

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}