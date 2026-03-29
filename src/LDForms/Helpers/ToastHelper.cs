using System.DirectoryServices.ActiveDirectory;
using LDForms;

namespace LD.FormsX.Helpers
{
    public static class ToastHelper
    {
        public static void ShowSuccess(string message, string title = "Success", int durationMilliseconds = 3000)
        {
            new ToastNotification(title, message, DialogType.Success, durationMilliseconds).Show();
        }

        public static void ShowWarning(string message, string title = "Warning", int durationMilliseconds = 3000)
        {
            new ToastNotification(title, message, DialogType.Warning, durationMilliseconds).Show();
        }

        public static void ShowError(string message, string title = "Error", int durationMilliseconds = 3000)
        {
            new ToastNotification(title, message, DialogType.Error, durationMilliseconds).Show();
        }

        public static void ShowInfo(string message, string title = "Information", int durationMilliseconds = 3000)
        {
            new ToastNotification(title, message, DialogType.Info, durationMilliseconds).Show();
        }
    }
}