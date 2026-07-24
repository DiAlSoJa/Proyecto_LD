using LD.Contracts.Constants;
using LD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LD.Infrastructure.Persistence.Seeders
{
    /// <summary>
    /// Siembra en BD los permisos que no se gestionan vía migración con HasData.
    /// Se ejecuta al arrancar la API, después de aplicar migraciones.
    /// </summary>
    public static class PermissionSeeder
    {
        private const string SuperAdminRoleId = "87b92599-3be7-4ab5-b19e-9e069e015d4e";

        public static void Seed(LdProyectDbContext db)
        {
            SeedSimplePermissions(db);
            SeedStandardLabelPrintPermission(db);
            SeedLoadMappingScanDeletePermission(db);
            SeedKittingFolioCapturePermission(db);
            AssignAllPermissionsToSuperAdmin(db);
        }

        private static void SeedSimplePermissions(LdProyectDbContext db)
        {
            // (Key, Name, ModuleId)
            var permissionsToSeed = new[]
            {
                (PermissionKeys.WarehouseTask_View,   "Ver tareas de almacén",        1),
                (PermissionKeys.WarehouseTask_Manage, "Administrar tareas de almacén", 1),
                (PermissionKeys.Movement_Create,      "Crear movimientos",            6),
                (PermissionKeys.Movement_Update,      "Editar movimientos",           6),
                (PermissionKeys.Movement_Delete,      "Eliminar movimientos",         6)
            };

            var missing = permissionsToSeed
                .Where(p => !db.Permissions.Any(x => x.Key == p.Item1))
                .Select(p => new Permission
                {
                    PermissionName = p.Item2,
                    Key            = p.Item1,
                    ModuleId       = p.Item3,
                    CreatedAt      = DateTime.UtcNow,
                    IsActive       = true
                })
                .ToList();

            if (missing.Count > 0)
            {
                db.Permissions.AddRange(missing);
                db.SaveChanges();
            }
        }

        private static void SeedStandardLabelPrintPermission(LdProyectDbContext db)
            => SeedPermissionAndAssignToSuperAdmin(
                db,
                PermissionKeys.StandardLabel_Print,
                "Imprimir etiquetas LD",
                moduleId: 22);

        private static void SeedLoadMappingScanDeletePermission(LdProyectDbContext db)
            => SeedPermissionAndAssignToSuperAdmin(
                db,
                PermissionKeys.LoadMappingScan_Delete,
                "Eliminar escaneo de mapeo de carga",
                moduleId: 18);

        private static void SeedKittingFolioCapturePermission(LdProyectDbContext db)
            => SeedPermissionAndAssignToSuperAdmin(
                db,
                PermissionKeys.KittingFolioCapture_Access,
                "Captura de folios Kitting",
                moduleId: 17);

        private static void AssignAllPermissionsToSuperAdmin(LdProyectDbContext db)
        {
            var assignedPermissionIds = db.RolePermissions
                .Where(rp => rp.RoleId == SuperAdminRoleId)
                .Select(rp => rp.PermissionId)
                .ToHashSet();

            var missingAssignments = db.Permissions
                .AsNoTracking()
                .Select(permission => permission.PermissionId)
                .ToList()
                .Where(permissionId => !assignedPermissionIds.Contains(permissionId))
                .Select(permissionId => new RolePermission
                {
                    RoleId = SuperAdminRoleId,
                    PermissionId = permissionId
                })
                .ToList();

            if (missingAssignments.Count == 0)
            {
                return;
            }

            db.RolePermissions.AddRange(missingAssignments);
            db.SaveChanges();
        }

        private static void SeedPermissionAndAssignToSuperAdmin(
            LdProyectDbContext db,
            string key,
            string name,
            int moduleId)
        {
            var permission = db.Permissions.FirstOrDefault(p => p.Key == key);

            if (permission == null)
            {
                permission = new Permission
                {
                    PermissionName = name,
                    Key            = key,
                    ModuleId       = moduleId,
                    CreatedAt      = DateTime.UtcNow,
                    IsActive       = true
                };

                db.Permissions.Add(permission);
                db.SaveChanges();
            }

            var alreadyAssigned = db.RolePermissions.Any(rp =>
                rp.RoleId == SuperAdminRoleId &&
                rp.PermissionId == permission.PermissionId);

            if (!alreadyAssigned)
            {
                db.RolePermissions.Add(new RolePermission
                {
                    RoleId       = SuperAdminRoleId,
                    PermissionId = permission.PermissionId
                });
                db.SaveChanges();
            }
        }
    }
}
