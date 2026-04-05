using LD.Contracts.Location;
using LD.FormsX.Features.Ubicaciones.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Ubicaciones
{
    public partial class NuevaUbicacionView : Window
    {
        private NuevaUbicacionViewModel ViewModel => (NuevaUbicacionViewModel)DataContext;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevaUbicacionView(NuevaUbicacionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetLocation(LocationDto location)
        {
            ViewModel.SetLocation(location);
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