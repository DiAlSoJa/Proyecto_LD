using System;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Dialogs
{
    public partial class NuevoASNEscaneoView : Window
    {
        public NuevoASNEscaneoView()
        {
            InitializeComponent();
        }

        public void AddMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtMensajes.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
            txtMensajes.ScrollToEnd();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtEscaneo.Focus();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void TxtEscaneo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;

            var scanValue = txtEscaneo.Text.Trim();
            if (string.IsNullOrWhiteSpace(scanValue))
                return;

            AddMessage($"Escaneo recibido: {scanValue}");
            txtEscaneo.Clear();
            txtEscaneo.Focus();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
