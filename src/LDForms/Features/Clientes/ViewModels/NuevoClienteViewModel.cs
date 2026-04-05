using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.Requests.Client;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Clientes.ViewModels;

public partial class NuevoClienteViewModel : ObservableObject
{
    private readonly ClientService _clientService;

    private int? _editClientId;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nuevo cliente";

    [ObservableProperty]
    private bool isSaving;

    // ── Información Comercial ──
    [ObservableProperty]
    private string clientIdText = "";

    [ObservableProperty]
    private string commercialName = "";

    [ObservableProperty]
    private string commercialAddress = "";

    [ObservableProperty]
    private string commercialNeighborhood = "";

    [ObservableProperty]
    private string commercialCity = "";

    [ObservableProperty]
    private string commercialZipCode = "";

    [ObservableProperty]
    private string commercialPhone = "";

    [ObservableProperty]
    private bool isActive;

    [ObservableProperty]
    private bool isProvider;

    // ── Información Fiscal ──
    [ObservableProperty]
    private string businessName = "";

    [ObservableProperty]
    private string rfc = "";

    [ObservableProperty]
    private string fiscalAddress = "";

    [ObservableProperty]
    private string fiscalNeighborhood = "";

    [ObservableProperty]
    private string fiscalCity = "";

    [ObservableProperty]
    private string fiscalZipCode = "";

    [ObservableProperty]
    private string fiscalEmail = "";

    [ObservableProperty]
    private string fiscalPhone = "";

    public NuevoClienteViewModel(ClientService clientService)
    {
        _clientService = clientService;
    }

    public void SetClient(ClientDto client)
    {
        _editClientId = client.Id;
        HeaderTitle = "Editar cliente";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (_editClientId != null)
            await CargarDatosAsync();
    }

    private async Task CargarDatosAsync()
    {
        try
        {
            var response = await _clientService.GetClientById(_editClientId ?? 0);

            if (!response.IsSuccess || response.Data == null)
            {
                DialogHelper.ShowError(response.Message ?? "No se pudo cargar el cliente.");
                return;
            }

            var c = response.Data;

            ClientIdText = c.ClientId?.ToString() ?? "";
            CommercialName = c.CommercialName ?? "";
            CommercialAddress = c.CommercialAddress ?? "";
            CommercialNeighborhood = c.Neightbourhoud ?? "";
            CommercialCity = c.City ?? "";
            CommercialZipCode = c.ZipCode ?? "";
            CommercialPhone = c.Phone ?? "";
            IsActive = c.IsActive;
            IsProvider = c.IsProvider;

            BusinessName = c.FiscalData?.BusinessName ?? "";
            Rfc = c.FiscalData?.Rfc ?? "";
            FiscalAddress = c.FiscalData?.FiscalAddress ?? "";
            FiscalNeighborhood = c.FiscalData?.Neightbourhoud ?? "";
            FiscalCity = c.FiscalData?.City ?? "";
            FiscalZipCode = c.FiscalData?.ZipCode ?? "";
            FiscalEmail = c.FiscalData?.Email ?? "";
            FiscalPhone = c.FiscalData?.Phone ?? "";
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    private bool HasFiscalData()
    {
        return !string.IsNullOrWhiteSpace(BusinessName) ||
               !string.IsNullOrWhiteSpace(Rfc) ||
               !string.IsNullOrWhiteSpace(FiscalAddress) ||
               !string.IsNullOrWhiteSpace(FiscalNeighborhood) ||
               !string.IsNullOrWhiteSpace(FiscalCity) ||
               !string.IsNullOrWhiteSpace(FiscalZipCode) ||
               !string.IsNullOrWhiteSpace(FiscalEmail) ||
               !string.IsNullOrWhiteSpace(FiscalPhone);
    }

    private ClientRequest BuildRequest() => new()
    {
        ClientId = _editClientId ?? 0,
        CommercialName = CommercialName.Trim(),
        CommercialAddress = CommercialAddress.Trim(),
        Neightbourhoud = CommercialNeighborhood.Trim(),
        City = CommercialCity.Trim(),
        ZipCode = CommercialZipCode.Trim(),
        Phone = CommercialPhone.Trim(),
        IsActive = IsActive,
        IsProvider = IsProvider,
        FiscalData = HasFiscalData()
            ? new ClientFiscalDataRequest
            {
                BusinessName = BusinessName.Trim(),
                Rfc = Rfc.Trim(),
                FiscalAddress = FiscalAddress.Trim(),
                Neightbourhoud = FiscalNeighborhood.Trim(),
                City = FiscalCity.Trim(),
                ZipCode = FiscalZipCode.Trim(),
                Email = FiscalEmail.Trim(),
                Phone = FiscalPhone.Trim()
            }
            : null
    };

    [RelayCommand]
    public async Task SaveAsync()
    {
        try
        {
            IsSaving = true;
            var request = BuildRequest();

            var result = _editClientId != null
                ? await _clientService.UpdateClient(_editClientId.Value, request)
                : await _clientService.CreateClient(request);

            if (result.IsSuccess)
            {
                DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
            }
            else
            {
                DialogHelper.ShowError(result.ErrorMessage ?? "Ocurrió un error al guardar.");
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
