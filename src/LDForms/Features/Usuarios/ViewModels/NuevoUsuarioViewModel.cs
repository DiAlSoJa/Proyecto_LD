using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Client.Services;
using LD.Contracts.DTOs;
using LD.Contracts.DTOs.User;
using LD.Contracts.Requests;
using LD.FormsX.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LD.FormsX.Features.Usuarios.ViewModels;

public partial class NuevoUsuarioViewModel : ObservableObject
{
    private readonly UserService _userService;
    private readonly LookupService _lookupService;

    private string? _editUserId;

    public Action? RequestClose { get; set; }
    public bool ResponseForm { get; private set; }

    [ObservableProperty]
    private string headerTitle = "Nuevo usuario";

    [ObservableProperty]
    private bool isSaving;

    // ── Combos ──
    [ObservableProperty]
    private List<DropDownDto> rolesSource = [];

    [ObservableProperty]
    private string? selectedRoleId;

    // ── Campos ──
    [ObservableProperty]
    private string username = "";

    [ObservableProperty]
    private string fullName = "";

    [ObservableProperty]
    private bool isActive = true;

    // Password se maneja desde code-behind (PasswordBox no soporta binding)

    public NuevoUsuarioViewModel(UserService userService, LookupService lookupService)
    {
        _userService = userService;
        _lookupService = lookupService;
    }

    private static string NormalizeUppercase(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.ToUpper();
    }

    partial void OnUsernameChanged(string value)
    {
        var normalized = NormalizeUppercase(value);
        if (Username != normalized)
            Username = normalized;
    }

    partial void OnFullNameChanged(string value)
    {
        var normalized = NormalizeUppercase(value);
        if (FullName != normalized)
            FullName = normalized;
    }

    public void SetUser(GetUserDto user)
    {
        _editUserId = user.User?.Id;
        HeaderTitle = "Editar usuario";
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        await CargarCombosAsync();
        if (_editUserId != null)
            await CargarDatosAsync();
    }

    private async Task CargarCombosAsync()
    {
        var response = await _lookupService.GetRoleLookup();
        if (response.IsSuccess)
            RolesSource = response.Data ?? [];
    }

    private async Task CargarDatosAsync()
    {
        try
        {
            var response = await _userService.GetUserById(_editUserId ?? "");

            if (!response.IsSuccess)
            {
                DialogHelper.ShowError(response.Message);
                return;
            }

            var user = response.Data;
            Username = user.Username ?? "";
            FullName = user.Name ?? "";
            IsActive = user.IsActive;
            SelectedRoleId = user.Role;
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
    }

    public async Task SaveAsync(string password, string confirmPassword)
    {
        try
        {
            IsSaving = true;

            var request = new UserRequest
            {
                Username = NormalizeUppercase(Username).Trim(),
                Name = NormalizeUppercase(FullName).Trim(),
                Password = password,
                ConfirmPassword = confirmPassword,
                Role = SelectedRoleId,
                IsActive = IsActive,
            };

            var result = _editUserId != null
                ? await _userService.UpdateUser(_editUserId, request)
                : await _userService.CreateUser(request);

            if (result.IsSuccess)
            {
                DialogHelper.ShowSuccess(result.Data ?? "Operación realizada correctamente.");
                ResponseForm = true;
                RequestClose?.Invoke();
            }
            else
            {
                DialogHelper.ShowError(result.Message);
            }
        }
        catch (Exception ex)
        {
            DialogHelper.ShowError(ex.Message);
        }
        finally
        {
            IsSaving = false;
        }
    }
}
