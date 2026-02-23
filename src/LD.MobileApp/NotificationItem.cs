using System;
using System.Collections.Generic;
using System.Text;

namespace MauiAppLogin;

public sealed class NotificationItem
{
    public string Title { get; set; } = "";
    public string? Detail { get; set; }   // opcional
    public bool IsRead { get; set; }

    // Ruta o clave para decidir navegación
    public string Target { get; set; } = "";   // ej: "CasetaEntrada", "InventarioGDL", etc.
}