using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using System.Windows.Media;
using LD.Contracts.Enums;
using LD.FormsX.Helpers;
using LD.FormsX.Views;
using LD.FormsX.Views.Catalogos;
using LD.Formx.Core;
using LDForms.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LDForms
{
    public partial class DashBoard : Window
    {
        private bool _omitNextClick;
        private readonly IServiceProvider _serviceProvider;

        public DashBoard(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetVisibility();
        }
        private void SetVisibility()
        {
            ClientesBtn.Visibility = HasModule(Module_e.Clients);
            ProyectosBtn.Visibility = HasModule(Module_e.Projects);
            AlmacenesBtn.Visibility = HasModule(Module_e.Warehouses);
            UbicacionesBtn.Visibility = HasModule(Module_e.Locations);
            ArticulosBtn.Visibility = HasModule(Module_e.Products);
            MovimientosBtn.Visibility = HasModule(Module_e.Movements);
            AsnBtn.Visibility = HasModule(Module_e.ASN);
            ChecklistBtn.Visibility = HasModule(Module_e.ChecklistLift);
            PatioBtn.Visibility = HasModule(Module_e.YardControl);
            CatalogosBtn.Visibility = HasModule(Module_e.Catalogs);
            SurtidoBtn.Visibility = HasModule(Module_e.Picking);
            EmbarquesBtn.Visibility = HasModule(Module_e.Shipments);
            InventarioBtn.Visibility = HasModule(Module_e.Inventory);
            InventarioRandomBtn.Visibility = HasModule(Module_e.RandomInventory);
            ReportesBtn.Visibility = HasModule(Module_e.Reports);
            UsuariosBtn.Visibility = HasModule(Module_e.Users);
            AuditoriaBtn.Visibility = HasModule(Module_e.Auditing);
        }

        private Visibility HasModule(Module_e module)
        {
            bool has = UserData.Authorization.Modules
                .Any(m => m.ModuleId == (int)module);

            return has ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ToggleMaximize();
                return;
            }

            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            ToggleMaximize();
        }

        private void ToggleMaximize()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                btnMaximize.Content = "□";
            }
            else
            {
                WindowState = WindowState.Maximized;
                btnMaximize.Content = "❐";
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                Close(); // ventana flotante
                return;
            }

            bool confirmar = DialogHelper.ShowConfirm("¿Está seguro de cerrar el sistema?");

            if (confirmar)
            {
                Application.Current.Shutdown();
            }



        }




        private void AbrirModuloEnTab(string key)
        {
            switch (key)
            {
                case "Clientes":
                    AbrirTab("Clientes", _serviceProvider.GetRequiredService<CatalogosClientesView>());
                    break;

                case "Almacenes":
                    AbrirTab("Almacenes", _serviceProvider.GetRequiredService<AlmacenesView>());
                    break;

                case "Proyectos":
                  //  AbrirTab("Proyectos", _serviceProvider.GetRequiredService<ProductosView>());
                    break;
                case "Ubicaciones":
                    AbrirTab("Ubicaciones", _serviceProvider.GetRequiredService<UbicacionesView>());
                    break;
                case "Articulos":
                    AbrirTab("Articulos", _serviceProvider.GetRequiredService<ArticulosView>());
                    break;
                case "Catalogos":
                    AbrirTab("Catalogos", _serviceProvider.GetRequiredService<CatalogosView>());
                    break;
            }
        }

        private void AbrirModuloEnVentana(string key)
        {
            switch (key)
            {
                case "Clientes":
                    AbrirVentana("Clientes", _serviceProvider.GetRequiredService<CatalogosClientesView>());
                    break;

                case "Almacenes":
                    AbrirVentana("Almacenes", _serviceProvider.GetRequiredService<AlmacenesView>());
                    break;

                case "Proyectos":
                    AbrirVentana("Proyectos", _serviceProvider.GetRequiredService<ProductosView>());
                    break;

                case "Ubicaciones":
                    AbrirVentana("Ubicaciones", _serviceProvider.GetRequiredService<UbicacionesView>());
                    break;

                case "Articulos":
                    AbrirVentana("Articulos", _serviceProvider.GetRequiredService<ArticulosView>());
                    break;

                case "Catalogos":
                    AbrirVentana("Catalogos", _serviceProvider.GetRequiredService<CatalogosView>());
                    break;
            }
        }





        private void DashboardTile_RightClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not string key)
                return;

            var menu = new ContextMenu();

            var tabItem = new MenuItem { Header = "Abrir en pestaña" };
            tabItem.Click += (_, __) => AbrirModuloEnTab(key);

            var windowItem = new MenuItem { Header = "Abrir en ventana" };
            windowItem.Click += (_, __) => AbrirModuloEnVentana(key);

            menu.Items.Add(tabItem);
            menu.Items.Add(windowItem);

            menu.IsOpen = true;
        }

        private void DashboardTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not string key)
                return;

            AbrirModuloEnTab(key);
        }







        private void AbrirTab(string titulo, UserControl vista)
        {
            if (titulo == "Menú")
            {
                MainTabControl.SelectedItem = TabMenu;
                return;
            }

            var existente = MainTabControl.Items
                .OfType<TabItem>()
                .FirstOrDefault(t => t.Tag?.ToString() == titulo);

            if (existente != null)
            {
                MainTabControl.SelectedItem = existente;
                return;
            }

            var headerPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            var txt = new TextBlock
            {
                Text = titulo,
                VerticalAlignment = VerticalAlignment.Center
            };

            var btnCerrar = new Button
            {
                Content = "×",
                Tag = titulo,
                Style = (Style)FindResource("TabCloseButtonStyle")
            };
            btnCerrar.Click += CerrarTab_Click;

            headerPanel.Children.Add(txt);
            headerPanel.Children.Add(btnCerrar);

            var tab = new TabItem
            {
                Header = headerPanel,
                Content = vista,
                Tag = titulo,
                Style = (Style)FindResource("ElegantTabItemStyle")
            };

            MainTabControl.Items.Add(tab);
            MainTabControl.SelectedItem = tab;
        }

        private void AbrirVentana(string titulo, UserControl vista)
        {
            var ventana = new Window
            {
                Title = titulo,
                Width = 1100,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = vista,
                Background = Brushes.White
            };

            ventana.Show();
        }

        private void CerrarTab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string titulo)
            {
                var tab = MainTabControl.Items
                    .OfType<TabItem>()
                    .FirstOrDefault(t => t.Tag?.ToString() == titulo);

                if (tab != null)
                {
                    bool eraLaActiva = MainTabControl.SelectedItem == tab;
                    MainTabControl.Items.Remove(tab);

                    if (eraLaActiva)
                        MainTabControl.SelectedItem = TabMenu;
                }
            }
        }

       
       



       



    }
}