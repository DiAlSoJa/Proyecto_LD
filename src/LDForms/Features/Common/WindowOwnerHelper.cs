using System.Windows;

namespace LD.FormsX.Features.Common;

internal static class WindowOwnerHelper
{
    public static Window? GetVisibleOwner(Window? window)
    {
        return window is { IsLoaded: true, IsVisible: true } ? window : null;
    }

    public static void AttachOwnerOrCenter(Window window, Window? owner)
    {
        if (owner is { IsLoaded: true, IsVisible: true })
        {
            window.Owner = owner;
            return;
        }

        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }
}
