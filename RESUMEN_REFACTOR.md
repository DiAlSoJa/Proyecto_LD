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

| Área | Descripción |
|------|-------------|
| `EquipmentQuestionController` | Inyecta `DbContext` directamente y hereda `ControllerBase` en vez de `CommonController`. Viola la arquitectura pero funciona. |
| `GetAssignedEquipmentQuery` | Carga todos los equipos en memoria para filtrar. Escala mal en catálogos grandes. |
| `EquipmentType_View` permission | Apunta a `"units.read"` como alias temporal (comentado en `PermissionKeys.cs`). |

---

## Convenciones a mantener

- MVVM con `CommunityToolkit.Mvvm` (`[ObservableProperty]`, `[RelayCommand]`)
- Lógica de negocio en `LD.Application` handlers (CQRS con MediatR), no en controllers ni en clientes
- Nuevos contratos (Request/Response/DTO) solo en `LD.Contracts`
- Nuevos servicios HTTP en `LD.Client/Services/` y registrados en `LD.Client/ConfigureServices.cs`
- Permisos nuevos primero en `LD.Contracts/Constants/PermissionKeys.cs`, luego sembrados en BD
- Migraciones EF con nombre descriptivo del feature
