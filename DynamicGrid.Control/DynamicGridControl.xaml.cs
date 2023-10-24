using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DynamicGrid.Control.Helpers;
using DynamicGrid.Control.Models;

namespace DynamicGrid.Control;

public partial class DynamicGridControl : UserControl
{
    public DynamicGridControl()
    {
        InitializeComponent();
    }

    #region Properties

    public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.Register(
        nameof(ColumnCount),
        typeof(int),
        typeof(DynamicGridControl), 
        new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, UpdateGridColumns));

    public static readonly DependencyProperty RowCountProperty = DependencyProperty.Register(
        nameof(RowCount), 
        typeof(int),
        typeof(DynamicGridControl),
        new FrameworkPropertyMetadata(4, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, UpdateGridRows));

    public static readonly DependencyProperty IsConfigureModeProperty = DependencyProperty.Register(
        nameof(IsConfigureMode), 
        typeof(bool), 
        typeof(DynamicGridControl),
        new PropertyMetadata(true));

    public static readonly DependencyProperty TemplateSelectorProperty = DependencyProperty.Register(
        nameof(TemplateSelector), 
        typeof(DataTemplateSelector), 
        typeof(DynamicGridControl),
        new PropertyMetadata(default(DataTemplateSelector)));

    public static readonly DependencyProperty GridGapProperty = DependencyProperty.Register(
        nameof(GridGap), 
        typeof(double),
        typeof(DynamicGridControl),
        new PropertyMetadata(5.0));

    public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register(
        nameof(HoverColor), 
        typeof(Brush), 
        typeof(DynamicGridControl), new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#535454")));

    public static readonly DependencyProperty FlashingColorProperty = DependencyProperty.Register(
        nameof(FlashingColor),
        typeof(Brush), 
        typeof(DynamicGridControl),
        new ((SolidColorBrush)new BrushConverter().ConvertFrom("#76bfe3")));

    public static readonly DependencyProperty GridLinesBrushProperty = DependencyProperty.Register(
        nameof(GridLinesBrush),
        typeof(Brush),
        typeof(DynamicGridControl),
        new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#BCBCBC")));

    public Brush GridLinesBrush
    {
        get => (Brush)GetValue(GridLinesBrushProperty);
        set => SetValue(GridLinesBrushProperty, value);
    }

    /// <summary>
    /// the color of the flashing borders the default is #76bfe3
    /// </summary>
    public Brush FlashingColor
    {
        get => (Brush)GetValue(FlashingColorProperty);
        set => SetValue(FlashingColorProperty, value);
    }

    /// <summary>
    /// the color of the squares when your in configure mode and
    /// the mouse is over it
    /// </summary>
    public Brush HoverColor
    {
        get => (Brush)GetValue(HoverColorProperty);
        set => SetValue(HoverColorProperty, value);
    }

    /// <summary>
    /// the amount of pixels between objects, the default is 5
    /// </summary>
    public double GridGap
    {
        get => (double)GetValue(GridGapProperty);
        set => SetValue(GridGapProperty, value);
    }

    /// <summary>
    /// Selects the template that dis displayed for the  items in the
    /// grid
    /// </summary>
    public DataTemplateSelector TemplateSelector
    {
        get => (DataTemplateSelector)GetValue(TemplateSelectorProperty);
        set => SetValue(TemplateSelectorProperty, value);
    }

    /// <summary>
    /// True if in configure mode and the grid lines are showing
    /// </summary>
    public bool IsConfigureMode
    {
        get => (bool)GetValue(IsConfigureModeProperty);
        set => SetValue(IsConfigureModeProperty, value);
    }

    /// <summary>
    /// The amount of row that are in the grid
    /// </summary>
    public int RowCount
    {
        get => (int)GetValue(RowCountProperty);
        set => SetValue(RowCountProperty, value);
    }

    /// <summary>
    /// the amount of columns that are int the grid
    /// </summary>
    public int ColumnCount
    {
        get => (int)GetValue(ColumnCountProperty);
        set => SetValue(ColumnCountProperty, value);
    }

    #endregion

    #region Attached Properties

    public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached(
        "Rows", typeof(int), typeof(DynamicGridControl), new PropertyMetadata(default(int)));

    public static void SetRows(DependencyObject element, int value)
    {
        element.SetValue(RowsProperty, value);
    }

    public static int GetRows(DependencyObject element)
    {
        return (int)element.GetValue(RowsProperty);
    }

    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached(
        "Columns", typeof(int), typeof(DynamicGridControl), new PropertyMetadata(default(int)));

    public static void SetColumns(DependencyObject element, int value)
    {
        element.SetValue(ColumnsProperty, value);
    }

    public static int GetColumns(DependencyObject element)
    {
        return (int)element.GetValue(ColumnsProperty);
    }
    
    private static readonly DependencyProperty AddDirectionProperty = DependencyProperty.RegisterAttached(
        "AddDirection", typeof(AddDirection), typeof(DynamicGridControl), new PropertyMetadata(default(AddDirection)));

    private static void SetAddDirection(DependencyObject element, AddDirection value)
    {
        element.SetValue(AddDirectionProperty, value);
    }

    private static AddDirection GetAddDirection(DependencyObject element)
    {
        return (AddDirection)element.GetValue(AddDirectionProperty);
    }

    #endregion

    private void HandleLoaded(object sender, RoutedEventArgs e)
    {
        InitConfigureGrid();
    }

    private void InitConfigureGrid()
    {
        var style = (Style)FindResource("AddGridItemLabel");
        var top = new Label(){Style = style, Height = ConfigureButtonWidth, Width = ConfigureGrid.ActualWidth };
        var right = new Label(){ Style = style, Width = ConfigureButtonWidth, Height = ConfigureGrid.ActualHeight};
        var left = new Label(){ Style = style, Width = ConfigureButtonWidth, Height = ConfigureGrid.ActualHeight};
        var bottom = new Label() { Style = style , Height = ConfigureButtonWidth, Width = ConfigureGrid.ActualWidth};

        SetAddDirection(top, AddDirection.Top);
        SetAddDirection(right, AddDirection.Right);
        SetAddDirection(left, AddDirection.Left);
        SetAddDirection(bottom, AddDirection.Bottom);
        
        ConfigureGrid.Children.Add(top);
        ConfigureGrid.Children.Add(bottom);
        ConfigureGrid.Children.Add(right);
        ConfigureGrid.Children.Add(left);
        Canvas.SetLeft(right, ConfigureGrid.ActualWidth - ConfigureButtonWidth);
        Canvas.SetLeft(left, 0);
        Canvas.SetLeft(top, 0);
        Canvas.SetLeft(bottom, 0);
        Canvas.SetTop(right, 0);
        Canvas.SetTop(left, 0);
        Canvas.SetTop(top, 0);
        Canvas.SetTop(bottom, ConfigureGrid.ActualHeight - ConfigureButtonWidth);

        UpdateGridLines();
    }

    private static void UpdateGridColumns(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DynamicGridControl gridControl)
        {
            gridControl.UpdateColumns((int)e.OldValue, (int)e.NewValue);
        }
    }

    private static void UpdateGridRows(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DynamicGridControl gridControl)
        {
            gridControl.UpdateRows((int)e.OldValue, (int)e.NewValue);
        }
    }

    private void UpdateRows(int oldValue, int newValue)
    {
        var information = ConfigureGrid
            .CalculateLocationInformation(ConfigureButtonWidth,
                GridGap,
                ColumnCount,
                newValue);
        ConfigureGrid
            .UpdateGridObjectCount(GridObjectType.Row,
                newValue,
                ConfigureBorderStyle(),
                information);
    }

    private void UpdateColumns(int oldValue, int newValue)
    {
        var information = ConfigureGrid
            .CalculateLocationInformation(ConfigureButtonWidth,
                GridGap,
                ColumnCount,
                newValue);
        ConfigureGrid
            .UpdateGridObjectCount(GridObjectType.Row,
                newValue,
                ConfigureBorderStyle(),
                information);
    }

    private void UpdateGridLines()
    {
        var information = ConfigureGrid.CalculateLocationInformation(
            ConfigureButtonWidth,
            GridGap,
            ColumnCount,
            RowCount);
        var borderStyle = ConfigureBorderStyle();

        ConfigureGrid.InitDynamicGrid( information, borderStyle);
    }

    private Style ConfigureBorderStyle()
    {
        return (Style)FindResource("DashedBorder");
    }

    private const double ConfigureButtonWidth = 20.0;
}