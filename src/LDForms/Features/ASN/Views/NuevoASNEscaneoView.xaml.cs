using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LD.Contracts.Requests;

namespace LD.FormsX.Views.Dialogs
{
    public partial class NuevoASNEscaneoView : Window
    {
        public ObservableCollection<ScanRuleDisplay> ScanRules { get; } = new();

        public event Func<IReadOnlyList<ScanRuleResult>, Task<bool>>? ScanCompleted;

        public NuevoASNEscaneoView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void SetScanConfigurations(IEnumerable<ScanConfigurationRequest>? configurations)
        {
            ScanRules.Clear();

            foreach (var config in (configurations ?? Enumerable.Empty<ScanConfigurationRequest>())
                .Where(HasAnyScanOrSaveCondition)
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
                && RuleMatches(rule.Configuration, scanValue));

            if (match == null)
            {
                AddMessage("No coincide con ninguna condición configurada.");
                return;
            }

            var savedValue = ApplySaveConfiguration(match.Configuration, scanValue);
            match.CapturedValue = savedValue;

            AddMessage($"{match.FieldName}: {savedValue}");

            if (ScanRules.All(rule => !string.IsNullOrWhiteSpace(rule.CapturedValue)))
                await CompleteCurrentScanAsync();
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
            var hasScanCondition = config.ScanTypeId.GetValueOrDefault(1) != 1
                && !string.IsNullOrWhiteSpace(config.ScanValue);

            var hasSaveCondition = config.SaveTypeId.GetValueOrDefault(1) != 1
                && config.SaveValue > 0;

            return hasScanCondition || hasSaveCondition;
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

        public string ConditionSummary => Configuration.ScanTypeId.GetValueOrDefault(1) switch
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
    }

    public record ScanRuleResult(ScanConfigurationRequest Configuration, string Value);
}
