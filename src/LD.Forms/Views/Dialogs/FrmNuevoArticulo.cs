using LD.Contracts.Client;
using LD.Contracts.Enums;
using LD.Contracts.Item;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Services;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LD.Forms.Views.Dialogs
{
    public partial class FrmNuevoArticulo : DraggableForm
    {

        private readonly ItemService _itemService;
        private  ItemDto? ItemSelected;
        private readonly DialogMessageService _dialogService;

        public FrmNuevoArticulo(ItemService itemService, DialogMessageService dialogService)
        {
            InitializeComponent();
            _itemService = itemService;
            _dialogService = dialogService;
            EnableDrag(panel2);
            EnableDrag(panel1);
        }
        public async void SetItem(ItemDto? item)
        {
            ItemSelected = item;
            await CargarDatosAsync();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);



        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _itemService.GetItemById(ItemSelected?.ItemId ?? 0);

                if (!response.IsSuccess)
                {
                    MessageBox.Show(response.Message);
                    return;
                }
                var client = response.Data;
                
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
        private Task<ApiResponseDto<string>> CreateClient(ItemRequest request) =>
        _itemService.CreateItem(request);

        private Task<ApiResponseDto<string>> EditClient(int clientId, ItemRequest request) =>
            _itemService.UpdateItem(clientId, request);
        private async Task<ApiResponseDto<string>> SaveClient(ItemRequest request)
        {
            return ItemSelected != null
                ? await EditClient(ItemSelected?.ItemId ?? 0, request)
                : await CreateClient(request);
        }
        private ItemRequest BuildRequest()
        {
            return new ItemRequest
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
                btnSave.Enabled = false;

                var request = BuildRequest();

                var result = await SaveClient(request);

                ShowResult(result);

                if (result.IsSuccess)
                    this.Close();
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
    }
}
