using LD.Client.Services;
using LD.Contracts.Enums;
using LD.Contracts.InventaryStatus;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Classes;
using LD.Forms.Services.FormServices;
using LD.Forms.Views.Dialogs;

namespace LD.Forms.Views.Forms
{
    public class FrmKitting : Form
    {
        private readonly KittingService _kittingService;
        private readonly KittingDetailService _kittingDetailService;
        private readonly KittingIssueService _kittingIssueService;
        private readonly InventaryStatusService _inventaryStatusService;
        private readonly DialogFormService _dialogFormService;
        private readonly DialogMessageService _dialogMessageService;

        private readonly BindingSource _kittingsBinding = new();
        private readonly BindingSource _detailsBinding = new();
        private readonly BindingSource _issuesBinding = new();

        private readonly Panel _gridContainer;
        private readonly DataGridView _kittingsGrid;
        private readonly DataGridView _detailsGrid;
        private readonly DataGridView _issuesGrid;
        private readonly GroupBox _detailsGroup;
        private readonly GroupBox _issuesGroup;
        private readonly Button _editBtn;
        private readonly Button _addDetailBtn;
        private readonly Button _editDetailBtn;
        private readonly Button _deleteDetailBtn;
        private readonly Button _addIssueBtn;
        private readonly Button _editIssueBtn;
        private readonly Button _deleteIssueBtn;
        private readonly Button _generateBaseIssuesBtn;
        private readonly Button _autocompleteIssuesBtn;
        private readonly Button _locateBtn;
        private readonly Button _confirmBtn;
        private readonly Button _cancelBtn;

        private readonly GridFilter<KittingDto> _gridFilter;

        private bool _loadingMasterData;
        private bool _loadingChildData;
        private bool _issueStatusCatalogWarningShown;
        private List<KittingIssueDetailDto> _allIssues = new();
        private readonly Dictionary<string, InventaryStatusDto> _issueStatusesByKey = new(StringComparer.OrdinalIgnoreCase);
        private KittingDto? _selectedKitting;
        private KittingDetailDto? _selectedDetail;
        private KittingIssueDetailDto? _selectedIssue;

        public FrmKitting(
            KittingService kittingService,
            KittingDetailService kittingDetailService,
            KittingIssueService kittingIssueService,
            InventaryStatusService inventaryStatusService,
            DialogFormService dialogFormService,
            DialogMessageService dialogMessageService)
        {
            _kittingService = kittingService;
            _kittingDetailService = kittingDetailService;
            _kittingIssueService = kittingIssueService;
            _inventaryStatusService = inventaryStatusService;
            _dialogFormService = dialogFormService;
            _dialogMessageService = dialogMessageService;

            Text = "Embarques";
            FormBorderStyle = FormBorderStyle.None;

            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82
            };

            var actions = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(12, 6, 0, 0),
                Height = 82
            };

            var reloadBtn = CreateToolbarButton("Actualizar");
            reloadBtn.Click += reloadBtn_Click;

            var addBtn = CreateToolbarButton("Nuevo Embarque");
            addBtn.Click += AddBtn_Click;

            _editBtn = CreateToolbarButton("Editar");
            _editBtn.Click += EditBtn_Click;

            _addDetailBtn = CreateToolbarButton("Agregar Linea");
            _addDetailBtn.Click += AddDetailBtn_Click;

            _editDetailBtn = CreateToolbarButton("Editar Linea");
            _editDetailBtn.Click += EditDetailBtn_Click;

            _deleteDetailBtn = CreateToolbarButton("Borrar Linea");
            _deleteDetailBtn.Click += DeleteDetailBtn_Click;

            _addIssueBtn = CreateToolbarButton("Agregar Issue");
            _addIssueBtn.Click += AddIssueBtn_Click;

            _editIssueBtn = CreateToolbarButton("Editar Issue");
            _editIssueBtn.Click += EditIssueBtn_Click;

            _deleteIssueBtn = CreateToolbarButton("Borrar Issue");
            _deleteIssueBtn.Click += DeleteIssueBtn_Click;

            _generateBaseIssuesBtn = CreateToolbarButton("Generar Issues Base");
            _generateBaseIssuesBtn.Click += GenerateBaseIssuesBtn_Click;

            _autocompleteIssuesBtn = CreateToolbarButton("Autocompletar Issue");
            _autocompleteIssuesBtn.Click += AutocompleteIssuesBtn_Click;

            _locateBtn = CreateToolbarButton("Ubicando");
            _locateBtn.Click += LocateBtn_Click;

            _confirmBtn = CreateToolbarButton("Confirmar");
            _confirmBtn.Click += ConfirmBtn_Click;

            _cancelBtn = CreateToolbarButton("Cancelar");
            _cancelBtn.Click += CancelBtn_Click;

            actions.Controls.Add(reloadBtn);
            actions.Controls.Add(addBtn);
            actions.Controls.Add(_editBtn);
            actions.Controls.Add(_addDetailBtn);
            actions.Controls.Add(_editDetailBtn);
            actions.Controls.Add(_deleteDetailBtn);
            actions.Controls.Add(_addIssueBtn);
            actions.Controls.Add(_editIssueBtn);
            actions.Controls.Add(_deleteIssueBtn);
            actions.Controls.Add(_generateBaseIssuesBtn);
            actions.Controls.Add(_autocompleteIssuesBtn);
            actions.Controls.Add(_locateBtn);
            actions.Controls.Add(_confirmBtn);
            actions.Controls.Add(_cancelBtn);

            topPanel.Controls.Add(actions);

            _kittingsGrid = CreateGrid();
            _kittingsGrid.DataSource = _kittingsBinding;
            _kittingsGrid.SelectionChanged += dataGridView1_SelectionChanged;
            _kittingsGrid.CellDoubleClick += dataGridView1_CellDoubleClick;

            _detailsGrid = CreateGrid();
            _detailsGrid.DataSource = _detailsBinding;
            _detailsGrid.SelectionChanged += detailsGrid_SelectionChanged;
            _detailsGrid.CellDoubleClick += detailsGrid_CellDoubleClick;

            _issuesGrid = CreateGrid();
            _issuesGrid.DataSource = _issuesBinding;
            _issuesGrid.SelectionChanged += issuesGrid_SelectionChanged;
            _issuesGrid.CellDoubleClick += issuesGrid_CellDoubleClick;

            _gridFilter = new GridFilter<KittingDto>(_kittingsGrid, _kittingsBinding);

            var mainSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 185
            };

            var childSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 165
            };

            mainSplit.Panel1.Controls.Add(CreateGridHost("Embarques", _kittingsGrid));

            _detailsGroup = CreateGridHost("Detalle de kitting", _detailsGrid);
            _issuesGroup = CreateGridHost("Issue details", _issuesGrid);

            childSplit.Panel1.Controls.Add(_detailsGroup);
            childSplit.Panel2.Controls.Add(_issuesGroup);
            mainSplit.Panel2.Controls.Add(childSplit);

            _gridContainer = new Panel
            {
                Dock = DockStyle.Fill
            };
            _gridContainer.Controls.Add(mainSplit);

            Controls.Add(_gridContainer);
            Controls.Add(topPanel);

            UpdateActionButtonsState();
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            await LoaderManager.Run(_gridContainer, CargarDatosAsync, "Trayendo embarques");
        }

        private async Task CargarDatosAsync()
        {
            try
            {
                await LoadIssueStatusCatalogAsync();
                _loadingMasterData = true;

                var result = await _kittingService.GetKittings();
                if (!result.IsSuccess)
                {
                    ShowMessage(result.Message, DialogMessageEnum.Error, result.ErrorMessage);
                    return;
                }

                var data = result.Data ?? new List<KittingDto>();
                _kittingsBinding.DataSource = data;
                _gridFilter.SetData(data);
                _gridFilter.BuildFilterColumns();

                ConfigureSummaryGrid();

                if (_kittingsGrid.Rows.Count == 0)
                {
                    _allIssues = new List<KittingIssueDetailDto>();
                    _selectedKitting = null;
                    _selectedDetail = null;
                    _selectedIssue = null;
                    _detailsBinding.DataSource = new List<KittingDetailDto>();
                    _issuesBinding.DataSource = new List<KittingIssueDetailDto>();
                    ConfigureDetailGrid();
                    ConfigureIssueGrid();
                    UpdateValidationPresentation();
                    UpdateActionButtonsState();
                    return;
                }

                var firstVisibleCell = _kittingsGrid.Rows[0].Cells
                    .Cast<DataGridViewCell>()
                    .FirstOrDefault(c => c.Visible);

                if (firstVisibleCell is not null)
                {
                    _kittingsGrid.CurrentCell = firstVisibleCell;
                }

                _kittingsGrid.Rows[0].Selected = true;
            }
            catch (Exception ex)
            {
                ShowMessage("Hubo un error al cargar los embarques.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _loadingMasterData = false;
            }

            await CargarDetalleSeleccionadoAsync();
        }

        private async Task LoadIssueStatusCatalogAsync()
        {
            try
            {
                var response = await _inventaryStatusService.GetInventaryStatus();
                if (!response.IsSuccess)
                {
                    _issueStatusesByKey.Clear();
                    ShowIssueStatusCatalogWarning(response.Message, response.ErrorMessage);
                    return;
                }

                CacheIssueStatuses(response.Data ?? new List<InventaryStatusDto>());
                _issueStatusCatalogWarningShown = false;
            }
            catch (Exception ex)
            {
                _issueStatusesByKey.Clear();
                ShowIssueStatusCatalogWarning("No se pudo cargar el catalogo de estatus para colorear issues.", ex.Message);
            }
        }

        private void CacheIssueStatuses(IEnumerable<InventaryStatusDto> statuses)
        {
            _issueStatusesByKey.Clear();

            foreach (var status in statuses)
            {
                if (!string.IsNullOrWhiteSpace(status.StatusId))
                {
                    _issueStatusesByKey[status.StatusId.Trim()] = status;
                }

                if (!string.IsNullOrWhiteSpace(status.Descripcion))
                {
                    _issueStatusesByKey[status.Descripcion.Trim()] = status;
                }
            }
        }

        private void ShowIssueStatusCatalogWarning(string message, string? details)
        {
            if (_issueStatusCatalogWarningShown)
            {
                return;
            }

            _issueStatusCatalogWarningShown = true;
            ShowMessage(message, DialogMessageEnum.Warning, details);
        }

        private async Task CargarDetalleSeleccionadoAsync()
        {
            if (_issueStatusesByKey.Count == 0)
            {
                await LoadIssueStatusCatalogAsync();
            }

            if (_kittingsGrid.CurrentRow?.DataBoundItem is not KittingDto selected)
            {
                _allIssues = new List<KittingIssueDetailDto>();
                _selectedKitting = null;
                _selectedDetail = null;
                _selectedIssue = null;
                _detailsBinding.DataSource = new List<KittingDetailDto>();
                _issuesBinding.DataSource = new List<KittingIssueDetailDto>();
                ConfigureDetailGrid();
                ConfigureIssueGrid();
                UpdateValidationPresentation();
                UpdateActionButtonsState();
                return;
            }

            _selectedKitting = selected;

            try
            {
                _loadingChildData = true;

                var detailResult = await _kittingDetailService.GetKittingDetailsByKittingId(selected.KittingId);
                if (!detailResult.IsSuccess)
                {
                    ShowMessage(detailResult.Message, DialogMessageEnum.Error, detailResult.ErrorMessage);
                    return;
                }

                var details = detailResult.Data ?? new List<KittingDetailDto>();
                _detailsBinding.DataSource = details;
                ConfigureDetailGrid();

                if (details.Count == 0)
                {
                    _allIssues = new List<KittingIssueDetailDto>();
                    _selectedDetail = null;
                    _selectedIssue = null;
                    _issuesBinding.DataSource = new List<KittingIssueDetailDto>();
                    ConfigureIssueGrid();
                    UpdateValidationPresentation();
                    UpdateActionButtonsState();
                    return;
                }

                SelectFirstRow(_detailsGrid);
                _selectedDetail = _detailsGrid.CurrentRow?.DataBoundItem as KittingDetailDto
                    ?? details.FirstOrDefault();
                _selectedIssue = null;

                var issueTasks = details
                    .Select(x => _kittingIssueService.GetKittingIssuesByKittingDetailId(x.KittingDetailId))
                    .ToList();

                var responses = await Task.WhenAll(issueTasks);

                var firstError = responses.FirstOrDefault(x => !x.IsSuccess);
                if (firstError is not null)
                {
                    ShowMessage(firstError.Message, DialogMessageEnum.Error, firstError.ErrorMessage);
                    return;
                }

                var issues = responses
                    .Where(x => x.Data is not null)
                    .SelectMany(x => x.Data!)
                    .OrderBy(x => x.KittingDetailId)
                    .ThenBy(x => x.KittingReceiptDetailId)
                    .ToList();

                _allIssues = issues;
                BindIssuesForSelectedDetail();
            }
            catch (Exception ex)
            {
                ShowMessage("Hubo un error al cargar el detalle del embarque.", DialogMessageEnum.Error, ex.Message);
            }
            finally
            {
                _loadingChildData = false;
            }
        }

        private async void AddBtn_Click(object? sender, EventArgs e)
        {
            var form = _dialogFormService.ShowDialog<FrmNuevoKitting>();
            if (form.ResponseForm)
            {
                await LoaderManager.Run(_gridContainer, CargarDatosAsync, "Trayendo embarques");
            }
        }

        private async void AddDetailBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedKitting is null)
            {
                ShowMessage("Selecciona un embarque para agregarle lineas.", DialogMessageEnum.Warning);
                return;
            }

            if (!EnsureSelectedKittingEditable("agregarle lineas"))
            {
                return;
            }

            var form = _dialogFormService.ShowDialog<FrmNuevoKittingDetail>(dialog =>
            {
                dialog.SetContext(_selectedKitting);
            });

            if (form.ResponseForm)
            {
                await LoaderManager.Run(_gridContainer, CargarDetalleSeleccionadoAsync, "Actualizando lineas");
            }
        }

        private async void EditDetailBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedKitting is null || _selectedDetail is null)
            {
                ShowMessage("Selecciona una linea para editarla.", DialogMessageEnum.Warning);
                return;
            }

            if (!EnsureSelectedKittingEditable("editar sus lineas"))
            {
                return;
            }

            var form = _dialogFormService.ShowDialog<FrmNuevoKittingDetail>(dialog =>
            {
                dialog.SetContext(_selectedKitting, _selectedDetail);
            });

            if (form.ResponseForm)
            {
                await LoaderManager.Run(_gridContainer, CargarDetalleSeleccionadoAsync, "Actualizando lineas");
            }
        }

        private async void DeleteDetailBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedDetail is null)
            {
                ShowMessage("Selecciona una linea para eliminarla.", DialogMessageEnum.Warning);
                return;
            }

            if (!EnsureSelectedKittingEditable("eliminar lineas"))
            {
                return;
            }

            var confirm = MessageBox.Show(
                $"Se eliminara la linea {_selectedDetail.PartNumber}.",
                "Eliminar linea",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var result = await _kittingDetailService.DeleteKittingDetail(_selectedDetail.KittingDetailId);
                if (!result.IsSuccess)
                {
                    ShowMessage(result.Message, DialogMessageEnum.Error, result.ErrorMessage);
                    return;
                }

                ShowMessage(result.Message, DialogMessageEnum.Success);
                await LoaderManager.Run(_gridContainer, CargarDetalleSeleccionadoAsync, "Actualizando lineas");
            }
            catch (Exception ex)
            {
                ShowMessage("Hubo un error al eliminar la linea.", DialogMessageEnum.Error, ex.Message);
            }
        }

        private async void AddIssueBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedDetail is null)
            {
                ShowMessage("Selecciona una linea para agregarle issue details.", DialogMessageEnum.Warning);
                return;
            }

            if (!EnsureSelectedKittingEditable("agregar issue details"))
            {
                return;
            }

            var form = _dialogFormService.ShowDialog<FrmNuevoKittingIssue>(dialog =>
            {
                dialog.SetContext(_selectedDetail);
            });

            if (form.ResponseForm)
            {
                await LoaderManager.Run(_gridContainer, CargarDetalleSeleccionadoAsync, "Actualizando issues");
            }
        }

        private async void EditIssueBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedDetail is null || _selectedIssue is null)
            {
                ShowMessage("Selecciona un issue detail para editarlo.", DialogMessageEnum.Warning);
                return;
            }

            if (!EnsureSelectedKittingEditable("editar issue details"))
            {
                return;
            }

            var form = _dialogFormService.ShowDialog<FrmNuevoKittingIssue>(dialog =>
            {
                dialog.SetContext(_selectedDetail, _selectedIssue);
            });

            if (form.ResponseForm)
            {
                await LoaderManager.Run(_gridContainer, CargarDetalleSeleccionadoAsync, "Actualizando issues");
            }
        }

        private async void DeleteIssueBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedIssue is null)
            {
                ShowMessage("Selecciona un issue detail para eliminarlo.", DialogMessageEnum.Warning);
                return;
            }

            if (!EnsureSelectedKittingEditable("eliminar issue details"))
            {
                return;
            }

            var confirm = MessageBox.Show(
                $"Se eliminara el issue detail {_selectedIssue.PartNumber}.",
                "Eliminar issue detail",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var result = await _kittingIssueService.DeleteKittingIssue(_selectedIssue.KittingReceiptDetailId);
                if (!result.IsSuccess)
                {
                    ShowMessage(result.Message, DialogMessageEnum.Error, result.ErrorMessage);
                    return;
                }

                ShowMessage(result.Message, DialogMessageEnum.Success);
                await LoaderManager.Run(_gridContainer, CargarDetalleSeleccionadoAsync, "Actualizando issues");
            }
            catch (Exception ex)
            {
                ShowMessage("Hubo un error al eliminar el issue detail.", DialogMessageEnum.Error, ex.Message);
            }
        }

        private async void GenerateBaseIssuesBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedKitting is null)
            {
                ShowMessage("Selecciona un embarque para generar sus issues base.", DialogMessageEnum.Warning);
                return;
            }

            if (!EnsureSelectedKittingEditable("generar sus issues base"))
            {
                return;
            }

            var details = _detailsBinding.DataSource as List<KittingDetailDto> ?? new List<KittingDetailDto>();
            if (details.Count == 0)
            {
                ShowMessage("El embarque no tiene lineas para generar issues base.", DialogMessageEnum.Warning);
                return;
            }

            var issues = _allIssues;
            var detailIdsWithIssues = issues
                .Select(x => x.KittingDetailId)
                .Distinct()
                .ToHashSet();

            var missingDetails = details
                .Where(x => !detailIdsWithIssues.Contains(x.KittingDetailId))
                .ToList();

            if (missingDetails.Count == 0)
            {
                ShowMessage("Todas las lineas ya tienen al menos un issue detail.", DialogMessageEnum.Info);
                return;
            }

            try
            {
                var createdCount = 0;

                foreach (var detail in missingDetails)
                {
                    var result = await _kittingIssueService.CreateKittingIssue(BuildBaseIssueRequest(detail));
                    if (!result.IsSuccess)
                    {
                        ShowMessage(result.Message, DialogMessageEnum.Error, result.ErrorMessage);
                        return;
                    }

                    createdCount++;
                }

                ShowMessage($"Se generaron {createdCount} issue details base.", DialogMessageEnum.Success);
                await LoaderManager.Run(_gridContainer, CargarDetalleSeleccionadoAsync, "Actualizando issues");
            }
            catch (Exception ex)
            {
                ShowMessage("Hubo un error al generar los issues base.", DialogMessageEnum.Error, ex.Message);
            }
        }

        private async void AutocompleteIssuesBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedKitting is null)
            {
                ShowMessage("Selecciona un embarque para autocompletar sus issues.", DialogMessageEnum.Warning);
                return;
            }

            if (!EnsureSelectedKittingEditable("autocompletar sus issue details"))
            {
                return;
            }

            if (_allIssues.Count == 0)
            {
                ShowMessage("El embarque no tiene issue details para autocompletar.", DialogMessageEnum.Warning);
                return;
            }

            var visibleIssues = _issuesBinding.DataSource as List<KittingIssueDetailDto> ?? new List<KittingIssueDetailDto>();
            var form = _dialogFormService.ShowDialog<FrmAutocompletarKittingIssue>(dialog =>
            {
                dialog.SetContext(_selectedDetail, _selectedIssue, visibleIssues.Count, _allIssues.Count);
            });

            if (!form.ResponseForm)
            {
                return;
            }

            var targetIssues = form.ApplyToAllShipment
                ? _allIssues
                : visibleIssues;

            if (targetIssues.Count == 0)
            {
                ShowMessage("No hay issue details en el alcance seleccionado.", DialogMessageEnum.Warning);
                return;
            }

            var updates = BuildAutocompleteUpdates(targetIssues, form);
            if (updates.Count == 0)
            {
                ShowMessage("No hubo cambios por aplicar con los valores capturados.", DialogMessageEnum.Info);
                return;
            }

            try
            {
                await LoaderManager.Run(_gridContainer, async () =>
                {
                    foreach (var update in updates)
                    {
                        var result = await _kittingIssueService.UpdateKittingIssue(update.KittingReceiptDetailId, update.Request);
                        if (!result.IsSuccess)
                        {
                            throw new InvalidOperationException(BuildApiErrorMessage(result));
                        }
                    }

                    await CargarDetalleSeleccionadoAsync();
                }, "Aplicando autocompletado");

                var skippedCount = targetIssues.Count - updates.Count;
                var message = skippedCount > 0
                    ? $"Se actualizaron {updates.Count} issue detail(s). Se omitieron {skippedCount} sin cambios."
                    : $"Se actualizaron {updates.Count} issue detail(s).";

                ShowMessage(message, DialogMessageEnum.Success);
            }
            catch (Exception ex)
            {
                await LoaderManager.Run(_gridContainer, CargarDetalleSeleccionadoAsync, "Actualizando issues");
                ShowMessage("Hubo un error al autocompletar los issue details.", DialogMessageEnum.Error, ex.Message);
            }
        }

        private async void LocateBtn_Click(object? sender, EventArgs e)
        {
            if (!EnsureSelectedKittingEditable("cambiarlo a Ubicando"))
            {
                return;
            }

            if (!EnsureStatusValidation("marcar como Ubicando"))
            {
                return;
            }

            await ChangeStatusAsync(
                () => _kittingService.LocateKitting(_selectedKitting!.KittingId),
                "Selecciona un embarque para cambiarlo a Ubicando.");
        }

        private async void ConfirmBtn_Click(object? sender, EventArgs e)
        {
            if (!EnsureSelectedKittingEditable("confirmarlo"))
            {
                return;
            }

            if (!EnsureStatusValidation("confirmar"))
            {
                return;
            }

            await ChangeStatusAsync(
                () => _kittingService.ConfirmKitting(_selectedKitting!.KittingId),
                "Selecciona un embarque para confirmarlo.");
        }

        private async void CancelBtn_Click(object? sender, EventArgs e)
        {
            if (!EnsureSelectedKittingEditable("cancelarlo"))
            {
                return;
            }

            await ChangeStatusAsync(
                () => _kittingService.CancelKitting(_selectedKitting!.KittingId),
                "Selecciona un embarque para cancelarlo.");
        }

        private async void EditBtn_Click(object? sender, EventArgs e)
        {
            if (_selectedKitting is null)
            {
                ShowMessage("Selecciona un embarque para editarlo.", DialogMessageEnum.Warning);
                return;
            }

            if (!EnsureSelectedKittingEditable("editarlo"))
            {
                return;
            }

            var form = _dialogFormService.ShowDialog<FrmNuevoKitting>(dialog =>
            {
                dialog.SetKitting(_selectedKitting);
            });

            if (form.ResponseForm)
            {
                await LoaderManager.Run(_gridContainer, CargarDatosAsync, "Trayendo embarques");
            }
        }

        private async void reloadBtn_Click(object? sender, EventArgs e)
        {
            await LoaderManager.Run(_gridContainer, CargarDatosAsync, "Trayendo embarques");
        }

        private async void dataGridView1_SelectionChanged(object? sender, EventArgs e)
        {
            if (_loadingMasterData || _loadingChildData)
            {
                return;
            }

            await CargarDetalleSeleccionadoAsync();
        }

        private void dataGridView1_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditBtn_Click(sender, EventArgs.Empty);
            }
        }

        private void detailsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (_loadingChildData)
            {
                return;
            }

            if (_detailsGrid.CurrentRow?.DataBoundItem is KittingDetailDto detail)
            {
                _selectedDetail = detail;
                BindIssuesForSelectedDetail();
                UpdateActionButtonsState();
                return;
            }

            _selectedDetail = null;
            BindIssuesForSelectedDetail();
            UpdateActionButtonsState();
        }

        private void issuesGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (_issuesGrid.CurrentRow?.DataBoundItem is KittingIssueDetailDto issue)
            {
                _selectedIssue = issue;
                UpdateActionButtonsState();
                return;
            }

            _selectedIssue = null;
            UpdateActionButtonsState();
        }

        private void detailsGrid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditDetailBtn_Click(sender, EventArgs.Empty);
            }
        }

        private void issuesGrid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                EditIssueBtn_Click(sender, EventArgs.Empty);
            }
        }

        private void ConfigureSummaryGrid()
        {
            ShowOnlyColumns(
                _kittingsGrid,
                nameof(KittingDto.KittingCode),
                nameof(KittingDto.Client),
                nameof(KittingDto.Project),
                nameof(KittingDto.InvoiceNumber),
                nameof(KittingDto.GuideNumber),
                nameof(KittingDto.Status),
                nameof(KittingDto.PackagesQty),
                nameof(KittingDto.TipoEntrega),
                nameof(KittingDto.FechaProgramada),
                nameof(KittingDto.Eta));
        }

        private void ConfigureDetailGrid()
        {
            ShowOnlyColumns(
                _detailsGrid,
                nameof(KittingDetailDto.PartNumber),
                nameof(KittingDetailDto.Description),
                nameof(KittingDetailDto.Quantity),
                nameof(KittingDetailDto.Status),
                nameof(KittingDetailDto.SD),
                nameof(KittingDetailDto.LotNumber),
                nameof(KittingDetailDto.ExpirationDate),
                nameof(KittingDetailDto.CustomerReference),
                nameof(KittingDetailDto.ExchangeRate),
                nameof(KittingDetailDto.PurchaseOrder),
                nameof(KittingDetailDto.CustomsDeclarationNumber),
                nameof(KittingDetailDto.StandardQuantity),
                nameof(KittingDetailDto.MaximumQuantity));

            ApplyDetailValidationState();
        }

        private void ConfigureIssueGrid()
        {
            ShowOnlyColumns(
                _issuesGrid,
                nameof(KittingIssueDetailDto.StandardId),
                nameof(KittingIssueDetailDto.PartNumber),
                nameof(KittingIssueDetailDto.Description),
                nameof(KittingIssueDetailDto.StandardQuantity),
                nameof(KittingIssueDetailDto.MaximumQuantity),
                nameof(KittingIssueDetailDto.SD),
                nameof(KittingIssueDetailDto.ReceivedQuantity),
                nameof(KittingIssueDetailDto.Status),
                nameof(KittingIssueDetailDto.LocationCode),
                nameof(KittingIssueDetailDto.LotNumber),
                nameof(KittingIssueDetailDto.ExpirationDate),
                nameof(KittingIssueDetailDto.Reference),
                nameof(KittingIssueDetailDto.PurchaseOrder),
                nameof(KittingIssueDetailDto.CustomsDeclarationNumber));

            ApplyIssueValidationState();
        }

        private static DataGridView CreateGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = true,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = SystemColors.ButtonHighlight,
                RowHeadersWidth = 51,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(253, 252, 213)
                }
            };
        }

        private static GroupBox CreateGridHost(string title, Control content)
        {
            var group = new GroupBox
            {
                Dock = DockStyle.Fill,
                Text = title,
                Padding = new Padding(8)
            };

            group.Controls.Add(content);
            return group;
        }

        private static Button CreateToolbarButton(string text)
        {
            return new Button
            {
                Text = text,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10, 4, 10, 4),
                Margin = new Padding(0, 0, 8, 0),
                UseVisualStyleBackColor = true
            };
        }

        private static void ShowOnlyColumns(DataGridView grid, params string[] visibleColumns)
        {
            var visibleSet = new HashSet<string>(visibleColumns, StringComparer.OrdinalIgnoreCase);

            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.Visible = visibleSet.Contains(column.Name) || visibleSet.Contains(column.DataPropertyName);
            }

            for (var index = 0; index < visibleColumns.Length; index++)
            {
                var column = grid.Columns
                    .Cast<DataGridViewColumn>()
                    .FirstOrDefault(x =>
                        string.Equals(x.Name, visibleColumns[index], StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(x.DataPropertyName, visibleColumns[index], StringComparison.OrdinalIgnoreCase));

                if (column is not null)
                {
                    column.DisplayIndex = index;
                }
            }
        }

        private void ShowMessage(string message, DialogMessageEnum type, string? details = null)
        {
            var finalMessage = string.IsNullOrWhiteSpace(details)
                ? message
                : $"{message}{Environment.NewLine}{Environment.NewLine}{details}";

            _dialogMessageService.Show(finalMessage, type);
        }

        private async Task ChangeStatusAsync(Func<Task<ApiResponseDto<string>>> action, string emptySelectionMessage)
        {
            if (_selectedKitting is null)
            {
                ShowMessage(emptySelectionMessage, DialogMessageEnum.Warning);
                return;
            }

            try
            {
                var result = await action();
                if (!result.IsSuccess)
                {
                    ShowMessage(result.Message, DialogMessageEnum.Error, result.ErrorMessage);
                    return;
                }

                ShowMessage(result.Message, DialogMessageEnum.Success);
                await LoaderManager.Run(_gridContainer, CargarDatosAsync, "Actualizando embarques");
            }
            catch (Exception ex)
            {
                ShowMessage("Hubo un error al actualizar el estatus del embarque.", DialogMessageEnum.Error, ex.Message);
            }
        }

        private void BindIssuesForSelectedDetail()
        {
            var issues = _selectedDetail is null
                ? new List<KittingIssueDetailDto>()
                : _allIssues
                    .Where(x => x.KittingDetailId == _selectedDetail.KittingDetailId)
                    .OrderBy(x => x.KittingReceiptDetailId)
                    .ToList();

            _issuesBinding.DataSource = issues;
            ConfigureIssueGrid();
            SelectFirstRow(_issuesGrid);
            _selectedIssue = _issuesGrid.CurrentRow?.DataBoundItem as KittingIssueDetailDto
                ?? issues.FirstOrDefault();
            UpdateValidationPresentation();
            UpdateActionButtonsState();
        }

        private void UpdateActionButtonsState()
        {
            var hasSelectedKitting = _selectedKitting is not null;
            var canEditSelectedKitting = hasSelectedKitting && !IsTerminalStatus(_selectedKitting?.Status);
            var hasSelectedDetail = _selectedDetail is not null;
            var hasSelectedIssue = _selectedIssue is not null;
            var hasDetails = (_detailsBinding.DataSource as List<KittingDetailDto>)?.Count > 0;

            _editBtn.Enabled = canEditSelectedKitting;
            _addDetailBtn.Enabled = canEditSelectedKitting;
            _editDetailBtn.Enabled = canEditSelectedKitting && hasSelectedDetail;
            _deleteDetailBtn.Enabled = canEditSelectedKitting && hasSelectedDetail;
            _addIssueBtn.Enabled = canEditSelectedKitting && hasSelectedDetail;
            _editIssueBtn.Enabled = canEditSelectedKitting && hasSelectedIssue;
            _deleteIssueBtn.Enabled = canEditSelectedKitting && hasSelectedIssue;
            _generateBaseIssuesBtn.Enabled = canEditSelectedKitting && hasDetails;
            _autocompleteIssuesBtn.Enabled = canEditSelectedKitting && _allIssues.Count > 0;
            _locateBtn.Enabled = canEditSelectedKitting;
            _confirmBtn.Enabled = canEditSelectedKitting;
            _cancelBtn.Enabled = canEditSelectedKitting;
        }

        private bool EnsureSelectedKittingEditable(string actionLabel)
        {
            if (_selectedKitting is null)
            {
                return false;
            }

            if (!IsTerminalStatus(_selectedKitting.Status))
            {
                return true;
            }

            ShowMessage(
                $"El embarque {_selectedKitting.KittingCode} ya esta {_selectedKitting.Status} y no permite {actionLabel}.",
                DialogMessageEnum.Warning);

            return false;
        }

        private bool EnsureStatusValidation(string actionLabel)
        {
            var detailMessages = BuildDetailValidationMessages();
            var issueMessages = BuildIssueValidationMessages();

            if (detailMessages.Count == 0 && issueMessages.Count == 0)
            {
                return true;
            }

            UpdateValidationPresentation(detailMessages, issueMessages);

            var blockingRows = detailMessages
                .Values
                .Concat(issueMessages.Values)
                .Distinct()
                .Take(6)
                .ToList();

            if (detailMessages.Count + issueMessages.Count > blockingRows.Count)
            {
                blockingRows.Add($"y {detailMessages.Count + issueMessages.Count - blockingRows.Count} pendiente(s) mas.");
            }

            var details = string.Join(Environment.NewLine, blockingRows.Select(x => $"- {x}"));
            ShowMessage(
                $"Antes de {actionLabel} el embarque, corrige las lineas marcadas en rojo o naranja.",
                DialogMessageEnum.Warning,
                details);

            return false;
        }

        private void UpdateValidationPresentation()
        {
            UpdateValidationPresentation(BuildDetailValidationMessages(), BuildIssueValidationMessages());
        }

        private void UpdateValidationPresentation(
            Dictionary<int, string> detailMessages,
            Dictionary<int, string> issueMessages)
        {
            ApplyDetailValidationState(detailMessages);
            ApplyIssueValidationState(issueMessages);
            UpdateValidationTitles(detailMessages, issueMessages);
        }

        private Dictionary<int, string> BuildDetailValidationMessages()
        {
            var detailMessages = new Dictionary<int, string>();
            var details = _detailsBinding.DataSource as List<KittingDetailDto> ?? new List<KittingDetailDto>();
            var issuesByDetail = _allIssues
                .GroupBy(x => x.KittingDetailId)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var detail in details)
            {
                if (!issuesByDetail.TryGetValue(detail.KittingDetailId, out var detailIssues) || detailIssues.Count == 0)
                {
                    detailMessages[detail.KittingDetailId] = $"{detail.PartNumber}: no tiene issue detail. Usa Generar Issues Base.";
                    continue;
                }

                var invalidCount = detailIssues.Count(x => HasIssueValidationErrors(x));
                if (invalidCount > 0)
                {
                    detailMessages[detail.KittingDetailId] = $"{detail.PartNumber}: tiene {invalidCount} issue(s) sin ubicacion, status o SD.";
                }
            }

            return detailMessages;
        }

        private Dictionary<int, string> BuildIssueValidationMessages()
        {
            var issueMessages = new Dictionary<int, string>();

            foreach (var issue in _allIssues)
            {
                var missingFields = GetMissingIssueFields(issue);
                if (missingFields.Count == 0)
                {
                    continue;
                }

                var issueLabel = string.IsNullOrWhiteSpace(issue.PartNumber)
                    ? $"Issue {issue.KittingReceiptDetailId}"
                    : issue.PartNumber;

                issueMessages[issue.KittingReceiptDetailId] =
                    $"{issueLabel}: falta {string.Join(", ", missingFields)}.";
            }

            return issueMessages;
        }

        private void ApplyDetailValidationState()
        {
            ApplyDetailValidationState(BuildDetailValidationMessages());
        }

        private void ApplyDetailValidationState(Dictionary<int, string> detailMessages)
        {
            foreach (DataGridViewRow row in _detailsGrid.Rows)
            {
                ResetRowStyle(row);

                if (row.DataBoundItem is not KittingDetailDto detail)
                {
                    continue;
                }

                if (!detailMessages.TryGetValue(detail.KittingDetailId, out var message))
                {
                    continue;
                }

                var hasIssues = _allIssues.Any(x => x.KittingDetailId == detail.KittingDetailId);
                ApplyValidationRowStyle(row, hasIssues ? Color.MistyRose : Color.LemonChiffon, message);
            }
        }

        private void ApplyIssueValidationState()
        {
            ApplyIssueValidationState(BuildIssueValidationMessages());
        }

        private void ApplyIssueValidationState(Dictionary<int, string> issueMessages)
        {
            foreach (DataGridViewRow row in _issuesGrid.Rows)
            {
                ResetRowStyle(row);

                if (row.DataBoundItem is not KittingIssueDetailDto issue)
                {
                    continue;
                }

                if (!issueMessages.TryGetValue(issue.KittingReceiptDetailId, out var message))
                {
                    ApplyIssueStatusRowStyle(row, issue);
                    continue;
                }

                ApplyValidationRowStyle(row, Color.MistyRose, message);
            }
        }

        private void ApplyIssueStatusRowStyle(DataGridViewRow row, KittingIssueDetailDto issue)
        {
            var style = GetIssueStatusStyle(issue.Status);
            if (!style.HasValue)
            {
                return;
            }

            row.DefaultCellStyle.BackColor = style.Value.BackColor;
            row.DefaultCellStyle.ForeColor = style.Value.ForeColor;
            row.DefaultCellStyle.SelectionBackColor = style.Value.BackColor;
            row.DefaultCellStyle.SelectionForeColor = style.Value.ForeColor;
        }

        private void UpdateValidationTitles(
            Dictionary<int, string> detailMessages,
            Dictionary<int, string> issueMessages)
        {
            _detailsGroup.Text = detailMessages.Count == 0
                ? "Detalle de kitting"
                : $"Detalle de kitting ({detailMessages.Count} pendiente(s))";

            var filteredIssues = _selectedDetail is null
                ? new List<KittingIssueDetailDto>()
                : _allIssues.Where(x => x.KittingDetailId == _selectedDetail.KittingDetailId).ToList();

            var selectedInvalidCount = filteredIssues.Count(x => issueMessages.ContainsKey(x.KittingReceiptDetailId));
            _issuesGroup.Text = selectedInvalidCount == 0
                ? "Issue details"
                : $"Issue details ({selectedInvalidCount} pendiente(s))";
        }

        private static void ApplyValidationRowStyle(DataGridViewRow row, Color backColor, string message)
        {
            row.DefaultCellStyle.BackColor = backColor;
            row.DefaultCellStyle.SelectionBackColor = backColor;
            row.DefaultCellStyle.SelectionForeColor = Color.Black;
            row.ErrorText = message;
        }

        private static void ResetRowStyle(DataGridViewRow row)
        {
            row.DefaultCellStyle.BackColor = row.Index % 2 == 0
                ? Color.White
                : Color.FromArgb(253, 252, 213);
            row.DefaultCellStyle.ForeColor = Color.Black;
            row.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
            row.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
            row.ErrorText = string.Empty;
        }

        private (Color BackColor, Color ForeColor)? GetIssueStatusStyle(string? statusValue)
        {
            if (string.IsNullOrWhiteSpace(statusValue))
            {
                return null;
            }

            TryResolveIssueStatus(statusValue, out var resolvedStatus);

            var normalizedText = NormalizeStatusText(
                statusValue,
                resolvedStatus?.StatusId,
                resolvedStatus?.Descripcion);

            if (ContainsStatusKeyword(normalizedText, "cancelado", "cancelled"))
            {
                return (Color.Gainsboro, Color.Black);
            }

            if (ContainsStatusKeyword(normalizedText, "confirmado", "confirmed"))
            {
                return (Color.Honeydew, Color.Black);
            }

            if (ContainsStatusKeyword(normalizedText, "ubicando", "locating", "picking", "surtiendo"))
            {
                return (Color.AliceBlue, Color.Black);
            }

            if (ContainsStatusKeyword(normalizedText, "creado", "created", "pendiente", "pending"))
            {
                return (Color.LightGoldenrodYellow, Color.Black);
            }

            if (ContainsStatusKeyword(normalizedText, "cuarentena", "quarantine", "hold", "retenido", "bloqueado", "blocked"))
            {
                return (Color.Moccasin, Color.Black);
            }

            if (ContainsStatusKeyword(normalizedText, "danado", "dano", "damaged", "damage", "reject", "rechazado"))
            {
                return (Color.LavenderBlush, Color.Black);
            }

            if (ContainsStatusKeyword(normalizedText, "caducado", "expired", "expirado"))
            {
                return (Color.Linen, Color.Black);
            }

            if (resolvedStatus?.Disponible == true)
            {
                return (Color.Honeydew, Color.Black);
            }

            return resolvedStatus is null
                ? null
                : (Color.WhiteSmoke, Color.Black);
        }

        private bool TryResolveIssueStatus(string? statusValue, out InventaryStatusDto? status)
        {
            status = null;
            if (string.IsNullOrWhiteSpace(statusValue))
            {
                return false;
            }

            var trimmedValue = statusValue.Trim();
            if (_issueStatusesByKey.TryGetValue(trimmedValue, out var directMatch))
            {
                status = directMatch;
                return true;
            }

            var separatorIndex = trimmedValue.IndexOf(" - ", StringComparison.Ordinal);
            if (separatorIndex > 0)
            {
                var statusId = trimmedValue[..separatorIndex].Trim();
                if (_issueStatusesByKey.TryGetValue(statusId, out var statusIdMatch))
                {
                    status = statusIdMatch;
                    return true;
                }

                var description = trimmedValue[(separatorIndex + 3)..].Trim();
                if (_issueStatusesByKey.TryGetValue(description, out var descriptionMatch))
                {
                    status = descriptionMatch;
                    return true;
                }
            }

            return false;
        }

        private static string NormalizeStatusText(params string?[] values)
        {
            var normalizedValues = values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => RemoveDiacritics(x!.Trim()).ToLowerInvariant())
                .Distinct();

            return string.Join(" ", normalizedValues);
        }

        private static bool ContainsStatusKeyword(string normalizedText, params string[] keywords)
        {
            return keywords.Any(keyword => normalizedText.Contains(keyword, StringComparison.Ordinal));
        }

        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var builder = new System.Text.StringBuilder(normalized.Length);

            foreach (var character in normalized)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(character) !=
                    System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(character);
                }
            }

            return builder.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        private static List<string> GetMissingIssueFields(KittingIssueDetailDto issue)
        {
            var missingFields = new List<string>();

            if (issue.LocationId is null && string.IsNullOrWhiteSpace(issue.LocationCode))
            {
                missingFields.Add("ubicacion");
            }

            if (string.IsNullOrWhiteSpace(issue.Status))
            {
                missingFields.Add("status");
            }

            if (string.IsNullOrWhiteSpace(issue.SD))
            {
                missingFields.Add("SD");
            }

            return missingFields;
        }

        private static bool HasIssueValidationErrors(KittingIssueDetailDto issue)
        {
            return GetMissingIssueFields(issue).Count > 0;
        }

        private static void SelectFirstRow(DataGridView grid)
        {
            if (grid.Rows.Count == 0)
            {
                return;
            }

            var firstVisibleCell = grid.Rows[0].Cells
                .Cast<DataGridViewCell>()
                .FirstOrDefault(x => x.Visible);

            if (firstVisibleCell is null)
            {
                return;
            }

            grid.CurrentCell = firstVisibleCell;
            grid.Rows[0].Selected = true;
        }

        private static bool IsTerminalStatus(string? status) =>
            string.Equals(status?.Trim(), "Confirmado", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(status?.Trim(), "Cancelado", StringComparison.OrdinalIgnoreCase);

        private List<KittingIssueAutocompleteUpdate> BuildAutocompleteUpdates(
            List<KittingIssueDetailDto> issues,
            FrmAutocompletarKittingIssue form)
        {
            var updates = new List<KittingIssueAutocompleteUpdate>();

            foreach (var issue in issues)
            {
                var request = BuildIssueRequest(issue);
                if (!ApplyAutocompleteValues(issue, request, form))
                {
                    continue;
                }

                updates.Add(new KittingIssueAutocompleteUpdate(issue.KittingReceiptDetailId, request));
            }

            return updates;
        }

        private static KittingIssueRequest BuildIssueRequest(KittingIssueDetailDto issue)
        {
            return new KittingIssueRequest
            {
                KittingReceiptDetailId = issue.KittingReceiptDetailId,
                KittingDetailId = issue.KittingDetailId,
                ProductId = issue.ProductId,
                StandardId = issue.StandardId,
                PartNumber = issue.PartNumber,
                Description = issue.Description,
                StandardQuantity = issue.StandardQuantity,
                MaximumQuantity = issue.MaximumQuantity,
                SD = issue.SD,
                ReceivedQuantity = issue.ReceivedQuantity,
                Status = issue.Status,
                LocationId = issue.LocationId,
                LocationCode = issue.LocationCode,
                LotNumber = issue.LotNumber,
                ExpirationDate = issue.ExpirationDate,
                Reference = issue.Reference,
                PurchaseOrder = issue.PurchaseOrder,
                CustomsDeclarationNumber = issue.CustomsDeclarationNumber
            };
        }

        private static bool ApplyAutocompleteValues(
            KittingIssueDetailDto issue,
            KittingIssueRequest request,
            FrmAutocompletarKittingIssue form)
        {
            var changed = false;

            if (form.LocationId.HasValue || !string.IsNullOrWhiteSpace(form.LocationCode))
            {
                var shouldApplyLocation = !form.OnlyFillEmpty || IssueHasNoLocation(issue);
                if (shouldApplyLocation &&
                    (issue.LocationId != form.LocationId ||
                     !string.Equals(issue.LocationCode, form.LocationCode, StringComparison.OrdinalIgnoreCase)))
                {
                    request.LocationId = form.LocationId;
                    request.LocationCode = form.LocationCode;
                    changed = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(form.StatusValue))
            {
                var shouldApplyStatus = !form.OnlyFillEmpty || string.IsNullOrWhiteSpace(issue.Status);
                if (shouldApplyStatus &&
                    !string.Equals(issue.Status, form.StatusValue, StringComparison.OrdinalIgnoreCase))
                {
                    request.Status = form.StatusValue;
                    changed = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(form.SdValue))
            {
                var shouldApplySd = !form.OnlyFillEmpty || string.IsNullOrWhiteSpace(issue.SD);
                if (shouldApplySd &&
                    !string.Equals(issue.SD, form.SdValue, StringComparison.OrdinalIgnoreCase))
                {
                    request.SD = form.SdValue;
                    changed = true;
                }
            }

            return changed;
        }

        private static bool IssueHasNoLocation(KittingIssueDetailDto issue)
        {
            return issue.LocationId is null && string.IsNullOrWhiteSpace(issue.LocationCode);
        }

        private static string BuildApiErrorMessage(ApiResponseDto<string> response)
        {
            return string.IsNullOrWhiteSpace(response.ErrorMessage)
                ? response.Message
                : $"{response.Message}{Environment.NewLine}{Environment.NewLine}{response.ErrorMessage}";
        }

        private static KittingIssueRequest BuildBaseIssueRequest(KittingDetailDto detail)
        {
            return new KittingIssueRequest
            {
                KittingDetailId = detail.KittingDetailId,
                ProductId = detail.ProductId,
                PartNumber = detail.PartNumber,
                Description = detail.Description,
                StandardQuantity = detail.StandardQuantity,
                MaximumQuantity = detail.MaximumQuantity,
                SD = detail.SD,
                ReceivedQuantity = detail.Quantity,
                Status = detail.Status,
                LotNumber = detail.LotNumber,
                ExpirationDate = detail.ExpirationDate,
                Reference = detail.CustomerReference,
                PurchaseOrder = detail.PurchaseOrder,
                CustomsDeclarationNumber = detail.CustomsDeclarationNumber
            };
        }

        private sealed record KittingIssueAutocompleteUpdate(int KittingReceiptDetailId, KittingIssueRequest Request);
    }
}
