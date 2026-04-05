using LD.Contracts.Project;
using LD.FormsX.Features.Proyectos.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Proyectos
{
    public partial class NuevoProyectoView : Window
    {
        private NuevoProyectoViewModel ViewModel => (NuevoProyectoViewModel)DataContext;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevoProyectoView(NuevoProyectoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetProject(ProjectDto project)
        {
            ViewModel.SetProject(project);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAsync();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e) => Close();

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}

              
