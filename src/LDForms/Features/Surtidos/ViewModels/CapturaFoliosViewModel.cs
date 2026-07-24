using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.DTOs;
using LD.Contracts.DTOs.KittingFolioCapture;
using LD.Contracts.Requests;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using LD.FormsX.Model.Lookup;
using LD.FormsX.Features.Surtidos.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Features.Surtidos.ViewModels;

public partial class CapturaFoliosViewModel : ObservableObject
{
    private readonly KittingFolioCaptureService _captureService;
    private readonly KittingService _kittingService;
    private readonly LookupService _lookupService;
    private readonly IServiceProvider _serviceProvider;
    private readonly List<UserProjectClientDto> _userProjectClients = new();
    private bool _loadingContext;
    private bool _loaded;

    public CapturaFoliosViewModel(
        KittingFolioCaptureService captureService,
        KittingService kittingService,
        LookupService lookupService,
        IServiceProvider serviceProvider)
    {
        _captureService = captureService;
        _kittingService = kittingService;
        _lookupService = lookupService;
        _serviceProvider = serviceProvider;
        var canAccess = UserData.HasPermission(PermissionKeys.KittingFolioCapture_Access);
        CanGenerate = canAccess;
        CanCancel = canAccess;
        CanView = canAccess;
    }

    public ObservableCollection<LookupItem> ClientLookupItems { get; } = new();

    public ObservableCollection<LookupItem> ProjectLookupItems { get; } = new();

    public ObservableCollection<KittingFolioCaptureDto> Capturas { get; } = new();

    public bool CanGenerate { get; }

    public bool CanCancel { get; }

    public bool CanView { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ActualizarCommand))]
    private int selectedClientId;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ActualizarCommand))]
    private int selectedProjectId;

    [ObservableProperty]
    private string selectedClientText = string.Empty;

    [ObservableProperty]
    private string selectedProjectText = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConsultarCommand))]
    [NotifyCanExecuteChangedFor(nameof(CancelarCommand))]
    private KittingFolioCaptureDto? selectedCapture;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ActualizarCommand))]
    [NotifyCanExecuteChangedFor(nameof(ConsultarCommand))]
    [NotifyCanExecuteChangedFor(nameof(CancelarCommand))]
    private bool isBusy;

    [ObservableProperty]
    private string statusText = "Selecciona un cliente y un proyecto para consultar o generar folios.";

    [ObservableProperty]
    private string busyMessage = "Cargando folios...";

    public async Task LoadAsync()
    {
        if (_loaded)
            return;

        _loaded = true;
        await LoadContextAsync();
    }

    [RelayCommand(CanExecute = nameof(CanActualizar))]
    private async Task ActualizarAsync()
    {
        await LoadCapturasAsync();
    }

    [RelayCommand(CanExecute = nameof(CanConsultar))]
    private async Task ConsultarAsync()
    {
        if (SelectedCapture is null)
            return;

        try
        {
            SetBusy(true, "Consultando folio...");

            var response = await _captureService.GetKittingFolioCaptureById(SelectedCapture.KittingFolioCaptureId);
            if (!response.IsSuccess || response.Data is null)
            {
                DialogHelper.ShowWarning(response.Message ?? "No se pudo obtener el folio capturado.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<CapturaFolioDetalleView>();
            dialog.SetCapture(response.Data);
            WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Application.Current?.MainWindow));
            dialog.ShowDialog();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            SetBusy(false);
        }
    }

    [RelayCommand(CanExecute = nameof(CanCancelar))]
    private async Task CancelarAsync()
    {
        if (SelectedCapture is null)
            return;

        var kittingId = SelectedCapture.KittingId;
        var kittingCode = string.IsNullOrWhiteSpace(SelectedCapture.KittingCode)
            ? kittingId.ToString(CultureInfo.InvariantCulture)
            : SelectedCapture.KittingCode;

        if (!DialogHelper.ShowConfirm(
                $"Deseas cancelar el Kitting {kittingCode}? Todas las lineas capturadas quedaran asociadas al Kitting cancelado.",
                "Cancelar Kitting"))
        {
            return;
        }

        try
        {
            SetBusy(true, "Cancelando Kitting...");

            var response = await _kittingService.CancelKitting(kittingId);
            if (!response.IsSuccess)
            {
                DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo cancelar el Kitting.");
                return;
            }

            DialogHelper.ShowSuccess(response.Message ?? "Kitting cancelado correctamente.");
            await LoadCapturasAsync();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            SetBusy(false);
        }
    }

    public async Task GenerateFromFileAsync(string filePath)
    {
        if (!CanGenerate)
        {
            DialogHelper.ShowWarning("No tienes permiso para generar Kitting.");
            return;
        }

        if (SelectedClientId <= 0 || SelectedProjectId <= 0)
        {
            DialogHelper.ShowWarning("Selecciona un cliente y un proyecto antes de generar.");
            return;
        }

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            DialogHelper.ShowWarning("Selecciona un archivo valido.");
            return;
        }

        try
        {
            SetBusy(true, "Analizando archivo...");

            var content = await File.ReadAllTextAsync(filePath, Encoding.Default);
            var request = new GenerateKittingFolioCaptureRequest
            {
                ClientId = SelectedClientId,
                ProjectId = SelectedProjectId,
                SourceFileName = Path.GetFileName(filePath),
                FileContent = content
            };

            var previewResponse = await _captureService.PreviewKittingFromFolioFile(request);
            if (!previewResponse.IsSuccess || previewResponse.Data is null)
            {
                SetBusy(false);
                DialogHelper.ShowError(previewResponse.ErrorMessage ?? previewResponse.Message ?? "No se pudo generar la vista previa.");
                return;
            }

            SetBusy(false);

            var previewDialog = _serviceProvider.GetRequiredService<CapturaFolioPreviewDialog>();
            previewDialog.SetPreview(previewResponse.Data);
            WindowOwnerHelper.AttachOwnerOrCenter(previewDialog, WindowOwnerHelper.GetVisibleOwner(Application.Current?.MainWindow));

            if (previewDialog.ShowDialog() != true)
                return;

            SetBusy(true, "Generando Kitting...");

            var response = await _captureService.GenerateKittingFromFolioFile(request);
            if (!response.IsSuccess)
            {
                SetBusy(false);
                DialogHelper.ShowError(response.ErrorMessage ?? response.Message ?? "No se pudo generar el Kitting.");
                return;
            }

            SetBusy(false);
            DialogHelper.ShowSuccess(response.Message ?? "Kitting generado correctamente.");
            await LoadCapturasAsync();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            SetBusy(false);
        }
    }

    partial void OnSelectedClientIdChanged(int value)
    {
        if (_loadingContext)
            return;

        SelectedProjectId = 0;
        SelectedProjectText = string.Empty;
        ProjectLookupItems.Clear();
        Capturas.Clear();
        SelectedCapture = null;
        StatusText = "Selecciona un proyecto para consultar los folios.";
        SetProjectsForClient(value);
    }

    partial void OnSelectedProjectIdChanged(int value)
    {
        if (_loadingContext)
            return;

        SelectedCapture = null;
        Capturas.Clear();
        StatusText = value > 0
            ? "Pulsa actualizar para consultar los folios capturados."
            : "Selecciona un proyecto para consultar los folios.";
    }

    private bool CanActualizar() =>
        CanView && !IsBusy && SelectedClientId > 0 && SelectedProjectId > 0;

    private bool CanConsultar() =>
        CanView && !IsBusy && SelectedCapture is not null;

    private bool CanCancelar() =>
        CanCancel
        && !IsBusy
        && SelectedCapture is not null
        && !IsTerminalStatus(SelectedCapture.KittingStatus);

    private async Task LoadContextAsync()
    {
        try
        {
            _loadingContext = true;
            ClientLookupItems.Clear();
            ProjectLookupItems.Clear();
            Capturas.Clear();
            SelectedCapture = null;

            if (string.IsNullOrWhiteSpace(UserData.Id))
            {
                DialogHelper.ShowWarning("No se pudo identificar el usuario actual.");
                StatusText = "No se pudo identificar el usuario actual.";
                return;
            }

            SetBusy(true, "Cargando clientes y proyectos...");

            var response = await _lookupService.GetProjectClientsByUserWarehouses(UserData.Id);
            if (!response.IsSuccess || response.Data is null)
            {
                DialogHelper.ShowWarning(response.Message ?? "No se pudieron cargar los clientes y proyectos.");
                StatusText = "No se pudieron cargar los clientes y proyectos.";
                return;
            }

            _userProjectClients.Clear();
            _userProjectClients.AddRange(response.Data);

            var clients = _userProjectClients
                .GroupBy(x => x.ClientId)
                .Select(group => new DropDownDto
                {
                    Key = group.Key.ToString(CultureInfo.InvariantCulture),
                    Value = group.First().Client
                })
                .OrderBy(x => x.Value);

            foreach (var item in clients.Select(ToLookupItem))
                ClientLookupItems.Add(item);

            StatusText = ClientLookupItems.Count > 0
                ? "Selecciona un cliente y un proyecto."
                : "No se encontraron clientes disponibles.";
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
            StatusText = "Ocurrio un error al cargar los clientes y proyectos.";
        }
        finally
        {
            _loadingContext = false;
            SetBusy(false);
        }
    }

    private void SetProjectsForClient(int clientId)
    {
        ProjectLookupItems.Clear();

        if (clientId <= 0)
            return;

        var projects = _userProjectClients
            .Where(x => x.ClientId == clientId)
            .GroupBy(x => x.ProjectId)
            .Select(group => new DropDownDto
            {
                Key = group.Key.ToString(CultureInfo.InvariantCulture),
                Value = group.First().Project
            })
            .OrderBy(x => x.Value);

        foreach (var item in projects.Select(ToLookupItem))
            ProjectLookupItems.Add(item);

        if (ProjectLookupItems.Count == 1)
        {
            var project = ProjectLookupItems[0];
            if (project.Data is DropDownDto dto && int.TryParse(dto.Key, out var projectId))
            {
                SelectedProjectId = projectId;
                SelectedProjectText = dto.Value ?? string.Empty;
            }
        }
    }

    private async Task LoadCapturasAsync()
    {
        if (!CanView)
            return;

        if (SelectedClientId <= 0 || SelectedProjectId <= 0)
        {
            DialogHelper.ShowWarning("Selecciona un cliente y un proyecto.");
            return;
        }

        try
        {
            SetBusy(true, "Consultando folios capturados...");
            SelectedCapture = null;
            Capturas.Clear();

            var response = await _captureService.GetKittingFolioCaptures(SelectedClientId, SelectedProjectId);
            if (!response.IsSuccess)
            {
                DialogHelper.ShowWarning(response.Message ?? "No se pudieron cargar los folios capturados.");
                StatusText = response.Message ?? "No se pudieron cargar los folios capturados.";
                return;
            }

            var data = response.Data ?? new List<KittingFolioCaptureDto>();
            foreach (var item in data)
                Capturas.Add(item);

            StatusText = Capturas.Count > 0
                ? $"Se encontraron {Capturas.Count} folio(s) capturado(s)."
                : "No se encontraron folios capturados para este cliente y proyecto.";
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
            StatusText = "Ocurrio un error al cargar los folios capturados.";
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SetBusy(bool value, string? message = null)
    {
        IsBusy = value;

        if (!string.IsNullOrWhiteSpace(message))
            BusyMessage = message;
    }

    private static LookupItem ToLookupItem(DropDownDto item)
    {
        return new LookupItem
        {
            Id = int.TryParse(item.Key, out var value) ? value : 0,
            Code = item.Value ?? string.Empty,
            Description = item.Key ?? string.Empty,
            Data = item
        };
    }

    private static bool IsTerminalStatus(string? status) =>
        KittingStatusNames.IsConfirmed(status)
        || KittingStatusNames.IsLoading(status)
        || KittingStatusNames.IsCancelled(status);
}
