# Resumen del Refactor – Checklist Feature

Documento autónomo para Claude Opus. Describe los cambios realizados en el feature de Checklist de Montacargas en los proyectos WPF (`LD.FormsX`) y MAUI (`LD.MobileApp`).

---

## Contexto del proyecto

**Proyecto_LD** es un WMS (warehouse management system) multi-cliente. Una API .NET 9/10 sirve a tres frontends: app móvil MAUI, escritorio WPF (`LD.FormsX`), y un WinForms heredado (`LD.Forms`, sin prioridad).

- API: `src/LD.Api` (puerto 8050)
- WPF: `src/LDForms/LD.FormsX.csproj`
- MAUI: `src/LD.MobileApp/LD.MobileApp.csproj` (RootNamespace: `MauiAppLogin`)
- Contratos compartidos: `src/LD.Contracts/`
- Clientes HTTP tipados: `src/LD.Client/`

---

## WPF – Separación en UserControls por tab

### Antes

`ChecListView.xaml` era una vista monolítica con 4 `TabItem` con todo el código XAML y C# inline en un solo code-behind (`ChecListView.xaml.cs`) de ~500 líneas.

### Después

La vista fue separada en una arquitectura de tabs con UserControls independientes:

```
src/LDForms/Features/CheckList/
├── Views/
│   ├── ChecListView.xaml               ← ahora solo contiene 4 ContentControl hosts
│   ├── ChecListView.xaml.cs            ← inyecta tabs vía IServiceProvider
│   ├── ChecListView.xaml.cs.bak        ← backup del original
│   ├── NuevoEquipoCheckListView.xaml   ← dialog de crear/editar equipo (ampliado)
│   ├── NuevoEquipoCheckListView.xaml.cs
│   ├── AsignarUsuarioEquipoView.xaml.cs
│   └── Tabs/
│       ├── EquiposTab.xaml             ← Tab 1: listado y CRUD de equipos
│       ├── EquiposTab.xaml.cs
│       ├── ConfiguracionPreguntasTab.xaml  ← Tab 2: CRUD de preguntas por tipo
│       ├── ConfiguracionPreguntasTab.xaml.cs
│       ├── ResumenTab.xaml             ← Tab 3: stub (sin endpoint aún)
│       ├── ResumenTab.xaml.cs
│       ├── ResumenBateriasTab.xaml     ← Tab 4: stub (sin endpoint aún)
│       └── ResumenBateriasTab.xaml.cs
└── ViewModels/
    ├── ChecklistViewModel.cs           ← orquestador con referencias a los 4 sub-VMs
    ├── EquiposTabViewModel.cs
    ├── ConfiguracionPreguntasTabViewModel.cs
    ├── ResumenTabViewModel.cs          ← stub
    └── ResumenBateriasTabViewModel.cs  ← stub
```

### Patrón de inyección de tabs

`ChecListView.xaml.cs` instancia cada tab a través del contenedor DI:

```csharp
EquiposTabHost.Content    = serviceProvider.GetRequiredService<EquiposTab>();
ConfigPregTabHost.Content = serviceProvider.GetRequiredService<ConfiguracionPreguntasTab>();
ResumenTabHost.Content    = serviceProvider.GetRequiredService<ResumenTab>();
ResumenBatTabHost.Content = serviceProvider.GetRequiredService<ResumenBateriasTab>();
```

### Registros DI agregados en `App.xaml.cs`

```csharp
services.AddTransient<ChecklistViewModel>();
services.AddTransient<EquiposTabViewModel>();
services.AddTransient<ConfiguracionPreguntasTabViewModel>();
services.AddTransient<ResumenTabViewModel>();
services.AddTransient<ResumenBateriasTabViewModel>();
services.AddTransient<EquiposTab>();
services.AddTransient<ConfiguracionPreguntasTab>();
services.AddTransient<ResumenTab>();
services.AddTransient<ResumenBateriasTab>();
```

### Patrón VM-View de eventos

Los ViewModels exponen eventos `event Action<List<T>>? OnXLoaded` que los code-behind suscriben para actualizar los `DataGrid.ItemsSource` (este patrón ya existía en `CatalogoCategoriasView`).

---

## WPF – NuevoEquipoCheckListView: soporte de imágenes

Se amplió el diálogo de crear/editar equipo para admitir imágenes izquierda/derecha:

- **XAML**: Nuevas filas en la grid (Row 14 y Row 16) con `TextBox` (ruta relativa), `Button` ("Cargar imagen") y `Image` (preview).
- **Code-behind**: `_imagePathLeft`/`_imagePathRight` + `CargarImagenAsync(side, textBox, imagePreview)` que llama a `EquipmentService.UploadImage()` y guarda el `RelativePath` devuelto.
- **BuildRequest()**: Usa las rutas nuevas si se cargaron; en caso contrario conserva las del equipo existente.
- **CargarEquipoAsync()**: Al editar, pre-carga las rutas actuales en los TextBox y muestra las imágenes vía `MostrarImagenDesdeUrl()` (usa `EquipmentService.GetImageUrl(relativePath)`).

---

## WPF – Tabs Resumen y ResumenBaterias (stubs)

Ambas tabs tienen su UserControl y ViewModel pero **no implementan lógica real** porque no existe endpoint en la API para recuperar checklists enviados desde la app móvil.

Los stubs incluyen:
- DatePicker de rango y botón "Buscar" (handlers con comentario `// Pendiente`)
- `DataGrid` con columnas apropiadas (incluidas columnas de batería para `ResumenBateriasTab`)
- Panel de imágenes y observaciones para el detalle del ítem seleccionado

**Acción pendiente**: Crear endpoint en `EquipmentController` (o nuevo `ChecklistController`) que devuelva los checklists guardados, y conectar los handlers stub.

---

## MAUI – ForkliftChecklistViewModel: carga dinámica de preguntas

**Antes**: El ViewModel tenía secciones hardcodeadas.

**Después** (`src/LD.MobileApp/Features/Checklist/ViewModels/ForkliftChecklistViewModel.cs`):

```csharp
public async Task InicializarAsync(EquipmentDto equipmentData)
{
    Equipment = equipmentData;
    await CargarPreguntasAsync();
}

private async Task CargarPreguntasAsync()
{
    var response = await _equipmentQuestionService.GetByEquipmentType(Equipment.EquipmentTypeId);
    // Construye ChecklistSection con ChecklistQuestion desde la respuesta API
    // IsYesNo → opciones "Sí"/"No"; OptionAnswerText → split por '\n'
}
```

Inyección: `EquipmentQuestionService` vía DI.

---

## MAUI – ForkliftChecklistPage: imagen dinámica del equipo

**Antes**: `BindingContext` hardcodeado en XAML y sin carga de imagen real.

**Después** (`src/LD.MobileApp/Features/Checklist/Views/ForkliftChecklistPage.xaml.cs`):

- Constructor recibe `ForkliftChecklistViewModel` y `EquipmentService` por DI.
- `[QueryProperty(nameof(Equipment), "Equipment")]` recibe el `EquipmentDto` de la navegación Shell.
- `CargarImagenEquipoAsync()` descarga `EquipmentService.DownloadImage(id, "left"|"right")` y asigna a `LeftForkliftImage.Source` / `RightForkliftImage.Source`.
- Todos los handlers originales (marcas X, fotos, guardar) se conservaron intactos.

---

## MAUI – Login: detección de equipo asignado

**Antes**: Login siempre navegaba a `//dashboard`.

**Después** (`src/LD.MobileApp/Features/Auth/ViewModels/LoginPageViewModel.cs`):

Después de `UserData.SetUserData(getMeResponse.Data)`, se ejecuta `NavegaSegunEquipoAsync()`:

```csharp
private async Task NavegaSegunEquipoAsync()
{
    var equiposResponse = await _equipmentService.GetEquipments();
    var equipoAsignado = equiposResponse.Data?.FirstOrDefault(e =>
        string.Equals(e.Turno1, UserData.UserName, OrdinalIgnoreCase) ||
        string.Equals(e.Turno2, UserData.UserName, OrdinalIgnoreCase) ||
        string.Equals(e.Turno3, UserData.UserName, OrdinalIgnoreCase));

    if (equipoAsignado is not null)
        await Shell.Current.GoToAsync(nameof(ForkliftChecklistPage),
            new Dictionary<string, object> { { "Equipment", equipoAsignado } });
    else
        await Shell.Current.GoToAsync("//dashboard");
}
```

Si el servicio lanza excepción, se captura silenciosamente y se navega al dashboard.

---

## MAUI – DI y routing

- `ForkliftChecklistPage` y `ForkliftChecklistViewModel` ya estaban registrados en `MauiProgram.cs`.
- `ForkliftChecklistViewModel` se agregó explícitamente a la sección ViewModels en `MauiProgram.cs`.
- `ForkliftChecklistPage` ya estaba registrado en `AppShell.xaml.cs` con `Routing.RegisterRoute`.
- `LoginViewModel` ahora recibe `EquipmentService` como tercer parámetro de constructor.

---

## Estado de bugs (sesión 2026-05-13)

Todos los bugs de la sesión anterior fueron resueltos. A continuación el estado final:

| # | Área | Descripción | Estado |
|---|------|-------------|--------|
| 1 | MAUI checklist | `OnGuardarClicked` no enviaba al API | ✅ Resuelto |
| 2 | WPF Resumen | Tabs stub sin datos reales | ✅ Resuelto |
| 3 | WPF Resumen | No existía `ChecklistController` | ✅ Resuelto |
| 4 | MAUI foto | Fotos no se enviaban al servidor | ✅ Resuelto |
| 5 | WPF preguntas | Grid no sincronizaba botón Editar tras guardar | ✅ Resuelto |
| 6 | MAUI checklist | **Horómetro capturado no se guardaba en el checklist** | ✅ Resuelto (sesión actual) |

### Fix #6 — Horómetro (sesión 2026-05-13)

El `HorometroEntry` (con soporte OCR) capturaba el valor pero nunca lo incluía en el `SubmitChecklistRequest`. Archivos modificados:

- `LD.Contracts/Checklist/SubmitChecklistRequest.cs` → `+ public decimal? Horometro { get; set; }`
- `LD.Domain/Entities/Checklist.cs` → `+ public decimal? Horometro { get; set; }`
- `LD.Application/Features/Checklist/Commands/SubmitChecklistCommand.cs` → mapeo de `Horometro`
- `LD.Infrastructure/Repositories/ChecklistRepository.cs` → `Hourmeter = c.Horometro ?? c.Equipment?.Hourmeter` (prioriza la lectura del checklist)
- `LD.MobileApp/Features/Checklist/Views/ForkliftChecklistPage.xaml.cs` → incluye `Horometro` en el request de envío
- Migración: `AddHorometroToChecklist` (aplicada)

### Fixes de seguridad (sesión 2026-05-13)

- `EquipmentController.GetImage`: removido `[AllowAnonymous]`, agregado `[Permission(ForkliftChecklist_View)]` + validación de extensiones + verificación de ruta canónica (igual que `ChecklistController.EsRutaSegura`)
- `EquipmentController.UploadImage`: agregado `[Permission(ForkliftChecklist_Create)]`

---

## Sprint 3 — Lógica 24h y Custom Dialogs (2026-05-14)

### 1. Nuevo endpoint `GET /api/checklist/daily-status`

| Campo | Valor |
|---|---|
| Ruta | `GET /api/checklist/daily-status` |
| Permiso | `PermissionKeys.ForkliftChecklist_Execute` (`forklift-checklist.execute`) |
| Toma `userId` de | JWT token (`CurrentUserId` en `CommonController`) |

**Respuesta**:
```json
{
  "hasAssignedEquipment": true,
  "hasCompletedToday": false,
  "lastChecklistAt": "2026-05-14T08:00:00Z",
  "equipment": { ... }
}
```

**Archivos modificados/creados**:
- `LD.Contracts/Checklist/ChecklistDailyStatusDto.cs` ← DTO de respuesta (nuevo)
- `LD.Application/Features/Checklist/Queries/GetChecklistDailyStatusQuery.cs` ← Query handler (nuevo)
- `LD.Application/Common/Interfaces/Repository/IChecklistRepository.cs` ← + `GetDailyStatusAsync`
- `LD.Infrastructure/Repositories/ChecklistRepository.cs` ← implementación (busca checklist de las últimas 24h por userId + equipmentId)
- `LD.Api/Controllers/ChecklistController.cs` ← endpoint `[HttpGet("daily-status")]`
- `LD.Client/Configuration/ApiEndpoints.cs` ← `Checklist_DailyStatus`
- `LD.Client/Services/ChecklistService.cs` ← `GetDailyStatusAsync()`

**Nota**: No se requirió migración. `CreatedAt` ya existe en `AuditableEntity` y se llena con UTC por el interceptor.

---

### 2. Componentes nuevos en MAUI

#### `src/LD.MobileApp/Controls/`

| Archivo | Descripción |
|---|---|
| `DialogType.cs` | Enum: Info / Warning / Error / Blocking |
| `CustomDialog.xaml` + `.xaml.cs` | `Popup` de CommunityToolkit.Maui; ícono, título, mensaje y botones configurables según `DialogType` |
| `DialogService.cs` | `IDialogService` + implementación; registro en DI como Singleton |

**Modos de `DialogType`**:
- `Info` → ícono ℹ️, botón "Entendido", cierra con `true`
- `Warning` → ícono ⚠️, botones "Cancelar" (false) / "Registrar otro" (true)
- `Error` → ícono ❌, botón "Cerrar", cierra con `false`
- `Blocking` → ícono 🔒, sin botones; solo se cierra programáticamente con `HideBlocking()`

**Registro en `MauiProgram.cs`**: `builder.Services.AddSingleton<IDialogService, DialogService>()`

---

### 3. Cambios en `LoginPageViewModel`

- `_equipmentService` eliminado del constructor; reemplazado por `_checklistService: ChecklistService`.
- Método `NavegaSegunEquipoAsync()` reescrito con nueva lógica de 3 ramas:
  1. `HasAssignedEquipment == false` → `GoToAsync(nameof(NoEquipmentPage))`
  2. `HasAssignedEquipment == true && HasCompletedToday == false` → `GoToAsync(nameof(ForkliftChecklistPage), { Equipment, IsMandatory=true })`
  3. `HasAssignedEquipment == true && HasCompletedToday == true` → `GoToAsync("//dashboard")` + guarda `ChecklistCompletedAt` en `Preferences`

---

### 4. Cambios en `ForkliftChecklistPage.xaml.cs`

- Nuevo `QueryProperty`: `[QueryProperty(nameof(IsMandatory), "IsMandatory")]`
- Constructor ampliado: `+ ChecklistService checklistService, IDialogService dialogService`
- `InicializarConEquipoAsync` → si `!_isMandatory`, llama a `VerificarChecklistDiarioAsync()`
- `VerificarChecklistDiarioAsync()` → llama a `GetDailyStatusAsync()`; si ya completó hoy muestra `DialogType.Warning` y navega `.." si el usuario cancela
- `OnBackButtonPressed()` → si `_isMandatory == true` retorna `true` (bloquea back) y muestra `DialogType.Info`
- Cuando el checklist se guarda exitosamente: `_isMandatory = false` antes de navegar

---

### 5. Estado de requisitos del Sprint 3

| Requisito | Estado |
|---|---|
| Endpoint `daily-status` devuelve equipo + flag de 24h | ✅ |
| `CreatedAt` se guarda en UTC al enviar checklist | ✅ (ya existía vía interceptor) |
| `CustomDialog.xaml` con 4 modos | ✅ |
| `IDialogService` registrado en DI | ✅ |
| Login navega según estado diario | ✅ |
| Checklist obligatorio bloquea botón Back | ✅ |
| Acceso manual muestra advertencia si ya completó hoy | ✅ |
| WPF Resumen: Fecha incluye hora | ✅ (`FechaDisplay` → `ToLocalTime().ToString("dd/MM/yyyy HH:mm")`) |
| WPF Resumen: Fotos visibles en grid de detalle (path/side) | ✅ |
| WPF Resumen: Botón "Ver imágenes" descarga y abre fotos | ✅ (usa temp dir + Process.Start) |
| WPF ResumenBaterias: Fecha incluye hora | ✅ |
| WPF ResumenBaterias: Filtro IsBattery sigue funcionando | ✅ (sin cambios en la lógica, el `Hourmeter` se muestra por herencia de `ChecklistSummaryDto`) |

---

## Endpoints API — estado actual

| Endpoint | Método | Permiso | Descripción |
|----------|--------|---------|-------------|
| `/api/equipment` | GET | ForkliftChecklist_View | Lista equipos |
| `/api/equipment/{id}` | GET | ForkliftChecklist_View | Detalle de equipo (devuelve EquipmentRequest) |
| `/api/equipment` | POST | ForkliftChecklist_Create | Crear equipo |
| `/api/equipment/{id}` | PUT | ForkliftChecklist_Update | Actualizar equipo (incluye Turn1/Turn2/Turn3 para asignación) |
| `/api/equipment/upload-image` | POST | ForkliftChecklist_Create | Sube imagen del equipo |
| `/api/equipment/image` | GET | ForkliftChecklist_View | Obtiene imagen por path relativo |
| `/api/equipment/{id}/image/{side}` | GET | ForkliftChecklist_View | Descarga imagen (side: left\|right) |
| `/api/equipment/assigned-to-me` | GET | solo [Authorize] | Equipo asignado al usuario logueado |
| `/api/equipmenttype` | GET | EquipmentType_View | Lista tipos de equipo |
| `/api/equipmentquestion/equipment-type/{typeId}` | GET | EquipmentType_View | Preguntas por tipo |
| `/api/equipmentquestion` | POST | EquipmentType_Create | Crear o actualizar pregunta (DetId=0 → crear) |
| `/api/equipmentquestion/{id}` | DELETE | EquipmentType_Update | Eliminar pregunta |
| `/api/checklist` | POST | Checklist_Submit | Enviar checklist completo |
| `/api/checklist/daily-status` | GET | ForkliftChecklist_Execute | Estado 24h: equipo asignado + último checklist |
| `/api/checklist` | GET | Checklist_ViewSummary | Listar checklists con filtros |
| `/api/checklist/{id}` | GET | Checklist_ViewSummary | Detalle de un checklist |
| `/api/checklist/upload-photo` | POST | Checklist_Submit | Subir foto del checklist |
| `/api/checklist/photo` | GET | Checklist_ViewSummary | Obtener foto por path relativo |

## Flujo de asignación de equipo a usuario

La asignación usa los campos `Turn1`/`Turn2`/`Turn3` de la entidad `Equipment` (strings con el `UserName`). No hay tabla separada.

- **WPF asigna**: `EquiposTab → AsignarUsuarioEquipoView` → GET equipo actual → actualiza Turn → PUT `/api/equipment/{id}`
- **MAUI lee**: `GET /api/equipment/assigned-to-me` → `GetAssignedEquipmentQuery` filtra `Turn1/2/3 == username`
- **TODO en código**: `GetAssignedEquipmentQuery.cs:47` — migrar a campo `AssignedUserId` cuando se refactorice la entidad (carga todos los equipos en memoria actualmente)

## Deuda técnica conocida (sin urgencia)

| # | Descripción | Archivo | Impacto | Prioridad |
|---|-------------|---------|---------|-----------|
| 1 | `EquipmentQuestionController` inyecta `DbContext` directamente y hereda `ControllerBase` en vez de `CommonController`. Viola la arquitectura pero funciona. | `EquipmentQuestionController.cs` | Bajo | Media |
| 2 | `GetAssignedEquipmentQuery` carga todos los equipos en memoria para filtrar por Turn1/2/3. | `GetAssignedEquipmentQuery.cs:47` | Bajo ahora, escala mal con >500 equipos | Baja |
| 3 | `EquipmentType_View` permission apunta a `"units.read"` como alias temporal. | `PermissionKeys.cs` | Bajo | Baja |
| 4 | `ShowBlocking` en `DialogService` usa `BeginInvokeOnMainThread(async () => await ShowPopupAsync(...))` — fire-and-forget sin captura de excepción. Si `GetCurrentPage()` devuelve null, la excepción se pierde silenciosamente. | `DialogService.cs:46` | Bajo — solo cuando page es null | Media |
| 5 | `GetChecklistDailyStatusQuery` carga todos los equipos en memoria para encontrar el equipo del usuario. Mismo patrón que `GetAssignedEquipmentQuery`. | `GetChecklistDailyStatusQuery.cs:52` | Bajo ahora, escala mal con >500 equipos | Baja |

---

---

## Sprint 3 — Hotfix flujo login (2026-05-14)

### Descripción

El flujo anterior redirigía a `NoEquipmentPage` (o a `ForkliftChecklistPage` en modo obligatorio) directamente desde el login, mezclando autenticación con lógica de negocio del checklist. El hotfix separa ambas responsabilidades.

### Flujo anterior

```
POST /api/auth/login
  → GetMeAsync
  → GET /api/checklist/daily-status
      → sin equipo:        NavegaSegunEquipoAsync → NoEquipmentPage
      → sin checklist hoy: NavegaSegunEquipoAsync → ForkliftChecklistPage (IsMandatory=true)
      → con checklist hoy: NavegaSegunEquipoAsync → //dashboard
```

### Flujo nuevo

```
POST /api/auth/login
  → GetMeAsync
  → //dashboard  ← siempre

Botón "Checklist de Montacargas" en el dashboard
  → GET /api/checklist/daily-status
      → sin equipo:        ShowInfoAsync + return (sin navegar)
      → con checklist hoy: ShowWarningAsync → cancela (return) o IsMandatory=false
      → sin checklist hoy: ForkliftChecklistPage (IsMandatory=true)
```

### Archivos modificados

| Archivo | Cambio |
|---|---|
| `src/LD.MobileApp/Features/Auth/ViewModels/LoginPageViewModel.cs` | `NavegaSegunEquipoAsync()` comentada; login siempre navega a `//dashboard` |
| `src/LD.MobileApp/Features/Dashboard/ViewModels/DashboardViewModel.cs` | `NavigateToChecklist()` reemplazado con verificación de daily-status + diálogos; inyecta `ChecklistService` e `IDialogService` |
| `src/LD.MobileApp/Features/Checklist/Views/ForkliftChecklistPage.xaml.cs` | `VerificarChecklistDiarioAsync()` comentada (duplicado eliminado); llamada removida de `InicializarConEquipoAsync` |

### Nota sobre NoEquipmentPage

`NoEquipmentPage` ya no se navega desde el login. La página permanece en el proyecto pero no tiene punto de entrada activo. Candidata a eliminar si en próximos sprints se confirma que tampoco se usará desde otro flujo.

---

## Convenciones a mantener

- MVVM con `CommunityToolkit.Mvvm` (`[ObservableProperty]`, `[RelayCommand]`)
- Lógica de negocio en `LD.Application` handlers (CQRS con MediatR), no en controllers ni en clientes
- Nuevos contratos (Request/Response/DTO) solo en `LD.Contracts`
- Nuevos servicios HTTP en `LD.Client/Services/` y registrados en `LD.Client/ConfigureServices.cs`
- Permisos nuevos primero en `LD.Contracts/Constants/PermissionKeys.cs`, luego sembrados en BD
- Migraciones EF con nombre descriptivo del feature
