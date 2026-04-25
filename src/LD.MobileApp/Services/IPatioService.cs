using MauiAppLogin.Models;

namespace MauiAppLogin.Services;

public interface IPatioService
{
    Task<List<VehiculoEnPatio>> GetVehiculosSinSalidaAsync();
    Task<List<Cortina>> GetCortinasDisponiblesAsync();
    Task<bool> AsignarCortinaAsync(int vehiculoId, int cortinaId);
    void RegistrarVehiculo(VehiculoEnPatio vehiculo);
}
