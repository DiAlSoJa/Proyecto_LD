using LD.Contracts.Client;
using LD.FormsX.Features.Clientes.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Dialogs
{
    public partial class NuevoClienteView : Window
    {
        private NuevoClienteViewModel ViewModel => (NuevoClienteViewModel)DataContext;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevoClienteView(NuevoClienteViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetClient(ClientDto client)
        {
            ViewModel.SetClient(client);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAsync();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }
    }
}