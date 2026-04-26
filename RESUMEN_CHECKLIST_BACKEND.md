# RESUMEN_CHECKLIST_BACKEND

Documentación técnica del feature de Checklist implementado en la rama `feature/patioPendientes`.

---

## 1. Endpoints REST

| Verbo | Ruta | Permiso | Descripción |
|---|---|---|---|
| `POST` | `/api/Checklist` | `forklift-checklist.execute` | Guarda un checklist completo (respuestas, marcas, fotos) |
| `GET` | `/api/Checklist` | `forklift-checklist.read` | Lista checklists con filtros: `from`, `to`, `equipmentTypeId`, `equipmentId` |
| `GET` | `/api/Checklist/{checklistId}` | `forklift-checklist.read` | Detalle completo de un checklist (respuestas, marcas, fotos con URL) |
| `POST` | `/api/Checklist/upload-photo` | `forklift-checklist.execute` | Sube foto; devuelve `relativePath` e `imageUrl` |
| `GET` | `/api/Checklist/photo?path={path}` | Anónimo | Sirve físicamente la foto por su `relativePath` |

---

## 2. Entidades de dominio

**`Checklist`** — cabecera del formulario
- `ChecklistId` (PK), `EquipmentId` (FK → Equipment), `EquipmentTypeId` (snapshot)
- `UserId`, `UserName`, `Turno`, `Observaciones`
- Hereda `AuditableEntity` (`CreatedAt`, `UpdatedAt`, `IsActive`, etc.)

**`ChecklistAnswer`** — respuesta por pregunta
- `ChecklistAnswerId`, `ChecklistId` (FK cascade), `QuestionId`
- `QuestionTextSnapshot`, `AnswerText`, `IsOk` (`true`=Sí/OK, `false`=No/Defecto, `null`=N/A)

**`ChecklistPhoto`** — foto adjunta
- `ChecklistPhotoId`, `ChecklistId` (FK cascade)
- `RelativePath` (ruta física relativa), `Side` ("left"|"right"|"custom"), `Order`

**`ChecklistDefectMark`** — marca de defecto sobre la imagen del equipo
- `ChecklistDefectMarkId`, `ChecklistId` (FK cascade)
- `Side` ("left"|"right"), `XPercent` y `YPercent` (decimal 0..1), `Note`

---

## 3. Migración EF Core

Nombre: `Add_Checklist_Feature`
Archivo: `src/LD.Infrastructure/Migrations/20260426071133_Add_Checklist_Feature.cs`

Tablas creadas:
- `Checklists`
- `ChecklistAnswers` (CASCADE desde Checklists)
- `ChecklistPhotos` (CASCADE desde Checklists)
- `ChecklistDefectMarks` (CASCADE desde Checklists)

FK: `Checklists.EquipmentId → Equipment` con `DeleteBehavior.Restrict`

---

## 4. Permisos

No se crearon permisos nuevos en BD. Se usaron alias en `PermissionKeys.cs`:

```csharp
public const string Checklist_Submit      = ForkliftChecklist_Execute; // "forklift-checklist.execute"
public const string Checklist_ViewSummary = ForkliftChecklist_View;    // "forklift-checklist.read"
```

---

## 5. Contratos (`LD.Contracts/Checklist/`)

| Archivo | Uso |
|---|---|
| `ChecklistAnswerDto.cs` | Respuesta individual (request y response) |
| `ChecklistDefectMarkDto.cs` | Marca de defecto (request y response); coordenadas siempre en 0..1 |
| `ChecklistPhotoDto.cs` | Foto (request: solo `RelativePath`; response: incluye `PhotoUrl`) |
| `SubmitChecklistRequest.cs` | Body del `POST /api/Checklist` |
| `SubmitChecklistResponse.cs` | Respuesta del submit: `ChecklistId` + `CreatedAt` |
| `GetChecklistsQueryRequest.cs` | Filtros del `GET /api/Checklist` |
| `ChecklistSummaryDto.cs` | Fila del grid WPF; incluye computed `FechaDisplay` y `OperativoDisplay` |
| `ChecklistDetailDto.cs` | Detalle completo (hereda resumen + listas de Answers, DefectMarks, Photos) |

---

## 6. Cliente HTTP (`LD.Client`)

**`ChecklistService`** (`src/LD.Client/Services/ChecklistService.cs`):
- `SubmitAsync(SubmitChecklistRequest)` → `POST /api/Checklist`
- `GetChecklistsAsync(GetChecklistsQueryRequest)` → `GET /api/Checklist?...`
- `GetByIdAsync(int checklistId)` → `GET /api/Checklist/{id}`
- `UploadPhotoAsync(byte[], fileName, side)` → `POST /api/Checklist/upload-photo`
- `GetPhotoUrl(string relativePath)` → URL pública para `GET /api/Checklist/photo?path=...`

Endpoints definidos en `ApiEndpoints.cs` bajo la sección `// CHECKLIST`.
Registrado en `LD.Client/ConfigureServices.cs` como `AddScoped<ChecklistService>()`.

---

## 7. Integración en clientes

### MAUI (`LD.MobileApp`)
- **`ForkliftChecklistViewModel`**: inyecta `ChecklistService`; expone `UploadPhotoAsync` y `SubmitAsync`.
- **`ForkliftChecklistPage.xaml.cs`**: captura fotos como `byte[]`, sube antes de enviar, convierte marcas de píxeles a porcentaje 0..1 con `ExtractMarks()`, llama `SubmitAsync`, navega a `//dashboard` en éxito.

### WPF (`LD.FormsX`)
- **`ResumenTabViewModel`**: inyecta `ChecklistService`; `BuscarAsync` + `CargarDetalleAsync`; eventos `OnChecklistsLoaded` y `OnChecklistDetailLoaded`.
- **`ResumenBateriasTabViewModel`**: igual pero filtra por tipos de equipo con `IsBattery == true` (resueltos dinámicamente con `EquipmentTypeService`).
- **`ResumenTab.xaml.cs`**: renderiza marcas X en `Canvas` (`canvasIzq`, `canvasDer`) usando coordenadas percent × dimensiones del canvas (193×331 px).
- **`ResumenBateriasTab.xaml.cs`**: misma lógica sin canvas de marcas.
- **Bug #5 fix** (`ConfiguracionPreguntasTabViewModel`): `GuardarPreguntaAsync` ahora llama internamente a `CargarPreguntasAsync` y resetea `SelectedQuestion` tras guardar, garantizando que el grid se recargue siempre.
