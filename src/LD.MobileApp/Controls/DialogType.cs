namespace MauiAppLogin.Controls;

public enum DialogType
{
    Info,       // ícono informativo, un botón "Entendido"
    Success,    // ícono éxito, un botón "Aceptar"
    Warning,    // ícono advertencia, dos botones "Cancelar" / "Continuar"
    Error,      // ícono error, un botón "Cerrar"
    Blocking    // sin botón de cerrar (checklist obligatorio pendiente)
}
