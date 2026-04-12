using CommunityToolkit.Mvvm.ComponentModel;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.DTOs.Family;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Catalogos.Familias.ViewModels;

public partial class NuevaFamiliaViewModel : ObservableObject
{
    private readonly FamilyService _familyService;
    private readonly LookupService _lookupService;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }
    public FamilyDto? SelectedFamily { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nueva familia";

    public NuevaFamiliaViewModel(FamilyService familyService, LookupService lookupService)
    {
        _familyService = familyService;
        _lookupService = lookupService;
    }

    public void SetFamily(FamilyDto family)
    {
        SelectedFamily = family;
        HeaderTitle = "Editar familia";
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

    public async Task<FamilyRequest?> GetFamilyAsync()
    {
        var response = await _familyService.GetFamilyById(SelectedFamily?.FamiliaId ?? 0);

        if (!response.IsSuccess || response.Data is null)
        {
            DialogHelper.ShowError(response.Message ?? "No se pudo cargar la familia.");
            return null;
        }

        return response.Data;
    }

    public async Task SaveAsync(FamilyRequest request)
    {
        try
        {
            var result = SelectedFamily is not null
                ? await _familyService.UpdateFamily(SelectedFamily.FamiliaId, request)
                : await _familyService.CreateFamily(request);

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
