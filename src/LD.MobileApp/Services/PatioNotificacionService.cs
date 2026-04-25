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
            Id       = _nextId++,
            Placa    = vehiculo.Placa,
            Cortina  = cortina.Numero,
            Operador = vehiculo.Operador,
            FechaHora = DateTime.Now,
            Leida    = false
        });
        return Task.CompletedTask;
    }

    public Task<List<PatioNotificacion>> GetNotificacionesPendientesAsync()
        => Task.FromResult(_notificaciones.Where(n => !n.Leida).ToList());
}
