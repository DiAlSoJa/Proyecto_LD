using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LD.FormsX.Views.DatabaseDiagram;

public partial class DatabaseDiagramView : UserControl
{
    private const double CardWidth = 250;
    private const double CardHeight = 190;
    private const double XGap = 92;
    private const double YGap = 78;
    private const int Columns = 5;

    private readonly Dictionary<string, Point> _tablePositions = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<UIElement> _relationElements = [];
    private IReadOnlyList<RelationDef> _visibleRelations = [];
    private string? _draggingTable;
    private Point _dragOffset;

    public DatabaseDiagramView()
    {
        InitializeComponent();
        LoadGroupFilters();
        BuildDiagram();
    }

    private void BuildDiagram()
    {
        DiagramCanvas.Children.Clear();
        _tablePositions.Clear();
        _relationElements.Clear();

        var selectedGroup = GroupFilterComboBox.SelectedValue?.ToString() ?? "Todos";
        var allTables = BuildTables();
        var allRelations = BuildRelations();
        var tables = FilterTables(allTables, allRelations, selectedGroup);
        var visibleTables = tables.Select(x => x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        _visibleRelations = allRelations
            .Where(x => visibleTables.Contains(x.From) && visibleTables.Contains(x.To))
            .ToList();
        PositionTables(tables);

        foreach (var relation in _visibleRelations)
        {
            DrawRelation(relation.From, relation.To, relation.IsLogical, relation.Label);
        }

        foreach (var table in tables)
        {
            DrawTable(table);
        }
    }

    private void LoadGroupFilters()
    {
        GroupFilterComboBox.ItemsSource = new[]
        {
            new GroupFilter("Todos", "Todos"),
            new GroupFilter("WMS", "WMS / Inventario"),
            new GroupFilter("Catalogos", "Catalogos"),
            new GroupFilter("ASN", "ASN / Recepcion"),
            new GroupFilter("Seguridad", "Seguridad / Patio"),
            new GroupFilter("Checklist", "Checklist / Equipos"),
            new GroupFilter("Operaciones", "Operaciones"),
            new GroupFilter("Permisos", "Permisos / Sistema")
        };
        GroupFilterComboBox.SelectedValue = "Todos";
    }

    private static IReadOnlyList<TableDef> FilterTables(
        IReadOnlyList<TableDef> allTables,
        IReadOnlyList<RelationDef> allRelations,
        string selectedGroup)
    {
        if (string.Equals(selectedGroup, "Todos", StringComparison.OrdinalIgnoreCase))
            return allTables;

        var selected = allTables
            .Where(x => x.Group.Equals(selectedGroup, StringComparison.OrdinalIgnoreCase))
            .Select(x => x.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var relation in allRelations)
        {
            if (selected.Contains(relation.From))
                selected.Add(relation.To);
            if (selected.Contains(relation.To))
                selected.Add(relation.From);
        }

        return allTables.Where(x => selected.Contains(x.Name)).ToList();
    }

    private static IReadOnlyList<TableDef> BuildTables() =>
    [
        new("Client", "Cliente", "Catalogos", "#0F766E", ["PK ClientId : int", "CommercialName : string", "City : string", "IsProvider : bool"]),
        new("ClientFiscalData", "Datos fiscales", "Catalogos", "#0F766E", ["PK ClientFiscalDataId : int", "FK ClientId : int", "Rfc : string", "BusinessName : string"]),
        new("ClientContact", "Contactos", "Catalogos", "#0F766E", ["PK ClientContactId : int", "FK ClientId : int", "Name : string", "Email : string"]),
        new("Warehouse", "Almacen", "WMS", "#0891B2", ["PK WarehouseId : int", "WarehouseName : string", "City : string", "IsProduction : bool"]),
        new("Location", "Ubicacion", "WMS", "#0891B2", ["PK LocationId : int", "FK WarehouseId : int", "LocationCode : string", "StatusId : string?"]),

        new("Project", "Proyecto", "WMS", "#2563EB", ["PK ProjectId : int", "FK ClientId : int", "FK WarehouseId : int", "FK LocationId : int?", "ScanRequired : bool", "UniqueLot : bool"]),
        new("StorageType", "Tipo almacenamiento", "Catalogos", "#2563EB", ["PK StorageTypeId : int", "StorageTypeName : string"]),
        new("ScanConfiguration", "Configuracion escaneo", "WMS", "#2563EB", ["PK ScanConfigurationId : int", "FK ProjectId : int", "Obligatorio : bool", "FK ScanTypeId : int", "FK ScanSaveTypeId : int"]),
        new("ScanType", "Tipo escaneo", "Catalogos", "#2563EB", ["PK ScanTypeId : int", "Name : string"]),
        new("ScanSaveType", "Tipo guardado", "Catalogos", "#2563EB", ["PK ScanSaveTypeId : int", "Name : string"]),

        new("Product", "Articulo", "Catalogos", "#7C3AED", ["PK ProductId : int", "FK ClientId : int?", "FK ProjectId : int?", "PartNumber : string", "Description : string"]),
        new("Category", "Categoria", "Catalogos", "#7C3AED", ["PK CategoryId : int", "FK ClientId : int", "FK ProjectId : int", "CategoryName : string"]),
        new("Family", "Familia", "Catalogos", "#7C3AED", ["PK FamilyId : int", "FK ClientId : int", "FK ProjectId : int", "FamilyName : string"]),
        new("Units", "Unidades", "Catalogos", "#7C3AED", ["PK UnitIdS : string", "Description : string"]),
        new("Currency", "Moneda", "Catalogos", "#7C3AED", ["PK CurrencyIdS : string", "Description : string"]),

        new("Asn", "ASN", "ASN", "#EA580C", ["PK AsnId : int", "FK ClientId : int", "FK ProjectId : int", "AsnCode : string?", "Status : string?"]),
        new("Detalle de ASN", "Partidas ASN", "ASN", "#EA580C", ["PK AsnDetailId : int", "FK AsnId : int", "FK ProductId : int?", "PartNumber : string", "Quantity : decimal"]),
        new("Detalle de recepción del ASN", "Recepciones ASN", "ASN", "#EA580C", ["PK AsnReceiptDetailId : int", "PalletNumber : int", "FK AsnDetailId : int", "FK LocationId : int?", "StandardId : int?", "ReceivedQuantity : decimal?"]),
        new("StandardLabel", "Standard ID", "ASN", "#EA580C", ["PK StandardId : int", "StandardCode : string", "FK ProductId : int?", "FK ProjectId : int?"]),
        new("StandarIdSequence", "Secuencia Standard ID", "ASN", "#EA580C", ["PK StandarIdSequenceId : int", "Prefix : string", "CurrentNumber : int"]),

        new("AvailableInventory", "Inventario disponible", "WMS", "#16A34A", ["PK AvailableInventoryId : int", "FK ClientId : int", "FK ProjectId : int", "FK ProductId : int?", "FK LocationId : int?", "PalletNumber : int?", "Qty : decimal?"]),
        new("InventoryMovement", "Movimientos", "WMS", "#16A34A", ["PK MovementId : int", "FK ClientId : int", "FK ProjectId : int", "FK ProductId : int?", "PalletNumber : int?", "DocumentType : enum", "MovementType : enum"]),
        new("CyclicInventory", "Inventario ciclico", "WMS", "#16A34A", ["PK CyclicInventoryId : int", "FK WarehouseId : int?", "Status : string"]),
        new("CyclicInventoryDetail", "Detalle inventario ciclico", "WMS", "#16A34A", ["PK CyclicInventoryDetailId : int", "FK CyclicInventoryId : int", "FK LocationId : int?", "CountedQty : decimal?"]),
        new("InventaryStatus", "Status inventario", "Catalogos", "#16A34A", ["PK InventoryStatusIdS, ClientId, ProjectId : composite", "FullName : string"]),

        new("UserWarehouse", "Usuario almacen", "Permisos", "#475569", ["PK UserWarehouseId : int", "FK UserId : string?", "FK WarehouseId : int?"]),
        new("Module", "Modulo", "Permisos", "#475569", ["PK ModuleId : int", "ModuleName : string"]),
        new("Permission", "Permiso", "Permisos", "#475569", ["PK PermissionId : int", "FK ModuleId : int", "PermissionKey : string"]),
        new("RolePermission", "Permisos rol", "Permisos", "#475569", ["PK RolePermissionId : int", "FK RoleId : string", "FK PermissionId : int"]),
        new("SystemField", "Campos sistema", "Permisos", "#475569", ["PK SystemFieldId : int", "FieldName : string"]),

        new("Equipment", "Equipo", "Checklist", "#CA8A04", ["PK EquipmentId : int", "FK EquipmentTypeId : int", "FK EquipmentSupplierId : int?", "SerialNumber : string"]),
        new("EquipmentType", "Tipo equipo", "Checklist", "#CA8A04", ["PK EquipmentTypeId : int", "EquipmentName : string"]),
        new("EquipmentSupplier", "Proveedor equipo", "Checklist", "#CA8A04", ["PK EquipmentSupplierId : int", "SupplierName : string"]),
        new("EquipmentQuestion", "Pregunta checklist", "Checklist", "#CA8A04", ["PK EquipmentQuestionId : int", "FK EquipmentTypeId : int?", "Question : string"]),
        new("EquipmentQuestionDet", "Detalle pregunta", "Checklist", "#CA8A04", ["PK EquipmentQuestionDetId : int", "FK EquipmentQuestionId : int", "Answer : string"]),

        new("Checklist", "Checklist", "Checklist", "#CA8A04", ["PK ChecklistId : int", "FK EquipmentId : int", "FK UserId : string", "ChecklistDate : DateTime"]),
        new("ChecklistAnswer", "Respuesta checklist", "Checklist", "#CA8A04", ["PK ChecklistAnswerId : int", "FK ChecklistId : int", "FK EquipmentQuestionId : int", "Answer : string"]),
        new("ChecklistDefectMark", "Marca defecto", "Checklist", "#CA8A04", ["PK ChecklistDefectMarkId : int", "FK ChecklistAnswerId : int", "X : decimal", "Y : decimal"]),
        new("ChecklistPhoto", "Foto checklist", "Checklist", "#CA8A04", ["PK ChecklistPhotoId : int", "FK ChecklistId : int", "Path : string"]),
        new("DamageReport", "Reporte danos", "Checklist", "#DC2626", ["PK DamageReportId : int", "FK EquipmentId : int?", "FK ProjectId : int?", "ReportDate : DateTime"]),

        new("SecurityRegistration", "Registro seguridad", "Seguridad", "#BE123C", ["PK SecurityRegistrationId : int", "FK VehicleId : int?", "FK DriverId : int?", "FK CortinaId : int?"]),
        new("SecurityTask", "Tarea seguridad", "Seguridad", "#BE123C", ["PK SecurityTaskId : int", "TaskName : string", "Status : string"]),
        new("Vehicle", "Vehiculo", "Seguridad", "#BE123C", ["PK VehicleId : int", "Plates : string", "VehicleNumber : string"]),
        new("Driver", "Chofer", "Seguridad", "#BE123C", ["PK DriverId : int", "Name : string", "Phone : string"]),
        new("Cortina", "Cortina", "Seguridad", "#BE123C", ["PK CortinaId : int", "Name : string", "IsAvailable : bool"]),

        new("OperationalTask", "Tarea operativa", "Operaciones", "#DB2777", ["PK OperationalTaskId : int", "FK WarehouseId : int?", "TaskType : string", "Status : string"]),
        new("PickingZone", "Zona picking", "Operaciones", "#DB2777", ["PK PickingZoneId : int", "FK WarehouseId : int?", "Name : string"]),
        new("Printer", "Impresora", "Operaciones", "#DB2777", ["PK PrinterId : int", "PrinterName : string", "Path : string"]),
        new("Dimensioner", "Dimensionador", "Operaciones", "#DB2777", ["PK DimensionerId : string", "Description : string"]),
        new("DireccionEntrega", "Direccion entrega", "Operaciones", "#DB2777", ["PK DireccionEntregaId : int", "FK ClientId : int?", "Address : string"])
    ];

    private static IReadOnlyList<RelationDef> BuildRelations() =>
    [
        new("Client", "ClientFiscalData", "ClientId"),
        new("Client", "ClientContact", "ClientId"),
        new("Client", "Project", "ClientId"),
        new("Warehouse", "Project", "WarehouseId"),
        new("Warehouse", "Location", "WarehouseId"),
        new("Location", "Project", "LocationId"),
        new("StorageType", "Project", "StorageTypeId"),
        new("Project", "ScanConfiguration", "ProjectId"),
        new("ScanType", "ScanConfiguration", "ScanTypeId"),
        new("ScanSaveType", "ScanConfiguration", "ScanSaveTypeId"),
        new("Client", "Product", "ClientId"),
        new("Project", "Product", "ProjectId"),
        new("Project", "Category", "ProjectId"),
        new("Project", "Family", "ProjectId"),
        new("Client", "Asn", "ClientId"),
        new("Project", "Asn", "ProjectId"),
        new("Asn", "Detalle de ASN", "AsnId"),
        new("Product", "Detalle de ASN", "ProductId"),
        new("Detalle de ASN", "Detalle de recepción del ASN", "AsnDetailId"),
        new("Location", "Detalle de recepción del ASN", "LocationId"),
        new("StandardLabel", "Detalle de recepción del ASN", "StandardId"),
        new("Client", "AvailableInventory", "ClientId"),
        new("Project", "AvailableInventory", "ProjectId"),
        new("Product", "AvailableInventory", "ProductId"),
        new("Location", "AvailableInventory", "LocationId"),
        new("StandardLabel", "AvailableInventory", "StandardId"),
        new("Client", "InventoryMovement", "ClientId"),
        new("Project", "InventoryMovement", "ProjectId"),
        new("Product", "InventoryMovement", "ProductId"),
        new("Location", "InventoryMovement", "LocationId"),
        new("Warehouse", "CyclicInventory", "WarehouseId"),
        new("CyclicInventory", "CyclicInventoryDetail", "CyclicInventoryId"),
        new("Location", "CyclicInventoryDetail", "LocationId"),
        new("Warehouse", "UserWarehouse", "WarehouseId"),
        new("Module", "Permission", "ModuleId"),
        new("Permission", "RolePermission", "PermissionId"),
        new("EquipmentType", "Equipment", "EquipmentTypeId"),
        new("EquipmentSupplier", "Equipment", "EquipmentSupplierId"),
        new("EquipmentType", "EquipmentQuestion", "EquipmentTypeId"),
        new("EquipmentQuestion", "EquipmentQuestionDet", "EquipmentQuestionId"),
        new("Equipment", "Checklist", "EquipmentId"),
        new("Checklist", "ChecklistAnswer", "ChecklistId"),
        new("EquipmentQuestion", "ChecklistAnswer", "EquipmentQuestionId"),
        new("ChecklistAnswer", "ChecklistDefectMark", "ChecklistAnswerId"),
        new("Checklist", "ChecklistPhoto", "ChecklistId"),
        new("Equipment", "DamageReport", "EquipmentId", true),
        new("Project", "DamageReport", "ProjectId", true),
        new("Vehicle", "SecurityRegistration", "VehicleId"),
        new("Driver", "SecurityRegistration", "DriverId"),
        new("Cortina", "SecurityRegistration", "CortinaId"),
        new("Warehouse", "OperationalTask", "WarehouseId"),
        new("Warehouse", "PickingZone", "WarehouseId"),
        new("Client", "DireccionEntrega", "ClientId", true)
    ];

    private void PositionTables(IReadOnlyList<TableDef> tables)
    {
        for (var i = 0; i < tables.Count; i++)
        {
            var col = i % Columns;
            var row = i / Columns;
            _tablePositions[tables[i].Name] = new Point(24 + col * (CardWidth + XGap), 32 + row * (CardHeight + YGap));
        }
    }

    private void DrawRelation(string from, string to, bool isLogical, string label)
    {
        if (!_tablePositions.TryGetValue(from, out var fromPoint) || !_tablePositions.TryGetValue(to, out var toPoint))
            return;

        var start = new Point(fromPoint.X + CardWidth, fromPoint.Y + CardHeight / 2);
        var end = new Point(toPoint.X, toPoint.Y + CardHeight / 2);

        if (end.X < start.X)
        {
            start = new Point(fromPoint.X + CardWidth / 2, fromPoint.Y + CardHeight);
            end = new Point(toPoint.X + CardWidth / 2, toPoint.Y);
        }

        var line = new Line
        {
            X1 = start.X,
            Y1 = start.Y,
            X2 = end.X,
            Y2 = end.Y,
            Stroke = new SolidColorBrush(isLogical ? Color.FromRgb(100, 116, 139) : Color.FromRgb(15, 118, 110)),
            StrokeThickness = 1.6,
            Opacity = 0.7
        };

        if (isLogical)
            line.StrokeDashArray = new DoubleCollection { 4, 3 };

        Panel.SetZIndex(line, 0);
        DiagramCanvas.Children.Insert(0, line);
        _relationElements.Add(line);

        var labelBorder = new Border
        {
            Background = Brushes.White,
            BorderBrush = new SolidColorBrush(Color.FromRgb(203, 213, 225)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(5),
            Padding = new Thickness(5, 2, 5, 2),
            Child = new TextBlock
            {
                Text = label,
                FontSize = 10,
                Foreground = new SolidColorBrush(Color.FromRgb(51, 65, 85))
            }
        };

        Canvas.SetLeft(labelBorder, (start.X + end.X) / 2 - 28);
        Canvas.SetTop(labelBorder, (start.Y + end.Y) / 2 - 12);
        Panel.SetZIndex(labelBorder, 1);
        DiagramCanvas.Children.Insert(Math.Min(1, DiagramCanvas.Children.Count), labelBorder);
        _relationElements.Add(labelBorder);
    }

    private void DrawTable(TableDef table)
    {
        var position = _tablePositions[table.Name];
        var color = (Color)ColorConverter.ConvertFromString(table.Color);

        var header = new Border
        {
            Background = new SolidColorBrush(color),
            CornerRadius = new CornerRadius(8, 8, 0, 0),
            Padding = new Thickness(12, 9, 12, 9),
            Child = new StackPanel
            {
                Children =
                {
                    new TextBlock { Text = table.Name, Foreground = Brushes.White, FontSize = 14, FontWeight = FontWeights.Bold },
                    new TextBlock { Text = table.Meta, Foreground = new SolidColorBrush(Color.FromRgb(226, 232, 240)), FontSize = 11, Margin = new Thickness(0, 2, 0, 0) }
                }
            }
        };

        var columnsPanel = new StackPanel { Margin = new Thickness(12, 10, 12, 12) };
        foreach (var column in table.Columns.Take(7))
        {
            var isKey = column.StartsWith("PK ", StringComparison.OrdinalIgnoreCase)
                || column.StartsWith("FK ", StringComparison.OrdinalIgnoreCase);
            columnsPanel.Children.Add(new TextBlock
            {
                Text = column,
                FontSize = 12,
                Foreground = new SolidColorBrush(isKey ? color : Color.FromRgb(51, 65, 85)),
                FontWeight = isKey ? FontWeights.SemiBold : FontWeights.Normal,
                Margin = new Thickness(0, 3, 0, 3),
                TextTrimming = TextTrimming.CharacterEllipsis
            });
        }

        var card = new Border
        {
            Width = CardWidth,
            Height = CardHeight,
            Background = Brushes.White,
            BorderBrush = new SolidColorBrush(Color.FromRgb(203, 213, 225)),
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(8),
            Child = new StackPanel
            {
                Children = { header, columnsPanel }
            },
            Cursor = Cursors.SizeAll,
            Tag = table.Name,
            ToolTip = "Arrastrar para reacomodar"
        };

        card.MouseLeftButtonDown += TableCard_MouseLeftButtonDown;
        card.MouseMove += TableCard_MouseMove;
        card.MouseLeftButtonUp += TableCard_MouseLeftButtonUp;

        Canvas.SetLeft(card, position.X);
        Canvas.SetTop(card, position.Y);
        Panel.SetZIndex(card, 10);
        DiagramCanvas.Children.Add(card);
    }

    private void TableCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not Border card || card.Tag is not string tableName)
            return;

        var mousePosition = e.GetPosition(DiagramCanvas);
        _draggingTable = tableName;
        _dragOffset = new Point(
            mousePosition.X - Canvas.GetLeft(card),
            mousePosition.Y - Canvas.GetTop(card));

        Panel.SetZIndex(card, 20);
        card.CaptureMouse();
        e.Handled = true;
    }

    private void TableCard_MouseMove(object sender, MouseEventArgs e)
    {
        if (_draggingTable is null || sender is not Border card || !card.IsMouseCaptured)
            return;

        var mousePosition = e.GetPosition(DiagramCanvas);
        var newLeft = Math.Clamp(mousePosition.X - _dragOffset.X, 0, DiagramCanvas.Width - CardWidth);
        var newTop = Math.Clamp(mousePosition.Y - _dragOffset.Y, 0, DiagramCanvas.Height - CardHeight);

        Canvas.SetLeft(card, newLeft);
        Canvas.SetTop(card, newTop);
        _tablePositions[_draggingTable] = new Point(newLeft, newTop);
        RedrawRelations();
        e.Handled = true;
    }

    private void TableCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is not Border card)
            return;

        card.ReleaseMouseCapture();
        Panel.SetZIndex(card, 10);
        _draggingTable = null;
        e.Handled = true;
    }

    private void RedrawRelations()
    {
        foreach (var relationElement in _relationElements)
        {
            DiagramCanvas.Children.Remove(relationElement);
        }

        _relationElements.Clear();

        foreach (var relation in _visibleRelations)
        {
            DrawRelation(relation.From, relation.To, relation.IsLogical, relation.Label);
        }
    }

    private void DiagramScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.Control)
            return;

        ZoomSlider.Value = Math.Clamp(ZoomSlider.Value + (e.Delta > 0 ? 0.08 : -0.08), ZoomSlider.Minimum, ZoomSlider.Maximum);
        e.Handled = true;
    }

    private void ZoomOut_Click(object sender, RoutedEventArgs e)
    {
        ZoomSlider.Value = Math.Max(ZoomSlider.Minimum, ZoomSlider.Value - 0.1);
    }

    private void ZoomIn_Click(object sender, RoutedEventArgs e)
    {
        ZoomSlider.Value = Math.Min(ZoomSlider.Maximum, ZoomSlider.Value + 0.1);
    }

    private void ResetZoom_Click(object sender, RoutedEventArgs e)
    {
        ZoomSlider.Value = 1;
    }

    private void GroupFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DiagramCanvas is null)
            return;

        BuildDiagram();
        DiagramScrollViewer.ScrollToHome();
    }

    private sealed record GroupFilter(string Key, string Name);
    private sealed record TableDef(string Name, string Meta, string Group, string Color, IReadOnlyList<string> Columns);
    private sealed record RelationDef(string From, string To, string Label, bool IsLogical = false);
}
