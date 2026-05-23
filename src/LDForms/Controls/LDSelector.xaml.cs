using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LD.FormsX.Controls;

public partial class LDSelector : UserControl
{
    // ── Dependency Properties ────────────────────────────────────────────────

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(LDSelector),
            new PropertyMetadata(string.Empty, OnLabelChanged));

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(LDSelector),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(LDSelector),
            new PropertyMetadata(null, OnItemsSourceChanged));

    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(LDSelector),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemChanged));

    public static readonly DependencyProperty SelectedValueProperty =
        DependencyProperty.Register(nameof(SelectedValue), typeof(object), typeof(LDSelector),
            new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedValueChanged));

    public static readonly DependencyProperty SelectedValuePathProperty =
        DependencyProperty.Register(nameof(SelectedValuePath), typeof(string), typeof(LDSelector),
            new PropertyMetadata(string.Empty, OnSelectedValuePathChanged));

    public static readonly DependencyProperty DisplayMemberPathProperty =
        DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(LDSelector),
            new PropertyMetadata(string.Empty, OnDisplayMemberPathChanged));

    public static readonly DependencyProperty HelpTextProperty =
        DependencyProperty.Register(nameof(HelpText), typeof(string), typeof(LDSelector),
            new PropertyMetadata(string.Empty, OnHelpTextChanged));

    public static readonly DependencyProperty HasErrorProperty =
        DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(LDSelector),
            new PropertyMetadata(false, OnHasErrorChanged));

    public static readonly DependencyProperty ErrorTextProperty =
        DependencyProperty.Register(nameof(ErrorText), typeof(string), typeof(LDSelector),
            new PropertyMetadata(string.Empty, OnHasErrorChanged));

    // ── CLR wrappers ────────────────────────────────────────────────────────

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public IEnumerable? ItemsSource
    {
        get => (IEnumerable?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public object? SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public string SelectedValuePath
    {
        get => (string)GetValue(SelectedValuePathProperty);
        set => SetValue(SelectedValuePathProperty, value);
    }

    public string DisplayMemberPath
    {
        get => (string)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }

    public string HelpText
    {
        get => (string)GetValue(HelpTextProperty);
        set => SetValue(HelpTextProperty, value);
    }

    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }

    public string ErrorText
    {
        get => (string)GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }

    public ComboBox ComboBoxElement => PART_ComboBox;

    // ── Constructor ─────────────────────────────────────────────────────────

    public LDSelector()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            UpdatePlaceholder();
            UpdateBorderState();
            UpdateHelpVisibility();
        };
    }

    // ── DP callbacks ────────────────────────────────────────────────────────

    private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDSelector ctrl)
            ctrl.PART_Label.Visibility = string.IsNullOrEmpty(e.NewValue as string)
                ? Visibility.Collapsed : Visibility.Visible;
    }

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDSelector ctrl)
            ctrl.PART_ComboBox.ItemsSource = e.NewValue as IEnumerable;
    }

    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDSelector ctrl)
        {
            if (ctrl.PART_ComboBox.SelectedItem != e.NewValue)
                ctrl.PART_ComboBox.SelectedItem = e.NewValue;
            ctrl.UpdatePlaceholder();
        }
    }

    private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDSelector ctrl)
        {
            if (ctrl.PART_ComboBox.SelectedValue != e.NewValue)
                ctrl.PART_ComboBox.SelectedValue = e.NewValue;
            ctrl.UpdatePlaceholder();
        }
    }

    private static void OnSelectedValuePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDSelector ctrl)
            ctrl.PART_ComboBox.SelectedValuePath = e.NewValue as string ?? string.Empty;
    }

    private static void OnDisplayMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDSelector ctrl)
            ctrl.PART_ComboBox.DisplayMemberPath = e.NewValue as string ?? string.Empty;
    }

    private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDSelector ctrl)
        {
            ctrl.UpdateBorderState();
            ctrl.UpdateHelpVisibility();
        }
    }

    private static void OnHelpTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDSelector ctrl)
            ctrl.UpdateHelpVisibility();
    }

    // ── Event handlers ───────────────────────────────────────────────────────

    private void PART_ComboBox_GotFocus(object sender, RoutedEventArgs e) => UpdateBorderState();

    private void PART_ComboBox_LostFocus(object sender, RoutedEventArgs e) => UpdateBorderState();

    private void PART_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Sync back to DPs
        if (SelectedItem != PART_ComboBox.SelectedItem)
            SelectedItem = PART_ComboBox.SelectedItem;

        if (SelectedValue != PART_ComboBox.SelectedValue)
            SelectedValue = PART_ComboBox.SelectedValue;

        UpdatePlaceholder();
    }

    // ── State helpers ────────────────────────────────────────────────────────

    private void UpdatePlaceholder()
    {
        PART_Placeholder.Visibility = PART_ComboBox.SelectedItem == null
            ? Visibility.Visible : Visibility.Collapsed;
    }

    private void UpdateBorderState()
    {
        if (HasError)
        {
            PART_Border.BorderBrush = TryFindBrush("AccentError");
        }
        else if (PART_ComboBox.IsKeyboardFocusWithin)
        {
            PART_Border.BorderBrush = TryFindBrush("AppPrimary");
        }
        else
        {
            PART_Border.BorderBrush = TryFindBrush("BorderInput");
        }

        PART_Error.Visibility = HasError ? Visibility.Visible : Visibility.Collapsed;
    }

    private void UpdateHelpVisibility()
    {
        PART_Help.Visibility = !HasError && !string.IsNullOrEmpty(HelpText)
            ? Visibility.Visible : Visibility.Collapsed;
    }

    private Brush TryFindBrush(string key)
    {
        var resource = TryFindResource(key);
        return resource as Brush ?? Brushes.Gray;
    }
}
