using LDForms;

namespace LD.FormsX.Helpers
{
    public static class ToastHelper
    {
        public static void ShowSuccess(string message, string title = "Éxito", int durationMilliseconds = 3000)
        {
            new ToastNotification(title, message, DialogType.Success, durationMilliseconds).Show();
        }

        public static void ShowWarning(string message, string title = "Advertencia", int durationMilliseconds = 3000)
        {
            new ToastNotification(title, message, DialogType.Warning, durationMilliseconds).Show();
        }

        public static void ShowError(string message, string title = "Error", int durationMilliseconds = 3000)
        {
            new ToastNotification(title, message, DialogType.Error, durationMilliseconds).Show();
        }

        public static void ShowInfo(string message, string title = "Información", int durationMilliseconds = 3000)
        {
            new ToastNotification(title, message, DialogType.Info, durationMilliseconds).Show();
        }
    }
}
