using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.Constants;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Clientes.ViewModels;

public partial class ClientesViewModel : ObservableObject
{
    private readonly ClientService _clientService;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string loadingMessage = "Trayendo clientes...";

    [ObservableProperty]
    private string statusText = "";

    [ObservableProperty]
    private ClientDto? selectedClient;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    public bool CanExecuteEdit => CanEdit && SelectedClient is not null;

    partial void OnSelectedClientChanged(ClientDto? value) =>
        OnPropertyChanged(nameof(CanExecuteEdit));

    public event Action<List<ClientDto>>? OnDataLoaded;

    public ClientesViewModel(ClientService clientService)
    {
        _clientService = clientService;

        CanCreate = UserData.HasPermission(PermissionKeys.Client_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.Client_Update);
        CanView = UserData.HasPermission(PermissionKeys.Client_View);
    }

    [RelayCommand]
    public async Task CargarDatosAsync()
    {
        if (!CanView) return;

        try
        {
            IsLoading = true;
            LoadingMessage = "Trayendo clientes...";

            var result = await _clientService.GetClients();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedClient = null;
            StatusText = $"Registros: {result.Data?.Count ?? 0}";
            OnDataLoaded?.Invoke(result.Data ?? []);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
