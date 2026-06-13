namespace LD.Application.Common.Interfaces;

public interface ITaskAssignmentTracker
{
    void AssignTask(string userId, int taskId);
    void ClearTask(string userId);
}
