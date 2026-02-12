
using LD.Contracts;
using LD.Contracts.Client;
using LD.Contracts.Project;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Forms.Services
{
    public class ProjectService
    {
        public readonly ApiService _api;
        public ProjectService()
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken??"");
        }

        public async Task<ApiResponseDto<List<ProjectDto>>> GetProjects()
        {
            return await _api.GetAsync<ApiResponseDto<List<ProjectDto>>>(ApiEndpoints.Project.GetAll);
        }
    }
}
