using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace LD.FormsX.Views.Catalogos
{
    public partial class CatalogosView : UserControl
    {
        private readonly IServiceProvider _serviceProvider;

        public CatalogosView(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;

            Loaded += CatalogosView_Loaded;
        }

        private void CatalogosView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Loaded -= CatalogosView_Loaded;
            CargarVistas();
        }

        private void CargarVistas()
        {
            try
            {
                TabEstatus.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoStatusView>());
                TabCategorias.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoCategoriasView>());
                TabUnidades.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoUnidadesView>());
                TabMonedas.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoMonedasView>());
                TabFamilias.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoFamiliasView>());
                TabDimensionador.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoDimensionadorView>());
            }
            catch (Exception ex)
            {
               
            }
        }

        private UIElement CrearContenedor(UserControl vista)
        {
            return new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Background = System.Windows.Media.Brushes.Transparent,
                Content = vista
            };
        }
    }
}