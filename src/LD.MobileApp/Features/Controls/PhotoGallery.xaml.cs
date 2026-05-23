using System.Collections;
using System.Windows.Input;

namespace MauiAppLogin.Views.Controls;

public partial class PhotoGallery : ContentView
{
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(PhotoGallery), null);

    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(PhotoGallery), null);

    public static readonly BindableProperty ViewCommandProperty =
        BindableProperty.Create(nameof(ViewCommand), typeof(ICommand), typeof(PhotoGallery), null);

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public ICommand? DeleteCommand
    {
        get => (ICommand?)GetValue(DeleteCommandProperty);
        set => SetValue(DeleteCommandProperty, value);
    }

    public ICommand? ViewCommand
    {
        get => (ICommand?)GetValue(ViewCommandProperty);
        set => SetValue(ViewCommandProperty, value);
    }

    public PhotoGallery()
    {
        InitializeComponent();
    }
}
