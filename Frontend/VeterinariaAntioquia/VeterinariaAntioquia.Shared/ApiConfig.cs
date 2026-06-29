namespace VeterinariaAntioquia.Shared;

/// <summary>
/// Configuración centralizada de la URL del API.
/// En modo DEBUG (Desarrollo) apunta a localhost.
/// En modo RELEASE (Producción) apunta al VPS.
/// </summary>
public static class ApiConfig
{
#if DEBUG
    /// <summary>Desarrollo — API corriendo en localhost</summary>
    public const string BaseUrl = "http://localhost:3000/";
#else
    /// <summary>Producción — API en el VPS</summary>
    public const string BaseUrl = "http://15.235.123.248:3000/";
#endif
}
