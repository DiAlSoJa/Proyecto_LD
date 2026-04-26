using LD.Client.Services;
using LD.Contracts.DTOs.Security;
using MauiAppLogin.Models;

namespace MauiAppLogin.Services;

public class PatioService : IPatioService
{
    private readonly PatioClientService _patioClient;

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
        catch { return []; }
    }

    public async Task<List<Cortina>> GetCortinasDisponiblesAsync()
    {
        try
        {
            var response = await _patioClient.GetCortinasDisponiblesAsync();
            if (!response.IsSuccess || response.Data is null) return [];
            return response.Data
                .Select(d => new Cortina { Id = d.CortinaId, Numero = d.Numero, Descripcion = d.Descripcion, EstaDisponible = d.EstaDisponible })
                .ToList();
        }
        catch { return []; }
    }

    public async Task<bool> AsignarCortinaAsync(int vehiculoId, int cortinaId)
    {
        try
        {
            var response = await _patioClient.AsignarCortinaAsync(vehiculoId, cortinaId);
            return response.IsSuccess;
        }
        catch { return false; }
    }

    public async Task<List<SecurityTaskDto>> GetTasksAsync(bool soloPendientes = false)
    {
        try
        {
            var response = await _patioClient.GetTasksAsync(soloPendientes);
            return response.IsSuccess ? (response.Data ?? []) : [];
        }
        catch { return []; }
    }

    public async Task<bool> AbrirCortinaAsync(int taskId, string? realizadaPor = null)
    {
        try
        {
            var response = await _patioClient.AbrirCortinaAsync(taskId, realizadaPor);
            return response.IsSuccess;
        }
        catch { return false; }
    }

    public async Task<bool> CerrarRegistroAsync(int taskId, string? realizadaPor = null)
    {
        try
        {
            var response = await _patioClient.CerrarRegistroAsync(taskId, realizadaPor);
            return response.IsSuccess;
        }
        catch { return false; }
    }
}
