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
using LD.Contracts.Location;
using LD.FormsX.Helpers;

namespace LD.FormsX.Views.Inventario
{
    /// <summary>
    /// Lógica de interacción para InventarioView.xaml
    /// </summary>
    public partial class InventarioView : UserControl
    {
        private readonly LocationService _service;
        private readonly IServiceProvider _serviceProvider;
        
          
        
        public InventarioView(LocationService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _serviceProvider = serviceProvider;
           

        }

    }
}
