using System;
using System.DirectoryServices.ActiveDirectory;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace LDForms
{
    public partial class ToastNotification : Window
    {
        private readonly int _durationMilliseconds;

        public ToastNotification(string title, string message, DialogType toastType, int durationMilliseconds = 3000)
        {
            InitializeComponent();

            _durationMilliseconds = durationMilliseconds;

            TxtTitle.Text = title;
            TxtMessage.Text = message;

            ApplyStyle(toastType);

            Loaded += ToastNotification_Loaded;
        }

        private async void ToastNotification_Loaded(object sender, RoutedEventArgs e)
        {
            PositionWindow();

            if (Resources["ShowAnimation"] is Storyboard showAnimation)
                showAnimation.Begin();

            await Task.Delay(_durationMilliseconds);

            await CloseWithAnimationAsync();
        }

        private void ApplyStyle(DialogType toastType)
        {
            SuccessIcon.Visibility = Visibility.Collapsed;
            WarningIcon.Visibility = Visibility.Collapsed;
            ErrorIcon.Visibility = Visibility.Collapsed;
            InfoIcon.Visibility = Visibility.Collapsed;

            switch (toastType)
            {
                case DialogType.Success:
                    AccentBar.Background = new SolidColorBrush(Color.FromRgb(88, 194, 156));
                    IconBackground.Background = new SolidColorBrush(Color.FromRgb(232, 248, 241));
                    SuccessIcon.Visibility = Visibility.Visible;
                    break;

                case DialogType.Warning:
                    AccentBar.Background = new SolidColorBrush(Color.FromRgb(240, 173, 78));
                    IconBackground.Background = new SolidColorBrush(Color.FromRgb(255, 244, 229));
                    WarningIcon.Visibility = Visibility.Visible;
                    break;

                case DialogType.Error:
                    AccentBar.Background = new SolidColorBrush(Color.FromRgb(224, 90, 90));
                    IconBackground.Background = new SolidColorBrush(Color.FromRgb(253, 236, 236));
                    ErrorIcon.Visibility = Visibility.Visible;
                    break;

                case DialogType.Info:
                    AccentBar.Background = new SolidColorBrush(Color.FromRgb(74, 144, 226));
                    IconBackground.Background = new SolidColorBrush(Color.FromRgb(234, 242, 253));
                    InfoIcon.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void PositionWindow()
        {
            var workArea = SystemParameters.WorkArea;

            Left = workArea.Right - Width - 16;
            Top = workArea.Bottom - Height - 16;
        }

        private async Task CloseWithAnimationAsync()
        {
            if (!IsLoaded)
                return;

            if (Resources["HideAnimation"] is Storyboard hideAnimation)
            {
                var tcs = new TaskCompletionSource<bool>();

                void OnCompleted(object? s, EventArgs e)
                {
                    hideAnimation.Completed -= OnCompleted;
                    tcs.TrySetResult(true);
                }

                hideAnimation.Completed += OnCompleted;
                hideAnimation.Begin();

                await tcs.Task;
            }

            Close();
        }

        private async void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            await CloseWithAnimationAsync();
        }
    }
}