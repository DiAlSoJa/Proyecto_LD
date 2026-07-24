using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IKittingFolioCaptureRepository : IRepository<KittingFolioCapture>
{
    Task<List<KittingFolioCapture>> GetManyWithRelationsAsync(int? clientId = null, int? projectId = null);
    Task<KittingFolioCapture?> GetByIdWithRelationsAsync(int id);
}
