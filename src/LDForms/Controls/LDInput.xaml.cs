using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LD.FormsX.Controls;

public partial class LDInput : UserControl
{
    // ── Dependency Properties ────────────────────────────────────────────────

    public static readonly DependencyProperty LabelProperty =
        DependencyProperty.Register(nameof(Label), typeof(string), typeof(LDInput),
            new PropertyMetadata(string.Empty, OnLabelChanged));

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(LDInput),
            new FrameworkPropertyMetadata(string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnTextPropertyChanged));

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(LDInput),
            new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty HelpTextProperty =
        DependencyProperty.Register(nameof(HelpText), typeof(string), typeof(LDInput),
            new PropertyMetadata(string.Empty, OnHelpTextChanged));

    public static readonly DependencyProperty HasErrorProperty =
        DependencyProperty.Register(nameof(HasError), typeof(bool), typeof(LDInput),
            new PropertyMetadata(false, OnHasErrorChanged));

    public static readonly DependencyProperty ErrorTextProperty =
        DependencyProperty.Register(nameof(ErrorText), typeof(string), typeof(LDInput),
            new PropertyMetadata(string.Empty, OnHasErrorChanged));

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(LDInput),
            new PropertyMetadata(false, OnIsReadOnlyChanged));

    public static readonly DependencyProperty MaxLengthProperty =
        DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(LDInput),
            new PropertyMetadata(0, OnMaxLengthChanged));

    public static readonly DependencyProperty InputHeightProperty =
        DependencyProperty.Register(nameof(InputHeight), typeof(double), typeof(LDInput),
            new PropertyMetadata(36d));

    public static readonly DependencyProperty AcceptsReturnProperty =
        DependencyProperty.Register(nameof(AcceptsReturn), typeof(bool), typeof(LDInput),
            new PropertyMetadata(false));

    public static readonly DependencyProperty AcceptsTabProperty =
        DependencyProperty.Register(nameof(AcceptsTab), typeof(bool), typeof(LDInput),
            new PropertyMetadata(false));

    public static readonly DependencyProperty InputTextWrappingProperty =
        DependencyProperty.Register(nameof(InputTextWrapping), typeof(TextWrapping), typeof(LDInput),
            new PropertyMetadata(TextWrapping.NoWrap));

    public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
        DependencyProperty.Register(nameof(VerticalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(LDInput),
            new PropertyMetadata(ScrollBarVisibility.Hidden));

    public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty =
        DependencyProperty.Register(nameof(HorizontalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(LDInput),
            new PropertyMetadata(ScrollBarVisibility.Disabled));

    public static readonly DependencyProperty TextVerticalAlignmentProperty =
        DependencyProperty.Register(nameof(TextVerticalAlignment), typeof(VerticalAlignment), typeof(LDInput),
            new PropertyMetadata(VerticalAlignment.Center));

    // ── CLR wrappers ────────────────────────────────────────────────────────

    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
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

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public double InputHeight
    {
        get => (double)GetValue(InputHeightProperty);
        set => SetValue(InputHeightProperty, value);
    }

    public bool AcceptsReturn
    {
        get => (bool)GetValue(AcceptsReturnProperty);
        set => SetValue(AcceptsReturnProperty, value);
    }

    public bool AcceptsTab
    {
        get => (bool)GetValue(AcceptsTabProperty);
        set => SetValue(AcceptsTabProperty, value);
    }

    public TextWrapping InputTextWrapping
    {
        get => (TextWrapping)GetValue(InputTextWrappingProperty);
        set => SetValue(InputTextWrappingProperty, value);
    }

    public ScrollBarVisibility VerticalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
        set => SetValue(VerticalScrollBarVisibilityProperty, value);
    }

    public ScrollBarVisibility HorizontalScrollBarVisibility
    {
        get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
        set => SetValue(HorizontalScrollBarVisibilityProperty, value);
    }

    public VerticalAlignment TextVerticalAlignment
    {
        get => (VerticalAlignment)GetValue(TextVerticalAlignmentProperty);
        set => SetValue(TextVerticalAlignmentProperty, value);
    }

    // ── Exposes inner TextBox for WpfGridFilter compatibility ────────────────
    public TextBox TextBoxElement => PART_TextBox;

    // ── Constructor ─────────────────────────────────────────────────────────

    public LDInput()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            SyncTextBoxToDP();
            UpdatePlaceholder();
            UpdateBorderState();
            UpdateHelpVisibility();
        };
    }

    // ── DP callbacks ────────────────────────────────────────────────────────

    private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDInput ctrl)
            ctrl.PART_Label.Visibility = string.IsNullOrEmpty(e.NewValue as string)
                ? Visibility.Collapsed : Visibility.Visible;
    }

    private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDInput ctrl)
        {
            ctrl.SyncTextBoxToDP();
            ctrl.UpdatePlaceholder();
        }
    }

    private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDInput ctrl)
        {
            ctrl.UpdateBorderState();
            ctrl.UpdateHelpVisibility();
        }
    }

    private static void OnHelpTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDInput ctrl)
            ctrl.UpdateHelpVisibility();
    }

    private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDInput ctrl)
            ctrl.PART_TextBox.IsReadOnly = (bool)e.NewValue;
    }

    private static void OnMaxLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is LDInput ctrl)
            ctrl.PART_TextBox.MaxLength = (int)e.NewValue;
    }

    // ── Event handlers ───────────────────────────────────────────────────────

    private void PART_TextBox_GotFocus(object sender, RoutedEventArgs e) => UpdateBorderState();

    private void PART_TextBox_LostFocus(object sender, RoutedEventArgs e) => UpdateBorderState();

    private void PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Sync from inner TextBox back to the DP (for two-way binding)
        var newText = PART_TextBox.Text;
        if (Text != newText)
            Text = newText;

        UpdatePlaceholder();
    }

    // ── State helpers ────────────────────────────────────────────────────────

    private void SyncTextBoxToDP()
    {
        if (PART_TextBox.Text != Text)
            PART_TextBox.Text = Text ?? string.Empty;
    }

    private void UpdatePlaceholder()
    {
        PART_Placeholder.Visibility = string.IsNullOrEmpty(PART_TextBox.Text)
            ? Visibility.Visible : Visibility.Collapsed;
    }

    private void UpdateBorderState()
    {
        if (HasError)
        {
            PART_Border.BorderBrush = TryFindBrush("AccentError");
        }
        else if (PART_TextBox.IsKeyboardFocusWithin)
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
