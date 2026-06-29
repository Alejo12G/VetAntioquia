namespace VeterinariaAntioquia.Shared.Services;

// ═══════════════════════════════════════════════════════════════
//  EspecieService.cs — Servicio de acceso a datos del módulo Especies
//
//  Responsabilidades:
//    · Encapsular todas las llamadas HTTP al API de especies.
//    · Devolver tuplas (data, error) para que los componentes Razor
//      puedan mostrar mensajes amigables sin try/catch en la UI.
//    · El token Bearer se inyecta automáticamente vía AuthTokenHandler.
//
//  Endpoints cubiertos:
//    GET    /api/especies
//    POST   /api/especies
//    PUT    /api/especies/:id
//    DELETE /api/especies/:id
// ═══════════════════════════════════════════════════════════════

using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using VeterinariaAntioquia.Shared.Models;

public class EspecieService
{
    private readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _http;

    public EspecieService(HttpClient http)
    {
        _http = http;
    }

    // ═══════════════════════════════════════════════════════════
    //  GET /api/especies
    //  Devuelve todas las especies registradas.
    // ═══════════════════════════════════════════════════════════
    public async Task<(List<Especie> data, string? error)> GetEspeciesAsync()
    {
        try
        {
            var response = await _http.GetAsync("api/especies");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (new(), "Sesión expirada. Vuelve a iniciar sesión.");

            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[EspecieService.Get] HTTP {(int)response.StatusCode}: {body}");
                return (new(), "No se pudieron cargar las especies.");
            }

            var result = JsonSerializer.Deserialize<List<Especie>>(body, _jsonOpts);
            return (result ?? new(), null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EspecieService.Get] EXCEPTION: {ex.Message}");
            return (new(), "Error de conexión al cargar especies.");
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  POST /api/especies
    //  Crea una nueva especie.
    //  Body: { nombre, nombreCientifico?, caracteristicas? }
    // ═══════════════════════════════════════════════════════════
    public async Task<(Especie? data, string? error)> CrearEspecieAsync(
        string nombre,
        string? nombreCientifico,
        string? caracteristicas)
    {
        try
        {
            var payload = JsonSerializer.Serialize(new
            {
                nombre,
                nombreCientifico,
                caracteristicas
            });

            var content  = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/especies", content);
            var body     = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (null, "Sesión expirada. Vuelve a iniciar sesión.");

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var err = JsonSerializer.Deserialize<JsonElement>(body, _jsonOpts);
                return (null, err.GetProperty("error").GetString() ?? "Nombre duplicado.");
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[EspecieService.Crear] HTTP {(int)response.StatusCode}: {body}");
                return (null, "No se pudo crear la especie.");
            }

            var result = JsonSerializer.Deserialize<Especie>(body, _jsonOpts);
            return (result, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EspecieService.Crear] EXCEPTION: {ex.Message}");
            return (null, "Error de conexión al crear la especie.");
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  PUT /api/especies/:id
    //  Edita una especie existente.
    //  Body: { nombre, nombreCientifico?, caracteristicas? }
    // ═══════════════════════════════════════════════════════════
    public async Task<(Especie? data, string? error)> EditarEspecieAsync(
        int id,
        string nombre,
        string? nombreCientifico,
        string? caracteristicas)
    {
        try
        {
            var payload = JsonSerializer.Serialize(new
            {
                nombre,
                nombreCientifico,
                caracteristicas
            });

            var content  = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await _http.PutAsync($"api/especies/{id}", content);
            var body     = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (null, "Sesión expirada. Vuelve a iniciar sesión.");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return (null, "La especie no existe.");

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var err = JsonSerializer.Deserialize<JsonElement>(body, _jsonOpts);
                return (null, err.GetProperty("error").GetString() ?? "Nombre duplicado.");
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[EspecieService.Editar] HTTP {(int)response.StatusCode}: {body}");
                return (null, "No se pudo actualizar la especie.");
            }

            var result = JsonSerializer.Deserialize<Especie>(body, _jsonOpts);
            return (result, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EspecieService.Editar] EXCEPTION: {ex.Message}");
            return (null, "Error de conexión al actualizar la especie.");
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  DELETE /api/especies/:id
    //  Elimina una especie.
    //  Retorna (true, null) si fue exitoso, (false, mensaje) si hubo error.
    // ═══════════════════════════════════════════════════════════
    public async Task<(bool ok, string? error)> EliminarEspecieAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/especies/{id}");
            var body     = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (false, "Sesión expirada. Vuelve a iniciar sesión.");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return (false, "La especie no existe.");

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                var err = JsonSerializer.Deserialize<JsonElement>(body, _jsonOpts);
                return (false, err.GetProperty("error").GetString() ?? "No se puede eliminar.");
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[EspecieService.Eliminar] HTTP {(int)response.StatusCode}: {body}");
                return (false, "No se pudo eliminar la especie.");
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[EspecieService.Eliminar] EXCEPTION: {ex.Message}");
            return (false, "Error de conexión al eliminar la especie.");
        }
    }
}
