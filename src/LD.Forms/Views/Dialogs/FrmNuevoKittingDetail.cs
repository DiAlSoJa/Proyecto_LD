using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Kitting;
using LD.Contracts.Product;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Classes;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;

namespace LD.Forms.Views.Dialogs
{
    public class FrmNuevoKittingDetail : DraggableForm
    {
        private readonly KittingService _kittingService;
        private readonly KittingDetailService _kittingDetailService;
        private readonly ProductService _productService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly DialogMessageService _dialogMessageService;

        private KittingDto? _selectedKitting;
        private KittingDetailDto? _selectedDetail;
        private KittingRequest? _kittingRequest;
        private bool _loadingData;

        private readonly Label _titleLabel;
        private readonly ComboBox _productCombo;
        private readonly TextBox _partNumberText;
        private readonly TextBox _descriptionText;
        private readonly TextBox _quantityText;
        private readonly ComboBox _statusCombo;
        private readonly TextBox _sdText;
        private readonly TextBox _lotText;
        private readonly DateTimePicker _expirationPicker;
        private readonly TextBox _referenceText;
        private readonly TextBox _exchangeRateText;
        private readonly TextBox _purchaseOrderText;
        private readonly TextBox _customsText;
        private readonly TextBox _standardQtyText;
        private readonly TextBox _maxQtyText;
        private readonly Button _saveButton;

        public FrmNuevoKittingDetail(
            KittingService kittingService,
            KittingDetailService kittingDetailService,
            ProductService productService,
            InventaryStatusService inventaryStatusService,
            DialogMessageService dialogMessageService)
        {
            _kittingService = kittingService;
            _kittingDetailService = kittingDetailService;
            _productService = productService;
            _inventaryStatusService = inventaryStatusService;
            _dialogMessageService = dialogMessageService;

            Text = "Linea de Embarque";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(860, 560);

            var root = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle
            };

            var titlePanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 38,
                BackColor = Color.ForestGreen
            };

            _titleLabel = new Label
            {
                Text = "Nueva Linea",
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(16, 9)
            };

            var closeButton = new Button
            {
                Text = "X",
                Dock = DockStyle.Right,
                Width = 44,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.ForestGreen,
                TabStop = false
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (_, _) => Close();

            titlePanel.Controls.Add(closeButton);
            titlePanel.Controls.Add(_titleLabel);

            var content = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                AutoScroll = true
            };

            _productCombo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 280
            };
            _productCombo.SelectedIndexChanged += comboProducto_SelectedIndexChanged;

            _partNumberText = CreateTextBox();
            _descriptionText = CreateTextBox();
            _quantityText = CreateTextBox();
            _statusCombo = InventaryStatusComboHelper.CreateComboBox();
            _sdText = CreateTextBox();
            _lotText = CreateTextBox();
            _expirationPicker = CreateNullableDatePicker();
            _referenceText = CreateTextBox();
            _exchangeRateText = CreateTextBox();
            _purchaseOrderText = CreateTextBox();
            _customsText = CreateTextBox();
            _standardQtyText = CreateTextBox();
            _maxQtyText = CreateTextBox();

            var table = CreateFieldTable(7);
            AddField(table, 0, "Producto:", _productCombo, "No. Parte:", _partNumberText);
            AddField(table, 1, "Descripcion:", _descriptionText, "Cantidad:", _quantityText);
            AddField(table, 2, "Status:", _statusCombo, "SD:", _sdText);
            AddField(table, 3, "Lote:", _lotText, "Caducidad:", _expirationPicker);
            AddField(table, 4, "Referencia:", _referenceText, "Tipo Cambio:", _exchangeRateText);
            AddField(table, 5, "Orden Compra:", _purchaseOrderText, "Pedimento:", _customsText);
            AddField(table, 6, "Cantidad Estandar:", _standardQtyText, "Cantidad Maxima:", _maxQtyText);

            content.Controls.Add(table);

            var footer = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 56,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(8)
            };

            var closeFooterButton = new Button
            {
                Text = "Cerrar",
                Width = 150,
                Height = 34
            };
            closeFooterButton.Click += (_, _) => Close();

            _saveButton = new Button
            {
                Text = "Guardar Linea",
                Width = 170,
                Height = 34
            };
            _saveButton.Click += btnSave_Click;

            footer.Controls.Add(closeFooterButton);
            footer.Controls.Add(_saveButton);

            root.Controls.Add(content);
            root.Controls.Add(footer);
            root.Controls.Add(titlePanel);

            Controls.Add(root);

            EnableDrag(titlePanel);
            EnableDrag(root);
        }

        public void SetContext(KittingDto kitting, KittingDetailDto? detail = null)
        {
            _selectedKitting = kitting;
            _selectedDetail = detail;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_selectedKitting is null)
            {
                ShowMessage("No se encontro el embarque seleccionado.", DialogMessageEnum.Error);
                Close();
                return;
            }

            await Task.WhenAll(
                CargarContextoAsync(),
                CargarStatusesAsync(_selectedDetail?.Status));

            if (_selectedDetail is not null)
            {
                await CargarDatosAsync();
            }
        }

        private async Task CargarStatusesAsync(string? preferredStatus = null)
        {
            var statuses = new List<InventaryStatusDto>();

            try
            {
                var response = await _inventaryStatusService.GetInventaryStatus();
                if (!response.IsSuccess)
                {
                    ShowMessage(response.Message, DialogMessageEnum.Warning, response.ErrorMessage);
                }
                else if (response.Data is not null)
                {
                    statuses = response.Data;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("No se pudo cargar el catalogo de estatus.", DialogMessageEnum.Warning, ex.Message);
            }

            InventaryStatusComboHelper.LoadOptions(_statusCombo, statuses, preferredStatus);
        }

        private async Task CargarContextoAsync()
        {
            try
            {
                _loadingData = true;

                var kittingResponse = await _kittingService.GetKittingById(_selectedKitting!.KittingId);
                if (!kittingResponse.IsSuccess || kittingResponse.Data is null)
                {
                    ShowMessage(kittingResponse.Message, DialogMessageEnum.Error, kittingResponse.ErrorMessage);
                    return;
                }

                _kittingRequest = kittingResponse.Data;

                var productsResponse = await _productService.GetProductByClientId(_kittingRequest.ClientId, _kittingRequest.ProjectId);
                if (!productsResponse.IsSuccess)
                {
                    ShowMessage(productsResponse.Message, DialogMessageEnum.Error, productsResponse.ErrorMessage);
                    return;
                }

                var products = productsResponse.Data ?? new List<ProductAutocompleteDto>();
                _productCombo.DataSource = products;
                _productCombo.DisplayMember = nameof(ProductAutocompleteDto.NumeroParte);
                _productCombo.ValueMember = nameof(ProductAutocompleteDto.ItemId);

                if (products.Count > 1)
                {
                    _productCombo.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("No se pudo cargar el contexto de la linea.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _loadingData = false;
            }
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                var response = await _kittingDetailService.GetKittingDetailById(_selectedDetail!.KittingDetailId);
                if (!response.IsSuccess || response.Data is null)
                {
                    ShowMessage(response.Message, DialogMessageEnum.Error, response.ErrorMessage);
                    return;
                }

                var request = response.Data;

                _loadingData = true;
                _titleLabel.Text = "Editar Linea";
                _saveButton.Text = "Guardar cambios";

                if (request.ProductId > 0)
                {
                    _productCombo.SelectedValue = request.ProductId;
                }

                _partNumberText.Text = request.PartNumber;
                _descriptionText.Text = request.Description ?? string.Empty;
                _quantityText.Text = request.Quantity.ToString();
                InventaryStatusComboHelper.SetValue(_statusCombo, request.Status);
                _sdText.Text = request.SD ?? string.Empty;
                _lotText.Text = request.LotNumber ?? string.Empty;
                SetDate(_expirationPicker, request.ExpirationDate);
                _referenceText.Text = request.CustomerReference ?? string.Empty;
                _exchangeRateText.Text = request.ExchangeRate?.ToString() ?? string.Empty;
                _purchaseOrderText.Text = request.PurchaseOrder ?? string.Empty;
                _customsText.Text = request.CustomsDeclarationNumber ?? string.Empty;
                _standardQtyText.Text = request.StandardQuantity?.ToString() ?? string.Empty;
                _maxQtyText.Text = request.MaximumQuantity?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                ShowMessage("No se pudo cargar la linea seleccionada.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _loadingData = false;
            }
        }

        private void comboProducto_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_loadingData)
            {
                return;
            }

            if (_productCombo.SelectedItem is not ProductAutocompleteDto product)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_partNumberText.Text))
            {
                _partNumberText.Text = product.NumeroParte;
            }

            if (string.IsNullOrWhiteSpace(_descriptionText.Text))
            {
                _descriptionText.Text = product.Descripcion;
            }

            if (string.IsNullOrWhiteSpace(_standardQtyText.Text) && product.StandardPackageValue.HasValue)
            {
                _standardQtyText.Text = product.StandardPackageValue.Value.ToString();
            }

            if (string.IsNullOrWhiteSpace(_maxQtyText.Text) && product.MaxUnitValue.HasValue)
            {
                _maxQtyText.Text = product.MaxUnitValue.Value.ToString();
            }
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                _saveButton.Enabled = false;

                var request = BuildRequest();
                if (request.KittingId <= 0)
                {
                    ShowMessage("No se encontro el embarque de la linea.", DialogMessageEnum.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(request.PartNumber))
                {
                    ShowMessage("Captura el numero de parte.", DialogMessageEnum.Warning);
                    return;
                }

                var result = await SaveAsync(request);
                if (!result.IsSuccess)
                {
                    ShowMessage(result.Message, DialogMessageEnum.Error, result.ErrorMessage);
                    return;
                }

                ResponseForm = true;
                ShowMessage(result.Message, DialogMessageEnum.Success);
                Close();
            }
            catch (Exception ex)
            {
                ShowMessage("Hubo un error al guardar la linea.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _saveButton.Enabled = true;
            }
        }

        private KittingDetailRequest BuildRequest()
        {
            return new KittingDetailRequest
            {
                KittingId = _selectedKitting?.KittingId ?? 0,
                ProductId = int.TryParse(_productCombo.SelectedValue?.ToString(), out var productId) ? productId : 0,
                PartNumber = _partNumberText.Text.Trim(),
                Description = NullIfWhiteSpace(_descriptionText.Text),
                Quantity = decimal.TryParse(_quantityText.Text, out var quantity) ? quantity : 0,
                Status = InventaryStatusComboHelper.GetValue(_statusCombo),
                SD = NullIfWhiteSpace(_sdText.Text),
                LotNumber = NullIfWhiteSpace(_lotText.Text),
                ExpirationDate = GetDate(_expirationPicker),
                CustomerReference = NullIfWhiteSpace(_referenceText.Text),
                ExchangeRate = decimal.TryParse(_exchangeRateText.Text, out var exchangeRate) ? exchangeRate : null,
                PurchaseOrder = NullIfWhiteSpace(_purchaseOrderText.Text),
                CustomsDeclarationNumber = NullIfWhiteSpace(_customsText.Text),
                StandardQuantity = decimal.TryParse(_standardQtyText.Text, out var standardQty) ? standardQty : null,
                MaximumQuantity = decimal.TryParse(_maxQtyText.Text, out var maxQty) ? maxQty : null
            };
        }

        private async Task<ApiResponseDto<string>> SaveAsync(KittingDetailRequest request)
        {
            return _selectedDetail is null
                ? await _kittingDetailService.CreateKittingDetail(request)
                : await _kittingDetailService.UpdateKittingDetail(_selectedDetail.KittingDetailId, request);
        }

        private static TableLayoutPanel CreateFieldTable(int rows)
        {
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4
            };

            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            for (var index = 0; index < rows; index++)
            {
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            return table;
        }

        private static void AddField(TableLayoutPanel table, int rowIndex, string leftLabel, Control leftControl, string rightLabel, Control rightControl)
        {
            table.Controls.Add(CreateFieldLabel(leftLabel), 0, rowIndex);
            leftControl.Dock = DockStyle.Fill;
            leftControl.Margin = new Padding(3, 3, 16, 8);
            table.Controls.Add(leftControl, 1, rowIndex);

            table.Controls.Add(CreateFieldLabel(rightLabel), 2, rowIndex);
            rightControl.Dock = DockStyle.Fill;
            rightControl.Margin = new Padding(3, 3, 0, 8);
            table.Controls.Add(rightControl, 3, rowIndex);
        }

        private static Label CreateFieldLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 7, 8, 0)
            };
        }

        private static TextBox CreateTextBox()
        {
            return new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        private static DateTimePicker CreateNullableDatePicker()
        {
            return new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true
            };
        }

        private static DateTime? GetDate(DateTimePicker picker) => picker.Checked ? picker.Value.Date : null;

        private static void SetDate(DateTimePicker picker, DateTime? value)
        {
            picker.Checked = value.HasValue;
            picker.Value = value ?? DateTime.Today;
        }

        private static string? NullIfWhiteSpace(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private void ShowMessage(string message, DialogMessageEnum type, string? details = null)
        {
            var finalMessage = string.IsNullOrWhiteSpace(details)
                ? message
                : $"{message}{Environment.NewLine}{Environment.NewLine}{details}";

            _dialogMessageService.Show(finalMessage, type);
        }
    }
}
