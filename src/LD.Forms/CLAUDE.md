# LD.Forms — Project Guide for Claude Code

## Project Overview

`LD.Forms` (net10.0-windows) is the **legacy WinForms client**. It is a Windows Forms application (`UseWindowsForms=true`) that was the original desktop client for the WMS. It is **no longer the primary desktop client** — that role has been replaced by `LD.FormsX` (WPF). Active development has largely shifted to `LD.FormsX`.

Despite being legacy, this project still compiles and connects to the same `LD.Api`. It references `LD.Client` and `LD.Contracts` like the other clients.

## Pattern

This project does NOT use the MVVM pattern. Instead it uses **MVP (Model-View-Presenter)**:
- Views: WinForms `Form` classes (e.g., `FrmLogin`, `FrmClientes`, `FrmInventario`)
- Presenters: `LoginPresenter`, `ClientPresenter` in `Presenters/`
- Interfaces: `ILoginView`, `ICreateUserView`, `IBaseMessageDialog` in `Views/Interfaces/`

The MVP approach is simpler than MVVM but not consistent with the WPF and MAUI clients.

## Structure

```
LD.Forms
├── Program.cs (entry point)
├── Classes/
│   ├── Formularios.cs           ← Helper for form management
│   ├── UserData.cs / UserSession.cs  ← Session state (mirrors LD.Client but local copy)
│   ├── LoaderManager.cs         ← Loading overlay management
│   ├── GridFilter.cs / AxGridFilter.cs / DataGridViewExtensions.cs
├── Configuration/
│   ├── ApiEndpoints.cs          ← URL definitions (local copy, not the shared one from LD.Client)
│   ├── ApiSettings.cs
│   ├── AppApplicationContext.cs ← Custom ApplicationContext for WinForms app
│   └── AppRoute.cs
├── Controls/
│   ├── LoaderControl            ← Custom loading control
│   ├── RoundedButton / RoundedPanel / TextBoxControl
├── Exceptions/
│   └── ApiException.cs
├── Presenters/
│   ├── LoginPresenter.cs
│   └── ClientPresenter.cs
├── Views/
│   ├── Dialogs/                 ← FrmNew<X> dialogs for creating records
│   │   ├── FrmNewCategory / FrmNewFamily / FrmNewCurrency / FrmNewUnit / FrmNewStatus
│   │   ├── FrmNewDimensioner
│   │   ├── FrmNuevoRol / FrmNuevoAleatorio / FrmNuevoMontacargas
│   │   ├── FrmNuevoASN / FrmNuevoASNEscaneo
│   │   ├── FrmVehiculosRegistrados / FrmChooseUser / FrmDocumentos / FrmParametersQuery
│   │   └── FrmRoles
│   ├── Forms/                   ← Main application forms
│   │   ├── FrmLogin / FrmPrincipal / FrmMenu
│   │   ├── FrmClientes / FrmProyectos / FrmUbicaciones / FrmUsuarios
│   │   ├── FrmInventario / FrmMovimientos
│   │   ├── FrmCatalogos / FrmCheckListMontacargas
│   │   ├── FrmControlPatio / FrmReportes / FrmASN
│   │   └── FrmPlantillaForm (template form)
│   └── Interfaces/
│       ├── ILoginView.cs / ICreateUserView.cs / IBaseMessageDialog.cs
├── Models/                      ← (empty folder, reserved)
├── Resources/
│   ├── logo.ico
│   └── Properties/Resources.resx
└── appsettings.json / appsettings.Development.json
```

## DI Setup

Uses `Microsoft.Extensions.Hosting` for configuration but has a lighter DI setup than the WPF client. The entry point (`Program.cs` or `AppApplicationContext`) wires up configuration reading and launches the login form.

## Configuration

Same pattern as `LD.FormsX`:
- `appsettings.json`: `ApiSettings:BaseUrl`
- Environment controlled by `DOTNET_ENVIRONMENT`

## Dependencies

**Project references:** `LD.Client`, `LD.Contracts`

**NuGet:** `Microsoft.Extensions.Configuration 10.0.3`, `Microsoft.Extensions.Hosting 10.0.3`, `Microsoft.Extensions.DependencyInjection 10.0.3`

## Development Status

This project is **low priority** and not actively developed. From the root CLAUDE.md: _"`LD.Forms`, ya no es prioridad"_.

- Do not invest effort in refactoring this project to MVVM
- New features for the desktop should go in `LD.FormsX`
- Bug fixes only if specifically requested

## Building

```bash
dotnet build src/LD.Forms/LD.Forms.csproj
```

## Things Claude Must NOT Do

- Do not add new major features to this project — use `LD.FormsX`
- Do not refactor to MVVM — the project uses MVP and that is acceptable for its legacy status
- Do not change the reference to `LD.Client` to a direct HTTP approach
