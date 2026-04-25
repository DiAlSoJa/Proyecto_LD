using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using MauiAppLogin.Models;

namespace MauiAppLogin.Services;

public class PatioService : IPatioService
{
    private readonly PatioClientService _patioClient;

    private readonly List<Cortina> _cortinas = new()
    {
        new() { Id = 1, Numero = "C-01", Descripcion = "Cortina 1 — Muelle Norte", EstaDisponible = true },
        new() { Id = 2, Numero = "C-02", Descripcion = "Cortina 2 — Muelle Norte", EstaDisponible = true },
        new() { Id = 3, Numero = "C-03", Descripcion = "Cortina 3 — Muelle Sur",   EstaDisponible = true },
        new() { Id = 4, Numero = "C-04", Descripcion = "Cortina 4 — Muelle Sur",   EstaDisponible = true },
        new() { Id = 5, Numero = "C-05", Descripcion = "Cortina 5 — Muelle Este",  EstaDisponible = true },
        new() { Id = 6, Numero = "C-06", Descripcion = "Cortina 6 — Muelle Este",  EstaDisponible = true },
    };

    private readonly List<VehiculoEnPatio> _vehiculosLocales = new();
    private int _nextVehiculoId = 1;

    public PatioService(PatioClientService patioClient)
    {
        _patioClient = patioClient;
    }

    public async Task<List<SecurityRegistrationDto>> GetVehiculosSinSalidaAsync()
    {
        try
        {
            var response = await _patioClient.GetVehiculosSinSalidaAsync();
            return response.IsSuccess ? (response.Data ?? []) : [];
        }
        catch
        {
            return [];
        }
    }

    public Task<List<Cortina>> GetCortinasDisponiblesAsync()
        => Task.FromResult(_cortinas.Where(c => c.EstaDisponible).ToList());

    public Task<bool> AsignarCortinaAsync(int vehiculoId, int cortinaId)
    {
        var vehiculo = _vehiculosLocales.FirstOrDefault(v => v.Id == vehiculoId);
        var cortina  = _cortinas.FirstOrDefault(c => c.Id == cortinaId);

        if (vehiculo is null || cortina is null || !cortina.EstaDisponible)
            return Task.FromResult(false);

        cortina.EstaDisponible   = false;
        vehiculo.CortinaAsignada = cortina.Numero;
        return Task.FromResult(true);
    }

    public void RegistrarVehiculo(VehiculoEnPatio vehiculo)
    {
        vehiculo.Id = _nextVehiculoId++;
        _vehiculosLocales.Add(vehiculo);
    }
}
