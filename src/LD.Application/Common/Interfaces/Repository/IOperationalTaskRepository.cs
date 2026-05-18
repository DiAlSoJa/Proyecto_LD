using LD.Domain.Entities;

namespace LD.Application.Common.Interfaces.Repository;

public interface IOperationalTaskRepository : IRepository<OperationalTask>
{
    Task<List<OperationalTask>> GetTasksAsync(bool soloPendientes, int? warehouseId);
    Task<OperationalTask?> GetTaskByIdAsync(int operationalTaskId);
}
