using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Forms.Configuration;

namespace LD.Client.Services
{
    public class UnitService
    {
        private readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public UnitService(ApiService api, ApiEndpoints apiEndpoints)
        {
            _api = api;
            _apiEndpoints = apiEndpoints;
        }
    }
}
