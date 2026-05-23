using System.Windows.Input;

namespace MauiAppLogin.Views.Controls;

public partial class DashboardHeader : ContentView
{
    public static readonly BindableProperty UsernameProperty =
        BindableProperty.Create(nameof(Username), typeof(string), typeof(DashboardHeader), string.Empty);

    public static readonly BindableProperty TaskBadgeTextProperty =
        BindableProperty.Create(nameof(TaskBadgeText), typeof(string), typeof(DashboardHeader), string.Empty);

    public static readonly BindableProperty ShowTaskBadgeProperty =
        BindableProperty.Create(nameof(ShowTaskBadge), typeof(bool), typeof(DashboardHeader), true);

    public static readonly BindableProperty NavigateToTasksCommandProperty =
        BindableProperty.Create(nameof(NavigateToTasksCommand), typeof(ICommand), typeof(DashboardHeader), null);

    public static readonly BindableProperty OpenUserMenuCommandProperty =
        BindableProperty.Create(nameof(OpenUserMenuCommand), typeof(ICommand), typeof(DashboardHeader), null);

    public string Username
    {
        get => (string)GetValue(UsernameProperty);
        set => SetValue(UsernameProperty, value);
    }

    public string TaskBadgeText
    {
        get => (string)GetValue(TaskBadgeTextProperty);
        set => SetValue(TaskBadgeTextProperty, value);
    }

    public bool ShowTaskBadge
    {
        get => (bool)GetValue(ShowTaskBadgeProperty);
        set => SetValue(ShowTaskBadgeProperty, value);
    }

    public ICommand? NavigateToTasksCommand
    {
        get => (ICommand?)GetValue(NavigateToTasksCommandProperty);
        set => SetValue(NavigateToTasksCommandProperty, value);
    }

    public ICommand? OpenUserMenuCommand
    {
        get => (ICommand?)GetValue(OpenUserMenuCommandProperty);
        set => SetValue(OpenUserMenuCommandProperty, value);
    }

    public DashboardHeader()
    {
        InitializeComponent();
    }

    private void OnUserTapped(object? sender, TappedEventArgs e)
        => OpenUserMenuCommand?.Execute(null);
}
