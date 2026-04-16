using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Category;
using LD.Contracts.Constants;
using LD.FormsX.Helpers;

namespace LD.FormsX.Features.Catalogos.Categorias.ViewModels;

public partial class CatalogoCategoriasViewModel : ObservableObject
{
    private readonly CategoryService _categoryService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private CategoryDto? selectedCategory;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    public event Action<List<CategoryDto>>? OnDataLoaded;

    public CatalogoCategoriasViewModel(CategoryService categoryService)
    {
        _categoryService = categoryService;
        CanCreate = UserData.HasPermission(PermissionKeys.Category_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.Category_Update);
        CanView = UserData.HasPermission(PermissionKeys.Category_View);

    }

    public async Task CargarDatosAsync()
    {
        try
        {
            var result = await _categoryService.GetCategory();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            SelectedCategory = null;
            StatusText = $"Registros: {result.Data.Count}";
            OnDataLoaded?.Invoke(result.Data);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }
}
