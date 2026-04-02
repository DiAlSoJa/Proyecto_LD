using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Forms.Core;
using LD.FormsX;
using LD.FormsX.Helpers;
using LD.FormsX.Movimientos;
using LD.FormsX.Views;
using LD.FormsX.Views.ASN;
using LD.FormsX.Views.Auditar;
using LD.FormsX.Views.Catalogos;
using LD.FormsX.Views.CheckList;
using LD.FormsX.Views.ControlPatio;
using LD.FormsX.Views.Inventario;
using LD.FormsX.Views.InventarioAleatorio;
using LD.FormsX.Views.Proyectos;
using LD.FormsX.Views.Reportes;
using LD.FormsX.Views.Usuarios;
using LD.Formx.Core;
using LDForms.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace LDForms
{
    public partial class DashBoard : Window
    {
        private bool _omitNextClick;
        private readonly IServiceProvider _serviceProvider;
        private const int MONITOR_DEFAULTTONEAREST = 0x00000002;

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        public DashBoard(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetVisibility();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var handle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            System.Windows.Interop.HwndSource.FromHwnd(handle)?.AddHook(WindowProc);
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_GETMINMAXINFO = 0x0024;

            if (msg == WM_GETMINMAXINFO)
            {
                WmGetMinMaxInfo(hwnd, lParam);
                handled = true;
            }

            return IntPtr.Zero;
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = Marshal.PtrToStructure<MINMAXINFO>(lParam);

            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, ref monitorInfo);

                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = rcWorkArea.left - rcMonitorArea.left;
                mmi.ptMaxPosition.y = rcWorkArea.top - rcMonitorArea.top;
                mmi.ptMaxSize.x = rcWorkArea.right - rcWorkArea.left;
                mmi.ptMaxSize.y = rcWorkArea.bottom - rcWorkArea.top;
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

     

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;

            public MONITORINFO()
            {
                cbSize = Marshal.SizeOf(typeof(MONITORINFO));
                rcMonitor = new RECT();
                rcWork = new RECT();
                dwFlags = 0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
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
                    AbrirTab("Proyectos", _serviceProvider.GetRequiredService<ProyectosView>());
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
                case "Inventario":
                    AbrirTab("Inventario", _serviceProvider.GetRequiredService<InventarioView>());
                    break;
                case "Movimientos":
                    AbrirTab("Movimientos", _serviceProvider.GetRequiredService<MovimientosView>());
                    break;
                case "ASN":
                    AbrirTab("ASN", _serviceProvider.GetRequiredService<ASNView>());
                    break;
                case "Auditar":
                    AbrirTab("Auditar", _serviceProvider.GetRequiredService<AuditarView>());
                    break;
                case "Aleatorio":
                    AbrirTab("Aleatorio", _serviceProvider.GetRequiredService<InventarioCiclicoView>());
                    break;
                case "CheckList":
                    AbrirTab("CheckList", _serviceProvider.GetRequiredService<CheckListView>());
                    break;
                case "Reportes":
                    AbrirTab("Reportes", _serviceProvider.GetRequiredService<ReportesView>());
                    break;
                case "Patio":
                    AbrirTab("Patio", _serviceProvider.GetRequiredService<ControlPatioView>());
                    break;
                case "Usuarios":
                    AbrirTab("Usuarios", _serviceProvider.GetRequiredService<UsuariosView>());
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
                    AbrirVentana("Proyectos", _serviceProvider.GetRequiredService<ProyectosView>());
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

                case "Inventario":
                    AbrirVentana("Inventario", _serviceProvider.GetRequiredService<InventarioView>());
                    break;

                case "Movimientos":
                    AbrirVentana("Movimientos", _serviceProvider.GetRequiredService<MovimientosView>());
                    break;
                case "ASN":
                    AbrirVentana("ASN", _serviceProvider.GetRequiredService<ASNView>());
                    break;
                case "Auditar":
                    AbrirVentana("Auditar", _serviceProvider.GetRequiredService<AuditarView>());
                    break;
                case "Aleatorio":
                    AbrirVentana("Aleatorio", _serviceProvider.GetRequiredService<InventarioCiclicoView>());
                    break;
                case "CheckList":
                    AbrirVentana("CheckList", _serviceProvider.GetRequiredService<CheckListView>());
                    break;
                case "Reportes":
                    AbrirVentana("Reportes", _serviceProvider.GetRequiredService<ReportesView>());
                    break;
                case "Patio":
                    AbrirVentana("Patio", _serviceProvider.GetRequiredService<ControlPatioView>());
                    break;
                case "Usuarios":
                    AbrirVentana("Usuarios", _serviceProvider.GetRequiredService<UsuariosView>());
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

        private void BtnAccount_Click(object sender, RoutedEventArgs e)
        {
            txtPopupUserName.Text = UserData.UserName ?? "Usuario";
            AccountPopup.IsOpen = !AccountPopup.IsOpen;
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            AccountPopup.IsOpen = false;

            if (!DialogHelper.ShowConfirm("¿Está seguro de cerrar sesión?"))
                return;

            UserSession.LogOut();
            _serviceProvider.GetRequiredService<ApiService>().ClearToken();

            var login = App.Services.GetRequiredService<MainWindow>();
            login.Show();
            Close();
        }
    }
}