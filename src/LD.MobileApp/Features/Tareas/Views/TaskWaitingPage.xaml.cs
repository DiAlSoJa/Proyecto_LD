using MauiAppLogin.ViewModels;

namespace MauiAppLogin;

public partial class TaskWaitingPage : ContentPage
{
    private readonly TaskWaitingViewModel _viewModel;

    public TaskWaitingPage(TaskWaitingViewModel viewModel)
    {
        InitializeComponent();
        _viewModel  = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnNavigatedToAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnNavigatedFrom();
    }
}
