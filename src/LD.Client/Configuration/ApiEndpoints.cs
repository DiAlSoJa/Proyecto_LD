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
        public string Security_GetPatioMonitor => $"{_baseApi}/security/patio-monitor";
        public string Security_GetCortinas     => $"{_baseApi}/security/cortinas";
        public string Security_AsignarCortina  => $"{_baseApi}/security/{{id}}/asignar-cortina";
        public string Security_GetTasks        => $"{_baseApi}/security/tasks";
        public string Security_AbrirCortina    => $"{_baseApi}/security/tasks/{{taskId}}/abrir";
        public string Security_CerrarRegistro  => $"{_baseApi}/security/tasks/{{taskId}}/cerrar";

        public string OperationalTask_GetAll      => $"{_baseApi}/OperationalTask";
        public string OperationalTask_GetById     => $"{_baseApi}/OperationalTask/{{taskId}}";
        public string OperationalTask_MyAssigned  => $"{_baseApi}/OperationalTask/my-assigned";
        public string OperationalTask_Create      => $"{_baseApi}/OperationalTask";
        public string OperationalTask_Complete    => $"{_baseApi}/OperationalTask/{{taskId}}/complete";
        public string OperationalTask_UploadImage => $"{_baseApi}/OperationalTask/upload-image";
        public string OperationalTask_GetImage    => $"{_baseApi}/OperationalTask/image?path={{path}}";

        // ======================
        // WAREHOUSE TASK
        // ======================

        public string WarehouseTask_GetAll       => $"{_baseApi}/WarehouseTask";
        public string WarehouseTask_GetById      => $"{_baseApi}/WarehouseTask/{{taskId}}";
        public string WarehouseTask_MyAssigned   => $"{_baseApi}/WarehouseTask/my-assigned";
        public string WarehouseTask_ConnectedUsers => $"{_baseApi}/WarehouseTask/connected-users";
        public string WarehouseTask_MarkAvailable => $"{_baseApi}/WarehouseTask/mark-available";
        public string WarehouseTask_CancelWaiting => $"{_baseApi}/WarehouseTask/cancel-waiting";
        public string WarehouseTask_Create       => $"{_baseApi}/WarehouseTask";
        public string WarehouseTask_Complete     => $"{_baseApi}/WarehouseTask/{{taskId}}/complete";
        public string WarehouseTask_UploadImage  => $"{_baseApi}/WarehouseTask/upload-image";
        public string WarehouseTask_GetImage     => $"{_baseApi}/WarehouseTask/image?path={{path}}";

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
        public string Lookup_CycleCountAuditorByUser => $"{_baseApi}/Lookup/auditor/user/{{userId}}";
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
        public string CyclicInventory_GetScans => $"{_baseApi}/CyclicInventory/{{cyclicInventoryId}}/details/{{cyclicInventoryDetailId}}/scans";
        public string CyclicInventory_CreateScan => $"{_baseApi}/CyclicInventory/{{cyclicInventoryId}}/details/{{cyclicInventoryDetailId}}/scans";
        public string CyclicInventory_DeleteScan => $"{_baseApi}/CyclicInventory/{{cyclicInventoryId}}/details/{{cyclicInventoryDetailId}}/scans/{{cyclicInventoryScanId}}";
        public string CyclicInventory_FinishLocation => $"{_baseApi}/CyclicInventory/{{cyclicInventoryId}}/details/{{cyclicInventoryDetailId}}/finish-location";

        // ======================
        // DAMAGE REPORT
        // ======================

        public string DamageReport_GetAll => $"{_baseApi}/DamageReport";
        public string DamageReport_GetById => $"{_baseApi}/DamageReport/{{damageReportId}}";
        public string DamageReport_Create => $"{_baseApi}/DamageReport";
        public string DamageReport_UploadImage => $"{_baseApi}/DamageReport/upload-image";
        public string DamageReport_GetImage => $"{_baseApi}/DamageReport/image?path={{path}}";

        // ======================
        // REPORT QUERIES
        // ======================

        public string ReportQuery_GetSummaries => $"{_baseApi}/ReportQuery";
        public string ReportQuery_GetAll => $"{_baseApi}/ReportQuery/manage";
        public string ReportQuery_GetById => $"{_baseApi}/ReportQuery/{{reportQueryId}}";
        public string ReportQuery_GetParameters => $"{_baseApi}/ReportQuery/{{reportQueryId}}/parameters";
        public string ReportQuery_Create => $"{_baseApi}/ReportQuery";
        public string ReportQuery_Update => $"{_baseApi}/ReportQuery/{{reportQueryId}}";
        public string ReportQuery_Delete => $"{_baseApi}/ReportQuery/{{reportQueryId}}";
        public string ReportQuery_Execute => $"{_baseApi}/ReportQuery/{{reportQueryId}}/execute";



        // ======================
        // INVENTORYSTATUS
        // ======================

        public string InventaryStatus_GetAll => $"{_baseApi}/inventaryStatus";
        public string InventaryStatus_GetById => $"{_baseApi}/inventaryStatus/{{statusId}}/{{clientId}}/{{projectId}}";
        public string InventaryStatus_Create => $"{_baseApi}/inventaryStatus";
        public string InventaryStatus_Update => $"{_baseApi}/inventaryStatus/{{statusId}}/{{clientId}}/{{projectId}}";
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
        public string Asn_GetLocatingPallets => $"{_baseApi}/asn/locating-pallets";
        public string Asn_LocatePallet => $"{_baseApi}/asn/{{asnId}}/locate-pallet";

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
        // KITTING
        // ======================

        public string Kitting_GetAll => $"{_baseApi}/kitting";
        public string Kitting_GetById => $"{_baseApi}/kitting/{{kittingId}}";
        public string Kitting_GetByClient => $"{_baseApi}/kitting/{{clientId}}/{{projectId}}";
        public string Kitting_Create => $"{_baseApi}/kitting";
        public string Kitting_Update => $"{_baseApi}/kitting/{{kittingId}}";
        public string Kitting_Confirm => $"{_baseApi}/kitting/{{kittingId}}/confirm";
        public string Kitting_Cancel => $"{_baseApi}/kitting/{{kittingId}}/cancel";
        public string Kitting_Locate => $"{_baseApi}/kitting/{{kittingId}}/locate";
        public string Kitting_SendToSupply => $"{_baseApi}/kitting/{{kittingId}}/send-to-supply";
        public string Kitting_UploadImage => $"{_baseApi}/kitting/{{kittingId}}/upload-image";
        public string Kitting_GetImage => $"{_baseApi}/kitting/image?path={{path}}";
        public string Kitting_ValidationPhotos => $"{_baseApi}/kitting/{{kittingId}}/validation-photos";
        public string Kitting_ValidationPhotoByKey => $"{_baseApi}/kitting/{{kittingId}}/validation-photos/{{photoKey}}";
        public string Kitting_ReplaceValidationPhotoByKey => $"{_baseApi}/kitting/{{kittingId}}/validation-photos/{{photoKey}}/replace";

        // ======================
        // DELIVERY ORDER / DO
        // ======================

        public string DeliveryOrder_CreateFromKittings => $"{_baseApi}/deliveryOrder/from-kittings";
        public string DeliveryOrder_AddKittingsToExisting => $"{_baseApi}/deliveryOrder/add-kittings";
        public string DeliveryOrder_FinishLoading => $"{_baseApi}/deliveryOrder/finish-loading";

        public string LoadMapping_GetAll => $"{_baseApi}/loadmapping";
        public string LoadMapping_GetLoadingOrders => $"{_baseApi}/loadmapping/loading-orders";
        public string LoadMapping_Create => $"{_baseApi}/loadmapping";
        public string LoadMapping_GetScans => $"{_baseApi}/loadmapping/{{loadMappingId}}/scans";
        public string LoadMapping_CreateScan => $"{_baseApi}/loadmapping/{{loadMappingId}}/scans";
        public string LoadMapping_DeleteScan => $"{_baseApi}/loadmapping/{{loadMappingId}}/scans/{{scanId}}";

        public string KittingDetail_GetAll => $"{_baseApi}/kittingDetail";
        public string KittingDetail_GetById => $"{_baseApi}/kittingDetail/{{kittingDetailId}}";
        public string KittingDetail_GetByKittingId => $"{_baseApi}/kittingDetail/kitting/{{kittingId}}";
        public string KittingDetail_Create => $"{_baseApi}/kittingDetail";
        public string KittingDetail_Update => $"{_baseApi}/kittingDetail/{{kittingDetailId}}";
        public string KittingDetail_Delete => $"{_baseApi}/kittingDetail/{{kittingDetailId}}";

        public string KittingIssue_GetAll => $"{_baseApi}/kittingIssue";
        public string KittingIssue_GetById => $"{_baseApi}/kittingIssue/{{kittingIssueDetailId}}";
        public string KittingIssue_GetByKittingDetailId => $"{_baseApi}/kittingIssue/kittingDetail/{{kittingDetailId}}";
        public string KittingIssue_Validate => $"{_baseApi}/kittingIssue/{{kittingIssueDetailId}}/validate";
        public string KittingIssue_AuditConfirm => $"{_baseApi}/kittingIssue/{{kittingIssueDetailId}}/audit-confirm";
        public string KittingIssue_Create => $"{_baseApi}/kittingIssue";
        public string KittingIssue_Update => $"{_baseApi}/kittingIssue/{{kittingIssueDetailId}}";
        public string KittingIssue_Delete => $"{_baseApi}/kittingIssue/{{kittingIssueDetailId}}";

        // ======================
        // CHECKLIST
        // ======================

        public string Checklist_Submit      => $"{_baseApi}/Checklist";
        public string Checklist_GetAll      => $"{_baseApi}/Checklist";
        public string Checklist_GetById     => $"{_baseApi}/Checklist/{{checklistId}}";
        public string Checklist_UploadPhoto => $"{_baseApi}/Checklist/upload-photo";
        public string Checklist_GetPhoto    => $"{_baseApi}/Checklist/photo?path={{path}}";
        public string Checklist_DailyStatus => $"{_baseApi}/Checklist/daily-status";

        // ======================
        // SIGNALR HUBS
        // ======================

        // _baseApi termina en "/api"; el hub vive fuera de ese prefijo
        private string BaseHost => _baseApi.EndsWith("/api")
            ? _baseApi[..^4]
            : _baseApi;

        public string NotificationsHub => $"{BaseHost}/hubs/notifications";

    }
}

