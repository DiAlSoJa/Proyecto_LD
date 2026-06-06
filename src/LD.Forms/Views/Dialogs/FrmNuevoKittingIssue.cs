using LD.Client.Services;
using LD.Contracts.DTOs;
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
    public class FrmNuevoKittingIssue : DraggableForm
    {
        private readonly KittingService _kittingService;
        private readonly KittingIssueService _kittingIssueService;
        private readonly ProductService _productService;
        private readonly LookupService _lookupService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly DialogMessageService _dialogMessageService;

        private KittingDetailDto? _selectedDetail;
        private KittingIssueDetailDto? _selectedIssue;
        private KittingRequest? _kittingRequest;
        private bool _loadingData;

        private readonly Label _titleLabel;
        private readonly ComboBox _productCombo;
        private readonly ComboBox _locationCombo;
        private readonly TextBox _standardIdText;
        private readonly TextBox _partNumberText;
        private readonly TextBox _descriptionText;
        private readonly TextBox _standardQtyText;
        private readonly TextBox _maxQtyText;
        private readonly TextBox _sdText;
        private readonly TextBox _receivedQtyText;
        private readonly ComboBox _statusCombo;
        private readonly TextBox _lotText;
        private readonly DateTimePicker _expirationPicker;
        private readonly TextBox _referenceText;
        private readonly TextBox _purchaseOrderText;
        private readonly TextBox _customsText;
        private readonly Button _saveButton;

        public FrmNuevoKittingIssue(
            KittingService kittingService,
            KittingIssueService kittingIssueService,
            ProductService productService,
            LookupService lookupService,
            InventaryStatusService inventaryStatusService,
            DialogMessageService dialogMessageService)
        {
            _kittingService = kittingService;
            _kittingIssueService = kittingIssueService;
            _productService = productService;
            _lookupService = lookupService;
            _inventaryStatusService = inventaryStatusService;
            _dialogMessageService = dialogMessageService;

            Text = "Issue Detail";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 620);

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
                Text = "Nuevo Issue Detail",
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

            _locationCombo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 280
            };

            _standardIdText = CreateTextBox();
            _partNumberText = CreateTextBox();
            _descriptionText = CreateTextBox();
            _standardQtyText = CreateTextBox();
            _maxQtyText = CreateTextBox();
            _sdText = CreateTextBox();
            _receivedQtyText = CreateTextBox();
            _statusCombo = InventaryStatusComboHelper.CreateComboBox();
            _lotText = CreateTextBox();
            _expirationPicker = CreateNullableDatePicker();
            _referenceText = CreateTextBox();
            _purchaseOrderText = CreateTextBox();
            _customsText = CreateTextBox();

            var table = CreateFieldTable(7);
            AddField(table, 0, "Producto:", _productCombo, "Ubicacion:", _locationCombo);
            AddField(table, 1, "Standard Id:", _standardIdText, "No. Parte:", _partNumberText);
            AddField(table, 2, "Descripcion:", _descriptionText, "Cantidad:", _receivedQtyText);
            AddField(table, 3, "Estandar:", _standardQtyText, "Maxima:", _maxQtyText);
            AddField(table, 4, "SD:", _sdText, "Status:", _statusCombo);
            AddField(table, 5, "Lote:", _lotText, "Caducidad:", _expirationPicker);
            AddField(table, 6, "Referencia:", _referenceText, "Orden/Pedimento:", CreateDualFieldPanel(_purchaseOrderText, _customsText));

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
                Text = "Guardar Issue",
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

        public void SetContext(KittingDetailDto detail, KittingIssueDetailDto? issue = null)
        {
            _selectedDetail = detail;
            _selectedIssue = issue;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_selectedDetail is null)
            {
                ShowMessage("No se encontro la linea seleccionada.", DialogMessageEnum.Error);
                Close();
                return;
            }

            await Task.WhenAll(
                CargarContextoAsync(),
                CargarStatusesAsync(_selectedIssue?.Status ?? _selectedDetail?.Status));

            if (_selectedIssue is not null)
            {
                await CargarDatosAsync();
            }
            else
            {
                PrellenarDesdeDetalle();
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

                var kittingResponse = await _kittingService.GetKittingById(_selectedDetail!.KittingId);
                if (!kittingResponse.IsSuccess || kittingResponse.Data is null)
                {
                    ShowMessage(kittingResponse.Message, DialogMessageEnum.Error, kittingResponse.ErrorMessage);
                    return;
                }

                _kittingRequest = kittingResponse.Data;

                var productTask = _productService.GetProductByClientId(_kittingRequest.ClientId, _kittingRequest.ProjectId);
                var locationTask = _lookupService.GetLocationLookup();

                await Task.WhenAll(productTask, locationTask);

                var productsResponse = await productTask;
                if (!productsResponse.IsSuccess)
                {
                    ShowMessage(productsResponse.Message, DialogMessageEnum.Error, productsResponse.ErrorMessage);
                    return;
                }

                var locationsResponse = await locationTask;
                if (!locationsResponse.IsSuccess)
                {
                    ShowMessage(locationsResponse.Message, DialogMessageEnum.Error, locationsResponse.ErrorMessage);
                    return;
                }

                _productCombo.DataSource = productsResponse.Data ?? new List<ProductAutocompleteDto>();
                _productCombo.DisplayMember = nameof(ProductAutocompleteDto.NumeroParte);
                _productCombo.ValueMember = nameof(ProductAutocompleteDto.ItemId);
                if (_productCombo.Items.Count > 1)
                {
                    _productCombo.SelectedIndex = -1;
                }

                _locationCombo.DataSource = locationsResponse.Data ?? new List<DropDownDto>();
                _locationCombo.DisplayMember = nameof(DropDownDto.Value);
                _locationCombo.ValueMember = nameof(DropDownDto.Key);
                if (_locationCombo.Items.Count > 1)
                {
                    _locationCombo.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("No se pudo cargar el contexto del issue detail.", DialogMessageEnum.Error, ex.Message);
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
                var response = await _kittingIssueService.GetKittingIssueById(_selectedIssue!.KittingReceiptDetailId);
                if (!response.IsSuccess || response.Data is null)
                {
                    ShowMessage(response.Message, DialogMessageEnum.Error, response.ErrorMessage);
                    return;
                }

                var request = response.Data;

                _loadingData = true;
                _titleLabel.Text = "Editar Issue Detail";
                _saveButton.Text = "Guardar cambios";

                if (request.ProductId.HasValue && request.ProductId.Value > 0)
                {
                    _productCombo.SelectedValue = request.ProductId.Value;
                }

                if (request.LocationId.HasValue)
                {
                    _locationCombo.SelectedValue = request.LocationId.Value.ToString();
                }

                _standardIdText.Text = request.StandardId ?? string.Empty;
                _partNumberText.Text = request.PartNumber;
                _descriptionText.Text = request.Description ?? string.Empty;
                _standardQtyText.Text = request.StandardQuantity?.ToString() ?? string.Empty;
                _maxQtyText.Text = request.MaximumQuantity?.ToString() ?? string.Empty;
                _sdText.Text = request.SD ?? string.Empty;
                _receivedQtyText.Text = request.ReceivedQuantity?.ToString() ?? string.Empty;
                InventaryStatusComboHelper.SetValue(_statusCombo, request.Status);
                _lotText.Text = request.LotNumber ?? string.Empty;
                SetDate(_expirationPicker, request.ExpirationDate);
                _referenceText.Text = request.Reference ?? string.Empty;
                _purchaseOrderText.Text = request.PurchaseOrder ?? string.Empty;
                _customsText.Text = request.CustomsDeclarationNumber ?? string.Empty;
            }
            catch (Exception ex)
            {
                ShowMessage("No se pudo cargar el issue detail seleccionado.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _loadingData = false;
            }
        }

        private void PrellenarDesdeDetalle()
        {
            _partNumberText.Text = _selectedDetail?.PartNumber ?? string.Empty;
            _descriptionText.Text = _selectedDetail?.Description ?? string.Empty;
            _standardQtyText.Text = _selectedDetail?.StandardQuantity?.ToString() ?? string.Empty;
            _maxQtyText.Text = _selectedDetail?.MaximumQuantity?.ToString() ?? string.Empty;
            _sdText.Text = _selectedDetail?.SD ?? string.Empty;
            _receivedQtyText.Text = _selectedDetail?.Quantity.ToString() ?? string.Empty;
            InventaryStatusComboHelper.SetValue(_statusCombo, _selectedDetail?.Status);
            _lotText.Text = _selectedDetail?.LotNumber ?? string.Empty;
            SetDate(_expirationPicker, _selectedDetail?.ExpirationDate);
            _referenceText.Text = _selectedDetail?.CustomerReference ?? string.Empty;
            _purchaseOrderText.Text = _selectedDetail?.PurchaseOrder ?? string.Empty;
            _customsText.Text = _selectedDetail?.CustomsDeclarationNumber ?? string.Empty;

            if (_selectedDetail?.ProductId is not null && _selectedDetail.ProductId.Value > 0)
            {
                _productCombo.SelectedValue = _selectedDetail.ProductId.Value;
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
                if (request.KittingDetailId <= 0)
                {
                    ShowMessage("No se encontro la linea asociada.", DialogMessageEnum.Warning);
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
                ShowMessage("Hubo un error al guardar el issue detail.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _saveButton.Enabled = true;
            }
        }

        private KittingIssueRequest BuildRequest()
        {
            return new KittingIssueRequest
            {
                KittingDetailId = _selectedDetail?.KittingDetailId ?? 0,
                ProductId = int.TryParse(_productCombo.SelectedValue?.ToString(), out var productId) ? productId : null,
                StandardId = NullIfWhiteSpace(_standardIdText.Text),
                PartNumber = _partNumberText.Text.Trim(),
                Description = NullIfWhiteSpace(_descriptionText.Text),
                StandardQuantity = decimal.TryParse(_standardQtyText.Text, out var standardQty) ? standardQty : null,
                MaximumQuantity = decimal.TryParse(_maxQtyText.Text, out var maxQty) ? maxQty : null,
                SD = NullIfWhiteSpace(_sdText.Text),
                ReceivedQuantity = decimal.TryParse(_receivedQtyText.Text, out var receivedQty) ? receivedQty : null,
                Status = InventaryStatusComboHelper.GetValue(_statusCombo),
                LocationId = int.TryParse(_locationCombo.SelectedValue?.ToString(), out var locationId) ? locationId : null,
                LocationCode = (_locationCombo.SelectedItem as DropDownDto)?.Value,
                LotNumber = NullIfWhiteSpace(_lotText.Text),
                ExpirationDate = GetDate(_expirationPicker),
                Reference = NullIfWhiteSpace(_referenceText.Text),
                PurchaseOrder = NullIfWhiteSpace(_purchaseOrderText.Text),
                CustomsDeclarationNumber = NullIfWhiteSpace(_customsText.Text)
            };
        }

        private async Task<ApiResponseDto<string>> SaveAsync(KittingIssueRequest request)
        {
            return _selectedIssue is null
                ? await _kittingIssueService.CreateKittingIssue(request)
                : await _kittingIssueService.UpdateKittingIssue(_selectedIssue.KittingReceiptDetailId, request);
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

        private static Panel CreateDualFieldPanel(TextBox purchaseOrderText, TextBox customsText)
        {
            var panel = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Fill
            };

            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            purchaseOrderText.Margin = new Padding(0, 0, 6, 0);
            customsText.Margin = new Padding(6, 0, 0, 0);

            panel.Controls.Add(purchaseOrderText, 0, 0);
            panel.Controls.Add(customsText, 1, 0);
            return panel;
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
