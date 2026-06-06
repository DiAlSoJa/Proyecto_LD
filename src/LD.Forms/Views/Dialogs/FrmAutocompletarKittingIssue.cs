using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.Enums;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Kitting;
using LD.Forms.Classes;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Common;

namespace LD.Forms.Views.Dialogs
{
    public class FrmAutocompletarKittingIssue : DraggableForm
    {
        private readonly LookupService _lookupService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly DialogMessageService _dialogMessageService;

        private readonly ComboBox _locationCombo;
        private readonly ComboBox _statusCombo;
        private readonly TextBox _sdText;
        private readonly RadioButton _selectedDetailScopeRadio;
        private readonly RadioButton _shipmentScopeRadio;
        private readonly CheckBox _onlyEmptyCheck;
        private readonly Button _applyButton;

        private KittingDetailDto? _selectedDetail;
        private KittingIssueDetailDto? _selectedIssue;
        private int _selectedDetailIssueCount;
        private int _shipmentIssueCount;
        private bool _loadingData;

        public int? LocationId { get; private set; }
        public string? LocationCode { get; private set; }
        public string? StatusValue { get; private set; }
        public string? SdValue { get; private set; }
        public bool OnlyFillEmpty { get; private set; } = true;
        public bool ApplyToAllShipment { get; private set; }

        public FrmAutocompletarKittingIssue(
            LookupService lookupService,
            InventaryStatusService inventaryStatusService,
            DialogMessageService dialogMessageService)
        {
            _lookupService = lookupService;
            _inventaryStatusService = inventaryStatusService;
            _dialogMessageService = dialogMessageService;

            Text = "Autocompletar Issue Details";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(640, 340);

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

            var titleLabel = new Label
            {
                Text = "Autocompletar Ubicacion / Status / SD",
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
            titlePanel.Controls.Add(titleLabel);

            var content = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14)
            };

            var descriptionLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 44,
                Text = "Captura los valores que quieras replicar. Los campos vacios no se aplican.",
            };

            _locationCombo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 260
            };

            _statusCombo = InventaryStatusComboHelper.CreateComboBox();
            _sdText = CreateTextBox();

            var fieldTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 2
            };
            fieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            fieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            AddField(fieldTable, 0, "Ubicacion:", _locationCombo);
            AddField(fieldTable, 1, "Status:", _statusCombo);
            AddField(fieldTable, 2, "SD:", _sdText);

            var scopeGroup = new GroupBox
            {
                Dock = DockStyle.Top,
                Height = 102,
                Text = "Aplicar a"
            };

            _selectedDetailScopeRadio = new RadioButton
            {
                AutoSize = true,
                Location = new Point(16, 28)
            };

            _shipmentScopeRadio = new RadioButton
            {
                AutoSize = true,
                Location = new Point(16, 56)
            };

            scopeGroup.Controls.Add(_selectedDetailScopeRadio);
            scopeGroup.Controls.Add(_shipmentScopeRadio);

            _onlyEmptyCheck = new CheckBox
            {
                Dock = DockStyle.Top,
                Height = 30,
                Text = "Solo completar campos vacios",
                Checked = true,
                Padding = new Padding(0, 6, 0, 0)
            };

            content.Controls.Add(scopeGroup);
            content.Controls.Add(_onlyEmptyCheck);
            content.Controls.Add(fieldTable);
            content.Controls.Add(descriptionLabel);

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

            _applyButton = new Button
            {
                Text = "Aplicar",
                Width = 170,
                Height = 34
            };
            _applyButton.Click += applyButton_Click;

            footer.Controls.Add(closeFooterButton);
            footer.Controls.Add(_applyButton);

            root.Controls.Add(content);
            root.Controls.Add(footer);
            root.Controls.Add(titlePanel);

            Controls.Add(root);

            EnableDrag(titlePanel);
            EnableDrag(root);
        }

        public void SetContext(
            KittingDetailDto? selectedDetail,
            KittingIssueDetailDto? selectedIssue,
            int selectedDetailIssueCount,
            int shipmentIssueCount)
        {
            _selectedDetail = selectedDetail;
            _selectedIssue = selectedIssue;
            _selectedDetailIssueCount = selectedDetailIssueCount;
            _shipmentIssueCount = shipmentIssueCount;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ConfigureScopeOptions();
            await Task.WhenAll(
                LoadLocationsAsync(),
                LoadStatusesAsync(_selectedIssue?.Status ?? _selectedDetail?.Status));
            SeedDefaultValues();
        }

        private async Task LoadLocationsAsync()
        {
            try
            {
                _loadingData = true;

                var response = await _lookupService.GetLocationLookup();
                if (!response.IsSuccess)
                {
                    ShowMessage(response.Message, DialogMessageEnum.Warning, response.ErrorMessage);
                    _locationCombo.DataSource = new List<DropDownDto>();
                    return;
                }

                var locations = response.Data ?? new List<DropDownDto>();
                _locationCombo.DataSource = locations;
                _locationCombo.DisplayMember = nameof(DropDownDto.Value);
                _locationCombo.ValueMember = nameof(DropDownDto.Key);
                _locationCombo.SelectedIndex = -1;

                if (_selectedIssue?.LocationId is not null)
                {
                    _locationCombo.SelectedValue = _selectedIssue.LocationId.Value.ToString();
                }
                else if (!string.IsNullOrWhiteSpace(_selectedIssue?.LocationCode))
                {
                    var selected = locations.FirstOrDefault(x =>
                        string.Equals(x.Value, _selectedIssue.LocationCode, StringComparison.OrdinalIgnoreCase));

                    if (selected is not null)
                    {
                        _locationCombo.SelectedValue = selected.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("No se pudieron cargar las ubicaciones.", DialogMessageEnum.Warning, ex.Message);
                _locationCombo.DataSource = new List<DropDownDto>();
            }
            finally
            {
                _loadingData = false;
            }
        }

        private async Task LoadStatusesAsync(string? preferredStatus = null)
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

        private void ConfigureScopeOptions()
        {
            _selectedDetailScopeRadio.Text = _selectedDetailIssueCount > 0
                ? $"Linea seleccionada ({_selectedDetailIssueCount} issue(s))"
                : "Linea seleccionada (sin issues)";

            _shipmentScopeRadio.Text = $"Todo el embarque ({_shipmentIssueCount} issue(s))";

            _selectedDetailScopeRadio.Enabled = _selectedDetailIssueCount > 0;
            _shipmentScopeRadio.Enabled = _shipmentIssueCount > 0;

            if (_selectedDetailIssueCount > 0)
            {
                _selectedDetailScopeRadio.Checked = true;
            }
            else
            {
                _shipmentScopeRadio.Checked = true;
            }
        }

        private void SeedDefaultValues()
        {
            InventaryStatusComboHelper.SetValue(
                _statusCombo,
                _selectedIssue?.Status
                ?? _selectedDetail?.Status);

            _sdText.Text = _selectedIssue?.SD
                ?? _selectedDetail?.SD
                ?? string.Empty;
        }

        private void applyButton_Click(object? sender, EventArgs e)
        {
            if (_loadingData)
            {
                return;
            }

            var selectedLocation = _locationCombo.SelectedItem as DropDownDto;
            var statusValue = InventaryStatusComboHelper.GetValue(_statusCombo);
            var sdValue = NullIfWhiteSpace(_sdText.Text);

            if (selectedLocation is null && string.IsNullOrWhiteSpace(statusValue) && string.IsNullOrWhiteSpace(sdValue))
            {
                ShowMessage("Captura al menos uno de estos valores: Ubicacion, Status o SD.", DialogMessageEnum.Warning);
                return;
            }

            if (!_selectedDetailScopeRadio.Checked && !_shipmentScopeRadio.Checked)
            {
                ShowMessage("Selecciona el alcance del autocompletado.", DialogMessageEnum.Warning);
                return;
            }

            LocationId = int.TryParse(selectedLocation?.Key, out var locationId) ? locationId : null;
            LocationCode = selectedLocation?.Value;
            StatusValue = statusValue;
            SdValue = sdValue;
            OnlyFillEmpty = _onlyEmptyCheck.Checked;
            ApplyToAllShipment = _shipmentScopeRadio.Checked;

            ResponseForm = true;
            Close();
        }

        private void ShowMessage(string message, DialogMessageEnum type, string? details = null)
        {
            var finalMessage = string.IsNullOrWhiteSpace(details)
                ? message
                : $"{message}{Environment.NewLine}{Environment.NewLine}{details}";

            _dialogMessageService.Show(finalMessage, type);
        }

        private static void AddField(TableLayoutPanel table, int rowIndex, string label, Control control)
        {
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.Controls.Add(CreateFieldLabel(label), 0, rowIndex);
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(3, 3, 0, 10);
            table.Controls.Add(control, 1, rowIndex);
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

        private static string? NullIfWhiteSpace(string? value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? null
                : value.Trim();
        }
    }
}
