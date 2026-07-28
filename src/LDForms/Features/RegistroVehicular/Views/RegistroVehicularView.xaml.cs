using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.DTOs.Security;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.RegistroVehicular;

public partial class RegistroVehicularView : UserControl
{
    private readonly SecurityService _securityService;
    private readonly PatioClientService _patioClientService;
    private readonly WpfGridFilter<SecurityRegistrationDto> _gridFilter;
    private bool _loaded;
    private SecurityRegistrationDto? _selectedRegistration;
    private int _totalCount;

    public RegistroVehicularView(
        SecurityService securityService,
        PatioClientService patioClientService)
    {
        InitializeComponent();

        _securityService = securityService;
        _patioClientService = patioClientService;

        _gridFilter = new WpfGridFilter<SecurityRegistrationDto>(dgRegistros, ldBuscar.TextBoxElement);
        _gridFilter.SetHiddenColumns(
            nameof(SecurityRegistrationDto.Fotos),
            nameof(SecurityRegistrationDto.Firma));
        _gridFilter.SetColumnOrder(
            nameof(SecurityRegistrationDto.SecurityRegistrationId),
            nameof(SecurityRegistrationDto.CreatedAt),
            nameof(SecurityRegistrationDto.Estado),
            nameof(SecurityRegistrationDto.Tipo),
            nameof(SecurityRegistrationDto.Nombre),
            nameof(SecurityRegistrationDto.Licencia),
            nameof(SecurityRegistrationDto.Vencimiento),
            nameof(SecurityRegistrationDto.Celular),
            nameof(SecurityRegistrationDto.TieneCaja),
            nameof(SecurityRegistrationDto.TipoVehiculo),
            nameof(SecurityRegistrationDto.Linea),
            nameof(SecurityRegistrationDto.Origen),
            nameof(SecurityRegistrationDto.Numero),
            nameof(SecurityRegistrationDto.Placa),
            nameof(SecurityRegistrationDto.NumeroCaja),
            nameof(SecurityRegistrationDto.PlacaCaja),
            nameof(SecurityRegistrationDto.Sello),
            nameof(SecurityRegistrationDto.CortinaNumero),
            nameof(SecurityRegistrationDto.IsActive));
        _gridFilter.SetColumnWidths(new Dictionary<string, double>
        {
            { nameof(SecurityRegistrationDto.SecurityRegistrationId), 95 },
            { nameof(SecurityRegistrationDto.CreatedAt), 155 },
            { nameof(SecurityRegistrationDto.Estado), 130 },
            { nameof(SecurityRegistrationDto.Tipo), 110 },
            { nameof(SecurityRegistrationDto.Nombre), 220 },
            { nameof(SecurityRegistrationDto.Licencia), 130 },
            { nameof(SecurityRegistrationDto.Vencimiento), 120 },
            { nameof(SecurityRegistrationDto.Celular), 120 },
            { nameof(SecurityRegistrationDto.TieneCaja), 90 },
            { nameof(SecurityRegistrationDto.TipoVehiculo), 130 },
            { nameof(SecurityRegistrationDto.Linea), 120 },
            { nameof(SecurityRegistrationDto.Origen), 120 },
            { nameof(SecurityRegistrationDto.Numero), 110 },
            { nameof(SecurityRegistrationDto.Placa), 120 },
            { nameof(SecurityRegistrationDto.NumeroCaja), 120 },
            { nameof(SecurityRegistrationDto.PlacaCaja), 120 },
            { nameof(SecurityRegistrationDto.Sello), 140 },
            { nameof(SecurityRegistrationDto.CortinaNumero), 110 },
            { nameof(SecurityRegistrationDto.IsActive), 80 }
        });
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (_loaded)
        {
            return;
        }

        _loaded = true;

        if (!UserData.HasPermission(PermissionKeys.Security_View))
        {
            DialogHelper.ShowWarning("No tienes permiso para consultar registros vehiculares.");
            SetControlsEnabled(false);
            txtStatus.Text = "Sin permiso para consultar registros vehiculares.";
            return;
        }

        btnPdf.IsEnabled = UserData.HasPermission(PermissionKeys.Security_Tasks_View);
        btnPdf.ToolTip = btnPdf.IsEnabled
            ? null
            : "Necesitas permiso de tareas de seguridad para generar el PDF.";

        await CargarDatosConLoaderAsync("Cargando registros vehiculares...");
    }

    private void SetControlsEnabled(bool enabled)
    {
        ldBuscar.IsEnabled = enabled;
        btnBuscar.IsEnabled = enabled;
        btnActualizar.IsEnabled = enabled;
        btnPdf.IsEnabled = false;
        dgRegistros.IsEnabled = enabled;
    }

    private async Task CargarDatosConLoaderAsync(string mensaje)
    {
        try
        {
            MostrarLoader(true, mensaje);
            await CargarDatosAsync();
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            MostrarLoader(false);
        }
    }

    private void MostrarLoader(bool mostrar, string mensaje = "Cargando...")
    {
        TxtLoading.Text = mensaje;
        LoadingOverlay.Visibility = mostrar ? Visibility.Visible : Visibility.Collapsed;
    }

    private async Task CargarDatosAsync()
    {
        var result = await _securityService.GetRegistrationsAsync(days: 30);
        if (!result.IsSuccess)
        {
            DialogHelper.ShowWarning(result.ErrorMessage ?? result.Message ?? "No se pudieron obtener los registros.");
            return;
        }

        var data = (result.Data ?? [])
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.SecurityRegistrationId)
            .ToList();

        _totalCount = data.Count;
        _gridFilter.SetData(data);
        txtTotal.Text = $"Total: {_totalCount}";
        txtStatus.Text = $"Registros visibles: {dgRegistros.Items.Count}";

        if (dgRegistros.Items.Count > 0)
        {
            dgRegistros.SelectedIndex = 0;
            dgRegistros.ScrollIntoView(dgRegistros.SelectedItem);
        }
        else
        {
            _selectedRegistration = null;
            txtSelectionHint.Text = "No hay registros para mostrar.";
            btnPdf.IsEnabled = false;
        }
    }

    private void UpdateSelectionState()
    {
        _selectedRegistration = _gridFilter.SelectedItem;

        btnPdf.IsEnabled = _selectedRegistration is not null
                           && UserData.HasPermission(PermissionKeys.Security_Tasks_View);

        txtSelectionHint.Text = _selectedRegistration is null
            ? "Selecciona un registro para ver su PDF."
            : $"Seleccionado: #{_selectedRegistration.SecurityRegistrationId} | {_selectedRegistration.Placa}";
    }

    private void RefreshVisibleStatus()
    {
        var visibleCount = dgRegistros.Items.Count;
        txtStatus.Text = _selectedRegistration is null
            ? $"Registros visibles: {visibleCount}"
            : $"Registros visibles: {visibleCount} de {_totalCount}";
    }

    private void DgRegistros_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        UpdateSelectionState();
        RefreshVisibleStatus();
    }

    private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
    {
        await CargarDatosConLoaderAsync("Actualizando registros vehiculares...");
    }

    private void BtnBuscar_Click(object sender, RoutedEventArgs e)
    {
        _gridFilter.Refresh();

        if (_selectedRegistration is not null && !dgRegistros.Items.Contains(_selectedRegistration))
        {
            _selectedRegistration = null;
        }

        if (_selectedRegistration is null && dgRegistros.Items.Count > 0)
        {
            dgRegistros.SelectedIndex = 0;
            dgRegistros.ScrollIntoView(dgRegistros.SelectedItem);
        }
        else
        {
            UpdateSelectionState();
        }

        RefreshVisibleStatus();
    }

    private async void BtnPdf_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRegistration is null)
        {
            DialogHelper.ShowWarning("Selecciona un registro para generar el PDF.");
            return;
        }

        if (!UserData.HasPermission(PermissionKeys.Security_Tasks_View))
        {
            DialogHelper.ShowWarning("No tienes permiso para ver el PDF de movimientos.");
            return;
        }

        try
        {
            MostrarLoader(true, "Preparando PDF...");

            var tasksResponse = await _patioClientService.GetTasksAsync(
                securityRegistrationId: _selectedRegistration.SecurityRegistrationId);

            if (!tasksResponse.IsSuccess)
            {
                DialogHelper.ShowWarning(tasksResponse.ErrorMessage ?? tasksResponse.Message ?? "No se pudieron cargar los movimientos.");
                return;
            }

            await SecurityRegistrationDocumentExporter.PreviewAsync(
                _selectedRegistration,
                tasksResponse.Data ?? [],
                _securityService.GetImageBytesAsync);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message, "PDF");
        }
        finally
        {
            MostrarLoader(false);
        }
    }

    private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            var window = Window.GetWindow(this);
            window?.DragMove();
        }
    }
}
