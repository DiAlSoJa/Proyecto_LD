using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.Product;
using LD.FormsX.Features.Common;
using LD.FormsX.Helpers;
using LD.FormsX.Model.Lookup;
using LD.FormsX.Views.Articulos;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views
{
    public partial class ArticulosView : UserControl, INotifyPropertyChanged
    {
        private readonly ProductService _service;
        private readonly LookupService _lookupService;
        private readonly IServiceProvider _serviceProvider;
        private readonly WpfGridFilter<ProductDto> _gridFilter;

        private ProductDto? _selectedX;
        private List<UserProjectClientDto> _userProjectClients = new();
        private bool _cargandoFiltros;
        private bool _loaded;
        private int _selectedClientId;
        private int _selectedProjectId;
        private string _selectedClientText = string.Empty;
        private string _selectedProjectText = string.Empty;

        public ObservableCollection<LookupItem> ClientLookupItems { get; } = new();
        public ObservableCollection<LookupItem> ProjectLookupItems { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public int SelectedClientId
        {
            get => _selectedClientId;
            set
            {
                if (_selectedClientId == value)
                    return;

                _selectedClientId = value;
                OnPropertyChanged(nameof(SelectedClientId));
            }
        }

        public int SelectedProjectId
        {
            get => _selectedProjectId;
            set
            {
                if (_selectedProjectId == value)
                    return;

                _selectedProjectId = value;
                OnPropertyChanged(nameof(SelectedProjectId));
            }
        }

        public string SelectedClientText
        {
            get => _selectedClientText;
            set
            {
                if (_selectedClientText == value)
                    return;

                _selectedClientText = value;
                OnPropertyChanged(nameof(SelectedClientText));
            }
        }

        public string SelectedProjectText
        {
            get => _selectedProjectText;
            set
            {
                if (_selectedProjectText == value)
                    return;

                _selectedProjectText = value;
                OnPropertyChanged(nameof(SelectedProjectText));
            }
        }

        public ArticulosView(ProductService serviceX, LookupService lookupService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _lookupService = lookupService;
            _serviceProvider = serviceProvider;
            DataContext = this;

            _gridFilter = new WpfGridFilter<ProductDto>(dg);
            _gridFilter.SetColumnWidths(new Dictionary<string, double>());
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_loaded)
                return;

            _loaded = true;
            await CargarClientesAsync();
            LimpiarGrid("Selecciona un cliente y un proyecto para consultar artículos.");
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
            if (!TryGetSelectedIds(out var clienteId, out var proyectoId))
            {
                LimpiarGrid("Selecciona un cliente y un proyecto para consultar artículos.");
                return;
            }

            var result = await _service.GetItems(clienteId, proyectoId);
            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            _gridFilter.SetData(result.Data?.OfType<ProductDto>());
            _selectedX = null;
            txtStatus.Text = $"Registros: {result.Data?.Count ?? 0}";
        }

        private async void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedIds(out _, out _))
            {
                DialogHelper.ShowWarning("Selecciona un cliente y un proyecto.");
                return;
            }

            await CargarDatosConLoaderAsync("Trayendo artículos...");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            _gridFilter.ClearFilter();
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _selectedX = _gridFilter.SelectedItem;
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedIds(out var clienteId, out var proyectoId))
            {
                DialogHelper.ShowWarning("Selecciona un cliente y un proyecto.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<NuevoArticuloView>();
            WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetContext(clienteId, proyectoId);

            var result = dialog.ShowDialog();
            if (result == true)
                await CargarDatosAsync();
        }

        private async void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedIds(out var clienteId, out var proyectoId))
            {
                DialogHelper.ShowWarning("Selecciona un cliente y un proyecto.");
                return;
            }

            if (_selectedX is null)
                return;

            var dialog = _serviceProvider.GetRequiredService<NuevoArticuloView>();
            WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetContext(clienteId, proyectoId);
            dialog.SetItem(_selectedX);

            var result = dialog.ShowDialog();
            if (result == true)
                await CargarDatosAsync();
        }

        private async void BtnCargaMasiva_Click(object sender, RoutedEventArgs e)
        {
            if (!TryGetSelectedIds(out var clienteId, out var proyectoId))
            {
                DialogHelper.ShowWarning("Selecciona un cliente y un proyecto.");
                return;
            }

            var dialog = _serviceProvider.GetRequiredService<CargaMasivaArticulosView>();
            WindowOwnerHelper.AttachOwnerOrCenter(dialog, WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));
            dialog.SetContext(clienteId, proyectoId, SelectedClientText, SelectedProjectText);
            var result = dialog.ShowDialog();
            if (result == true)
                await CargarDatosAsync();
        }

        private async Task CargarClientesAsync()
        {
            try
            {
                _cargandoFiltros = true;
                ClientLookupItems.Clear();
                ProjectLookupItems.Clear();

                if (string.IsNullOrWhiteSpace(UserData.Id))
                {
                    DialogHelper.ShowWarning("No se pudo identificar el usuario actual para cargar clientes y proyectos.");
                    return;
                }

                var response = await _lookupService.GetProjectClientsByUserWarehouses(UserData.Id);
                if (!response.IsSuccess || response.Data == null)
                {
                    DialogHelper.ShowWarning(response.Message);
                    return;
                }

                _userProjectClients = response.Data;
                var clientes = _userProjectClients
                    .GroupBy(x => x.ClientId)
                    .Select(group => new DropDownDto
                    {
                        Key = group.Key.ToString(),
                        Value = group.First().Client
                    })
                    .OrderBy(x => x.Value);

                foreach (var item in clientes.Select(ToLookupItem))
                    ClientLookupItems.Add(item);

                ClearClientSelection();
                ClearProjectSelection();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _cargandoFiltros = false;
            }

        }

        private Task CargarProyectosAsync()
        {
            try
            {
                _cargandoFiltros = true;
                ProjectLookupItems.Clear();

                if (SelectedClientId <= 0)
                {
                    ClearProjectSelection();
                    LimpiarGrid("Selecciona un cliente y un proyecto para consultar artículos.");
                    return Task.CompletedTask;
                }

                var proyectos = _userProjectClients
                    .Where(x => x.ClientId == SelectedClientId)
                    .GroupBy(x => x.ProjectId)
                    .Select(group => new DropDownDto
                    {
                        Key = group.Key.ToString(),
                        Value = group.First().Project
                    })
                    .OrderBy(x => x.Value)
                    .ToList();
                if (proyectos.Count == 0)
                {
                    ClearProjectSelection();
                    LimpiarGrid("Selecciona un proyecto para consultar artículos.");
                    return Task.CompletedTask;
                }

                foreach (var item in proyectos.Select(ToLookupItem))
                    ProjectLookupItems.Add(item);

                ClearProjectSelection();
                LimpiarGrid("Selecciona un proyecto para consultar artículos.");
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _cargandoFiltros = false;
            }

            return Task.CompletedTask;
        }

        private bool TryGetSelectedIds(out int clienteId, out int proyectoId)
        {
            clienteId = SelectedClientId;
            proyectoId = SelectedProjectId;
            return clienteId > 0 && proyectoId > 0;
        }

        private void LimpiarGrid(string mensaje)
        {
            _selectedX = null;
            _gridFilter.ClearFilter();
            _gridFilter.SetData(Array.Empty<ProductDto>());
            txtStatus.Text = mensaje;
        }

        private async void LookupCliente_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            if (_cargandoFiltros || !_loaded)
                return;

            if (sender is not InlineLookupEditor editor || editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not DropDownDto selectedClient)
                return;

            SelectedClientId = int.TryParse(selectedClient.Key, out var clientId) ? clientId : 0;
            SelectedClientText = selectedClient.Value ?? string.Empty;

            await CargarProyectosAsync();
        }

        private async void LookupProyecto_SelectionConfirmed(object sender, RoutedEventArgs e)
        {
            if (_cargandoFiltros || !_loaded)
                return;

            if (sender is not InlineLookupEditor editor || editor.SelectedLookupItem is not LookupItem lookupItem)
                return;

            if (lookupItem.Data is not DropDownDto selectedProject)
                return;

            SelectedProjectId = int.TryParse(selectedProject.Key, out var projectId) ? projectId : 0;
            SelectedProjectText = selectedProject.Value ?? string.Empty;

            if (!TryGetSelectedIds(out _, out _))
            {
                LimpiarGrid("Selecciona un cliente y un proyecto para consultar artículos.");
                return;
            }

            await CargarDatosConLoaderAsync("Trayendo artículos...");
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

        private void ClearClientSelection()
        {
            SelectedClientId = 0;
            SelectedClientText = string.Empty;
            lookupCliente?.ClearSelection();
        }

        private void ClearProjectSelection()
        {
            SelectedProjectId = 0;
            SelectedProjectText = string.Empty;
            lookupProyecto?.ClearSelection();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

