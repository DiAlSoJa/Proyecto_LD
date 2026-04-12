using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Category;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Catalogos.Categorias.ViewModels;

public partial class CatalogoCategoriasViewModel : ObservableObject
{
    private readonly CategoryService _categoryService;

    [ObservableProperty]
    private string statusText = string.Empty;

    [ObservableProperty]
    private CategoryDto? selectedCategory;

    public event Action<List<CategoryDto>>? OnDataLoaded;

    public CatalogoCategoriasViewModel(CategoryService categoryService)
    {
        _categoryService = categoryService;
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
