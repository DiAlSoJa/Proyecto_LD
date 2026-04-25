namespace MauiAppLogin.Services;

public interface ILoaderService
{
    bool IsVisible { get; }
    string Message { get; }
    void Show(string message = "Cargando...");
    void Hide();
}
