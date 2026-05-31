using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.FormsX;
using LD.FormsX.Helpers;
using LDForms.Features.DashBoard.ViewModels;
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
        private readonly IServiceProvider _serviceProvider;
        private readonly DashBoardViewModel _viewModel;
        private const int MONITOR_DEFAULTTONEAREST = 0x00000002;

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        public DashBoard(IServiceProvider serviceProvider, DashBoardViewModel viewModel)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _viewModel = viewModel;
            DataContext = _viewModel;
            _viewModel.NavigationRequested += ViewModel_NavigationRequested;
        }

        protected override void OnClosed(EventArgs e)
        {
            _viewModel.NavigationRequested -= ViewModel_NavigationRequested;
            base.OnClosed(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle)?.AddHook(WindowProc);
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
                MONITORINFO monitorInfo = new();
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

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ToggleMaximize();
                return;
            }

            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                Close();
                return;
            }

            bool confirmar = DialogHelper.ShowConfirm("¿Está seguro de cerrar el sistema?");

            if (confirmar)
            {
                Application.Current.Shutdown();
            }
        }

        private void ViewModel_NavigationRequested(object? sender, DashboardNavigationRequestedEventArgs e)
        {
            if (e.Mode == DashboardOpenMode.Tab)
            {
                AbrirTab(e.Title, e.View);
                return;
            }

            AbrirVentana(e.Title, e.View);
        }

        private async Task ImprimirEtiquetasStandardIdAsync()
        {
            if (!UserData.HasPermission(PermissionKeys.StandardLabel_Print))
            {
                DialogHelper.ShowWarning("No tienes permiso para imprimir etiquetas.", "Permiso requerido");
                return;
            }

            var option = ShowPrintOptionDialog();
            if (option != StandardLabelPrintOptionsDialog.StandardIdOption)
                return;

            var quantity = ShowQuantityDialog();
            if (!quantity.HasValue)
                return;

            try
            {
                var service = _serviceProvider.GetRequiredService<StandardLabelService>();
                var response = await service.GenerateStandardIds(quantity.Value);

                if (response.IsFailure || response.Data == null || response.Data.Count == 0)
                {
                    DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudieron generar los StandardId.");
                    return;
                }

                StandardIdLabelPrinter.PrintLabels(response.Data);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private string? ShowPrintOptionDialog()
        {
            var dialog = new StandardLabelPrintOptionsDialog
            {
                Owner = this
            };

            return dialog.ShowDialog() == true
                ? dialog.SelectedOption
                : null;
        }

        private int? ShowQuantityDialog()
        {
            var dialog = new StandardLabelQuantityDialog
            {
                Owner = this
            };

            return dialog.ShowDialog() == true ? dialog.Quantity : null;
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
                    {
                        MainTabControl.SelectedItem = TabMenu;
                    }
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
            {
                return;
            }

            UserSession.LogOut();
            _serviceProvider.GetRequiredService<ApiService>().ClearToken();

            var login = App.Services.GetRequiredService<MainWindow>();
            login.Show();
            Close();
        }
    }
}
