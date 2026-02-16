
using LD.Contracts;
using LD.Contracts.Client;
using LD.Contracts.Location;
using LD.Contracts.Project;
using LD.Contracts.Requests;
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
        public async Task<ApiResponseDto<ProjectDto>> GetLocationById(int projectId)
        {
            return await _api.GetAsync<ApiResponseDto<ProjectDto>>(ApiEndpoints.Project.GetById.Replace("{id}", projectId.ToString()));
        }

        public async Task<ApiResponseDto<List<ProjectDto>>> GetProjects()
        {
            return await _api.GetAsync<ApiResponseDto<List<ProjectDto>>>(ApiEndpoints.Project.GetAll);
        }

        public async Task<ApiResponseDto<string>> CreateProject(ProjectRequest request)
        {
            return await _api.PostAsync<ProjectRequest, ApiResponseDto<string>>(ApiEndpoints.Project.Create, request);
        }

        public async Task<ApiResponseDto<string>> UpdateProject(int projectId, ProjectRequest request)
        {
            return await _api.PutAsync<ProjectRequest, ApiResponseDto<string>>(ApiEndpoints.Project.Update.Replace("{id}", projectId.ToString()), request);
        }

        public async Task<ApiResponseDto<string>> ArchiveLocation(int projectId)
        {
            return await _api.DeleteAsync<ApiResponseDto<string>>(ApiEndpoints.Project.Delete.Replace("{id}", projectId.ToString()));
        }
    }
}
