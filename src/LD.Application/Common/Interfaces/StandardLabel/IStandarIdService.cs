using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.StandarLabel
{
    public interface IStandarIdService
    {
        Task<List<string>> GenerateStandarIdsAsync(int quantity);

        Task<StandardLabel?> GetByStandarIdStrAsync(string standarIdStr);

        Task<bool> IsStandarIdAssignedAsync(int standarId);

        Task<List<StandardLabel>> AssignStandarIdsToReceiptDetailsAsync(
            List<int> asnReceiptDetailIds,
            string userId);
    }
}
