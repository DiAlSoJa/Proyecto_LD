using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Kitting;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LD.FormsX.Features.Embarques.Views
{
    public partial class ValidarEmbarqueDialog : Window, INotifyPropertyChanged
    {
        private const string CorrectSoundFileName = "correcto.mp3";
        private const string ErrorSoundFileName = "error.mp3";
        private static readonly Brush SuccessMessageBackground = new SolidColorBrush(Color.FromRgb(240, 253, 244));
        private static readonly Brush SuccessMessageForeground = new SolidColorBrush(Color.FromRgb(22, 101, 52));
        private static readonly Brush SuccessMessageBorder = new SolidColorBrush(Color.FromRgb(34, 197, 94));
        private static readonly Brush ErrorMessageBackground = new SolidColorBrush(Color.FromRgb(254, 242, 242));
        private static readonly Brush ErrorMessageForeground = new SolidColorBrush(Color.FromRgb(153, 27, 27));
        private static readonly Brush ErrorMessageBorder = new SolidColorBrush(Color.FromRgb(239, 68, 68));

        private readonly KittingDetailService _kittingDetailService;
        private readonly KittingIssueService _kittingIssueService;
        private readonly List<MediaPlayer> _activePlayers = [];

        private KittingDto? _kitting;
        private string _statusMessage = "Escanea una etiqueta para validar.";
        private string _kittingSummary = string.Empty;

        public ObservableCollection<KittingIssueDetailDto> SurtidoItems { get; } = new();
        public ObservableCollection<KittingIssueDetailDto> ValidadoItems { get; } = new();
        public bool HasChanges { get; private set; }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage == value)
                    return;

                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public string KittingSummary
        {
            get => _kittingSummary;
            set
            {
                if (_kittingSummary == value)
                    return;

                _kittingSummary = value;
                OnPropertyChanged(nameof(KittingSummary));
            }
        }

        public string SurtidoCountText => $"{SurtidoItems.Count} registro(s)";

        public string ValidadoCountText => $"{ValidadoItems.Count} registro(s)";

        public event PropertyChangedEventHandler? PropertyChanged;

        public ValidarEmbarqueDialog(
            KittingDetailService kittingDetailService,
            KittingIssueService kittingIssueService)
        {
            InitializeComponent();
            DataContext = this;

            _kittingDetailService = kittingDetailService;
            _kittingIssueService = kittingIssueService;
        }

        public void SetKitting(KittingDto kitting)
        {
            _kitting = kitting;
            KittingSummary = $"{kitting.KittingCode} | Cliente: {kitting.Client} | Proyecto: {kitting.Project}";
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtEscaneo.Focus();
            await LoadIssueListsAsync();
        }

        private async Task LoadIssueListsAsync()
        {
            try
            {
                SurtidoItems.Clear();
                ValidadoItems.Clear();
                OnPropertyChanged(nameof(SurtidoCountText));
                OnPropertyChanged(nameof(ValidadoCountText));

                if (_kitting == null || _kitting.KittingId <= 0)
                {
                    SetErrorMessage("No se encontro un embarque valido para cargar.");
                    return;
                }

                var detailsResponse = await _kittingDetailService.GetKittingDetailsByKittingId(_kitting.KittingId);
                if (!detailsResponse.IsSuccess || detailsResponse.Data == null)
                {
                    SetErrorMessage(detailsResponse.Message ?? "No se pudieron cargar los detalles del embarque.");
                    return;
                }

                var detailIds = detailsResponse.Data
                    .Where(x => x.KittingDetailId > 0)
                    .Select(x => x.KittingDetailId)
                    .ToList();

                if (detailIds.Count == 0)
                {
                    SetErrorMessage("El embarque no tiene lineas para validar.");
                    return;
                }

                var issueTasks = detailIds.Select(async detailId =>
                {
                    var issuesResponse = await _kittingIssueService.GetKittingIssuesByKittingDetailId(detailId);
                    return issuesResponse.IsSuccess && issuesResponse.Data != null
                        ? issuesResponse.Data
                        : new List<KittingIssueDetailDto>();
                });

                var issues = (await Task.WhenAll(issueTasks))
                    .SelectMany(x => x)
                    .ToList();

                foreach (var issue in issues
                    .Where(x => IsSupplyStatus(x.SupplyStatus, KittingStatusNames.Validacion))
                    .OrderBy(x => x.StandardIdStr ?? string.Empty)
                    .ThenBy(x => x.PartNumber ?? string.Empty)
                    .ThenBy(x => x.ReceivedQuantity ?? 0m))
                {
                    SurtidoItems.Add(issue);
                }

                foreach (var issue in issues
                    .Where(x => IsSupplyStatus(x.SupplyStatus, KittingStatusNames.Cargando))
                    .OrderBy(x => x.StandardIdStr ?? string.Empty)
                    .ThenBy(x => x.PartNumber ?? string.Empty)
                    .ThenBy(x => x.ReceivedQuantity ?? 0m))
                {
                    ValidadoItems.Add(issue);
                }

                OnPropertyChanged(nameof(SurtidoCountText));
                OnPropertyChanged(nameof(ValidadoCountText));
                SetSuccessMessage($"Listo. Validación: {SurtidoItems.Count} | Cargando: {ValidadoItems.Count}.");
            }
            catch (Exception ex)
            {
                SetErrorMessage("Ocurrio un error al cargar los issues del embarque.");
                DialogHelper.ShowError(ex.Message);
            }
        }

        private async void TxtEscaneo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;

            var scanValue = txtEscaneo.Text.Trim();
            if (string.IsNullOrWhiteSpace(scanValue))
                return;

            await ProcessScanAsync(scanValue);
            txtEscaneo.Clear();
            txtEscaneo.Focus();
        }

        private async Task ProcessScanAsync(string scanValue)
        {
            var match = FindMatchingIssue(scanValue);
            if (match == null)
            {
                SetErrorMessage($"No se encontro una linea de Validación con ese StandardIdStr: {scanValue}");
                PlayErrorSound();
                return;
            }

            if (IsSupplyStatus(match.SupplyStatus, KittingStatusNames.Cargando))
            {
                SetErrorMessage($"La linea {match.PartNumber} ya esta en Cargando.");
                PlayErrorSound();
                return;
            }

            if (!IsSupplyStatus(match.SupplyStatus, KittingStatusNames.Validacion))
            {
                SetErrorMessage($"La linea {match.StandardIdStr} no esta en estatus Validación.");
                PlayErrorSound();
                return;
            }

            var request = new KittingIssueValidateRequest
            {
                StandardIdStr = scanValue.Trim()
            };

            var response = await _kittingIssueService.ValidateKittingIssue(match.KittingReceiptDetailId, request);
            if (!response.IsSuccess)
            {
                SetErrorMessage(response.ErrorMessage ?? response.Message ?? "No se pudo validar la linea.");
                PlayErrorSound();
                return;
            }

            await RefreshIssueRowFromServerAsync(match);
            MoveToValidated(match);
            HasChanges = true;

            SetSuccessMessage(response.Message ?? $"Cargando: {match.StandardIdStr} | Cantidad: {match.ReceivedQuantity}");
            PlayCorrectSound();
            txtEscaneo.SelectAll();
        }

        private KittingIssueDetailDto? FindMatchingIssue(string scanValue)
        {
            return SurtidoItems.FirstOrDefault(issue =>
                MatchesScan(issue.StandardIdStr, scanValue));
        }

        private static bool MatchesScan(string? source, string scanValue) =>
            !string.IsNullOrWhiteSpace(source)
            && string.Equals(source.Trim(), scanValue.Trim(), StringComparison.OrdinalIgnoreCase);

        private async Task RefreshIssueRowFromServerAsync(KittingIssueDetailDto issueRow)
        {
            if (issueRow.KittingReceiptDetailId <= 0)
                return;

            var response = await _kittingIssueService.GetKittingIssueById(issueRow.KittingReceiptDetailId);
            if (!response.IsSuccess || response.Data == null)
                return;

            ApplyIssueRequest(issueRow, response.Data);
        }

        private static void ApplyIssueRequest(KittingIssueDetailDto issueRow, KittingIssueRequest request)
        {
            issueRow.KittingReceiptDetailId = request.KittingReceiptDetailId;
            issueRow.KittingDetailId = request.KittingDetailId;
            issueRow.ProductId = request.ProductId;
            issueRow.StandardId = request.StandardId ?? string.Empty;
            issueRow.StandardIdStr = request.StandardId ?? string.Empty;
            issueRow.PartNumber = request.PartNumber ?? string.Empty;
            issueRow.Description = request.Description ?? string.Empty;
            issueRow.StandardQuantity = request.StandardQuantity;
            issueRow.MaximumQuantity = request.MaximumQuantity;
            issueRow.SD = request.SD ?? string.Empty;
            issueRow.ReceivedQuantity = request.ReceivedQuantity;
            issueRow.Status = request.Status ?? string.Empty;
            issueRow.SupplyStatus = request.SupplyStatus ?? string.Empty;
            issueRow.LocationId = request.LocationId;
            issueRow.LocationCode = request.LocationCode ?? string.Empty;
            issueRow.LotNumber = request.LotNumber ?? string.Empty;
            issueRow.ExpirationDate = request.ExpirationDate;
            issueRow.Reference = request.Reference ?? string.Empty;
            issueRow.PurchaseOrder = request.PurchaseOrder ?? string.Empty;
            issueRow.CustomsDeclarationNumber = request.CustomsDeclarationNumber ?? string.Empty;
        }

        private void MoveToValidated(KittingIssueDetailDto issueRow)
        {
            var itemToMove = SurtidoItems.FirstOrDefault(x => x.KittingReceiptDetailId == issueRow.KittingReceiptDetailId);
            if (itemToMove == null)
                return;

            SurtidoItems.Remove(itemToMove);
            if (ValidadoItems.All(x => x.KittingReceiptDetailId != itemToMove.KittingReceiptDetailId))
            {
                itemToMove.SupplyStatus = KittingStatusNames.Cargando;
                ValidadoItems.Add(itemToMove);
            }

            OnPropertyChanged(nameof(SurtidoCountText));
            OnPropertyChanged(nameof(ValidadoCountText));
        }

        private static bool IsSupplyStatus(string? value, string expected) =>
            string.Equals(value?.Trim(), expected, StringComparison.OrdinalIgnoreCase) ||
            (string.Equals(expected, KittingStatusNames.Validacion, StringComparison.OrdinalIgnoreCase) &&
             string.Equals(value?.Trim(), KittingStatusNames.LegacyValidacion, StringComparison.OrdinalIgnoreCase)) ||
            (string.Equals(expected, KittingStatusNames.Cargando, StringComparison.OrdinalIgnoreCase) &&
             string.Equals(value?.Trim(), KittingStatusNames.LegacyCargando, StringComparison.OrdinalIgnoreCase));

        private void SetSuccessMessage(string message)
        {
            StatusMessage = message;
            txtUltimoMensaje.Text = message;
            txtUltimoMensaje.Background = SuccessMessageBackground;
            txtUltimoMensaje.Foreground = SuccessMessageForeground;
            txtUltimoMensaje.BorderBrush = SuccessMessageBorder;
        }

        private void SetErrorMessage(string message)
        {
            StatusMessage = message;
            txtUltimoMensaje.Text = message;
            txtUltimoMensaje.Background = ErrorMessageBackground;
            txtUltimoMensaje.Foreground = ErrorMessageForeground;
            txtUltimoMensaje.BorderBrush = ErrorMessageBorder;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void PlayCorrectSound() => PlaySound(CorrectSoundFileName);

        private void PlayErrorSound() => PlaySound(ErrorSoundFileName);

        private void PlaySound(string fileName)
        {
            try
            {
                var soundPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Raw", fileName);
                if (!File.Exists(soundPath))
                    return;

                var player = new MediaPlayer();
                player.MediaEnded += OnMediaPlayerFinished;
                player.MediaFailed += OnMediaPlayerFailed;
                _activePlayers.Add(player);
                player.Open(new Uri(soundPath, UriKind.Absolute));
                player.Play();
            }
            catch
            {
            }
        }

        private void OnMediaPlayerFinished(object? sender, EventArgs e)
        {
            DisposeMediaPlayer(sender as MediaPlayer);
        }

        private void OnMediaPlayerFailed(object? sender, ExceptionEventArgs e)
        {
            DisposeMediaPlayer(sender as MediaPlayer);
        }

        private void DisposeMediaPlayer(MediaPlayer? player)
        {
            if (player == null)
                return;

            player.MediaEnded -= OnMediaPlayerFinished;
            player.MediaFailed -= OnMediaPlayerFailed;
            player.Close();
            _activePlayers.Remove(player);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
