using LD.Contracts.Client;
using LD.Views.Dialogs;
using LD.Forms.Classes;
using LD.Forms.Services;

namespace LD.Forms.Views.Forms
{
    public partial class FrmClientes : Form
    {
        private readonly ClientService _clientService;
        private Formularios? formularios;
        private ClientDto? selectedClient { get; set; }

        private BindingSource _clientsBinding = new();
        public FrmClientes(Formularios f)
        {
            InitializeComponent();
            this.formularios = f;
            _clientService = new();
            dataGridView1.DataSource = _clientsBinding;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmNuevoCliente frmNuevoCliente = new FrmNuevoCliente();
            frmNuevoCliente.ShowDialog();
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            FrmNuevoCliente frmNuevoCliente = new FrmNuevoCliente(selectedClient);
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
                var result = await _clientService.GetClients();

                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Message);
                    return;
                }
                //dataGridView1.AutoGenerateColumns = false;
                _clientsBinding.DataSource = result.Data;
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

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                    return;

                var cliente = dataGridView1.CurrentRow.DataBoundItem as ClientDto;

                if (cliente == null)
                    return;

                selectedClient = cliente;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
