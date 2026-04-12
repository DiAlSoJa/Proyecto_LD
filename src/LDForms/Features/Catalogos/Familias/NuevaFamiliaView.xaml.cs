using LD.Contracts.DTOs.Family;
using LD.Contracts.Requests;
using LD.FormsX.Features.Catalogos.Familias.ViewModels;
using LD.FormsX.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LD.FormsX.Views.Familias
{
    public partial class NuevaFamiliaView : Window
    {
        private bool _cargandoDatos = false;
        private bool _datosInicialesCargados;

        private NuevaFamiliaViewModel ViewModel => (NuevaFamiliaViewModel)DataContext;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevaFamiliaView(NuevaFamiliaViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };
        }

        public void SetFamily(FamilyDto family)
        {
            ViewModel.SetFamily(family);
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (!_datosInicialesCargados)
                await CargarDatosInicialesAsync();
        }

        private async Task CargarDatosInicialesAsync()
        {
            try
            {
                _cargandoDatos = true;
                _datosInicialesCargados = true;

                var clientes = await ViewModel.GetClientsAsync();
                if (clientes.Count > 0)
                {
                    cmbCliente.ItemsSource = clientes;
                    cmbCliente.DisplayMemberPath = "Value";
                    cmbCliente.SelectedValuePath = "Key";
                    cmbCliente.SelectedIndex = -1;
                }
            }
            finally
            {
                _cargandoDatos = false;
            }

            if (ViewModel.SelectedFamily != null)
                await CargarDatosAsync();
        }

        private async Task SetCombosProjects(string projectSel = "")
        {
            if (cmbCliente.SelectedValue == null)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            if (!int.TryParse(cmbCliente.SelectedValue.ToString(), out int clienteId) || clienteId <= 0)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            var proyectos = await ViewModel.GetProjectsAsync(clienteId);

            if (proyectos.Count == 0)
            {
                cmbProyecto.ItemsSource = null;
                return;
            }

            cmbProyecto.DisplayMemberPath = "Value";
            cmbProyecto.SelectedValuePath = "Key";
            cmbProyecto.ItemsSource = proyectos;

            if (!string.IsNullOrWhiteSpace(projectSel))
                cmbProyecto.SelectedValue = projectSel;
            else if (proyectos.Count > 1)
                cmbProyecto.SelectedIndex = -1;
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                _cargandoDatos = true;

                var item = await ViewModel.GetFamilyAsync();
                if (item is null)
                    return;

                txtNombre.Text = item.FamilyName;
                cmbCliente.SelectedValue = item.ClientId.ToString();

                await SetCombosProjects(item.ProjectId.ToString());
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                _cargandoDatos = false;
            }
        }

        private FamilyRequest BuildRequest()
        {
            return new FamilyRequest
            {
                FamilyName = txtNombre.Text.Trim(),
                ClientId = int.TryParse(cmbCliente.SelectedValue?.ToString(), out int clienteId) ? clienteId : 0,
                ProjectId = int.TryParse(cmbProyecto.SelectedValue?.ToString(), out int projectId) ? projectId : 0
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                var request = BuildRequest();
                await ViewModel.SaveAsync(request);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }

        private async void cmbCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cargandoDatos)
                return;

            await SetCombosProjects();
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
    }
}
