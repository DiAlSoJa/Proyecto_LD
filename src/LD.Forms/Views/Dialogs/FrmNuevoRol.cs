using LD.Client.Services;
using LD.Contracts.Client;
using LD.Contracts.DTOs.Auth;
using LD.Contracts.Enums;
using LD.Contracts.Requests.Client;
using LD.Contracts.Responses;
using LD.Forms.Classes;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoRol : DraggableForm
    {
        private readonly ModuleService _moduleService;
        private ClientDto? ClientSelected { get; set; }
        private readonly DialogMessageService _dialogService;

        public FrmNuevoRol(ModuleService moduleService, DialogMessageService dialogService)
        {
            InitializeComponent();
            EnableDrag(panel2);
            EnableDrag(panel1);
            _moduleService = moduleService;
            _dialogService = dialogService;

        }

        public async void SetClient(ClientDto client)
        {
            ClientSelected = client;
            await CargarDatosAsync();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            treePermissions.Nodes.Clear();

            var response = await _moduleService.GetModules();
            var allModules = response.Data ?? new List<ModuleAuthorizationDto>();

            foreach (var module in allModules)
            {
                TreeNode moduleNode = new TreeNode(module.ModuleName);

                foreach (var permission in module.Permissions)
                {
                    TreeNode permissionNode = new TreeNode(permission.PermissionName)
                    {
                        Tag = permission.PermissionId
                    };

                    moduleNode.Nodes.Add(permissionNode);
                }

                treePermissions.Nodes.Add(moduleNode);
            }


        }

        private async Task CargarDatosAsync()
        {
            try
            {
                //var response = await _clientService.GetClientById(ClientSelected?.Id ?? 0);

                //if (!response.IsSuccess)
                //{
                //    MessageBox.Show(response.Message);
                //    return;
                //}
                //var client = response.Data;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //private Task<ApiResponseDto<string>> CreateClient(ClientRequest request) =>
        //    _clientService.CreateClient(request);

        //private Task<ApiResponseDto<string>> EditClient(int clientId, ClientRequest request) =>
        //    _clientService.UpdateClient(clientId, request);
        //private async Task<ApiResponseDto<string>> SaveClient(ClientRequest request)
        //{
        //    return ClientSelected != null
        //        ? await EditClient(ClientSelected?.Id ?? 0, request)
        //        : await CreateClient(request);
        //}

        private ClientRequest BuildRequest()
        {

            return new ClientRequest
            {

            };
        }

        private void ShowResult(ApiResponseDto<string> result)
        {
            _dialogService.Show(
                 result.IsSuccess ? result.Data ?? "" : $"Hubo un error: {Environment.NewLine}{result.ErrorMessage ?? ""}",
                  result.IsSuccess ? DialogMessageEnum.Info : DialogMessageEnum.Error
                );


        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //btnSave.Enabled = false;

                //var request = BuildRequest();

                //var result = await SaveClient(request);

                //ShowResult(result);

                //ResponseForm = result.IsSuccess;
                //if (result.IsSuccess)
                //    this.Close();
            }
            catch (Exception ex)
            {
                _dialogService.Show($"Hubo un error: {Environment.NewLine}{ex.Message}", DialogMessageEnum.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private List<int> GetPermissions()
        {
            List<int> permissions = new();

            foreach (TreeNode module in treePermissions.Nodes)
            {
                foreach (TreeNode permission in module.Nodes)
                {
                    if (permission.Checked)
                    {
                        permissions.Add(
                            int.TryParse( permission?.Tag.ToString(),out int p)?p:0
                            );
                    }
                }
            }
            return permissions;
        }
        private void treePermissions_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }
        }
    }
}
