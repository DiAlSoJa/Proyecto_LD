using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Configuration
{
    public static class ApiEndpoints
    {
        //private const string BASE_API = "http://localhost:52525/api";
        private const string BASE_API = "https://ldapi20260216175843-d4hzf4hyfmbkgfep.mexicocentral-01.azurewebsites.net";

        // ======================
        // AUTH
        // ======================
        public static class Auth
        {
            public const string Login = $"{BASE_API}/auth/login";
            public const string Register = $"{BASE_API}/auth/register";
            public const string ResetPassword = $"{BASE_API}/auth/reset-password";
            public const string ForgotPassword = $"{BASE_API}/auth/forgot-password";
            public const string RefreshToken = $"{BASE_API}/auth/refresh";
        }

        // ======================
        // ARTICLE
        // ======================
        public static class Item
        {
            public const string GetAll = $"{BASE_API}/item";
            public const string GetById = $"{BASE_API}/item/{{id}}";
            public const string Create = $"{BASE_API}/item";
            public const string Update = $"{BASE_API}/item/{{id}}";
            public const string Delete = $"{BASE_API}/item/{{id}}";
        }

        // ======================
        // CATEGORY
        // ======================
        public static class Category
        {
            public const string GetAll = $"{BASE_API}/category";
            public const string GetById = $"{BASE_API}/category/{{id}}";
            public const string Create = $"{BASE_API}/category";
            public const string Update = $"{BASE_API}/category/{{id}}";
            public const string Delete = $"{BASE_API}/category/{{id}}";
        }

        // ======================
        // CLIENT
        // ======================
        public static class Client
        {
            public const string GetAll = $"{BASE_API}/client";
            public const string GetById = $"{BASE_API}/client/{{id}}";
            public const string Create = $"{BASE_API}/client";
            public const string Update = $"{BASE_API}/client/{{id}}";
            public const string Delete = $"{BASE_API}/client/{{id}}";
        }

        // ======================
        // CONTACT
        // ======================
        public static class Contact
        {
            public const string GetAll = $"{BASE_API}/contact";
            public const string GetById = $"{BASE_API}/contact/{{id}}";
            public const string Create = $"{BASE_API}/contact";
            public const string Update = $"{BASE_API}/contact/{{id}}";
            public const string Delete = $"{BASE_API}/contact/{{id}}";
        }

        // ======================
        // CURRENCY
        // ======================
        public static class Currency
        {
            public const string GetAll = $"{BASE_API}/currency";
            public const string GetById = $"{BASE_API}/currency/{{id}}";
            public const string Create = $"{BASE_API}/currency";
            public const string Update = $"{BASE_API}/currency/{{id}}";
            public const string Delete = $"{BASE_API}/currency/{{id}}";
        }

        // ======================
        // DRIVER
        // ======================
        public static class Driver
        {
            public const string GetAll = $"{BASE_API}/driver";
            public const string GetById = $"{BASE_API}/driver/{{id}}";
            public const string Create = $"{BASE_API}/driver";
            public const string Update = $"{BASE_API}/driver/{{id}}";
            public const string Delete = $"{BASE_API}/driver/{{id}}";
        }

        // ======================
        // LOCATION
        // ======================
        public static class Location
        {
            public const string GetAll = $"{BASE_API}/location";
            public const string GetById = $"{BASE_API}/location/{{id}}";
            public const string Create = $"{BASE_API}/location";
            public const string Update = $"{BASE_API}/location/{{id}}";
            public const string Delete = $"{BASE_API}/location/{{id}}";
        }

        // ======================
        // PRINTER
        // ======================
        public static class Printer
        {
            public const string GetAll = $"{BASE_API}/printer";
            public const string GetById = $"{BASE_API}/printer/{{id}}";
            public const string Create = $"{BASE_API}/printer";
            public const string Update = $"{BASE_API}/printer/{{id}}";
            public const string Delete = $"{BASE_API}/printer/{{id}}";
        }

        // ======================
        // PROJECT
        // ======================
        public static class Project
        {
            public const string GetAll = $"{BASE_API}/project";
            public const string GetById = $"{BASE_API}/project/{{id}}";
            public const string Create = $"{BASE_API}/project";
            public const string Update = $"{BASE_API}/project/{{id}}";
            public const string Delete = $"{BASE_API}/project/{{id}}";
        }

        // ======================
        // ROLE
        // ======================
        public static class Role
        {
            public const string GetAll = $"{BASE_API}/role";
            public const string GetById = $"{BASE_API}/role/{{id}}";
            public const string Create = $"{BASE_API}/role";
            public const string Update = $"{BASE_API}/role/{{id}}";
            public const string Delete = $"{BASE_API}/role/{{id}}";
        }

        // ======================
        // UNIT
        // ======================
        public static class Unit
        {
            public const string GetAll = $"{BASE_API}/unit";
            public const string GetById = $"{BASE_API}/unit/{{id}}";
            public const string Create = $"{BASE_API}/unit";
            public const string Update = $"{BASE_API}/unit/{{id}}";
            public const string Delete = $"{BASE_API}/unit/{{id}}";
        }

        // ======================
        // USER
        // ======================
        public static class User
        {
            public const string GetAll = $"{BASE_API}/user";
            public const string GetById = $"{BASE_API}/user/{{id}}";
            public const string Create = $"{BASE_API}/user";
            public const string Update = $"{BASE_API}/user/{{id}}";
            public const string Delete = $"{BASE_API}/user/{{id}}";
        }

        // ======================
        // VEHICLE
        // ======================
        public static class Vehicle
        {
            public const string GetAll = $"{BASE_API}/vehicle";
            public const string GetById = $"{BASE_API}/vehicle/{{id}}";
            public const string Create = $"{BASE_API}/vehicle";
            public const string Update = $"{BASE_API}/vehicle/{{id}}";
            public const string Delete = $"{BASE_API}/vehicle/{{id}}";
        }

        // ======================
        // WAREHOUSE
        // ======================
        public static class Warehouse
        {
            public const string GetAll = $"{BASE_API}/warehouse";
            public const string GetById = $"{BASE_API}/warehouse/{{id}}";
            public const string Create = $"{BASE_API}/warehouse";
            public const string Update = $"{BASE_API}/warehouse/{{id}}";
            public const string Delete = $"{BASE_API}/warehouse/{{id}}";
        }
    }
}

