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
    /// <summary>Producción — API a través de Nginx (Puerto 80)</summary>
    public const string BaseUrl = "http://veterinaria.dz-rp.com/";
#endif
}
