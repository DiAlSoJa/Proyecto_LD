namespace MauiAppLogin.Services;

public interface IAppStateService
{
    bool IsBackground { get; }
    void SetBackground(bool value);
}

public sealed class AppStateService : IAppStateService
{
    public bool IsBackground { get; private set; }

    public void SetBackground(bool value) => IsBackground = value;
}
