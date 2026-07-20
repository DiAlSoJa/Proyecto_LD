using CommunityToolkit.Maui.Views;

namespace MauiAppLogin.Controls;

public partial class CustomDialog : Popup
{
    public CustomDialog(string title, string message, DialogType type,
        Action? onConfirm = null, Action? onCancel = null)
    {
        InitializeComponent();
        ConfigurarTipo(type, title, message, onConfirm, onCancel);
    }

    private void ConfigurarTipo(DialogType type, string title, string message,
        Action? onConfirm, Action? onCancel)
    {
        TitleLabel.Text = title;
        MessageLabel.Text = message;

        switch (type)
        {
            case DialogType.Info:
                IconLabel.Text = "\u2139\uFE0F";
                AgregarBoton("Entendido", "#1F3A5F", "#FFFFFF", () =>
                {
                    onConfirm?.Invoke();
                    Close(true);
                });
                break;

            case DialogType.Success:
                IconLabel.Text = "\u2705";
                AgregarBoton("Aceptar", "#16A34A", "#FFFFFF", () =>
                {
                    onConfirm?.Invoke();
                    Close(true);
                });
                break;

            case DialogType.Warning:
                IconLabel.Text = "\u26A0\uFE0F";
                AgregarBoton("Cancelar", "#F3F4F6", "#374151", () =>
                {
                    onCancel?.Invoke();
                    Close(false);
                });
                AgregarBoton("Continuar", "#1F3A5F", "#FFFFFF", () =>
                {
                    onConfirm?.Invoke();
                    Close(true);
                });
                break;

            case DialogType.Error:
                IconLabel.Text = "\u274C";
                AgregarBoton("Cerrar", "#DC2626", "#FFFFFF", () =>
                {
                    onConfirm?.Invoke();
                    Close(false);
                });
                break;

            case DialogType.Blocking:
                IconLabel.Text = "\U0001F512";
                // Sin boton: el popup solo se cierra programaticamente cuando el checklist se completa
                break;
        }
    }

    private void AgregarBoton(string texto, string bgColor, string textColor, Action onClick)
    {
        var btn = new Button
        {
            Text = texto,
            BackgroundColor = Color.FromArgb(bgColor),
            TextColor = Color.FromArgb(textColor),
            CornerRadius = 8,
            Padding = new Thickness(20, 10),
            FontSize = 14,
            FontFamily = "OpenSansRegular"
        };
        btn.Clicked += (_, _) => onClick();
        ButtonsContainer.Children.Add(btn);
    }
}
