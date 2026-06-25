using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LD.Client.Services;
using LD.Contracts.DTOs.Family;
using LD.FormsX.Views.Ubicaciones;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.Auditar
{
    /// <summary>
    /// Lógica de interacción para AuditarView.xaml
    /// </summary>
    public partial class AuditarView : UserControl
    {
        private readonly FamilyService _service;
        private readonly IServiceProvider _serviceProvider;

        public ObservableCollection<LocationCardPreview> LeftCards { get; } = new();
        public ObservableCollection<LocationCardPreview> RightCards { get; } = new();
        public ObservableCollection<AuditoriaResumenItem> AuditoriaResumenItems { get; } = new();

        private FamilyDto? _selectedX;
        private bool _loaded;

        public AuditarView(FamilyService serviceX, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _service = serviceX;
            _serviceProvider = serviceProvider;
            DataContext = this;
            LoadSampleCards();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) { }
        private void BtnActualizar_Click(object sender, RoutedEventArgs e) { }
        private void BtnNuevaAuditoria_Click(object sender, RoutedEventArgs e) { }

        private async void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = _serviceProvider.GetRequiredService<NuevaAuditoriaView>();
            LD.FormsX.Features.Common.WindowOwnerHelper.AttachOwnerOrCenter(dialog, LD.FormsX.Features.Common.WindowOwnerHelper.GetVisibleOwner(Window.GetWindow(this)));

            var result = dialog.ShowDialog();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e) { }
        private void dgAuditorias_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void dgDetalleAuditoria_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void LoadSampleCards()
        {
            LeftCards.Clear();
            RightCards.Clear();

            for (var i = 1; i <= 10; i++)
            {
                var standardId = $"20260621{i:0000}";
                var partNumber = $"PN-{i:00000}";
                var kitting = $"KIT{i:00000}";
                var leftQuantity = 100 + i;

                LeftCards.Add(new LocationCardPreview
                {
                    Title = standardId,
                    Subtitle = $"Parte: {partNumber}",
                    Details = $"{kitting} {leftQuantity:0.00}"
                });

                var standardIdRight = $"20260622{i:0000}";
                var partNumberRight = $"PN-{(i + 10):00000}";
                var kittingRight = $"KIT{(i + 10):00000}";
                var rightQuantity = 120 + i;

                RightCards.Add(new LocationCardPreview
                {
                    Title = standardIdRight,
                    Subtitle = $"Parte: {partNumberRight}",
                    Details = $"{kittingRight} {rightQuantity:0.00}"
                });
            }
        }

    }

    public sealed class LocationCardPreview
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public sealed class AuditoriaResumenItem
    {
        public string Cliente { get; set; } = string.Empty;
        public string Proyecto { get; set; } = string.Empty;
        public string OrdenEntrega { get; set; } = string.Empty;
        public string Kitting { get; set; } = string.Empty;
    }
}

