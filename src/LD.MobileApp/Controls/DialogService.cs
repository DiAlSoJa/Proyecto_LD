using CommunityToolkit.Maui.Views;

namespace MauiAppLogin.Controls;

public interface IDialogService
{
    Task ShowInfoAsync(string title, string message);
    Task ShowSuccessAsync(string title, string message);
    Task<bool> ShowWarningAsync(string title, string message);
    Task ShowErrorAsync(string title, string message);
    void ShowBlocking(string title, string message);
    void HideBlocking();
}

public class DialogService : IDialogService
{
    private CustomDialog? _blockingDialog;

    private static Page GetCurrentPage()
        => Shell.Current?.CurrentPage
           ?? Application.Current!.Windows[0].Page!;

    public async Task ShowInfoAsync(string title, string message)
    {
        var dialog = new CustomDialog(title, message, DialogType.Info);
        await GetCurrentPage().ShowPopupAsync(dialog);
    }

    public async Task ShowSuccessAsync(string title, string message)
    {
        var dialog = new CustomDialog(title, message, DialogType.Success);
        await GetCurrentPage().ShowPopupAsync(dialog);
    }

    public async Task<bool> ShowWarningAsync(string title, string message)
    {
        var dialog = new CustomDialog(title, message, DialogType.Warning);
        var result = await GetCurrentPage().ShowPopupAsync(dialog);
        return result is true;
    }

    public async Task ShowErrorAsync(string title, string message)
    {
        var dialog = new CustomDialog(title, message, DialogType.Error);
        await GetCurrentPage().ShowPopupAsync(dialog);
    }

    // Muestra un popup de bloqueo sin botón de cierre.
    // Solo se cierra llamando a HideBlocking() (cuando el checklist se envía exitosamente).
    public void ShowBlocking(string title, string message)
    {
        _blockingDialog = new CustomDialog(title, message, DialogType.Blocking);
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await GetCurrentPage().ShowPopupAsync(_blockingDialog);
        });
    }

    public void HideBlocking()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _blockingDialog?.Close();
            _blockingDialog = null;
        });
    }
}
