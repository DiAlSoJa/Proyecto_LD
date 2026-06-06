using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.Enums;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;

namespace LD.Forms.Views.Dialogs
{
    public class FrmNuevoKitting : DraggableForm
    {
        private readonly KittingService _kittingService;
        private readonly LookupService _lookupService;
        private readonly ProjectService _projectService;
        private readonly DialogMessageService _dialogMessageService;

        private KittingDto? _selectedKitting;
        private bool _loadingData;

        private readonly Label _titleLabel;
        private readonly Label _codePreviewLabel;

        private readonly ComboBox _clientCombo;
        private readonly ComboBox _projectCombo;
        private readonly TextBox _invoiceText;
        private readonly TextBox _guideText;
        private readonly DateTimePicker _etaPicker;
        private readonly TextBox _packagesText;
        private readonly CheckBox _isReturnCheck;
        private readonly CheckBox _customerMovementCheck;
        private readonly TextBox _transportLineText;
        private readonly TextBox _vehicleTypeText;
        private readonly TextBox _driverNameText;
        private readonly TextBox _vehiclePlateText;
        private readonly TextBox _sealText;
        private readonly TextBox _contactText;
        private readonly TextBox _addressText;
        private readonly TextBox _colonyText;
        private readonly TextBox _cityText;
        private readonly TextBox _phoneText;
        private readonly TextBox _zipCodeText;
        private readonly TextBox _deliveryTypeText;
        private readonly DateTimePicker _scheduledDatePicker;
        private readonly Button _saveButton;

        public FrmNuevoKitting(
            KittingService kittingService,
            LookupService lookupService,
            ProjectService projectService,
            DialogMessageService dialogMessageService)
        {
            _kittingService = kittingService;
            _lookupService = lookupService;
            _projectService = projectService;
            _dialogMessageService = dialogMessageService;

            Text = "Nuevo Embarque";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(980, 720);

            var rootPanel = new Panel
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
                Text = "Nuevo Embarque",
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

            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                AutoScroll = true
            };

            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1
            };

            _clientCombo = CreateComboBox();
            _projectCombo = CreateComboBox();
            _invoiceText = CreateTextBox();
            _guideText = CreateTextBox();
            _etaPicker = CreateNullableDatePicker();
            _packagesText = CreateTextBox();
            _isReturnCheck = new CheckBox { Text = "Devolucion", AutoSize = true };
            _customerMovementCheck = new CheckBox { Text = "Movimiento requerido por cliente", AutoSize = true };
            _transportLineText = CreateTextBox();
            _vehicleTypeText = CreateTextBox();
            _driverNameText = CreateTextBox();
            _vehiclePlateText = CreateTextBox();
            _sealText = CreateTextBox();
            _contactText = CreateTextBox();
            _addressText = CreateTextBox();
            _colonyText = CreateTextBox();
            _cityText = CreateTextBox();
            _phoneText = CreateTextBox();
            _zipCodeText = CreateTextBox();
            _deliveryTypeText = CreateTextBox();
            _scheduledDatePicker = CreateNullableDatePicker();

            _clientCombo.SelectedIndexChanged += comboCliente_SelectedIndexChanged;
            _projectCombo.SelectedIndexChanged += comboProyecto_SelectedIndexChanged;

            _codePreviewLabel = new Label
            {
                AutoSize = true,
                ForeColor = Color.DarkGreen,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Padding = new Padding(0, 6, 0, 0),
                Text = "Selecciona un proyecto para generar el codigo."
            };

            mainLayout.Controls.Add(CreateGeneralGroup(), 0, 0);
            mainLayout.Controls.Add(CreateTransportGroup(), 0, 1);
            mainLayout.Controls.Add(CreateDeliveryGroup(), 0, 2);

            contentPanel.Controls.Add(mainLayout);

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
                Text = "Guardar Embarque",
                Width = 170,
                Height = 34
            };
            _saveButton.Click += btnSave_Click;

            footer.Controls.Add(closeFooterButton);
            footer.Controls.Add(_saveButton);

            rootPanel.Controls.Add(contentPanel);
            rootPanel.Controls.Add(footer);
            rootPanel.Controls.Add(titlePanel);

            Controls.Add(rootPanel);

            EnableDrag(titlePanel);
            EnableDrag(rootPanel);
        }

        public void SetKitting(KittingDto? kitting)
        {
            _selectedKitting = kitting;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            await CargarClientesAsync();

            if (_selectedKitting is not null)
            {
                await CargarDatosAsync();
            }
            else
            {
                await ActualizarPreviewCodigoAsync();
            }
        }

        private GroupBox CreateGeneralGroup()
        {
            var group = new GroupBox
            {
                Text = "Kitting",
                Dock = DockStyle.Top,
                Width = 930,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 0, 12)
            };

            var table = CreateFieldTable(5);

            AddField(table, 0, "Cliente:", _clientCombo, "Proyecto:", _projectCombo);
            table.Controls.Add(_codePreviewLabel, 0, 1);
            table.SetColumnSpan(_codePreviewLabel, 4);
            AddField(table, 2, "No. Factura:", _invoiceText, "No. Guia:", _guideText);
            AddField(table, 3, "ETA:", _etaPicker, "Bultos:", _packagesText);

            var optionsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = true
            };
            optionsPanel.Controls.Add(_isReturnCheck);
            optionsPanel.Controls.Add(_customerMovementCheck);

            table.Controls.Add(optionsPanel, 0, 4);
            table.SetColumnSpan(optionsPanel, 4);

            group.Controls.Add(table);
            return group;
        }

        private GroupBox CreateTransportGroup()
        {
            var group = new GroupBox
            {
                Text = "Transporte",
                Dock = DockStyle.Top,
                Width = 930,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 0, 12)
            };

            var table = CreateFieldTable(3);
            AddField(table, 0, "Linea de Transporte:", _transportLineText, "Tipo de Vehiculo:", _vehicleTypeText);
            AddField(table, 1, "Nombre de Chofer:", _driverNameText, "Placas:", _vehiclePlateText);
            AddField(table, 2, "Sello:", _sealText, string.Empty, new Panel());

            group.Controls.Add(table);
            return group;
        }

        private GroupBox CreateDeliveryGroup()
        {
            var group = new GroupBox
            {
                Text = "Entrega",
                Dock = DockStyle.Top,
                Width = 930,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 0, 12)
            };

            var table = CreateFieldTable(4);
            AddField(table, 0, "Contacto:", _contactText, "Telefono:", _phoneText);
            AddField(table, 1, "Direccion:", _addressText, "Colonia:", _colonyText);
            AddField(table, 2, "Ciudad:", _cityText, "Codigo Postal:", _zipCodeText);
            AddField(table, 3, "Tipo Entrega:", _deliveryTypeText, "Fecha Programada:", _scheduledDatePicker);

            group.Controls.Add(table);
            return group;
        }

        private async Task CargarClientesAsync()
        {
            try
            {
                _loadingData = true;

                var response = await _lookupService.GetClientLookup();
                if (!response.IsSuccess)
                {
                    ShowMessage(response.Message, DialogMessageEnum.Error, response.ErrorMessage);
                    return;
                }

                var clients = response.Data ?? new List<DropDownDto>();
                _clientCombo.DataSource = clients;
                _clientCombo.DisplayMember = "Value";
                _clientCombo.ValueMember = "Key";

                if (clients.Count > 1)
                {
                    _clientCombo.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("No se pudieron cargar los clientes.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _loadingData = false;
            }
        }

        private async Task CargarProyectosAsync(string? selectedProjectId = null)
        {
            var clientId = GetSelectedId(_clientCombo);
            if (clientId <= 0)
            {
                _projectCombo.DataSource = null;
                _codePreviewLabel.Text = "Selecciona un cliente y un proyecto.";
                return;
            }

            try
            {
                _loadingData = true;

                var response = await _lookupService.GetProjectClientLookup(clientId);
                if (!response.IsSuccess)
                {
                    _projectCombo.DataSource = null;
                    ShowMessage(response.Message, DialogMessageEnum.Error, response.ErrorMessage);
                    return;
                }

                var projects = response.Data ?? new List<DropDownDto>();
                _projectCombo.DataSource = projects;
                _projectCombo.DisplayMember = "Value";
                _projectCombo.ValueMember = "Key";

                if (!string.IsNullOrWhiteSpace(selectedProjectId))
                {
                    _projectCombo.SelectedValue = selectedProjectId;
                }
                else if (projects.Count > 1)
                {
                    _projectCombo.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("No se pudieron cargar los proyectos.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _loadingData = false;
            }
        }

        private async Task CargarDatosAsync()
        {
            if (_selectedKitting is null)
            {
                return;
            }

            try
            {
                var response = await _kittingService.GetKittingById(_selectedKitting.KittingId);
                if (!response.IsSuccess || response.Data is null)
                {
                    ShowMessage(response.Message, DialogMessageEnum.Error, response.ErrorMessage);
                    return;
                }

                var request = response.Data;

                _loadingData = true;

                _titleLabel.Text = "Editar Embarque";
                _saveButton.Text = "Guardar cambios";

                _clientCombo.SelectedValue = request.ClientId.ToString();
                await CargarProyectosAsync(request.ProjectId.ToString());
                _projectCombo.SelectedValue = request.ProjectId.ToString();

                _invoiceText.Text = request.InvoiceNumber ?? string.Empty;
                _guideText.Text = request.GuideNumber ?? string.Empty;
                SetDate(_etaPicker, request.Eta);
                _packagesText.Text = request.PackagesQty?.ToString() ?? string.Empty;
                _isReturnCheck.Checked = request.IsReturn;
                _customerMovementCheck.Checked = request.IsCustomerMovementRequired;
                _transportLineText.Text = request.TransportLine ?? string.Empty;
                _vehicleTypeText.Text = request.VehicleType ?? string.Empty;
                _driverNameText.Text = request.DriverName ?? string.Empty;
                _vehiclePlateText.Text = request.VehiclePlate ?? string.Empty;
                _sealText.Text = request.SealNumber ?? string.Empty;
                _contactText.Text = request.Contacto ?? string.Empty;
                _addressText.Text = request.Direccion ?? string.Empty;
                _colonyText.Text = request.Colonia ?? string.Empty;
                _cityText.Text = request.Ciudad ?? string.Empty;
                _phoneText.Text = request.Telefono ?? string.Empty;
                _zipCodeText.Text = request.CodigoPostal ?? string.Empty;
                _deliveryTypeText.Text = request.TipoEntrega ?? string.Empty;
                SetDate(_scheduledDatePicker, request.FechaProgramada);

                _codePreviewLabel.Text = string.IsNullOrWhiteSpace(request.KittingCode)
                    ? "Codigo actual no disponible."
                    : $"Codigo actual: {request.KittingCode}";
            }
            catch (Exception ex)
            {
                ShowMessage("No se pudo cargar el embarque seleccionado.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _loadingData = false;
            }
        }

        private async Task ActualizarPreviewCodigoAsync()
        {
            var projectId = GetSelectedId(_projectCombo);
            if (projectId <= 0)
            {
                _codePreviewLabel.Text = "Selecciona un proyecto para generar el codigo.";
                _codePreviewLabel.ForeColor = Color.DarkGreen;
                return;
            }

            try
            {
                var response = await _projectService.GetProjectById(projectId);
                if (!response.IsSuccess || response.Data is null)
                {
                    _codePreviewLabel.Text = response.Message;
                    _codePreviewLabel.ForeColor = Color.Firebrick;
                    return;
                }

                var project = response.Data;
                if (string.IsNullOrWhiteSpace(project.KittingPrefix))
                {
                    _codePreviewLabel.Text = "El proyecto no tiene configurado KittingPrefix.";
                    _codePreviewLabel.ForeColor = Color.Firebrick;
                    return;
                }

                var nextNumber = 1;
                if (!string.IsNullOrWhiteSpace(project.KittingNumber) &&
                    int.TryParse(project.KittingNumber, out var parsedNumber) &&
                    parsedNumber > 0)
                {
                    nextNumber = parsedNumber;
                }

                _codePreviewLabel.Text = $"Codigo sugerido: {project.KittingPrefix}{nextNumber:D5}";
                _codePreviewLabel.ForeColor = Color.DarkGreen;
            }
            catch (Exception ex)
            {
                _codePreviewLabel.Text = ex.Message;
                _codePreviewLabel.ForeColor = Color.Firebrick;
            }
        }

        private async void comboCliente_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_loadingData)
            {
                return;
            }

            await CargarProyectosAsync();
            await ActualizarPreviewCodigoAsync();
        }

        private async void comboProyecto_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_loadingData)
            {
                return;
            }

            await ActualizarPreviewCodigoAsync();
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                _saveButton.Enabled = false;

                var request = BuildRequest();
                if (request.ClientId <= 0)
                {
                    ShowMessage("Selecciona un cliente.", DialogMessageEnum.Warning);
                    return;
                }

                if (request.ProjectId <= 0)
                {
                    ShowMessage("Selecciona un proyecto.", DialogMessageEnum.Warning);
                    return;
                }

                var result = await SaveKittingAsync(request);
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
                ShowMessage("Hubo un error al guardar el embarque.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _saveButton.Enabled = true;
            }
        }

        private KittingRequest BuildRequest()
        {
            return new KittingRequest
            {
                ClientId = GetSelectedId(_clientCombo),
                ProjectId = GetSelectedId(_projectCombo),
                InvoiceNumber = NullIfWhiteSpace(_invoiceText.Text),
                GuideNumber = NullIfWhiteSpace(_guideText.Text),
                Eta = GetDate(_etaPicker),
                PackagesQty = int.TryParse(_packagesText.Text, out var packages) ? packages : null,
                IsReturn = _isReturnCheck.Checked,
                IsCustomerMovementRequired = _customerMovementCheck.Checked,
                TransportLine = NullIfWhiteSpace(_transportLineText.Text),
                VehicleType = NullIfWhiteSpace(_vehicleTypeText.Text),
                DriverName = NullIfWhiteSpace(_driverNameText.Text),
                VehiclePlate = NullIfWhiteSpace(_vehiclePlateText.Text),
                SealNumber = NullIfWhiteSpace(_sealText.Text),
                Contacto = NullIfWhiteSpace(_contactText.Text),
                Direccion = NullIfWhiteSpace(_addressText.Text),
                Colonia = NullIfWhiteSpace(_colonyText.Text),
                Ciudad = NullIfWhiteSpace(_cityText.Text),
                Telefono = NullIfWhiteSpace(_phoneText.Text),
                CodigoPostal = NullIfWhiteSpace(_zipCodeText.Text),
                TipoEntrega = NullIfWhiteSpace(_deliveryTypeText.Text),
                FechaProgramada = GetDate(_scheduledDatePicker),
                Status = _selectedKitting?.Status
            };
        }

        private async Task<ApiResponseDto<string>> SaveKittingAsync(KittingRequest request)
        {
            return _selectedKitting is null
                ? await _kittingService.CreateKitting(request)
                : await _kittingService.UpdateKitting(_selectedKitting.KittingId, request);
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
            var leftText = CreateFieldLabel(leftLabel);
            table.Controls.Add(leftText, 0, rowIndex);
            leftControl.Dock = DockStyle.Fill;
            leftControl.Margin = new Padding(3, 3, 16, 8);
            table.Controls.Add(leftControl, 1, rowIndex);

            if (!string.IsNullOrWhiteSpace(rightLabel))
            {
                var rightText = CreateFieldLabel(rightLabel);
                table.Controls.Add(rightText, 2, rowIndex);
            }

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

        private static ComboBox CreateComboBox()
        {
            return new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 280
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

        private static int GetSelectedId(ComboBox combo)
        {
            return int.TryParse(combo.SelectedValue?.ToString(), out var id) ? id : 0;
        }

        private static string? NullIfWhiteSpace(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static DateTime? GetDate(DateTimePicker picker)
        {
            return picker.Checked ? picker.Value.Date : null;
        }

        private static void SetDate(DateTimePicker picker, DateTime? value)
        {
            picker.Checked = value.HasValue;
            picker.Value = value ?? DateTime.Today;
        }

        private void ShowMessage(string message, DialogMessageEnum type, string? details = null)
        {
            var finalMessage = string.IsNullOrWhiteSpace(details)
                ? message
                : $"{message}{Environment.NewLine}{Environment.NewLine}{details}";

            _dialogMessageService.Show(finalMessage, type);
        }
    }
}
