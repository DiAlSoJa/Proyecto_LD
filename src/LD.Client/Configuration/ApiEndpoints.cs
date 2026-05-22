using LD.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Configuration
{
    public class ApiEndpoints
    {

        private readonly string _baseApi;


        public ApiEndpoints(IOptions<ApiSettings> options)
        {
            _baseApi = options.Value.BaseUrl;
        }
        // ======================
        // AUTH
        // ======================
        public string Login => $"{_baseApi}/auth/login";
        public string Register => $"{_baseApi}/auth/register";
        public string GetMe => $"{_baseApi}/auth/me";
        public string ResetPassword => $"{_baseApi}/auth/reset-password";
        public string ForgotPassword => $"{_baseApi}/auth/forgot-password";
        public string RefreshToken => $"{_baseApi}/auth/refresh";

        // ======================
        // ITEM
        // ======================

        public string Product_GetAll => $"{_baseApi}/Product";
        public string Product_GetById => $"{_baseApi}/Product/{{id}}";
        public string Product_GetByClientId => $"{_baseApi}/Product/{{clientId}}/{{projectId}}";
        public string Product_Create => $"{_baseApi}/Product";
        public string Product_Update => $"{_baseApi}/Product/{{id}}";
        public string Product_Delete => $"{_baseApi}/Product/{{id}}";
        // ======================
        // ARTICLE
        // ======================

        public string Article_GetAll => $"{_baseApi}/item";
        public string Article_GetById => $"{_baseApi}/item/{{id}}";
        public string Article_Create => $"{_baseApi}/item";
        public string Article_Update => $"{_baseApi}/item/{{id}}";
        public string Article_Delete => $"{_baseApi}/item/{{id}}";

        // ======================
        // CATEGORY
        // ======================

        public string Category_GetAll => $"{_baseApi}/category";
        public string Category_GetById => $"{_baseApi}/category/{{categoryId}}";
        public string Category_Create => $"{_baseApi}/category";
        public string Category_Update => $"{_baseApi}/category/{{categoryId}}";
        public string Category_Delete => $"{_baseApi}/category/{{categoryId}}";
        

        // ======================
        // CLIENT
        // ======================

        public string Client_GetAll => $"{_baseApi}/client";
        public string Client_GetById => $"{_baseApi}/client/{{id}}";
        public string Client_Create => $"{_baseApi}/client";
        public string Client_Update => $"{_baseApi}/client/{{id}}";
        public string Client_Delete => $"{_baseApi}/client/{{id}}";
        

        // ======================
        // CONTACT
        // ======================
       
        public string Contact_GetAll => $"{_baseApi}/contact";
        public string Contact_GetById => $"{_baseApi}/contact/{{id}}";
        public string Contact_Create => $"{_baseApi}/contact";
        public string Contact_Update => $"{_baseApi}/contact/{{id}}";
        public string Contact_Delete => $"{_baseApi}/contact/{{id}}";
        

        // ======================
        // CURRENCY
        // ======================

        public string Currency_GetAll => $"{_baseApi}/currency";
        public string Currency_GetById => $"{_baseApi}/currency/{{currencyId}}";
        public string Currency_Create => $"{_baseApi}/currency";
        public string Currency_Update => $"{_baseApi}/currency/{{currencyId}}";
        public string Currency_Delete => $"{_baseApi}/currency/{{currencyId}}";
        

        // ======================
        // DRIVER
        // ======================
      
        public string Driver_GetAll => $"{_baseApi}/driver";
        public string Driver_GetById => $"{_baseApi}/driver/{{id}}";
        public string Driver_Create => $"{_baseApi}/driver";
        public string Driver_Update => $"{_baseApi}/driver/{{id}}";
        public string Driver_Delete => $"{_baseApi}/driver/{{id}}";
        

        // ======================
        // LOCATION
        // ======================
      
        public string Location_GetAll => $"{_baseApi}/location";
        public string Location_GetById => $"{_baseApi}/location/{{id}}";
        public string Location_Create => $"{_baseApi}/location";
        public string Location_Update => $"{_baseApi}/location/{{id}}";
        public string Location_Delete => $"{_baseApi}/location/{{id}}";
        public string Location_CreationRange => $"{_baseApi}/location/create-range";
       

        // ======================
        // PRINTER
        // ======================
       
        public string Printer_GetAll => $"{_baseApi}/printer";
        public string Printer_GetById => $"{_baseApi}/printer/{{id}}";
        public string Printer_Create => $"{_baseApi}/printer";
        public string Printer_Update => $"{_baseApi}/printer/{{id}}";
        public string Printer_Delete => $"{_baseApi}/printer/{{id}}";

        // ======================
        // STANDARD LABEL
        // ======================

        public string StandardLabel_Generate => $"{_baseApi}/StandardLabel/generate";
        public string StandardLabel_GetByCode => $"{_baseApi}/StandardLabel/{{code}}";
        

        // ======================
        // PROJECT
        // ======================
   
        public string Project_GetAll => $"{_baseApi}/project";
        public string Project_GetById => $"{_baseApi}/project/{{id}}";
        public string Project_Create => $"{_baseApi}/project";
        public string Project_Update => $"{_baseApi}/project/{{id}}";
        public string Project_Delete => $"{_baseApi}/project/{{id}}";
        

    
        

        // ======================
        // UNIT
        // ======================
 
        public string Unit_GetAll => $"{_baseApi}/unit";
        public string Unit_GetById => $"{_baseApi}/unit/{{unitIdS}}";
        public string Unit_Create => $"{_baseApi}/unit";
        public string Unit_Update => $"{_baseApi}/unit/{{unitIdS}}";
        public string Unit_Delete => $"{_baseApi}/unit/{{unitIdS}}";

        // ======================
        // EQUIPMENT TYPE
        // ======================

        public string EquipmentType_GetAll => $"{_baseApi}/EquipmentType";
        public string EquipmentType_GetById => $"{_baseApi}/EquipmentType/{{equipmentTypeId}}";
        public string EquipmentType_Create => $"{_baseApi}/EquipmentType";
        public string EquipmentType_Update => $"{_baseApi}/EquipmentType/{{equipmentTypeId}}";
        public string EquipmentType_Delete => $"{_baseApi}/EquipmentType/{{equipmentTypeId}}";

        // ======================
        // EQUIPMENT
        // ======================

        public string Equipment_GetAll      => $"{_baseApi}/Equipment";
        public string Equipment_AssignedToMe => $"{_baseApi}/Equipment/assigned-to-me";
        public string Equipment_GetById     => $"{_baseApi}/Equipment/{{equipmentId}}";
        public string Equipment_Create => $"{_baseApi}/Equipment";
        public string Equipment_Update => $"{_baseApi}/Equipment/{{equipmentId}}";
        public string Equipment_UploadImage => $"{_baseApi}/Equipment/upload-image";
        public string Equipment_GetImage => $"{_baseApi}/Equipment/image?path={{path}}";
        public string Equipment_GetImageBySide => $"{_baseApi}/Equipment/{{equipmentId}}/image/{{side}}";
        public string EquipmentType_UploadImage => $"{_baseApi}/Equipment/upload-image";
        public string EquipmentType_GetImage => $"{_baseApi}/Equipment/image?path={{path}}";

        // ======================
        // EQUIPMENT SUPPLIER
        // ======================

        public string EquipmentSupplier_GetAll => $"{_baseApi}/EquipmentSupplier";
        public string EquipmentSupplier_Create => $"{_baseApi}/EquipmentSupplier";
        

        // ======================
        // USER
        // ======================
   
        public string User_GetAll => $"{_baseApi}/user";
        public string User_GetById => $"{_baseApi}/user/{{id}}";
        public string User_Create => $"{_baseApi}/user";
        public string User_Update => $"{_baseApi}/user/{{id}}";
        public string User_Delete => $"{_baseApi}/user/{{id}}";
        public string User_AssignWarehouses => $"{_baseApi}/user/{{id}}/warehouses";
        // ======================
        // ROLE
        // ======================

        public string Role_GetAll => $"{_baseApi}/role";
        public string Role_GetById => $"{_baseApi}/role/{{id}}";
        public string Role_Create => $"{_baseApi}/role";
        public string Role_Update => $"{_baseApi}/role/{{id}}";
        public string Role_Delete => $"{_baseApi}/role/{{id}}";

        // ======================
        // SECURITY / SEGURIDAD
        // ======================

        public string Security_GetRegistrations => $"{_baseApi}/security";
        public string Security_Register        => $"{_baseApi}/security";
        public string Security_GetSinSalida    => $"{_baseApi}/security/sin-salida";
        public string Security_GetCortinas     => $"{_baseApi}/security/cortinas";
        public string Security_AsignarCortina  => $"{_baseApi}/security/{{id}}/asignar-cortina";
        public string Security_GetTasks        => $"{_baseApi}/security/tasks";
        public string Security_AbrirCortina    => $"{_baseApi}/security/tasks/{{taskId}}/abrir";
        public string Security_CerrarRegistro  => $"{_baseApi}/security/tasks/{{taskId}}/cerrar";

        public string OperationalTask_GetAll      => $"{_baseApi}/OperationalTask";
        public string OperationalTask_GetById     => $"{_baseApi}/OperationalTask/{{taskId}}";
        public string OperationalTask_Create      => $"{_baseApi}/OperationalTask";
        public string OperationalTask_Complete    => $"{_baseApi}/OperationalTask/{{taskId}}/complete";
        public string OperationalTask_UploadImage => $"{_baseApi}/OperationalTask/upload-image";
        public string OperationalTask_GetImage    => $"{_baseApi}/OperationalTask/image?path={{path}}";

        // ======================
        // VEHICLE
        // ======================

        public string Vehicle_GetAll => $"{_baseApi}/vehicle";
        public string Vehicle_GetById => $"{_baseApi}/vehicle/{{id}}";
        public string Vehicle_Create => $"{_baseApi}/vehicle";
        public string Vehicle_Update => $"{_baseApi}/vehicle/{{id}}";
        public string Vehicle_Delete => $"{_baseApi}/vehicle/{{id}}";

        // ======================
        // EQUIPMENT QUESTION
        // ======================

        public string EquipmentQuestion_GetByEquipmentType => $"{_baseApi}/equipmentquestion/equipment-type/{{equipmentTypeId}}";
        public string EquipmentQuestion_Save => $"{_baseApi}/equipmentquestion";
        public string EquipmentQuestion_Delete => $"{_baseApi}/equipmentquestion/{{equipmentQuestionDetId}}";

        
        // ======================
        // WAREHOUSE
        // ======================

        public string Warehouse_GetAll => $"{_baseApi}/warehouse";
        public string Warehouse_GetById => $"{_baseApi}/warehouse/{{id}}";
        public string Warehouse_Create => $"{_baseApi}/warehouse";
        public string Warehouse_Update => $"{_baseApi}/warehouse/{{id}}";
        public string Warehouse_Delete => $"{_baseApi}/warehouse/{{id}}";

        // ======================
        // LOOKUP
        // ======================
        public string Lookup_GetAll => $"{_baseApi}/Lookup";
        public string Lookup_Warehouse => $"{_baseApi}/Lookup/warehouse";
        public string Lookup_WarehouseByUser => $"{_baseApi}/Lookup/warehouse/user/{{userId}}";
        public string Lookup_Client => $"{_baseApi}/Lookup/client";
        public string Lookup_Project => $"{_baseApi}/Lookup/project";
        public string Lookup_ProjectClient => $"{_baseApi}/Lookup/project";
        public string Lookup_ProjectClientByUserWarehouses => $"{_baseApi}/Lookup/project-client/user/{{userId}}";
        public string Lookup_SystemField => $"{_baseApi}/Lookup/systemfield";


        public string Lookup_Location => $"{_baseApi}/Lookup/location";
        public string Lookup_LocationWarehouse => $"{_baseApi}/Lookup/location";
        public string Lookup_Role => $"{_baseApi}/Lookup/role";
        public string Lookup_Category => $"{_baseApi}/Lookup/category";
        public string Lookup_Family => $"{_baseApi}/Lookup/family";
        public string Lookup_Unit => $"{_baseApi}/Lookup/unit";
        public string Lookup_Dimensioner => $"{_baseApi}/Lookup/dimensioner";
        public string Lookup_ScanType => $"{_baseApi}/Lookup/scantype";
        public string Lookup_ScanSaveType => $"{_baseApi}/Lookup/scansavetype";


        // ======================
        // MODULES
        // ======================

        public string Module_GetAll => $"{_baseApi}/module";

        // ======================
        // INVENTORY MOVEMENT
        // ======================

        public string InventoryMovement_GetAll => $"{_baseApi}/InventoryMovement";
        public string InventoryMovement_GetById => $"{_baseApi}/InventoryMovement/{{movementId}}";
        public string InventoryMovement_Create => $"{_baseApi}/InventoryMovement";
        public string InventoryMovement_Update => $"{_baseApi}/InventoryMovement/{{movementId}}";

        // ======================
        // AVAILABLE INVENTORY
        // ======================

        public string AvailableInventory_GetAll => $"{_baseApi}/AvailableInventory";
        public string AvailableInventory_ChangeLocation => $"{_baseApi}/AvailableInventory/change-location";
        public string AvailableInventory_ChangeStatus => $"{_baseApi}/AvailableInventory/change-status";
        public string AvailableInventory_ChangeWarehouse => $"{_baseApi}/AvailableInventory/change-warehouse";

        // ======================
        // CYCLIC INVENTORY
        // ======================

        public string CyclicInventory_GetAll => $"{_baseApi}/CyclicInventory";
        public string CyclicInventory_GetById => $"{_baseApi}/CyclicInventory/{{cyclicInventoryId}}";
        public string CyclicInventory_Create => $"{_baseApi}/CyclicInventory";
        public string CyclicInventory_Update => $"{_baseApi}/CyclicInventory/{{cyclicInventoryId}}";

        // ======================
        // DAMAGE REPORT
        // ======================

        public string DamageReport_GetAll => $"{_baseApi}/DamageReport";
        public string DamageReport_GetById => $"{_baseApi}/DamageReport/{{damageReportId}}";
        public string DamageReport_Create => $"{_baseApi}/DamageReport";
        public string DamageReport_UploadImage => $"{_baseApi}/DamageReport/upload-image";
        public string DamageReport_GetImage => $"{_baseApi}/DamageReport/image?path={{path}}";



        // ======================
        // INVENTORYSTATUS
        // ======================

        public string InventaryStatus_GetAll => $"{_baseApi}/inventaryStatus";
        public string InventaryStatus_GetById => $"{_baseApi}/inventaryStatus/{{statusId}}";
        public string InventaryStatus_Create => $"{_baseApi}/inventaryStatus";
        public string InventaryStatus_Update => $"{_baseApi}/inventaryStatus/{{statusId}}";
        public string InventaryStatus_Delete => $"{_baseApi}/inventaryStatus/{{statusId}}";


        // ======================
        // FAMILY
        // ======================

        public string Family_GetAll => $"{_baseApi}/family";
        public string Family_GetById => $"{_baseApi}/family/{{familyId}}";
        public string Family_Create => $"{_baseApi}/family";
        public string Family_Update => $"{_baseApi}/family/{{familyId}}";
        public string Family_Delete => $"{_baseApi}/family/{{familyId}}";

        // ======================
        // DIMENSIONER
        // ======================

        public string Dimensioner_GetAll => $"{_baseApi}/dimensioner";
        public string Dimensioner_GetById => $"{_baseApi}/dimensioner/{{dimensionerId}}";
        public string Dimensioner_Create => $"{_baseApi}/dimensioner";
        public string Dimensioner_Update => $"{_baseApi}/dimensioner/{{dimensionerId}}";
        public string Dimensioner_Delete => $"{_baseApi}/dimensioner/{{dimensioner}}";

        // ======================
        // ASN
        // ======================

        public string Asn_GetAll => $"{_baseApi}/asn";
        public string Asn_GetById => $"{_baseApi}/asn/{{asnId}}";
        public string Asn_Create => $"{_baseApi}/asn";
        public string Asn_Update => $"{_baseApi}/asn/{{asnId}}";
        public string Asn_Delete => $"{_baseApi}/asn/{{asnId}}";
        public string Asn_Confirm => $"{_baseApi}/asn/{{asnId}}/confirm";
        public string Asn_Cancel => $"{_baseApi}/asn/{{asnId}}/cancel";
        public string Asn_Locate => $"{_baseApi}/asn/{{asnId}}/locate";

        public string AsnDetail_GetAll => $"{_baseApi}/asnDetail";
        public string AsnDetail_GetById => $"{_baseApi}/asnDetail/{{asnId}}";
        public string AsnDetail_GetByAsnId => $"{_baseApi}/asnDetail/asn/{{asnId}}";
        public string AsnDetail_Create => $"{_baseApi}/asnDetail";
        public string AsnDetail_Update => $"{_baseApi}/asnDetail/{{asnId}}";
        public string AsnDetail_Delete => $"{_baseApi}/asnDetail/{{asnId}}";

        public string AsnReceipt_GetAll => $"{_baseApi}/AsnReceipt";
        public string AsnReceipt_GetById => $"{_baseApi}/AsnReceipt/{{asnId}}";
        public string AsnReceipt_GetByAsnDetailId => $"{_baseApi}/AsnReceipt/asnReceiptId/{{asnDetailId}}";
        public string AsnReceipt_Create => $"{_baseApi}/AsnReceipt";
        public string AsnReceipt_Update => $"{_baseApi}/AsnReceipt/{{asnId}}";
        public string AsnReceipt_Delete => $"{_baseApi}/AsnReceipt/{{asnId}}";

        // ======================
        // CHECKLIST
        // ======================

        public string Checklist_Submit      => $"{_baseApi}/Checklist";
        public string Checklist_GetAll      => $"{_baseApi}/Checklist";
        public string Checklist_GetById     => $"{_baseApi}/Checklist/{{checklistId}}";
        public string Checklist_UploadPhoto => $"{_baseApi}/Checklist/upload-photo";
        public string Checklist_GetPhoto    => $"{_baseApi}/Checklist/photo?path={{path}}";
        public string Checklist_DailyStatus => $"{_baseApi}/Checklist/daily-status";


    }
}

