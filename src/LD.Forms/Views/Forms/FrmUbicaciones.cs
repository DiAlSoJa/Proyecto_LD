using LD.Forms.Views.Dialogs;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;

namespace LD.Forms.Views.Forms
{
    public partial class FrmUbicaciones : Form
    {
        private Formularios formularios;
        private readonly LocationService _locationService;
        private readonly DialogFormService _dialogFormService;

        public FrmUbicaciones(LocationService locationService, DialogFormService dialogFormService)
        {
            InitializeComponent();
            _locationService = locationService;
            _dialogFormService = dialogFormService;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevaUbicacion>();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevaUbicacionMasiva>(config =>
            {
                
            });

        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo clientes");

        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var clientResponse = await _locationService.GetLocations();



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

        private void button3_Click(object sender, EventArgs e)
        {
            _dialogFormService.ShowDialog<FrmNuevaUbicacion>(config =>
            {
                config.SetLocation(new());
            });
        }
    }
}
