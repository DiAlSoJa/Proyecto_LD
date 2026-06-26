using LD.Contracts.DTOs.DeliveryOrder;
using LD.Contracts.Requests;
using LD.Contracts.Responses;
using LD.Forms.Configuration;

namespace LD.Client.Services;

public class DeliveryOrderService
{
    private readonly ApiService _api;
    private readonly ApiEndpoints _apiEndpoints;

    public DeliveryOrderService(ApiService api, ApiEndpoints apiEndpoints)
    {
        _api = api;
        _apiEndpoints = apiEndpoints;
    }

    public async Task<ApiResponseDto<string>> CreateDeliveryOrderFromKittings(List<int> kittingIds)
    {
        var request = new CreateDeliveryOrderRequest
        {
            KittingIds = kittingIds ?? new List<int>()
        };

        return await _api.PostAsync<CreateDeliveryOrderRequest, ApiResponseDto<string>>(
            _apiEndpoints.DeliveryOrder_CreateFromKittings,
            request);
    }

    public async Task<ApiResponseDto<string>> AddKittingsToExistingDeliveryOrder(string? deliveryOrderCode, List<int> kittingIds)
    {
        var request = new AddKittingsToDeliveryOrderRequest
        {
            DeliveryOrderCode = deliveryOrderCode,
            KittingIds = kittingIds ?? new List<int>()
        };

        return await _api.PostAsync<AddKittingsToDeliveryOrderRequest, ApiResponseDto<string>>(
            _apiEndpoints.DeliveryOrder_AddKittingsToExisting,
            request);
    }

    public async Task<ApiResponseDto<FinishDeliveryOrderLoadingResultDto>> FinishDeliveryOrderLoading(string? deliveryOrderCode)
    {
        var normalizedCode = deliveryOrderCode?.Trim();
        if (string.IsNullOrWhiteSpace(normalizedCode))
        {
            return new ApiResponseDto<FinishDeliveryOrderLoadingResultDto>
            {
                IsSuccess = false,
                Code = 400,
                Message = "Debes seleccionar una orden de entrega."
            };
        }

        var request = new FinishDeliveryOrderLoadingRequest
        {
            DeliveryOrderCode = normalizedCode
        };

        return await _api.PostAsync<FinishDeliveryOrderLoadingRequest, ApiResponseDto<FinishDeliveryOrderLoadingResultDto>>(
            _apiEndpoints.DeliveryOrder_FinishLoading,
            request);
    }
}
