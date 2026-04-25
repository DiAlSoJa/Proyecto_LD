using MauiAppLogin.Models;

namespace MauiAppLogin.Services;

public class PatioNotificacionService : IPatioNotificacionService
{
    private readonly List<PatioNotificacion> _notificaciones = new();
    private int _nextId = 1;

    public Task NotificarAsignacionCortinaAsync(VehiculoEnPatio vehiculo, Cortina cortina)
    {
        _notificaciones.Add(new PatioNotificacion
        {
            Id        = _nextId++,
            Placa     = vehiculo.Placa,
            Cortina   = cortina.Numero,
            Operador  = vehiculo.Operador,
            FechaHora = DateTime.Now,
            Leida     = false,
            Completada = false
        });
        return Task.CompletedTask;
    }

    public Task<List<PatioNotificacion>> GetNotificacionesPendientesAsync()
        => Task.FromResult(_notificaciones.Where(n => !n.Completada).ToList());

    public Task<List<PatioNotificacion>> GetTodasAsync()
        => Task.FromResult(_notificaciones.ToList());

    public Task<bool> CompletarTareaAsync(int id)
    {
        var tarea = _notificaciones.FirstOrDefault(n => n.Id == id);
        if (tarea is null) return Task.FromResult(false);

        tarea.Completada = true;
        tarea.Leida      = true;
        return Task.FromResult(true);
    }
}
