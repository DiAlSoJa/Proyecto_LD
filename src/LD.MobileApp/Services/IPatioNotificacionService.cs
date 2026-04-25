using MauiAppLogin.Models;

namespace MauiAppLogin.Services;

public interface IPatioNotificacionService
{
    Task NotificarAsignacionCortinaAsync(VehiculoEnPatio vehiculo, Cortina cortina);
    Task<List<PatioNotificacion>> GetNotificacionesPendientesAsync();
    Task<List<PatioNotificacion>> GetTodasAsync();
    Task<bool> CompletarTareaAsync(int id);
}
