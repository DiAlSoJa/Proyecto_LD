using LD.Forms.Views.Dialogs;
using LD.Forms.Classes;
using LD.Forms.Services;

namespace LD.Forms.Views.Forms
{
    public partial class FrmUbicaciones : Form
    {
        private Formularios formularios;
        private readonly LocationService _locationService;
        public FrmUbicaciones()
        {
            InitializeComponent();
        }
        public FrmUbicaciones(Formularios f)
        {
            InitializeComponent();
            _locationService = new();
            this.formularios = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevaUbicacion frmNuevoCliente = new FrmNuevaUbicacion();
            frmNuevoCliente.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmNuevaUbicacionMasiva frmNuevaUbicacionMasiva = new FrmNuevaUbicacionMasiva();
            frmNuevaUbicacionMasiva.ShowDialog();
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
    }
}
