using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.Location;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Dialogs;

namespace LD.Forms.Views.Forms
{
    public partial class FrmUbicaciones : Form
    {
        private Formularios formularios;
        private readonly LocationService _locationService;
        private readonly DialogFormService _dialogFormService;

        private BindingSource _locationBinding = new();
        private GridFilter<LocationDto> _gridFilter;
        private LocationDto? locationSelected { get; set; }
        public FrmUbicaciones(LocationService locationService, DialogFormService dialogFormService)
        {
            InitializeComponent();
            _locationService = locationService;
            _dialogFormService = dialogFormService;
            dataGridView1.DataSource = _locationBinding;
            _gridFilter = new GridFilter<LocationDto>(dataGridView1, _locationBinding);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevaUbicacion>();
            if (form.ResponseForm) await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo ubicaciones");
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevaUbicacionMasiva>(config =>
            {

            });
            if (form.ResponseForm) await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo ubicaciones");

        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo ubicaciones");

        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var result = await _locationService.GetLocations();

                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
                _locationBinding.DataSource = result.Data;

                _gridFilter.SetData(result.Data);
                dataGridView1 = _gridFilter.BuildFilterColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo clientes");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevaUbicacion>(config =>
            {
                config.SetLocation(locationSelected);
            });
            if (form.ResponseForm) await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo ubicaciones");
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                    return;

                var location = dataGridView1.CurrentRow.DataBoundItem as LocationDto;

                if (location == null)
                    return;

                locationSelected = location;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
