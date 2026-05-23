# CLAUDE.md

Guía para Claude Code (claude.ai/code) al trabajar en este repositorio. Lee este archivo completo antes de hacer cambios — la mayoría de errores vienen de no respetar la capa correcta o de duplicar lógica que ya existe.

## Resumen del proyecto

**Proyecto_LD** es un WMS (warehouse management system) multi-cliente para operaciones logísticas. Una sola API .NET sirve a tres frontends: app móvil MAUI, escritorio WPF (`LD.FormsX`), y un WinForms heredado (`LD.Forms`, ya no es prioridad).

**Versiones**: .NET 9 / .NET 10. SQL Server (Express local).

## Comandos build & run

```bash
# Restaurar paquetes
dotnet restore

# Build solución completa
dotnet build LD.sln

# Correr API (puerto 8050, aplica migraciones al iniciar)
dotnet run --project src/LD.Api

# Build clientes
dotnet build src/LD.MobileApp/LD.MobileApp.csproj
dotnet build src/LDForms/LD.FormsX.csproj

# Migraciones EF Core (siempre con startup-project = LD.Api)
dotnet ef migrations add <NombreDeLaFeature> --project src/LD.Infrastructure --startup-project src/LD.Api
dotnet ef database update --project src/LD.Infrastructure --startup-project src/LD.Api
```

> **Nombrado de migraciones**: usa el nombre del feature principal (ej. `AddScanConfiguration`, `UpdateAsnReceipt`). No "Migration1" ni timestamps en el nombre.

No hay tests automatizados configurados.

## Arquitectura

Clean Architecture en la API, MVVM en los clientes, todos comparten contratos.

```
Clients (MobileApp / FormsX)        ← MVVM con CommunityToolkit.Mvvm
    ↓
LD.Client                           ← HTTP clients tipados
    ↓
LD.Api                              ← Controllers + middlewares + policies
    ↓
LD.Application                      ← CQRS (MediatR), validators, profiles
    ↓
LD.Infrastructure                   ← EF Core, Identity, JWT, repositorios, interceptores
    ↓
LD.Domain                           ← Entidades puras, sin dependencias
    ↓
LD.Contracts                        ← Compartido entre API y clientes (DTOs, Requests, Responses, Enums, Constants)
```

### Reglas de capas (qué va dónde)

| Si necesitas... | Ponlo en... |
|---|---|
| Una entidad de dominio nueva | `LD.Domain/Entities/` |
| Una migración / cambio de schema | `LD.Infrastructure/Migrations/` (vía `dotnet ef`) |
| Configuración de entidad EF | `LdProyectDbContext` (no en archivos `IEntityTypeConfiguration` separados) |
| Lógica de negocio / handler | `LD.Application/Features/<Feature>/Commands` o `/Queries` |
| Validación de input | `LD.Application/Features/<Feature>/Validators` (FluentValidation) |
| Mapeo entidad ↔ DTO | `LD.Application/Features/<Feature>/Profiles` (AutoMapper) |
| Endpoint REST | `LD.Api/Controllers/` |
| Acceso a BD reutilizable | `LD.Infrastructure/Repositories/` (extender `Repository<T>` o crear repo específico) |
| Cliente HTTP de un endpoint | `LD.Client/Services/<Feature>Service.cs` |
| Modelo que cliente envía al API | `LD.Contracts/Requests/` |
| Modelo que API devuelve | `LD.Contracts/Responses/` o `LD.Contracts/DTOs/` |
| Enum compartido | `LD.Contracts/Enums/` |
| Constante (incluyendo permisos) | `LD.Contracts/Constants/` |
| ViewModel de pantalla | `<Cliente>/Features/<Feature>/` |

> **Nota sobre DTOs vs Responses**: Los DTOs en `LD.Contracts/DTOs/` también se usan como response del API en muchos casos. Está bien — no dupliques creando un Response idéntico al DTO. Si la respuesta es diferente al DTO base (por ejemplo lleva metadata, paginación, etc.), entonces sí va en `Responses/`.

## Happy path para agregar una feature nueva

Este es el flujo que se sigue end-to-end. **Síguelo en este orden y respeta cada capa.**

1. **Petición desde el cliente** (MAUI/WPF): ViewModel llama a un método del service de `LD.Client`.
2. **Service en `LD.Client`** → método tipado en `<Feature>Service.cs` que arma la URL y manda el `Request`.
3. **Middlewares** de la API procesan auth, logging, errores.
4. **Controller** en `LD.Api/Controllers/` → expone el endpoint con `[Permission(PermissionKeys.X_Y)]` y delega al `Mediator.Send(...)`.
5. **Behaviors de MediatR** (validación, logging) corren antes del handler.
6. **Command / Query handler** en `LD.Application/Features/<Feature>/...` → lógica de negocio.
7. **Interceptor** (`AuditableEntitySaveChangesInterceptor`) llena `CreatedAt`/`UpdatedAt` automáticamente.
8. **Entity** en `LD.Domain` se persiste vía repositorio + `DbContext`.

### Estructura por feature en `LD.Application/Features/<Feature>/`

Ya está estandarizada — síguela:

```
Features/
└── Category/
    ├── Commands/
    │   ├── CreateCategoryCommand.cs       ← Request + Handler en el mismo archivo (vertical slice)
    │   └── UpdateCategoryCommand.cs
    ├── Queries/
    │   ├── CategoryQuery.cs               ← lista
    │   └── CategoryByIdQuery.cs           ← detalle
    ├── Validators/
    │   ├── CreateCategoryValidator.cs
    │   └── UpdateCategoryValidator.cs
    └── Profiles/
        └── CategoryProfile.cs
```

**Convenciones de naming** (no las inventes, respétalas):
- Commands: `<Verb><Entity>Command.cs` → `CreateCategoryCommand`, `UpdateCategoryCommand`
- Queries: `<Entity>Query.cs` (lista) o `<Entity>By<Criteria>Query.cs` (filtro) → `CategoryQuery`, `CategoryByIdQuery`
- Validators: `<Verb><Entity>Validator.cs` → `CreateCategoryValidator`
- Profiles: `<Entity>Profile.cs` → `CategoryProfile`

## Permisos y autorización

Los permisos **viven en la base de datos** y se cargan como policies dinámicas al arrancar la API. Para proteger un endpoint:

```csharp
[HttpGet]
[Permission(PermissionKeys.Category_View)]
public async Task<IActionResult> getCategory()
    => ResultExtensions.ToActionResult(await Mediator.Send(new CategoryQuery()));
```

- **Atributo custom**: `[Permission(...)]` (no uses `[Authorize(Policy = ...)]` directo).
- **Lista canónica de permisos**: `LD.Contracts/Constants/PermissionKeys.cs`. **Si necesitas un permiso nuevo, agrégalo ahí primero** y luego siémbralo en BD — no inventes strings sueltos.
- Roles y asignación de permisos se administran vía `RoleController`.
- El refresh token está en proceso de implementación/diagnóstico (ver sección "Estado del refresh token" más abajo). **No implementes refresh adicional sin pedirlo.**

## Clientes (MAUI y WPF)

Ambos clientes usan **MVVM con `CommunityToolkit.Mvvm`** (mayormente `ObservableObject` y `[ObservableProperty]` / `[RelayCommand]`).

| Aspecto | MAUI (`LD.MobileApp`) | WPF (`LD.FormsX`) |
|---|---|---|
| Navegación | MAUI Shell (`AppShell.xaml`) | NavigationService propio |
| MVVM | CommunityToolkit.Mvvm | CommunityToolkit.Mvvm |
| Estructura | `Features/<Feature>/{Pages,ViewModels}` | `Features/<Feature>/...` |
| Hardware | ZXing (barcode), Plugin.Maui.OCR | N/A |

### Inyección de dependencias en clientes

Los servicios HTTP de `LD.Client` se registran con **una sola línea**:

```csharp
services.AddLDClient(options =>
{
    options.BaseUrl = configuration["ApiSettings:BaseUrl"] ?? "";
});
```

Eso registra todos los `<Feature>Service` de `LD.Client/Services/`. **No los registres uno por uno.** Si agregas un service nuevo en `LD.Client`, asegúrate de incluirlo en `ConfigureServices.cs` de ese proyecto.

### `ApiService`

Es el HTTP client base. Adjunta el JWT automáticamente. Los services de feature (`ProductService`, `CategoryService`, etc.) lo consumen. **No inventes nuevos clientes HTTP** — usa el que ya existe.

### URL del API

`ApiSettings:BaseUrl` en el `appsettings.json` de cada cliente. Default red local: `http://192.168.0.112:8050/api`.

### Loading modal en peticiones HTTP (MAUI)

**Regla obligatoria**: en `LD.MobileApp`, **toda petición HTTP que se ejecute desde un ViewModel debe envolver la llamada con `ShowBlocking` / `HideBlocking`**, sin excepción. El `HideBlocking` siempre va en el bloque `finally`.

```csharp
try
{
    _dialogService.ShowBlocking("Cargando", "Obteniendo datos...");
    var result = await _someService.GetSomethingAsync();
    // procesar result
}
catch (Exception ex)
{
    await _dialogService.ShowErrorAsync("Error", ex.Message);
}
finally
{
    _dialogService.HideBlocking();
}
```

Aplica a: `[RelayCommand]` methods, `OnAppearing`/`OnNavigatedTo`, y cualquier método que dispare un `await _xxxService.XxxAsync(...)`. No aplica a peticiones de login (que tienen su propio manejo de estado visual).

Al revisar o editar cualquier ViewModel MAUI, si detectas una llamada HTTP sin `ShowBlocking`/`HideBlocking` → agrégala en esa misma edición.

### Diálogos de estado en MAUI (`IDialogService`)

**Regla**: en `LD.MobileApp`, **nunca uses `DisplayAlertAsync`** para mostrar errores, advertencias o confirmaciones. Usa siempre `IDialogService` (inyectado por DI, singleton).

```csharp
// Inyectar en el constructor del ViewModel
private readonly IDialogService _dialogService;

// Uso según el tipo de mensaje
await _dialogService.ShowErrorAsync("Error", "Mensaje de error");
await _dialogService.ShowSuccessAsync("Listo", "Operación exitosa");
await _dialogService.ShowInfoAsync("Información", "Mensaje informativo");
bool continuar = await _dialogService.ShowWarningAsync("Advertencia", "¿Deseas continuar?");

// Bloqueo de pantalla (se cierra solo con HideBlocking)
_dialogService.ShowBlocking("Espera", "Procesando...");
_dialogService.HideBlocking();
```

Tipos disponibles en `DialogType`: `Info`, `Success`, `Warning`, `Error`, `Blocking`.
- `DisplayPromptAsync` sigue siendo válido para capturar input del usuario (no es un diálogo de estado).
- `LogoutDialog` y `OptionPopup` son popups especiales de UX, no diálogos de estado — se usan directamente.

### Entry en formularios MAUI (`LabeledEntry` / `PasswordEntry`)

**Regla**: en `LD.MobileApp`, **nunca uses `<Entry>` estándar dentro de un `<Border>` manual** en formularios. Usa siempre los controles custom de `Features/Controls/`:

- `<controls:LabeledEntry>` — texto normal. Namespace: `xmlns:controls="clr-namespace:MauiAppLogin.Views.Controls"`.
- `<controls:PasswordEntry>` — contraseña (propiedad `Password`, no `Text`).

```xaml
<!-- Correcto -->
<controls:LabeledEntry
    LabelText="Nombre:"
    Text="{Binding Nombre}"
    Placeholder="Ej. Carlos Ramírez"/>

<!-- Incorrecto — Entry estándar dentro de Border manual -->
<VerticalStackLayout>
    <Label Text="Nombre:"/>
    <Border><Entry Text="{Binding Nombre}"/></Border>
</VerticalStackLayout>
```

**BindableProperties de `LabeledEntry`**: `LabelText` (string), `Text` (string, TwoWay), `Placeholder` (string), `IconSource` (ImageSource?).

### Controles de fotos en MAUI (`PhotoGallery` / `PhotoThumb` / `ImagePreviewPopup`)

**Regla**: siempre que se muestren fotos capturadas en un formulario MAUI, usa los controles de `Features/Controls/`:

| Situación | Control a usar |
|---|---|
| Una o más imágenes (galería scrollable) | `<controls:PhotoGallery>` |
| Imagen individual con botón eliminar | `<controls:PhotoThumb>` directamente |
| Ver imagen en pantalla completa (tap) | `ImagePreviewPopup` desde el ViewModel |

```xaml
<!-- Galería (múltiples imágenes, scroll horizontal) -->
<controls:PhotoGallery
    ItemsSource="{Binding Photos}"
    DeleteCommand="{Binding RemovePhotoCommand}"
    ViewCommand="{Binding ViewPhotoCommand}"
    MinimumHeightRequest="90"/>
```

```csharp
// ViewPhotoCommand en el ViewModel
ViewPhotoCommand = new MvvmHelpers.Commands.Command<TFotoItem>(item =>
{
    if (item?.Source is null) return;
    (Shell.Current.CurrentPage ?? Application.Current?.MainPage)
        ?.ShowPopup(new ImagePreviewPopup(item.Source));
});
```

- `PhotoGallery` expone: `ItemsSource` (IEnumerable), `DeleteCommand` (ICommand), `ViewCommand` (ICommand).
- `PhotoThumb` expone: `PhotoSource` (ImageSource), `DeleteCommand`, `DeleteCommandParameter`, `ViewCommand`, `ViewCommandParameter`.
- Nunca uses `CollectionView ItemsLayout="HorizontalList"` ni thumbnails manuales — usa estos controles.
- El ícono de eliminar es un **bote de basura** (🗑) — no usar ✕ en miniaturas de fotos.

### Regla de revisión obligatoria (MAUI)

Al leer o editar **cualquier** Page o ViewModel del proyecto MAUI, si detectas:
- Un `<Entry>` estándar dentro de un `<Border>` manual → migrarlo a `<controls:LabeledEntry>` en esa misma edición.
- Un `DisplayAlert` / `DisplayAlertAsync` / `DisplayActionSheet` → migrarlo a `IDialogService` en esa misma edición.
- Un `CollectionView` o galería de fotos manual → migrarlo a `<controls:PhotoGallery>` en esa misma edición.

No dejes Pages con estos patrones sin migrar al pasar por ellas. Hay un inventario pendiente de ~60 instancias de `DisplayAlertAsync` en el proyecto; migralas conforme se edite cada archivo, no todas de golpe.

## Contracts: Requests, Responses, DTOs

`LD.Contracts` es la única capa compartida entre API y clientes. Aquí no va lógica, solo modelos.

```
LD.Contracts/
├── Requests/        ← lo que el cliente manda al API (ej. CreateCategoryRequest)
├── Responses/       ← respuestas tipadas (incluye ApiResponseDto<T>)
├── DTOs/            ← modelos de datos compartidos (con frecuencia también funcionan como response)
├── Enums/
├── Constants/       ← PermissionKeys vive aquí
├── AvailableInventory/
└── Equipment/
```

**Reglas**:
- Si la respuesta del API es esencialmente la entidad serializada → usa el DTO existente, no crees `XxxResponse` duplicado.
- Si la respuesta agrega cosas (paginación, totales, metadata) → crea un `Response` específico.
- Los nombres terminan en `Request`, `Response`, `Dto`. Sé consistente.

## Cosas que Claude suele equivocar (¡lee esto!)

Errores frecuentes detectados en este proyecto:

1. **Poner lógica en el lugar equivocado**. La lógica de negocio va en `LD.Application` (handlers). No en controllers, no en services del cliente, no en el repositorio. El controller solo despacha al `Mediator`.
2. **Duplicar código reutilizable**. Antes de crear un helper/método/mapper nuevo, busca en `LD.Application/Common/` y en `LD.Infrastructure/Services/`. Si algo se repite en dos features, súbelo a `Common`.
3. **Inventar servicios o endpoints que no existen**. Antes de llamar `await _algoService.X(...)`, verifica que exista en `LD.Client/Services/` y que tenga su contraparte en algún controller.
4. **No respetar el patrón CQRS**. Una operación = un Command/Query. No metas dos operaciones en un mismo handler.
5. **Configurar entidades fuera del DbContext**. Las configuraciones de EF van en `LdProyectDbContext` (`OnModelCreating`), **no** en clases `IEntityTypeConfiguration` separadas. Mantén esa convención.
6. **Crear permisos sueltos**. Cualquier nuevo permiso primero va a `PermissionKeys.cs` y luego se siembra en BD.
7. **Olvidar registrar el service nuevo de `LD.Client`** en `ConfigureServices.cs` del propio `LD.Client`.
8. **Tocar `Program.cs`, configuración de JWT, o el pipeline de middlewares sin pedirlo**. Estos archivos están estables, requieren confirmación antes de modificar.

## Archivos sensibles (preguntar antes de modificar)

- `src/LD.Api/Program.cs`
- `src/LD.Infrastructure/Persistence/LdProyectDbContext.cs` (configuración de modelo, sí; cambios estructurales, preguntar)
- Configuración de JWT, Identity, Serilog
- `AuditableEntitySaveChangesInterceptor` y otros interceptores
- `PermissionHandler` / `PermissionService`

## Configuración clave

**Base de datos** (`src/LD.Api/appsettings.json`):
```
Server=localhost\SQLEXPRESS;Database=Warehouse_System;Trusted_Connection=True;
```
Las migraciones se aplican automáticamente al arrancar la API (`app.MigrateDatabase()`).

**JWT**: issuer `LdProyectAPI`, audience `LdProyectClient`, clave simétrica en `appsettings.json → JwtSettings`.

**Logging**: Serilog → `logs/` con archivos JSON rolling. Configurado en `Program.cs` antes del host build.

## Conceptos de dominio

- **ASN (Advanced Shipping Notice)**: declaraciones de embarque entrantes con líneas de detalle (`AsnDetail`) y registros de recepción (`AsnReceiptDetail`).
- **Jerarquía de ubicaciones**: `Warehouse → Module → PickingZone → Location`.
- **Scanning configurable**: `ScanConfiguration`, `ScanType`, `ScanSaveType`, `SystemField` permiten configurar flujos de escaneo por tipo de operación. Configurable desde WPF, consumido desde MAUI.
- **UserWarehouse**: many-to-many que limita a qué almacenes puede operar cada usuario.
- **Auditoría automática**: cualquier entidad que herede del base auditable obtiene `CreatedAt`/`UpdatedAt` vía interceptor — no llenes esos campos manualmente.

## Estado del refresh token (bug activo — 2026-05-22)

**Síntoma**: La app MAUI recibe 401 en múltiples endpoints (`/api/Lookup/warehouse/user/{id}`, `/api/OperationalTask`, `/api/security/tasks`) y los 401 se acumulan sin que ocurra ningún intento de renovación automática. El log del API muestra `DenyAnonymousAuthorizationRequirement: Requires an authenticated user` + `AuthenticationScheme: Bearer was challenged`.

**Estado del diagnóstico**: pendiente. No se sabe aún si:
- El 401 es por token expirado o token nunca válido.
- El cliente dispara (o no) el refresh.
- El backend tiene o no endpoint de refresh.

**Regla mientras no se resuelva**: no implementes ni modifiques ningún flujo de auth/refresh sin que el usuario lo pida explícitamente y haya dado su OK al diagnóstico previo.

## WPF (LD.FormsX): Reglas de calidad de código

Estas reglas aplican a **cualquier** vista o ViewModel de `LD.FormsX` que se lea o modifique. Se aplican en la misma edición; no se posponen.

### Regla 1 — Code-behind → ViewModel

Todo manejo de eventos que contenga lógica (llamadas a servicios, guards de nulidad, actualización de estado, navegación post-acción) debe vivir en el ViewModel usando `[RelayCommand]` y `[ObservableProperty]` de CommunityToolkit.Mvvm.

**Permitido en code-behind**: `InitializeComponent()`, drag con `DragMove()`, el puente de `PasswordChanged` (limitación de WPF), `Loaded`/`Window_Loaded` que solo llaman un método de VM, y apertura de diálogos cuando se necesite `Window.GetWindow(this)` para establecer `Owner`.

**Prohibido en code-behind**: guards de nulidad (`if (ViewModel.X is null) return;`), refreshes condicionales, llamadas a servicios, manipulación de estado visible.

Las condiciones habilitadoras deben expresarse como `CanExecute` en el comando (o como propiedad computed en el VM) y bindearse en XAML: `IsEnabled="{Binding CanExecuteEdit}"`.

### Regla 2 — Controles de input estandarizados

En formularios WPF nunca usar `TextBox` o `ComboBox` en bruto. Usar los controles custom:
- `<controls:LDInput>` en lugar de `TextBox` (incluyendo campos de búsqueda).
- `<controls:LDSelector>` en lugar de `ComboBox`.

Namespace: `xmlns:controls="clr-namespace:LD.FormsX.Controls"`.

`LDInput` expone `TextBoxElement` (la `TextBox` interna) para los casos que requieren la referencia directa (ej. `WpfGridFilter`).

Al leer o editar una vista existente, migrar los controles raw que se encuentren.

### Regla 3 — Sin colores hex inline

Ningún color puede ser un literal hex (`#233167`, `#E5E7EB`, etc.) directamente en una propiedad XAML. Todo color debe estar definido como recurso en `src/LDForms/Resources/Styles/AppTheme.xaml` y referenciado con `{StaticResource NombreToken}`.

Al encontrar un hex inline, moverlo al diccionario primero, luego referenciar el token.

Los accent colors de los tiles del Dashboard son tokens semánticos (`IconClientes`, `IconAlmacen`, etc.) — **no** un único color genérico.

### Regla 4 — Escaneo obligatorio

Cada vez que se lee o edita una vista, aplicar las Reglas 1, 2 y 3 en esa misma edición.

## Checklist mental antes de entregar un cambio

- [ ] ¿La lógica de negocio quedó en un Command/Query handler?
- [ ] ¿El controller solo despacha al Mediator?
- [ ] ¿Hay validator si el Request acepta input del usuario?
- [ ] ¿Hay profile de AutoMapper si convierto entre entity y DTO?
- [ ] ¿El permiso usado existe en `PermissionKeys`?
- [ ] ¿Si agregué service en `LD.Client`, lo registré en su `ConfigureServices`?
- [ ] ¿Los Request/Response están en `LD.Contracts`, no duplicados en clientes?
- [ ] ¿La migración tiene un nombre descriptivo del feature?
- [ ] ¿Respeté la convención de naming de archivos del feature folder?