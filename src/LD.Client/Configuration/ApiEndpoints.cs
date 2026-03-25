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
        // USER
        // ======================
   
        public string User_GetAll => $"{_baseApi}/user";
        public string User_GetById => $"{_baseApi}/user/{{id}}";
        public string User_Create => $"{_baseApi}/user";
        public string User_Update => $"{_baseApi}/user/{{id}}";
        public string User_Delete => $"{_baseApi}/user/{{id}}";
        // ======================
        // ROLE
        // ======================

        public string Role_GetAll => $"{_baseApi}/role";
        public string Role_GetById => $"{_baseApi}/role/{{id}}";
        public string Role_Create => $"{_baseApi}/role";
        public string Role_Update => $"{_baseApi}/role/{{id}}";
        public string Role_Delete => $"{_baseApi}/role/{{id}}";

        // ======================
        // VEHICLE
        // ======================

        public string Vehicle_GetAll => $"{_baseApi}/vehicle";
        public string Vehicle_GetById => $"{_baseApi}/vehicle/{{id}}";
        public string Vehicle_Create => $"{_baseApi}/vehicle";
        public string Vehicle_Update => $"{_baseApi}/vehicle/{{id}}";
        public string Vehicle_Delete => $"{_baseApi}/vehicle/{{id}}";
        

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
        public string Lookup_Client => $"{_baseApi}/Lookup/client";
        public string Lookup_Project => $"{_baseApi}/Lookup/project";
        public string Lookup_ProjectClient => $"{_baseApi}/Lookup/project";

        public string Lookup_Location => $"{_baseApi}/Lookup/location";
        public string Lookup_Role => $"{_baseApi}/Lookup/role";
        public string Lookup_Category => $"{_baseApi}/Lookup/category";
        public string Lookup_Family => $"{_baseApi}/Lookup/family";
        public string Lookup_Unit => $"{_baseApi}/Lookup/unit";


        // ======================
        // MODULES
        // ======================

        public string Module_GetAll => $"{_baseApi}/module";



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

    }
}

