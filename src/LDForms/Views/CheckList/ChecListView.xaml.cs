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
using LD.Client.Services;
using LD.Contracts.DTOs.Family;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.CheckList
{
    /// <summary>
    /// Lógica de interacción para ChecListView.xaml
    /// </summary>
    public partial class CheckListView : UserControl
    {
        private readonly FamilyService _service;
        private readonly IServiceProvider _serviceProvider;


        private FamilyDto? _selectedX;
        private bool _loaded;
        public CheckListView(FamilyService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _serviceProvider = serviceProvider;

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e) { }

        private void BtnActualizarEquipo_Click(object sender, RoutedEventArgs e) { }
        private void BtnAsignarUsuario_Click(object sender, RoutedEventArgs e) { }
       

        private async void BtnNuevoEquipo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevoEquipoCheckListView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();


        }


        private void BtnEditarEquipo_Click(object sender, RoutedEventArgs e) { }
        private void BtnEliminarEquipo_Click(object sender, RoutedEventArgs e) { }
        private void dgEquipo_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void BtnBuscarResumen_Click(object sender, RoutedEventArgs e) { }
        private void dgResumen_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void BtnVerImagenes_Click(object sender, RoutedEventArgs e) { }

        private void BtnBuscarBaterias_Click(object sender, RoutedEventArgs e) { }
        private void dgBaterias_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void BtnGuardarPregunta_Click(object sender, RoutedEventArgs e) { }
        private void BtnEditarPregunta_Click(object sender, RoutedEventArgs e) { }
        private void BtnEliminarPregunta_Click(object sender, RoutedEventArgs e) { }
        private void dgPreguntas_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
    }
}
