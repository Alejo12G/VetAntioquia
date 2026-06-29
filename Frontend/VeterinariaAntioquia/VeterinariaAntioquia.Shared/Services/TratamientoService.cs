namespace VeterinariaAntioquia.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;
using VeterinariaAntioquia.Shared.Models;

public class TratamientoService
{
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _http;

    public TratamientoService(HttpClient http) => _http = http;

    private StringContent Json(object payload) =>
        new(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

    private async Task<string?> ReadErrorAsync(HttpResponseMessage r)
    {
        try
        {
            var body = await r.Content.ReadAsStringAsync();
            var doc = JsonSerializer.Deserialize<JsonElement>(body, _jsonOpts);
            return doc.TryGetProperty("error", out var e) ? e.GetString() : null;
        }
        catch { return null; }
    }

    public async Task<(Tratamiento? data, string? error)> CrearTratamientoAsync(TratamientoRequest req)
    {
        try
        {
            var r = await _http.PostAsync("api/tratamientos", Json(req));
            if (!r.IsSuccessStatusCode)
                return (null, await ReadErrorAsync(r) ?? "Error al recetar el medicamento.");

            var body = await r.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Tratamiento>(body, _jsonOpts);
            return (result, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TratamientoService] POST Exception: {ex.Message}");
            return (null, "Error de conexión.");
        }
    }

    public async Task<(bool ok, string? error)> CambiarEstadoAsync(int id, string nuevoEstado)
    {
        try
        {
            var req = new { estado = nuevoEstado };
            var r = await _http.PatchAsync($"api/tratamientos/{id}/estado", Json(req));
            if (!r.IsSuccessStatusCode)
                return (false, await ReadErrorAsync(r) ?? "Error al cambiar estado.");
            
            return (true, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TratamientoService] PATCH Exception: {ex.Message}");
            return (false, "Error de conexión.");
        }
    }
}
