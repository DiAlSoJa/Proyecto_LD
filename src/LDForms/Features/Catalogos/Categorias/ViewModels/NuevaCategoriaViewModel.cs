using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.Category;
using LD.Contracts.DTOs;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Catalogos.Categorias.ViewModels;

public partial class NuevaCategoriaViewModel : ObservableObject
{
    private readonly CategoryService _categoryService;
    private readonly LookupService _lookupService;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }
    public CategoryDto? SelectedCategory { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nueva categoría";

    public NuevaCategoriaViewModel(CategoryService categoryService, LookupService lookupService)
    {
        _categoryService = categoryService;
        _lookupService = lookupService;
    }

    public void SetCategory(CategoryDto category)
    {
        SelectedCategory = category;
        HeaderTitle = "Editar categoría";
    }

    public async Task<List<DropDownDto>> GetClientsAsync()
    {
        var response = await _lookupService.GetClientLookup();
        return response.IsSuccess && response.Data is not null ? response.Data : [];
    }

    public async Task<List<DropDownDto>> GetProjectsAsync(int clientId)
    {
        var response = await _lookupService.GetProjectClientLookup(clientId);
        return response.IsSuccess && response.Data is not null ? response.Data : [];
    }

    public async Task<CategoryRequest?> GetCategoryAsync()
    {
        var response = await _categoryService.GetCategoryById(SelectedCategory?.CategoriaId ?? 0);

        if (!response.IsSuccess || response.Data is null)
        {
            DialogHelper.ShowError(response.Message ?? "No se pudo cargar la categoría.");
            return null;
        }

        return response.Data;
    }

    public async Task SaveAsync(CategoryRequest request)
    {
        try
        {
            var result = SelectedCategory is not null
                ? await _categoryService.UpdateCategory(SelectedCategory.CategoriaId, request)
                : await _categoryService.CreateCategory(request);

            if (result.IsSuccess)
            {
                DialogHelper.ShowSuccess(result.Data ?? "Guardado correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
            }
            else
            {
                DialogHelper.ShowError(result.ErrorMessage ?? "Hubo un error al guardar.");
            }
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }
}
