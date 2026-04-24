using LD.Contracts.EquipmentType;
using LD.Contracts.Requests;
using LD.FormsX.Features.Catalogos.Equipos.ViewModels;
using LD.FormsX.Helpers;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LD.FormsX.Views.Equipos
{
    public partial class NuevoEquipoView : Window
    {
        private NuevoEquipoViewModel ViewModel => (NuevoEquipoViewModel)DataContext;
        private string? _selectedLeftImageFile;
        private string? _selectedRightImageFile;
        private string _currentLeftImagePath = string.Empty;
        private string _currentRightImagePath = string.Empty;

        public bool ResponseForm => ViewModel.ResponseForm;

        public NuevoEquipoView(NuevoEquipoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.RequestClose = () =>
            {
                DialogResult = true;
                Close();
            };

            btnSeleccionarImagenIzq.Click += BtnSeleccionarImagenIzq_Click;
            btnSeleccionarImagenDer.Click += BtnSeleccionarImagenDer_Click;
        }

        public void SetEquipmentType(EquipmentTypeDto equipmentType)
        {
            ViewModel.SetEquipmentType(equipmentType);
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (ViewModel.SelectedEquipmentType is not null)
                await CargarDatosAsync();
        }

        private async Task CargarDatosAsync()
        {
            var equipmentType = await ViewModel.GetEquipmentTypeAsync();
            if (equipmentType is null)
                return;

            txtId.Text = equipmentType.EquipmentTypeId.ToString();
            txtNombre.Text = equipmentType.EquipmentName;
            chkIsBattery.IsChecked = equipmentType.IsBattery;
            _currentLeftImagePath = equipmentType.ImagePathLeft ?? string.Empty;
            _currentRightImagePath = equipmentType.ImagePathRight ?? string.Empty;
            await CargarImagenesActualesAsync();
        }

        private EquipmentTypeRequest BuildRequest()
        {
            return new EquipmentTypeRequest
            {
                EquipmentTypeId = ViewModel.SelectedEquipmentType?.EquipmentTypeId ?? 0,
                EquipmentName = txtNombre.Text.Trim(),
                IsBattery = chkIsBattery.IsChecked ?? false,
                ImagePathLeft = _currentLeftImagePath,
                ImagePathRight = _currentRightImagePath
            };
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.IsEnabled = false;

                await SubirImagenesSeleccionadasAsync();
                var request = BuildRequest();
                await ViewModel.SaveAsync(request, ViewModel.SelectedEquipmentType?.EquipmentTypeId ?? 0);
            }
            catch (Exception ex)
            {
                DialogHelper.ShowError(ex.Message);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
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

        private void BtnSeleccionarImagenIzq_Click(object sender, RoutedEventArgs e)
        {
            var selectedFile = SeleccionarImagen();
            if (string.IsNullOrWhiteSpace(selectedFile))
                return;

            _selectedLeftImageFile = selectedFile;
            CargarImagenDesdeArchivo(imgMontacargasIzq, selectedFile);
        }

        private void BtnSeleccionarImagenDer_Click(object sender, RoutedEventArgs e)
        {
            var selectedFile = SeleccionarImagen();
            if (string.IsNullOrWhiteSpace(selectedFile))
                return;

            _selectedRightImageFile = selectedFile;
            CargarImagenDesdeArchivo(imgMontacargasDer, selectedFile);
        }

        private string? SeleccionarImagen()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Seleccionar imagen",
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp|Todos los archivos|*.*",
                Multiselect = false
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        private async Task SubirImagenesSeleccionadasAsync()
        {
            if (!string.IsNullOrWhiteSpace(_selectedLeftImageFile))
            {
                var uploadResponse = await ViewModel.UploadImageAsync(_selectedLeftImageFile, "left");
                if (!uploadResponse.IsSuccess || uploadResponse.Data is null)
                    throw new InvalidOperationException(uploadResponse.Message ?? uploadResponse.ErrorMessage ?? "No se pudo subir la imagen izquierda.");

                _currentLeftImagePath = uploadResponse.Data.RelativePath;
            }

            if (!string.IsNullOrWhiteSpace(_selectedRightImageFile))
            {
                var uploadResponse = await ViewModel.UploadImageAsync(_selectedRightImageFile, "right");
                if (!uploadResponse.IsSuccess || uploadResponse.Data is null)
                    throw new InvalidOperationException(uploadResponse.Message ?? uploadResponse.ErrorMessage ?? "No se pudo subir la imagen derecha.");

                _currentRightImagePath = uploadResponse.Data.RelativePath;
            }
        }

        private async Task CargarImagenesActualesAsync()
        {
            await CargarImagenAsync(imgMontacargasIzq, loaderImagenIzq, _currentLeftImagePath);
            await CargarImagenAsync(imgMontacargasDer, loaderImagenDer, _currentRightImagePath);
        }

        private async Task CargarImagenAsync(
            System.Windows.Controls.Image imageControl,
            LD.FormsX.Controls.LoadingOverlay loader,
            string relativePath)
        {
            imageControl.Source = null;

            if (!string.IsNullOrWhiteSpace(relativePath))
            {
                loader.Show("Cargando...");

                try
                {
                    var imageBytes = await ViewModel.DownloadImageAsync(relativePath);
                    CargarImagenDesdeBytes(imageControl, imageBytes);
                    return;
                }
                catch
                {
                }
                finally
                {
                    loader.Hide();
                }
            }
        }

        private static void CargarImagenDesdeArchivo(System.Windows.Controls.Image imageControl, string filePath)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(filePath, UriKind.Absolute);
            bitmap.EndInit();
            bitmap.Freeze();
            imageControl.Source = bitmap;
        }

        private static void CargarImagenDesdeBytes(System.Windows.Controls.Image imageControl, byte[] imageBytes)
        {
            using var stream = new MemoryStream(imageBytes);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            bitmap.Freeze();
            imageControl.Source = bitmap;
        }
    }
}
