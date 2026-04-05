using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LD.Client.Services;
using LD.FormsX.Helpers;

namespace LD.FormsX.Views.Articulos
{
   
    
        public partial class ProductLookupWindow : Window
        {
            private readonly ProductService _productService;
            private readonly int _clientId;
            private readonly int _projectId;

            private List<ProductLookupItemVm> _allItems = new();

            public ProductLookupItemVm? SelectedProduct { get; private set; }

            public ProductLookupWindow(ProductService productService, int clientId, int projectId)
            {
                InitializeComponent();
                _productService = productService;
                _clientId = clientId;
                _projectId = projectId;

                Loaded += ProductLookupWindow_Loaded;
            }

            private async void ProductLookupWindow_Loaded(object sender, RoutedEventArgs e)
            {
                await LoadProductsAsync();
            }

            private async Task LoadProductsAsync()
            {
                try
                {
                    var response = await _productService.GetProductByClientId(_clientId, _projectId); // ajusta al método real

                    if (!response.IsSuccess || response.Data == null)
                    {
                        DialogHelper.ShowWarning(response.Message ?? "No se pudieron cargar los productos.");
                        return;
                    }

                    _allItems = response.Data                       
                        .Select(x => new ProductLookupItemVm
                        {                            
                            PartNumber = x.Key ?? string.Empty,
                            Description = x.Value ?? string.Empty
                        })
                        .OrderBy(x => x.PartNumber)
                        .ToList();

                    dgProductos.ItemsSource = _allItems;
                }
                catch (Exception ex)
                {
                    DialogHelper.ShowError(ex.Message);
                }
            }

            private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
            {
                ApplyFilter();
            }

            private void BtnBuscar_Click(object sender, RoutedEventArgs e)
            {
                ApplyFilter();
            }

            private void ApplyFilter()
            {
                var filtro = txtBuscar.Text.Trim();

                if (string.IsNullOrWhiteSpace(filtro))
                {
                    dgProductos.ItemsSource = _allItems;
                    return;
                }

                dgProductos.ItemsSource = _allItems
                    .Where(x =>
                        x.PartNumber.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                        x.Description.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            private void BtnAceptar_Click(object sender, RoutedEventArgs e)
            {
                if (dgProductos.SelectedItem is not ProductLookupItemVm item)
                {
                    DialogHelper.ShowWarning("Selecciona un producto.");
                    return;
                }

                SelectedProduct = item;
                DialogResult = true;
            }

            private void dgProductos_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                if (dgProductos.SelectedItem is ProductLookupItemVm item)
                {
                    SelectedProduct = item;
                    DialogResult = true;
                }
            }

            private void BtnCancelar_Click(object sender, RoutedEventArgs e)
            {
                DialogResult = false;
            }
        }

        public class ProductLookupItemVm
        {
            public int ProductId { get; set; }
            public string PartNumber { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }
    }