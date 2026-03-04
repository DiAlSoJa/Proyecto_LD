using LD.Contracts.Client;
using LD.Contracts.Location;
using LD.Contracts.Project;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD.Client
{
    public class ProjectService
    {
        public readonly ApiService _api;
        private readonly ApiEndpoints _apiEndpoints;
        public ProjectService(ApiEndpoints apiEndpoints, ApiService api)
        {
            _api = api;
            //_api.SetBearerToken(UserSession.AccessToken ?? "");
            _apiEndpoints = apiEndpoints;
        }
        public async Task<ApiResponseDto<ProjectDto>> GetProjectById(int projectId)
        {
            return await _api.GetAsync<ApiResponseDto<ProjectDto>>(_apiEndpoints.Project_GetById.Replace("{id}", projectId.ToString()));
        }

        public async Task<ApiResponseDto<List<ProjectDto?>>> GetProjects()
        {
            return await _api.GetAsync<ApiResponseDto<List<ProjectDto?>>>(_apiEndpoints.Project_GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateProject(ProjectRequest request)
        {
            return await _api.PostAsync<ProjectRequest, ApiResponseDto<string>>(_apiEndpoints.Project_Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateProject(int projectId, ProjectRequest request)
        {
            return await _api.PutAsync<ProjectRequest, ApiResponseDto<string>>(_apiEndpoints.Project_Update.Replace("{id}", projectId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveLocation(int projectId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(_apiEndpoints.Project_Delete.Replace("{id}", projectId.ToString()));
        }
    }
}
