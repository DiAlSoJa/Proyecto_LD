using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LD.FormsX.Views.Articulos
{
    public partial class CargaMasivaArticulosView : Window, INotifyPropertyChanged
    {
        private string _clientName = string.Empty;
        private string _projectName = string.Empty;

        public ObservableCollection<CargaMasivaArticuloRow> Items { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public CargaMasivaArticulosView()
        {
            InitializeComponent();
            DataContext = this;
            Items.CollectionChanged += (_, _) => UpdateTotals();
            UpdateContextText();
            UpdateTotals();
        }

        public void SetContext(int clientId, int projectId, string clientName, string projectName)
        {
            _clientName = string.IsNullOrWhiteSpace(clientName) ? clientId.ToString() : clientName;
            _projectName = string.IsNullOrWhiteSpace(projectName) ? projectId.ToString() : projectName;
            UpdateContextText();
        }

        private void BtnPegar_Click(object sender, RoutedEventArgs e)
        {
            var rawText = Clipboard.GetText();
            ImportRows(rawText);
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            Items.Clear();
            UpdateTotals();
        }

        private void BtnAceptar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void ImportRows(string rawText)
        {
            if (string.IsNullOrWhiteSpace(rawText))
                return;

            var lines = rawText
                .Replace("\r\n", "\n")
                .Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var columns = line.Split('\t');
                if (columns.Length == 0)
                    continue;

                var partNumber = columns.ElementAtOrDefault(0)?.Trim() ?? string.Empty;
                var description = columns.ElementAtOrDefault(1)?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(partNumber) && string.IsNullOrWhiteSpace(description))
                    continue;

                Items.Add(new CargaMasivaArticuloRow
                {
                    PartNumber = partNumber,
                    Description = description
                });
            }

            UpdateTotals();
        }

        private void UpdateContextText()
        {
            if (txtContexto == null)
                return;

            txtContexto.Text = $"Cliente: {_clientName}   Proyecto: {_projectName}";
        }

        private void UpdateTotals()
        {
            OnPropertyChanged(nameof(Items));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CargaMasivaArticuloRow
    {
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
