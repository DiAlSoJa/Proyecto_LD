using MediatR;

namespace LD.Application.Features.OperationalTasks.Notifications;

public class TaskCompletedNotification : INotification
{
    public int CompletedTaskId { get; init; }
    public string CompletedByUserId { get; init; } = string.Empty;
}
