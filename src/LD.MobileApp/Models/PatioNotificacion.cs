namespace MauiAppLogin.Models;

public class PatioNotificacion
{
    public int Id { get; set; }
    public string Placa { get; set; } = "";
    public string Cortina { get; set; } = "";
    public string Operador { get; set; } = "";
    public DateTime FechaHora { get; set; }
    public bool Leida { get; set; }

    public string Titulo => $"Vehículo {Placa} — Cortina {Cortina}";
    public string Subtitulo => $"{FechaHora:dd/MM/yyyy HH:mm} · Operador: {Operador}";
}
