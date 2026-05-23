using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LD.Contracts.Enums;

namespace LDForms.Features.DashBoard.ViewModels;

public partial class ModuleTileVm : ObservableObject
{
    public ModuleTileVm(
        string key,
        string title,
        string iconKind,
        string iconColor,
        Module_e module,
        bool isPrintTile,
        string? printMenuHeader,
        IAsyncRelayCommand openCommand,
        IRelayCommand openInWindowCommand)
    {
        Key = key;
        Title = title;
        IconKind = iconKind;
        IconColor = iconColor;
        Module = module;
        IsPrintTile = isPrintTile;
        PrintMenuHeader = printMenuHeader;
        OpenCommand = openCommand;
        OpenInWindowCommand = openInWindowCommand;
    }

    public string Key { get; }

    public string Title { get; }

    public string IconKind { get; }

    public string IconColor { get; }

    public Module_e Module { get; }

    public bool IsPrintTile { get; }

    public string? PrintMenuHeader { get; }

    [ObservableProperty]
    private bool isVisible;

    public IAsyncRelayCommand OpenCommand { get; }

    public IRelayCommand OpenInWindowCommand { get; }
}
