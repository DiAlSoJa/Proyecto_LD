using LD.Dialogs;
using LD.Forms.Classes;
using LD.Forms.Services;

namespace LD.Forms
{
    public partial class FrmClientes : Form
    {
        private readonly ClientService _clientService;
        private Formularios? formularios;

        public FrmClientes(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
            _clientService = new();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevoCliente frmNuevoCliente = new FrmNuevoCliente();
            frmNuevoCliente.ShowDialog();
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
                var clientResponse = await _clientService.GetClients();

               

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
