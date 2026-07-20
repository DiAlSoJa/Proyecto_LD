using LDForms;

namespace LD.FormsX.Helpers
{
    public static class DialogHelper
    {
        public static bool ShowSuccess(string message, string title = "Éxito")
        {
            var dialog = new MessageDialog(title, message, DialogType.Success);
            return dialog.ShowDialog() == true;
        }

        public static bool ShowWarning(string message, string title = "Advertencia")
        {
            var dialog = new MessageDialog(title, message, DialogType.Warning);
            return dialog.ShowDialog() == true;
        }

        public static bool ShowError(string message, string title = "Error")
        {
            var dialog = new MessageDialog(title, message, DialogType.Error);
            return dialog.ShowDialog() == true;
        }
        public static bool ShowInfo(string message, string title = "Información")
        {
            var dialog = new MessageDialog(title, message, DialogType.Info);
            return dialog.ShowDialog() == true;
        }

        public static bool ShowConfirm(string message, string title = "Confirmar")
        {
            var dialog = new MessageDialog(title, message, DialogType.Confirm);
            return dialog.ShowDialog() == true;
        }
    }
}
