using LD.Contracts.DTOs.Security;
using MauiAppLogin.Models;

namespace MauiAppLogin.Services;

public interface IPatioService
{
    Task<List<SecurityRegistrationDto>> GetVehiculosSinSalidaAsync();
    Task<List<Cortina>> GetCortinasDisponiblesAsync();
    Task<bool> AsignarCortinaAsync(int vehiculoId, int cortinaId);
    Task<List<SecurityTaskDto>> GetTasksAsync(bool soloPendientes = false);
    Task<bool> AbrirCortinaAsync(int taskId, string? realizadaPor = null);
    Task<bool> CerrarRegistroAsync(int taskId, string? realizadaPor = null);
}
