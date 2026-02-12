

using LD.Contracts.Item;
using LD.Forms.Classes;
using LD.Forms.Classes.DTOs;
using LD.Forms.Configuration;

namespace LD.Forms.Services
{
    public class ItemService
    {
        public readonly ApiService _api;
        public ItemService()
        {
            _api = new ApiService();
            _api.SetBearerToken(UserSession.AccessToken??"");
        }

        public async Task<ApiResponseDto<List<ItemDto>>> GetItems()
        {
            return await _api.GetAsync<ApiResponseDto<List<ItemDto>>>(ApiEndpoints.Item.GetAll);
        }
    }
}
