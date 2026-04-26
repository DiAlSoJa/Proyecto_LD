# RESUMEN_CHECKLIST_HARDENING

Documentación técnica del hardening y mejoras aplicadas sobre el feature Checklist en la rama `feature/patioPendientes`.

---

## 1. Resumen ejecutivo

Se aplicaron 4 mejoras sobre el feature Checklist: (1) hardening de seguridad en los endpoints de fotos, (2) endpoint `assigned-to-me` para eliminar la descarga masiva de equipos en login, (3) compresión de imágenes en MAUI antes de subir (~80-95 % de reducción de peso), y (4) renderizado dinámico de marcas X en WPF usando `ActualWidth`/`ActualHeight` en lugar de valores fijos. El sistema de permisos se analizó y se mantiene como aliases globales (decisión documentada en la sección 6). No se introdujeron cambios destructivos ni migración de BD.

---

## 2. Seguridad

### 2.1 Decisión sobre autenticación del endpoint de fotos

**Opción A implementada**: se eliminó `[AllowAnonymous]` del endpoint `GET /api/Checklist/photo` y se reemplazó con `[Permission(PermissionKeys.Checklist_ViewSummary)]`. Razón: los clientes HTTP (`ApiService`) ya adjuntan el JWT en cada request, por lo que no requirió cambios en WPF ni MAUI. El endpoint de imagen de equipo (`EquipmentController.GetImage`) sigue siendo anónimo — ese es un cambio independiente a evaluar en una iteración futura.

### 2.2 Validaciones de path traversal en `GetPhoto`

Helper privado `EsRutaSegura(string relativePath, out string rutaCompleta)`:

| Regla | Implementación |
|---|---|
| Sin `..` | `relativePath.Contains("..")` → false |
| Sin carácter nulo | `relativePath.Contains('\0')` → false |
| No ruta absoluta | `Path.IsPathRooted(relativePath)` → false |
| Extensión permitida | Whitelist: `.jpg`, `.jpeg`, `.png`, `.webp` |
| Ruta canónica acotada | `Path.GetFullPath` del candidato debe comenzar con `baseCanonica + PathSeparator` |
| Solo nombre de archivo | Se usa `Path.GetFileName` — descarta cualquier subdirectorio incluido en el input |

Respuesta en caso de fallo: `404 NotFound` (no `400` — evita dar información al atacante).

### 2.3 Validaciones de upload (`POST /api/Checklist/upload-photo`)

| Validación | Regla |
|---|---|
| Archivo vacío | `file.Length == 0` → `400 BadRequest` |
| Tamaño máximo | > 10 MB → `400 BadRequest` |
| Extensión | Whitelist `.jpg/.jpeg/.png/.webp` → `400 BadRequest` |
| `side` | Whitelist exacta `left/right/custom`; cualquier otro valor → `400 BadRequest` |

---

## 3. Endpoint `assigned-to-me`

### Antes

Login en MAUI descargaba **todos los equipos** (`GET /api/Equipment`) y filtraba en cliente por `Turno1/Turno2/Turno3`. Un payload típico de 20-100 equipos se transfería solo para obtener 0 o 1 resultado.

### Después

Nuevo endpoint `GET /api/Equipment/assigned-to-me`:

- Solo devuelve el equipo del usuario autenticado (`Data = null` si sin asignación).
- Resuelve el `UserName` desde el `UserId` del JWT vía `IApplicationUserManager.GetUserByIdAsync`.
- Compara con `Turn1`/`Turn2`/`Turn3` de la entidad (case-insensitive) — el comentario `// TODO: migrar a AssignedUserId cuando exista` documenta la deuda técnica.
- No requiere permiso adicional (todos los usuarios autenticados pueden consultar su propio equipo).
- `LoginPageViewModel.NavegaSegunEquipoAsync` usa `GetAssignedToMeAsync()` directamente. Se agregó `Debug.WriteLine` en el catch para no perder errores silenciados.

| Métrica | Antes | Después |
|---|---|---|
| Requests en login | 1 (pero con payload grande) | 1 (payload mínimo) |
| Payload aprox. (10 equipos) | ~8 KB | < 1 KB |
| Filtrado en | Cliente MAUI | Servidor |

---

## 4. Compresión de imágenes

### Librería

**SkiaSharp 2.88.x** — paquete `SkiaSharp` agregado al proyecto MAUI. Elegida por su estabilidad y soporte multiplataforma (Android/iOS/Windows) sin requerir inicialización adicional en `MauiProgram.cs`. Para SkiaSharp 3.x (que depreca `SKFilterQuality` por `SKSamplingOptions`) se requeriría actualizar `ImageCompressor.cs`.

### Helper `ImageCompressor.cs`

Ruta: `src/LD.MobileApp/Common/Imaging/ImageCompressor.cs`

Parámetros por defecto:

| Parámetro | Valor | Razón |
|---|---|---|
| `maxLado` | 1920 px | Cubre FullHD sin exceso; el ojo humano no distingue más en una pantalla de teléfono |
| `calidadJpeg` | 80 | Equilibrio calidad/peso estándar de la industria para imágenes de operaciones |
| Formato salida | JPEG | Compresión lossy óptima para fotos de cámara |

Flujo: Decode → Resize (si mayor > 1920 px) → Re-encode JPEG 80 → `byte[]`.

### Integración en `ForkliftChecklistPage.xaml.cs`

- Las fotos se comprimen **antes** de `UploadPhotoAsync`.
- Si la compresión falla (imagen corrupta, etc.), se sube el original como fallback — no se bloquea el flujo.
- Las fotos se suben **en serie** (no en paralelo) para evitar saturar la conexión móvil.
- Contador en overlay: `"Subiendo foto N de M..."` y `"Guardando checklist..."`.

### Comparativa de peso (estimada)

| Escenario | Antes | Después | Reducción |
|---|---|---|---|
| Foto típica de cámara Android (12 MP) | ~4-8 MB | ~250-450 KB | ~90-95 % |
| Foto moderada (8 MP) | ~2-4 MB | ~150-300 KB | ~85-93 % |
| Tiempo de compresión (dispositivo mid-range) | N/A | ~150-400 ms | — |

> Nota: La comparativa es estimada — el sanity check con foto real está pendiente de ejecución en dispositivo físico (ver sección 7).

---

## 5. Marcas dinámicas en canvas WPF

### Antes

`RenderizarMarcas` usaba `canvas.Width` y `canvas.Height` (valores explícitos del XAML = 193×331 px fijos). Si alguien modificaba el XAML, las marcas se descolocaban sin error visible.

### Después

- Usa `canvas.ActualWidth` / `canvas.ActualHeight` — valores reales post-layout.
- Guarda `_ultimasMarcas` como campo de instancia.
- Suscribe `SizeChanged` de ambos canvas en el constructor → redibuja automáticamente al cambiar el tamaño de la ventana.
- Guard: si `ActualWidth <= 0 || ActualHeight <= 0` (canvas no medido aún) se salta la marca; `SizeChanged` la redibujará cuando el canvas esté listo.
- La lógica es idéntica en resultado con el XAML actual (193×331); el cambio es futuro-proof.

---

## 6. Permisos — decisión de granularidad

**Decisión: mantener permisos globales como aliases.**

Análisis del sistema: todos los features existentes usan permisos globales por módulo (no hay permisos por tipo de equipo, por almacén individual, ni por categoría de producto). Los permisos de checklist reutilizan `forklift-checklist.execute` y `forklift-checklist.read`, que son los correctos según la BD.

Crear `checklist.submit` y `checklist.view-summary` como permisos separados requeriría:
1. Migración de BD para insertar los nuevos registros en la tabla `Permissions`.
2. Actualizar roles existentes.
3. Riesgo de romper la autorización para usuarios que ya tienen `forklift-checklist.*`.

No hay presión de negocio para granularidad por tipo de equipo en este momento. Los aliases actuales en `PermissionKeys.cs` son la solución correcta.

---

## 7. Resultado del sanity check (pendiente de ejecución en dispositivo)

El sanity check end-to-end (sección 6 del plan) requiere dispositivo físico Android y acceso a la red local con el servidor corriendo. Está pendiente de ejecutar manualmente. Los puntos de verificación son:

| Paso | Estado |
|---|---|
| Login con usuario asignado → endpoint `assigned-to-me` responde correctamente | ⏳ Pendiente |
| Navega directo al checklist (no al dashboard) | ⏳ Pendiente |
| Fotos comprimen de MB a KB antes de subir | ⏳ Pendiente |
| Submit guarda el checklist | ⏳ Pendiente |
| WPF muestra el checklist en Resumen | ⏳ Pendiente |
| Fotos cargan vía endpoint autenticado | ⏳ Pendiente |
| Marcas X posicionadas correctamente en ambos lados | ⏳ Pendiente |
| Redimensionar ventana WPF reposiciona las X | ⏳ Pendiente |
| Checklist de batería aparece en ResumenBateriasTab | ⏳ Pendiente |

---

## 8. Archivos creados/modificados

### `LD.Api`
- `src/LD.Api/Controllers/ChecklistController.cs` — hardening completo (path traversal, auth, upload validation)
- `src/LD.Api/Controllers/EquipmentController.cs` — endpoint `GET /api/equipment/assigned-to-me`

### `LD.Application`
- `src/LD.Application/Features/Equipment/Queries/GetAssignedEquipmentQuery.cs` — nuevo query handler

### `LD.Client`
- `src/LD.Client/Configuration/ApiEndpoints.cs` — `Equipment_AssignedToMe`
- `src/LD.Client/Services/EquipmentService.cs` — `GetAssignedToMeAsync()`
- `src/LD.Client/Services/ChecklistService.cs` — `GetPhotoBytesAsync()` (para uso futuro en WPF)

### `LD.MobileApp`
- `src/LD.MobileApp/LD.MobileApp.csproj` — `SkiaSharp 2.88.*`
- `src/LD.MobileApp/Common/Imaging/ImageCompressor.cs` — nuevo helper de compresión
- `src/LD.MobileApp/Features/Auth/ViewModels/LoginPageViewModel.cs` — refactor `NavegaSegunEquipoAsync`
- `src/LD.MobileApp/Features/Checklist/ViewModels/ForkliftChecklistViewModel.cs` — `IsSaving`, `StatusSubida`
- `src/LD.MobileApp/Features/Checklist/Views/ForkliftChecklistPage.xaml` — overlay de carga
- `src/LD.MobileApp/Features/Checklist/Views/ForkliftChecklistPage.xaml.cs` — compresión + upload en serie

### `LD.FormsX`
- `src/LDForms/Features/CheckList/Views/Tabs/ResumenTab.xaml.cs` — marcas dinámicas con `ActualWidth`/`ActualHeight` + `SizeChanged`

---

## 9. Pendientes para futuras iteraciones

| Ítem | Prioridad | Notas |
|---|---|---|
| Modo offline con cola de checklists | Alta | Guardar en SQLite local y sincronizar al recuperar red |
| Tests automatizados de integración | Media | Especialmente para `GetAssignedEquipmentQuery` y validaciones de path |
| Exportar resumen a Excel/PDF en WPF | Media | Implementar `BtnVerImagenes_Click` (galería de fotos) |
| Notificaciones de defectos críticos | Media | Push notification si `IsOk = false` en preguntas críticas |
| Migrar Turn1/Turn2/Turn3 a campo `AssignedUserId` | Baja | Requiere migración de BD y UI de asignación |
| Endpoint de fotos en `EquipmentController.GetImage` | Baja | También anónimo; evaluar si requiere autenticación |
| SkiaSharp 3.x | Baja | Actualizar `ImageCompressor` a `SKSamplingOptions` cuando se migre |
