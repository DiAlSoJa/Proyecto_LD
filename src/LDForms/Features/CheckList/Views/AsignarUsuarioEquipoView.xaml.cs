using LD.Client.Services;
using LD.Contracts.DTOs.User;
using LD.Contracts.Equipment;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.CheckList
{
    public partial class AsignarUsuarioEquipoView : Window
    {
        private readonly UserService _userService;
        private readonly EquipmentService _equipmentService;
        private EquipmentDto? _selectedEquipment;
        private readonly List<TurnoAsignacionItem> _turnos = new();

        public AsignarUsuarioEquipoView(UserService userService, EquipmentService equipmentService)
        {
            InitializeComponent();
            _userService = userService;
            _equipmentService = equipmentService;
            Loaded += AsignarUsuarioEquipoView_Loaded;
        }

        public void SetEquipment(EquipmentDto equipment)
        {
            _selectedEquipment = equipment;
            txtHeader.Text = $"Asignar usuarios - {equipment.NoEquipo}";
            _turnos.Clear();
            _turnos.Add(new TurnoAsignacionItem("Turno 1", equipment.Turno1));
            _turnos.Add(new TurnoAsignacionItem("Turno 2", equipment.Turno2));
            _turnos.Add(new TurnoAsignacionItem("Turno 3", equipment.Turno3));
            dgTurnos.ItemsSource = null;
            dgTurnos.ItemsSource = _turnos;
        }

        private async void AsignarUsuarioEquipoView_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarUsuariosAsync();
        }

        private async Task CargarUsuariosAsync()
        {
            try
            {
                var response = await _userService.GetUsers();
                if (!response.IsSuccess || response.Data is null)
                {
                    dgUsuarios.ItemsSource = null;
                    DialogHelper.ShowError(response.Message ?? response.ErrorMessage ?? "No se pudieron cargar los usuarios.");
                    return;
                }

                dgUsuarios.ItemsSource = response.Data
                    .Where(x => x.User?.Activo == true)
                    .OrderBy(x => x.User?.Nombre ?? x.User?.UserName)
                    .ToList();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
        }

        private void BtnAsignar_Click(object sender, RoutedEventArgs e)
        {
            var turnoSeleccionado = dgTurnos.SelectedItem as TurnoAsignacionItem;
            var usuarioSeleccionado = dgUsuarios.SelectedItem as GetUserDto;

            if (turnoSeleccionado is null)
            {
                DialogHelper.ShowWarning("Selecciona un turno.");
                return;
            }

            if (usuarioSeleccionado?.User is null)
            {
                DialogHelper.ShowWarning("Selecciona un usuario.");
                return;
            }

            turnoSeleccionado.UsuarioAsignado = usuarioSeleccionado.User.UserName ?? string.Empty;
            dgTurnos.Items.Refresh();
        }

        private void BtnQuitar_Click(object sender, RoutedEventArgs e)
        {
            var turnoSeleccionado = dgTurnos.SelectedItem as TurnoAsignacionItem;
            if (turnoSeleccionado is null)
            {
                DialogHelper.ShowWarning("Selecciona un turno.");
                return;
            }

            turnoSeleccionado.UsuarioAsignado = string.Empty;
            dgTurnos.Items.Refresh();
        }

        private async void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedEquipment is null)
                {
                    DialogHelper.ShowWarning("No hay equipo seleccionado.");
                    return;
                }

                btnGuardar.IsEnabled = false;

                var equipmentResponse = await _equipmentService.GetEquipmentById(_selectedEquipment.EquipmentId);
                if (!equipmentResponse.IsSuccess || equipmentResponse.Data is null)
                {
                    DialogHelper.ShowError(equipmentResponse.Message ?? equipmentResponse.ErrorMessage ?? "No se pudo cargar el equipo.");
                    return;
                }

                var request = equipmentResponse.Data;
                request.Turn1 = _turnos.ElementAtOrDefault(0)?.UsuarioAsignado ?? string.Empty;
                request.Turn2 = _turnos.ElementAtOrDefault(1)?.UsuarioAsignado ?? string.Empty;
                request.Turn3 = _turnos.ElementAtOrDefault(2)?.UsuarioAsignado ?? string.Empty;

                var updateResponse = await _equipmentService.UpdateEquipment(_selectedEquipment.EquipmentId, request);
                if (!updateResponse.IsSuccess)
                {
                    DialogHelper.ShowError(updateResponse.Message ?? updateResponse.ErrorMessage ?? "No se pudo actualizar la asignación.");
                    return;
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnGuardar.IsEnabled = true;
            }
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private sealed class TurnoAsignacionItem
        {
            public TurnoAsignacionItem(string nombreTurno, string usuarioAsignado)
            {
                NombreTurno = nombreTurno;
                UsuarioAsignado = usuarioAsignado;
            }

            public string NombreTurno { get; }
            public string UsuarioAsignado { get; set; }
        }
    }
}
