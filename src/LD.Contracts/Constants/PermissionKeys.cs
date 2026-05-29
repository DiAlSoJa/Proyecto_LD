using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Contracts.Constants;

public static class PermissionKeys
{

    public const string Client_View = "clients.read";
    public const string Client_Create = "clients.create";
    public const string Client_Update = "clients.update";
    public const string Client_Delete = "clients.delete";

    public const string Project_View = "projects.read";
    public const string Project_Create = "projects.create";
    public const string Project_Update = "projects.update";
    public const string Project_Delete = "projects.delete";


    public const string Warehouse_View = "warehouses.read";
    public const string Warehouse_Create = "warehouses.create";
    public const string Warehouse_Update = "warehouses.update";
    public const string Warehouse_Delete = "warehouses.delete";


    public const string Location_View = "locations.read";
    public const string Location_Create = "locations.create";
    public const string Location_Update = "locations.update";
    public const string Location_Delete = "locations.delete";



    public const string Product_View = "products.read";
    public const string Product_Create = "products.create";
    public const string Product_Update = "products.update";
    public const string Product_Delete = "products.delete";

    // MOVEMENTS
    public const string Movement_View = "movements.read";
    public const string Movement_Create = "movements.create";
    public const string Movement_Update = "movements.update";
    public const string Movement_Delete = "movements.delete";

    // ASN
    public const string Asn_View   = "asn.read";
    public const string Asn_Create = "asn.read";// "asn.create"; // falta dar de alta
    public const string Asn_Update = "asn.read";//"asn.update"; // falta dar de alta
    public const string Asn_Delete = "asn.read";//"asn.delete"; // falta dar de alta

    // FORKLIFT CHECKLIST
    public const string ForkliftChecklist_View    = "forklift-checklist.read";

    // Aliases semánticos para el feature de submit/resumen de checklists.
    // Reutilizan permisos existentes del módulo 8 para no crear registros duplicados en BD.
    public const string Checklist_Submit      = ForkliftChecklist_Execute; // forklift-checklist.execute
    public const string Checklist_ViewSummary = ForkliftChecklist_View;   // forklift-checklist.read
    public const string ForkliftChecklist_Create  = "forklift-checklist.create";
    public const string ForkliftChecklist_Update  = "forklift-checklist.update";
    public const string ForkliftChecklist_Delete  = "forklift-checklist.delete";
    public const string ForkliftChecklist_Execute = "forklift-checklist.execute";
    public const string ForkliftChecklist_Approve = "forklift-checklist.approve";

    // YARD CONTROL
    public const string YardControl_View = "yard-control.read";

    // CATALOGS
    public const string Catalog_View = "catalogs.read";

    // PICKING
    public const string Picking_View = "picking.read";

    // AUDITING
    public const string Auditing_View = "auditing.read";

    // SHIPMENTS
    public const string Shipment_View = "shipments.read";

    // INVENTORY
    public const string Inventory_View         = "inventory.read";
    public const string Inventory_Audit_View   = "inventory.audit.read";
    public const string Inventory_Audit_Execute = "inventory.audit.execute";
    public const string Inventory_List_View    = "inventory.list.read";
    public const string Inventory_List_Export  = "inventory.list.export";

    // CYCLE COUNT
    public const string CycleCount_View = "cycle-count.read";
    public const string CycleCount_Create = "cycle-count.read";
    public const string CycleCount_Update = "cycle-count.read";

    // REPORTS
    public const string Report_View = "reports.read";

    // STANDARD LABELS / IMPRESION
    public const string StandardLabel_Print = "standard-label.print";

    // USERS
    public const string User_View = "users.read";
    public const string User_Create = "users.create";
    public const string User_Update = "users.update";
    public const string User_Delete = "users.delete";

    // WAREHOUSE STAFF / ALMACENISTA
    public const string WarehouseStaff_View                  = "warehouse-staff.read";
    public const string WarehouseStaff_LocationChange_Execute = "warehouse-staff.location-change.execute";
    public const string WarehouseStaff_Supply_Execute         = "warehouse-staff.supply.execute";
    public const string WarehouseStaff_Asn_View              = "warehouse-staff.asn.read";
    public const string WarehouseStaff_Asn_Execute           = "warehouse-staff.asn.execute";
    public const string WarehouseStaff_Tasks_View            = "warehouse-staff.tasks.read";
    public const string WarehouseStaff_Tasks_Manage          = "warehouse-staff.tasks.manage";

    // SECURITY / SEGURIDAD
    public const string Security_View         = "security.read";
    public const string Security_Create       = "security.create";
    public const string Security_Tasks_View   = "security.tasks.read";
    public const string Security_Tasks_Manage = "security.tasks.manage";
    public const string Cortina_Assign        = "security.cortina.assign";

    public const string Vehicle_View   = "security.vehicles.read";
    public const string Vehicle_Create = "security.vehicles.create";
    public const string Vehicle_Update = "security.vehicles.update";
    public const string Vehicle_Delete = "security.vehicles.delete";


    // QUERIES / CONSULTAS
    public const string Query_View = "queries.read";

    // DAMAGE REPORT / REPORTE DE DAÑOS
    public const string DamageReport_View   = "damage-report.read";
    public const string DamageReport_Create = "damage-report.create";

    // OPERATIONS / OPERACIONES
    public const string Operation_View    = "operations.read";
    public const string Operation_Execute = "operations.execute";

    // CATEGORIES / CATEGORÍAS
    public const string Category_View   = "categories.read";
    public const string Category_Create = "categories.create";
    public const string Category_Update = "categories.update";

    // DIMENSIONER / DIMENSIONADOR
    public const string Dimensioner_View   = "dimensioner.read";
    public const string Dimensioner_Create = "dimensioner.create";
    public const string Dimensioner_Update = "dimensioner.update";

    // FAMILIES / FAMILIAS
    public const string Family_View   = "families.read";
    public const string Family_Create = "families.create";
    public const string Family_Update = "families.update";

    // CURRENCIES / MONEDAS
    public const string Currency_View   = "currencies.read";
    public const string Currency_Create = "currencies.create";
    public const string Currency_Update = "currencies.update";

    // STATUS / ESTATUS
    public const string Status_View   = "status.read";
    public const string Status_Create = "status.create";
    public const string Status_Update = "status.update";

    // UNITS / UNIDADES
    public const string Unit_View   = "units.read";
    public const string Unit_Create = "units.create";
    public const string Unit_Update = "units.update";

    // EQUIPMENT TYPES / TIPOS DE EQUIPO
    public const string EquipmentType_View   = "units.read";//"equipment-types.read";
    public const string EquipmentType_Create = "units.read";//"equipment-types.create";
    public const string EquipmentType_Update = "units.read";//"equipment-types.update";





}
