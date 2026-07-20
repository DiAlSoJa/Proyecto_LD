using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LDForms;

namespace LDForms
{
    public partial class MessageDialog : Window
    {
        public bool Confirmed { get; private set; }

        public MessageDialog(string title, string message, DialogType dialogType)
        {
            InitializeComponent();

            TxtTitle.Text = title;
            TxtMessage.Text = message;

            ApplyStyle(dialogType);
        }

        private void ApplyStyle(DialogType dialogType)
        {
            SuccessIcon.Visibility = Visibility.Collapsed;
            WarningIcon.Visibility = Visibility.Collapsed;
            ErrorIcon.Visibility = Visibility.Collapsed;
            ConfirmIcon.Visibility = Visibility.Collapsed;
            InfoIcon.Visibility = Visibility.Collapsed;

            BtnAccept.Visibility = Visibility.Visible;
            ConfirmButtonsPanel.Visibility = Visibility.Collapsed;

            switch (dialogType)
            {
                case DialogType.Info:
                    IconBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF2FD")); // #
                    BtnAccept.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A90E2"));      // #
                    BtnAccept.Content = "Entendido";
                    InfoIcon.Visibility = Visibility.Visible;
                    break;
                case DialogType.Success:
                    IconBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F8F1"));
                    BtnAccept.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#58C29C"));
                    BtnAccept.Content = "Aceptar";
                    SuccessIcon.Visibility = Visibility.Visible;
                    break;

                case DialogType.Warning:
                    IconBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF4E5"));
                    BtnAccept.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F0AD4E"));
                    BtnAccept.Content = "Revisar";
                    WarningIcon.Visibility = Visibility.Visible;
                    break;

                case DialogType.Error:
                    IconBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDECEC"));
                    BtnAccept.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E05A5A"));
                    BtnAccept.Content = "Cerrar";
                    ErrorIcon.Visibility = Visibility.Visible;
                    break;

                case DialogType.Confirm:
                    IconBackground.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EAF2FD"));
                    ConfirmIcon.Visibility = Visibility.Visible;

                    BtnAccept.Visibility = Visibility.Collapsed;
                    ConfirmButtonsPanel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            DialogResult = true;
            Close();
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            DialogResult = false;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            DialogResult = false;
            Close();
        }

        private void MainBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
