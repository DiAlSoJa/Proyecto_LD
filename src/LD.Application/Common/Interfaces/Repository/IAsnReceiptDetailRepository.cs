using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LD.Contracts.ASN;
using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository
{
    public interface IAsnReceiptDetailRepository : IRepository<AsnReceiptDetailDto>
    {
        Task<List<Asn>> GetAllWithRelationsAsync();
        Task<List<AsnReceiptDetailDto>> GetAsnReceiptByAsnIdAsync(int detailAsnId);
    }
}
