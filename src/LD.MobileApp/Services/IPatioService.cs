using LD.Contracts.DTOs.Security;
using MauiAppLogin.Models;

namespace MauiAppLogin.Services;

public interface IPatioService
{
    Task<List<SecurityRegistrationDto>> GetVehiculosSinSalidaAsync();
    Task<List<Cortina>> GetCortinasDisponiblesAsync();
    Task<bool> AsignarCortinaAsync(int vehiculoId, int cortinaId);
    void RegistrarVehiculo(VehiculoEnPatio vehiculo);
}
