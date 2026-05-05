using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LD.Contracts.Enums;
using LD.Contracts.Requests;

namespace LD.FormsX.Views.Dialogs
{
    public partial class NuevoASNEscaneoView : Window
    {
        public ObservableCollection<ScanRuleDisplay> ScanRules { get; } = new();

        public event Func<IReadOnlyList<ScanRuleResult>, Task<bool>>? ScanCompleted;
        public event Func<string, Task<UnmatchedScanResult?>>? UnmatchedScanReceived;

        private List<ScanConfigurationRequest> _scanConfigurations = [];

        public NuevoASNEscaneoView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void SetScanConfigurations(IEnumerable<ScanConfigurationRequest>? configurations)
        {
            ScanRules.Clear();
            _scanConfigurations = (configurations ?? Enumerable.Empty<ScanConfigurationRequest>()).ToList();

            foreach (var config in _scanConfigurations
                .Where(config => HasAnyScanOrSaveCondition(config) || IsPartNumberOrStandardIdConfiguration(config))
                .OrderBy(c => c.Order)
                .ThenBy(c => c.SystemFieldId))
            {
                ScanRules.Add(new ScanRuleDisplay(config));
            }

            if (!ScanRules.Any())
                AddMessage("Este proyecto no tiene condiciones de escaneo o guardado configuradas.");
        }

        public void AddMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            txtMensajes.AppendText($"[{timestamp}] {message}{Environment.NewLine}");
            txtMensajes.ScrollToEnd();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtEscaneo.Focus();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
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
            AddMessage($"Escaneo recibido: {scanValue}");

            var match = ScanRules.FirstOrDefault(rule =>
                string.IsNullOrWhiteSpace(rule.CapturedValue)
                && HasScanCondition(rule.Configuration)
                && RuleMatches(rule.Configuration, scanValue));

            if (match == null)
            {
                if (HasPartNumberOrStandardIdConfiguration() && await TryApplyUnmatchedScanAsync(scanValue))
                    return;

                AddMessage("No coincide con ninguna condición configurada.");
                return;
            }

            var savedValue = ApplySaveConfiguration(match.Configuration, scanValue);
            match.CapturedValue = savedValue;

            AddMessage($"{match.FieldName}: {savedValue}");

            if (ScanRules.All(rule => !string.IsNullOrWhiteSpace(rule.CapturedValue)))
                await CompleteCurrentScanAsync();
        }

        private async Task<bool> TryApplyUnmatchedScanAsync(string scanValue)
        {
            if (UnmatchedScanReceived == null)
                return false;

            var result = await UnmatchedScanReceived.Invoke(scanValue);
            if (result == null)
                return false;

            if (!result.Success)
            {
                AddMessage(result.Message);
                return true;
            }

            var targetRule = ScanRules.FirstOrDefault(rule =>
                string.IsNullOrWhiteSpace(rule.CapturedValue)
                && IsSystemFieldConfiguration(rule.Configuration, result.SystemFieldId));

            if (targetRule == null)
            {
                AddMessage(string.IsNullOrWhiteSpace(result.Message)
                    ? "El valor existe, pero el campo destino no esta en las condiciones configuradas."
                    : result.Message);
                return true;
            }

            targetRule.CapturedValue = result.Value;
            AddMessage($"{targetRule.FieldName}: {result.Value}");

            if (ScanRules.All(rule => !string.IsNullOrWhiteSpace(rule.CapturedValue)))
                await CompleteCurrentScanAsync();

            return true;
        }

        private async Task CompleteCurrentScanAsync()
        {
            var results = ScanRules
                .Select(rule => new ScanRuleResult(rule.Configuration, rule.CapturedValue))
                .ToList();

            var created = ScanCompleted != null && await ScanCompleted.Invoke(results);

            if (!created)
            {
                AddMessage("Escaneo completo, pero no se pudo generar la línea de recepción.");
                return;
            }

            AddMessage("Escaneo completo. Se generó una línea de recepción.");

            foreach (var rule in ScanRules)
                rule.CapturedValue = string.Empty;
        }

        private static bool RuleMatches(ScanConfigurationRequest config, string scanValue)
        {
            var scanTypeId = config.ScanTypeId.GetValueOrDefault(1);
            var expected = config.ScanValue?.Trim() ?? string.Empty;

            return scanTypeId switch
            {
                1 => true,
                2 => !string.IsNullOrEmpty(expected)
                    && scanValue.StartsWith(expected, StringComparison.OrdinalIgnoreCase),
                3 => int.TryParse(expected, NumberStyles.Integer, CultureInfo.InvariantCulture, out var length)
                    && scanValue.Length == length,
                4 => decimal.TryParse(scanValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var scannedNumber)
                    && decimal.TryParse(expected, NumberStyles.Number, CultureInfo.InvariantCulture, out var expectedNumber)
                    && scannedNumber < expectedNumber,
                _ => true
            };
        }

        private static bool HasAnyScanOrSaveCondition(ScanConfigurationRequest config)
        {
            var hasSaveCondition = config.SaveTypeId.GetValueOrDefault(1) != 1
                && config.SaveValue > 0;

            return HasScanCondition(config) || hasSaveCondition;
        }

        private static bool HasScanCondition(ScanConfigurationRequest config)
        {
            if (IsPartNumberOrStandardIdConfiguration(config))
                return false;

            return config.ScanTypeId.GetValueOrDefault(1) != 1
                && !string.IsNullOrWhiteSpace(config.ScanValue);
        }

        private bool HasPartNumberOrStandardIdConfiguration()
        {
            return _scanConfigurations.Any(config =>
                config.SystemFieldId == (int)SystemField_e.StandardId
                || config.SystemFieldId == (int)SystemField_e.PartNumber
                || string.Equals(config.SystemFieldName?.Trim(), "standard_id", StringComparison.OrdinalIgnoreCase)
                || string.Equals(config.SystemFieldName?.Trim(), "standardid", StringComparison.OrdinalIgnoreCase)
                || string.Equals(config.SystemFieldName?.Trim(), "part_number", StringComparison.OrdinalIgnoreCase)
                || string.Equals(config.SystemFieldName?.Trim(), "partnumber", StringComparison.OrdinalIgnoreCase)
                || string.Equals(config.SystemFieldName?.Trim(), "numero de parte", StringComparison.OrdinalIgnoreCase)
                || string.Equals(config.SystemFieldName?.Trim(), "número de parte", StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsPartNumberOrStandardIdConfiguration(ScanConfigurationRequest config)
        {
            return IsSystemFieldConfiguration(config, (int)SystemField_e.PartNumber)
                || IsSystemFieldConfiguration(config, (int)SystemField_e.StandardId);
        }

        private static bool IsSystemFieldConfiguration(ScanConfigurationRequest config, int systemFieldId)
        {
            if (config.SystemFieldId == systemFieldId)
                return true;

            var fieldName = config.SystemFieldName?.Trim();

            return systemFieldId switch
            {
                (int)SystemField_e.StandardId => string.Equals(fieldName, "standard_id", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(fieldName, "standardid", StringComparison.OrdinalIgnoreCase),
                (int)SystemField_e.PartNumber => string.Equals(fieldName, "part_number", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(fieldName, "partnumber", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(fieldName, "numero de parte", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(fieldName, "nÃºmero de parte", StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }

        private static string ApplySaveConfiguration(ScanConfigurationRequest config, string scanValue)
        {
            var saveTypeId = config.SaveTypeId.GetValueOrDefault(1);
            var count = Math.Max(config.SaveValue, 0);

            return saveTypeId switch
            {
                2 => count >= scanValue.Length ? string.Empty : scanValue[count..],
                3 => count >= scanValue.Length ? string.Empty : scanValue[..^count],
                _ => scanValue
            };
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnCerrarVentana_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class ScanRuleDisplay : INotifyPropertyChanged
    {
        private string _capturedValue = string.Empty;

        public ScanRuleDisplay(ScanConfigurationRequest configuration)
        {
            Configuration = configuration;
        }

        public ScanConfigurationRequest Configuration { get; }
        public int Order => Configuration.Order;
        public string FieldName => string.IsNullOrWhiteSpace(Configuration.ClientField)
            ? Configuration.SystemFieldName
            : Configuration.ClientField;

        public string ConditionSummary => IsStandardIdConfiguration(Configuration)
            ? "Es etiqueta LD"
            : IsPartNumberConfiguration(Configuration)
                ? "Es n\u00FAmero de parte"
                : Configuration.ScanTypeId.GetValueOrDefault(1) switch
        {
            1 => "Sin condición",
            2 => $"Empieza con '{Configuration.ScanValue}'",
            3 => $"Longitud = {Configuration.ScanValue}",
            4 => $"Número < {Configuration.ScanValue}",
            _ => "Sin condición"
        };

        public string SaveSummary => Configuration.SaveTypeId.GetValueOrDefault(1) switch
        {
            2 => $"Quitar primeros {Configuration.SaveValue}",
            3 => $"Quitar últimos {Configuration.SaveValue}",
            _ => "Guardar completo"
        };

        public string CapturedValue
        {
            get => _capturedValue;
            set
            {
                if (_capturedValue == value)
                    return;

                _capturedValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CapturedValue)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private static bool IsStandardIdConfiguration(ScanConfigurationRequest config)
        {
            var fieldName = config.SystemFieldName?.Trim();

            return config.SystemFieldId == (int)SystemField_e.StandardId
                || string.Equals(fieldName, "standard_id", StringComparison.OrdinalIgnoreCase)
                || string.Equals(fieldName, "standardid", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsPartNumberConfiguration(ScanConfigurationRequest config)
        {
            var fieldName = config.SystemFieldName?.Trim();

            return config.SystemFieldId == (int)SystemField_e.PartNumber
                || string.Equals(fieldName, "part_number", StringComparison.OrdinalIgnoreCase)
                || string.Equals(fieldName, "partnumber", StringComparison.OrdinalIgnoreCase)
                || string.Equals(fieldName, "numero de parte", StringComparison.OrdinalIgnoreCase)
                || string.Equals(fieldName, "n\u00FAmero de parte", StringComparison.OrdinalIgnoreCase);
        }
    }

    public record ScanRuleResult(ScanConfigurationRequest Configuration, string Value);

    public record UnmatchedScanResult(bool Success, int SystemFieldId, string Value, string Message);
}
