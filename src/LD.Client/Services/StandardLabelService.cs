using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Contracts.DTOs.StandardLabel;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class StandardLabelService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public StandardLabelService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<List<string>>> GenerateStandardIds(int quantity)
    {
        var request = new GenerateStandardLabelsRequest
        {
            Quantity = quantity
        };

        return await _api.PostAsync<GenerateStandardLabelsRequest, ApiResponseDto<List<string>>>(
            _apiEndpoints.StandardLabel_Generate,
            request);
    }

    public async Task<ApiResponseDto<StandardLabelDto?>> GetByCode(string code)
    {
        return await _api.GetAsync<ApiResponseDto<StandardLabelDto?>>(
            _apiEndpoints.StandardLabel_GetByCode.Replace("{code}", Uri.EscapeDataString(code.Trim())));
    }
}
