using LD.Client.Services;
using LD.Contracts.Client;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace LD.Forms.Views.Forms
{
    public partial class FrmClientes : Form
    {
        private readonly ClientService _clientService;
        private readonly DialogFormService _dialogFormService;



        private GridFilter<ClientDto>_gridFilter;
        private BindingSource _clientsBinding = new();
        private ClientDto? selectedClient { get; set; }
        public FrmClientes(ClientService clientService, DialogFormService dialogFormService)
        {
            InitializeComponent();
            _clientService = clientService;
            _dialogFormService = dialogFormService;
            dataGridView1.DataSource = _clientsBinding;
            _gridFilter = new GridFilter<ClientDto>(dataGridView1, _clientsBinding);
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            var form =_dialogFormService.ShowDialog<FrmNuevoCliente>();
            if(form.ResponseForm) await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo clientes");
        }
        

        private async void EditBtn_Click(object sender, EventArgs e)
        {
            if (selectedClient is null) return;
            var form = _dialogFormService.ShowDialog<FrmNuevoCliente>(frm =>
            {
                frm.SetClient(selectedClient);
            });
            if (form.ResponseForm) await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo clientes");
        }
        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await LoaderManager.Run(gridContainer, async () => await CargarDatosAsync(), "Trayendo clientes");

        }

        private async Task CargarDatosAsync()
        {
            var result = await _clientService.GetClients();

            if (!result.IsSuccess)
            {
                MessageBox.Show(result.Message);
                return;
            }
            _clientsBinding.DataSource = result.Data;
            
            _gridFilter.SetData(result.Data);
            dataGridView1 = _gridFilter.BuildFilterColumns();
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

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            //_gridFilter = new GridAxEnterpriseFilter<ClientDto>(dataGridView1, _clientsBinding);
        }
    }
}
