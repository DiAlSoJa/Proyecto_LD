using System;
using System.Windows;
using System.Windows.Controls;
using LD.Client.Configuration;
using LD.Contracts.Constants;
using LD.Contracts.Enums;
using LD.FormsX.Views;
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
            SetVisibility();
            CargarVistas();
        }

        private void SetVisibility()
        {
            RemoveIfNoModule(TabEstatus,       Module_e.Status);
            RemoveIfNoModule(TabCategorias,    Module_e.Categories);
            RemoveIfNoModule(TabUnidades,      Module_e.Units);
            RemoveIfNoEquipmentPermissions(TabEquipos);
            RemoveIfNoModule(TabTiposCamion,   Module_e.Catalogs);
            RemoveIfNoModule(TabMonedas,       Module_e.Currencies);
            RemoveIfNoModule(TabFamilias,      Module_e.Families);
            RemoveIfNoModule(TabDimensionador, Module_e.Dimensioner);
        }

        private void RemoveIfNoModule(TabItem tab, Module_e module)
        {
            if (!UserData.HasModule((int)module))
                MainTabControl.Items.Remove(tab);
        }

        private void RemoveIfNoEquipmentPermissions(TabItem tab)
        {
            if (!UserData.HasPermission(PermissionKeys.EquipmentType_View)
                && !UserData.HasPermission(PermissionKeys.EquipmentType_Create)
                && !UserData.HasPermission(PermissionKeys.EquipmentType_Update))
            {
                MainTabControl.Items.Remove(tab);
            }
        }

        private void CargarVistas()
        {
            try
            {
                TabEstatus.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoStatusView>());
                TabCategorias.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoCategoriasView>());
                TabUnidades.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoUnidadesView>());
                TabEquipos.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoEquiposView>());
                TabTiposCamion.Content = CrearContenedor(_serviceProvider.GetRequiredService<CatalogoTiposCamionView>());
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
