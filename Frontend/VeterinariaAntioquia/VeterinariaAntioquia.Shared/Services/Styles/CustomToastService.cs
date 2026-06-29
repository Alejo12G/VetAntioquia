namespace VeterinariaAntioquia.Shared.Services.Styles;
using VeterinariaAntioquia.Shared.Models.Styles;

/// <summary>
/// Servicio responsable de gestionar la emisión de notificaciones tipo Toast
/// a lo largo de toda la aplicación. Utiliza un patrón de eventos para 
/// comunicar notificaciones desde cualquier capa de la aplicación hacia la vista.
/// </summary>
public class CustomToastService
{
    /// <summary>
    /// Evento disparado cuando se solicita mostrar una nueva notificación.
    /// </summary>
    public event Action<string, ToastType>? OnShow;

    public void ShowSuccess(string message) => OnShow?.Invoke(message, ToastType.Success);
    public void ShowInfo(string message) => OnShow?.Invoke(message, ToastType.Info);
    public void ShowWarning(string message) => OnShow?.Invoke(message, ToastType.Warning);
    public void ShowError(string message) => OnShow?.Invoke(message, ToastType.Error);
}