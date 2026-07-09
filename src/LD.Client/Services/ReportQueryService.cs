using LD.Contracts.DTOs.ReportQueries;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class ReportQueryService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public ReportQueryService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<ReportQuerySummaryDto>>> GetReportQuerySummariesAsync()
    {
        return await _api.GetAsync<ApiResponseDto<List<ReportQuerySummaryDto>>>(
            _apiEndpoints.ReportQuery_GetSummaries);
    }

    public async Task<ApiResponseDto<List<ReportQueryDto>>> GetReportQueriesAsync()
    {
        return await _api.GetAsync<ApiResponseDto<List<ReportQueryDto>>>(
            _apiEndpoints.ReportQuery_GetAll);
    }

    public async Task<ApiResponseDto<ReportQueryDto>> GetReportQueryByIdAsync(int reportQueryId)
    {
        return await _api.GetAsync<ApiResponseDto<ReportQueryDto>>(
            _apiEndpoints.ReportQuery_GetById.Replace("{reportQueryId}", reportQueryId.ToString()));
    }

    public async Task<ApiResponseDto<List<ReportQueryParameterDto>>> GetReportQueryParametersAsync(int reportQueryId)
    {
        return await _api.GetAsync<ApiResponseDto<List<ReportQueryParameterDto>>>(
            _apiEndpoints.ReportQuery_GetParameters.Replace("{reportQueryId}", reportQueryId.ToString()));
    }

    public async Task<ApiResponseDto<string>> CreateReportQueryAsync(ReportQueryRequest request)
    {
        return await _api.PostAsync<ReportQueryRequest, ApiResponseDto<string>>(
            _apiEndpoints.ReportQuery_Create,
            request);
    }

    public async Task<ApiResponseDto<string>> UpdateReportQueryAsync(int reportQueryId, ReportQueryRequest request)
    {
        return await _api.PutAsync<ReportQueryRequest, ApiResponseDto<string>>(
            _apiEndpoints.ReportQuery_Update.Replace("{reportQueryId}", reportQueryId.ToString()),
            request);
    }

    public async Task<ApiResponseDto<string>> DeleteReportQueryAsync(int reportQueryId)
    {
        return await _api.DeleteAsync<ApiResponseDto<string>>(
            _apiEndpoints.ReportQuery_Delete.Replace("{reportQueryId}", reportQueryId.ToString()));
    }

    public async Task<ApiResponseDto<ReportQueryExecutionResultDto>> ExecuteReportQueryAsync(
        int reportQueryId,
        ReportQueryExecutionRequest request)
    {
        return await _api.PostAsync<ReportQueryExecutionRequest, ApiResponseDto<ReportQueryExecutionResultDto>>(
            _apiEndpoints.ReportQuery_Execute.Replace("{reportQueryId}", reportQueryId.ToString()),
            request);
    }
}
