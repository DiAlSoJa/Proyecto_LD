using System;
using System.Windows;
using System.Windows.Input;
using LD.Contracts.Project;
using LD.FormsX.Features.Proyectos.ViewModels;

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
            UpdateWindowButtons();

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

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ToggleWindowState();
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void BtnMinimizarVentana_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximizarVentana_Click(object sender, RoutedEventArgs e)
        {
            ToggleWindowState();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            UpdateWindowButtons();
        }

        private void ToggleWindowState()
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void UpdateWindowButtons()
        {
            if (btnMaximizarVentana == null)
                return;

            btnMaximizarVentana.Content = WindowState == WindowState.Maximized ? "❐" : "□";
        }
    }
}
