using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Proyectos.ViewModels;

public partial class NuevoProyectoViewModel : ObservableObject
{
    private readonly ProjectService _projectService;
    private readonly LookupService _lookupService;

    private int? _editProjectId;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nuevo proyecto";

    [ObservableProperty]
    private bool isSaving;

    // ── Combos source ──
    [ObservableProperty]
    private List<DropDownDto> clientsSource = [];

    [ObservableProperty]
    private List<DropDownDto> warehousesSource = [];
    [ObservableProperty]
    private List<DropDownDto> systemFieldsSource = [];
    [ObservableProperty]
    private List<DropDownDto> scanTypeSource = [];
    [ObservableProperty]
    private List<DropDownDto> saveScanTypeSource = [];
    private List<DropDownDto> _unitsSource = [];
    public List<DropDownDto> UnitsSource
    {
        get => _unitsSource;
        set => SetProperty(ref _unitsSource, value);
    }
    // ── Datos generales ──
    [ObservableProperty]
    private string? selectedClientId;

    [ObservableProperty]
    private string? selectedWarehouseId;

    [ObservableProperty]
    private string projectName = "";

    [ObservableProperty]
    private bool isActive;

    [ObservableProperty]
    private bool autoPicking;

    // ── Tipo de almacenamiento ──
    [ObservableProperty]
    private int? storageTypeId;

    partial void OnStorageTypeIdChanged(int? value)
    {
        OnPropertyChanged(nameof(IsFifo));
        OnPropertyChanged(nameof(IsLifo));
        OnPropertyChanged(nameof(IsLote));
        OnPropertyChanged(nameof(IsCaducidad));
    }

    public bool IsFifo
    {
        get => StorageTypeId == 1;
        set { if (value) StorageTypeId = 1; }
    }

    public bool IsLifo
    {
        get => StorageTypeId == 2;
        set { if (value) StorageTypeId = 2; }
    }

    public bool IsLote
    {
        get => StorageTypeId == 3;
        set { if (value) StorageTypeId = 3; }
    }

    public bool IsCaducidad
    {
        get => StorageTypeId == 4;
        set { if (value) StorageTypeId = 4; }
    }

    // ── Opciones ──
    [ObservableProperty]
    private bool allowsBackorder;

    [ObservableProperty]
    private bool isDistributionArea;

    [ObservableProperty]
    private bool isFiscalWarehouse;

    [ObservableProperty]
    private bool allowsOversizedItems;

    [ObservableProperty]
    private bool requiresLabels;

    // ── Unidades ──
    [ObservableProperty]
    private string? entrada;

    [ObservableProperty]
    private string? storageArea;

    [ObservableProperty]
    private string? reworkArea;

    [ObservableProperty]
    private string? salida;

    // ── Notificaciones ──
    [ObservableProperty]
    private bool receiptNotificationEnabled;

    [ObservableProperty]
    private string receiptNotificationMethod = "";

    [ObservableProperty]
    private bool shipmentNotificationEnabled;

    [ObservableProperty]
    private string shipmentNotificationMethod = "";

    [ObservableProperty]
    private string tiempoNormal = "";

    [ObservableProperty]
    private string tiempoUrgente = "";

    // ── Prefijos ──
    [ObservableProperty]
    private int asnNumber = 0;

    [ObservableProperty]
    private string asnPrefix = "";

    [ObservableProperty]
    private string kittingNumber = "";

    [ObservableProperty]
    private string kittingPrefix = "";

    [ObservableProperty]
    private string doNumber = "";

    [ObservableProperty]
    private string doPrefix = "";

    [ObservableProperty]
    private bool reciveRequired;

    // ── Configuraciones de escaneo ──
    public ObservableCollection<DropDownDto> AvailableSystemFields { get; } = [];
    public ObservableCollection<ScanConfigurationRequest> ScanConfigurations { get; } = [];

    [ObservableProperty]
    private DropDownDto? selectedSystemField;

    [ObservableProperty]
    private ScanConfigurationRequest? selectedScanConfig;

    public NuevoProyectoViewModel(ProjectService projectService, LookupService lookupService)
    {
        _projectService = projectService;
        _lookupService = lookupService;
    }

    public void SetProject(ProjectDto project)
    {
        _editProjectId = project.ProjectId;
        HeaderTitle = "Editar proyecto";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        await CargarCombosAsync();
        if (_editProjectId != null)
            await CargarDatosAsync();
    }

    private async Task CargarCombosAsync()
    {
        var clientes = await _lookupService.GetClientLookup();
        var almacenes = await _lookupService.GetWarehouseLookup();
        var systemFields = await _lookupService.GetSystemFieldLookup();
        var scanSaves = await _lookupService.GetScanSaveTypeLookup();
        var scanTypes = await _lookupService.GetScanTypeLookup();
        var units = await _lookupService.GetUnitLookup();


        if (clientes.IsSuccess)
            ClientsSource = clientes.Data ?? [];

        if (almacenes.IsSuccess)
            WarehousesSource = almacenes.Data ?? [];

        if (systemFields.IsSuccess)
        {
            SystemFieldsSource = systemFields.Data ?? [];
            PopulateAvailableFields();
        }
        if (scanSaves.IsSuccess)
        {
            SaveScanTypeSource = scanSaves.Data ?? [];
            PopulateAvailableFields();
        }
        if (scanTypes.IsSuccess)
        {
            ScanTypeSource = scanTypes.Data ?? [];
            PopulateAvailableFields();
        }

        if (units.IsSuccess)
            UnitsSource = units.Data ?? [];
    }

    private void PopulateAvailableFields()
    {
        var usedIds = ScanConfigurations.Select(s => s.SystemFieldId).ToHashSet();
        AvailableSystemFields.Clear();
        foreach (var field in SystemFieldsSource.Where(f => !usedIds.Contains(int.TryParse(f.Key, out var id) ? id : -1)))
            AvailableSystemFields.Add(field);
    }

    [RelayCommand]
    private void AgregarCampo()
    {
        if (SelectedSystemField is null) return;

        ScanConfigurations.Add(new ScanConfigurationRequest
        {
            SystemFieldId = int.TryParse(SelectedSystemField.Key, out var id) ? id : 0,
            SystemFieldName = SelectedSystemField.Value ?? "",

            Order = ScanConfigurations.Count + 1
        });

        AvailableSystemFields.Remove(SelectedSystemField);
        SelectedSystemField = null;
    }

    [RelayCommand]
    private void QuitarCampo()
    {
        if (SelectedScanConfig is null) return;

        var removed = SelectedScanConfig;
        ScanConfigurations.Remove(removed);

        // Devolver al listado disponible
        var original = SystemFieldsSource.FirstOrDefault(f => f.Key == removed.SystemFieldId.ToString());
        if (original is not null)
            AvailableSystemFields.Add(original);

        // Reordenar
        int i = 1;
        foreach (var sc in ScanConfigurations)
            sc.Order = i++;

        SelectedScanConfig = null;
    }

    private async Task CargarDatosAsync()
    {
        try
        {
            var response = await _projectService.GetProjectById(_editProjectId ?? 0);

            if (!response.IsSuccess)
            {
                DialogHelper.ShowError(response.Message);
                return;
            }

            var p = response.Data;

            SelectedClientId = p.ClientId?.ToString();
            SelectedWarehouseId = p.WarehouseId?.ToString();
            ProjectName = p.ProjectName ?? "";

            IsActive = p.IsActive;
            AutoPicking = p.AutoPicking;

            StorageTypeId = p.StorageTypeId;

            AllowsBackorder = p.AllowsBackorder;
            IsDistributionArea = p.IsDistributionArea;
            IsFiscalWarehouse = p.IsFiscalWarehouse;
            AllowsOversizedItems = p.AllowsOversizedItems;
            RequiresLabels = p.RequiresLabels;

            Entrada = p.Entrada;
            StorageArea = p.StorageArea;
            ReworkArea = p.ReworkArea;
            Salida = p.Salida;

            ReceiptNotificationEnabled = p.ReceiptNotificationEnabled;
            ReceiptNotificationMethod = p.ReceiptNotificationMethod ?? "";
            ShipmentNotificationEnabled = p.ShipmentNotificationEnabled;
            ShipmentNotificationMethod = p.ShipmentNotificationMethod ?? "";

            TiempoNormal = p.NormalHrs?.ToString() ?? "";
            TiempoUrgente = p.UrgentHrs?.ToString() ?? "";

            AsnNumber = p.AsnNumber.GetValueOrDefault(0);
            AsnPrefix = p.AsnPrefix ?? "";
            KittingNumber = p.KittingNumber ?? "";
            KittingPrefix = p.KittingPrefix ?? "";
            DoNumber = p.DoNumber ?? p.DeliveryOrderNumber ?? "";
            DoPrefix = p.DoPrefix ?? p.DeliveryOrderPrefix ?? "";
            ReciveRequired = p.ReciveRequired;

            // Cargar configuraciones de escaneo
            ScanConfigurations.Clear();
            foreach (var sc in p.ScanConfigurations)
                ScanConfigurations.Add(sc);
            PopulateAvailableFields();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    private ProjectRequest BuildRequest() => new()
    {
        ProjectId = _editProjectId,
        ClientId = int.TryParse(SelectedClientId, out int cId) ? cId : null,
        WarehouseId = int.TryParse(SelectedWarehouseId, out int wId) ? wId : null,
        ProjectName = ProjectName.Trim(),
        IsActive = IsActive,
        AutoPicking = AutoPicking,

        StorageTypeId = StorageTypeId,

        AllowsBackorder = AllowsBackorder,
        IsDistributionArea = IsDistributionArea,
        IsFiscalWarehouse = IsFiscalWarehouse,
        AllowsOversizedItems = AllowsOversizedItems,
        RequiresLabels = RequiresLabels,

        Entrada = string.IsNullOrEmpty(Entrada) ? null : Entrada,
        StorageArea = string.IsNullOrEmpty(StorageArea) ? null : StorageArea,
        ReworkArea = string.IsNullOrEmpty(ReworkArea) ? null : ReworkArea,
        Salida = string.IsNullOrEmpty(Salida) ? null : Salida,

        ReceiptNotificationEnabled = ReceiptNotificationEnabled,
        ReceiptNotificationMethod = ReceiptNotificationMethod,
        ShipmentNotificationEnabled = ShipmentNotificationEnabled,
        ShipmentNotificationMethod = ShipmentNotificationMethod,
        InternalNotificationEnabled = false,
        InternalNotificationMethod = null,

        NormalHrs = decimal.TryParse(TiempoNormal, out decimal n) ? n : null,
        UrgentHrs = decimal.TryParse(TiempoUrgente, out decimal u) ? u : null,

        AsnNumber = AsnNumber,
        AsnPrefix = AsnPrefix,
        KittingNumber = KittingNumber,
        KittingPrefix = KittingPrefix,
        DoNumber = DoNumber,
        DoPrefix = DoPrefix,
        DeliveryOrderNumber = DoNumber,
        DeliveryOrderPrefix = DoPrefix,
        ReciveRequired = ReciveRequired,
        ScanConfigurations = ScanConfigurations.ToList(),
    };

    [RelayCommand]
    public async Task SaveAsync()
    {
        try
        {
            IsSaving = true;
            var request = BuildRequest();

            var result = _editProjectId != null
                ? await _projectService.UpdateProject(_editProjectId.Value, request)
                : await _projectService.CreateProject(request);

            if (result.IsSuccess)
            {
                DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
            }
            else
            {
                DialogHelper.ShowError(result.ErrorMessage,result.Message);
            }
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsSaving = false;
        }
    }
}
