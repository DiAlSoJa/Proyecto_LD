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
using LD.FormsX.Helpers;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.Auditar
{
    /// <summary>
    /// Lógica de interacción para AuditarView.xaml
    /// </summary>
    public partial class AuditarView : UserControl
    {
        private readonly FamilyService _service;
        private readonly IServiceProvider _serviceProvider;
        

        private FamilyDto? _selectedX;
        private bool _loaded;
        public AuditarView(FamilyService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _serviceProvider = serviceProvider;          

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e) { }
        private void BtnActualizar_Click(object sender, RoutedEventArgs e) { }
        private void BtnNuevaAuditoria_Click(object sender, RoutedEventArgs e) { }
        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaAuditoriaView>();
            dialog.Owner = Window.GetWindow(this);

            var result = dialog.ShowDialog();

            
        }
        private void BtnEditar_Click(object sender, RoutedEventArgs e) { }
        private void dgAuditorias_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void dgDetalleAuditoria_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
    }
}
