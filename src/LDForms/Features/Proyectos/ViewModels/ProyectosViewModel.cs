using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Configuration;
using LD.Client.Services;
using LD.Contracts.Constants;
using LD.Contracts.Project;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Proyectos.ViewModels;

public partial class ProyectosViewModel : ObservableObject
{
    private readonly ProjectService _projectService;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string loadingMessage = "Trayendo proyectos...";

    [ObservableProperty]
    private string statusText = "";

    [ObservableProperty]
    private ProjectDto? selectedProject;

    [ObservableProperty]
    private bool canCreate;

    [ObservableProperty]
    private bool canEdit;

    [ObservableProperty]
    private bool canView;

    public event Action<List<ProjectDto>>? OnDataLoaded;

    public ProyectosViewModel(ProjectService projectService)
    {
        _projectService = projectService;

        CanCreate = UserData.HasPermission(PermissionKeys.Project_Create);
        CanEdit = UserData.HasPermission(PermissionKeys.Project_Update);
        CanView = UserData.HasPermission(PermissionKeys.Project_View);
    }

    [RelayCommand]
    public async Task CargarDatosAsync()
    {
        if (!CanView) return;

        try
        {
            IsLoading = true;
            LoadingMessage = "Trayendo proyectos...";

            var result = await _projectService.GetProjects();

            if (!result.IsSuccess)
            {
                DialogHelper.ShowWarning(result.Message);
                return;
            }

            var data = result.Data?.Where(x => x != null).Select(x => x!).ToList() ?? [];
            SelectedProject = null;
            StatusText = $"Registros: {data.Count}";
            OnDataLoaded?.Invoke(data);
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
