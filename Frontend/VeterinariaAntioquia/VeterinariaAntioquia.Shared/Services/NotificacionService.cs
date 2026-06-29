namespace VeterinariaAntioquia.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;
using VeterinariaAntioquia.Shared.Models;

public class NotificacionService
{
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _http;

    public NotificacionService(HttpClient http) => _http = http;

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

    public async Task<(List<Notificacion> data, string? error)> GetMisNotificacionesAsync()
    {
        try
        {
            var r = await _http.GetAsync("api/notificaciones");
            if (!r.IsSuccessStatusCode)
                return (new(), await ReadErrorAsync(r) ?? "Error al obtener notificaciones.");

            var body = await r.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Notificacion>>(body, _jsonOpts);
            return (result ?? new(), null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NotificacionService] GET Exception: {ex.Message}");
            return (new(), "Error de conexión.");
        }
    }

    public async Task<(bool ok, string? error)> MarcarComoLeidaAsync(string id)
    {
        try
        {
            var r = await _http.PatchAsync($"api/notificaciones/{id}/leer", null);
            if (!r.IsSuccessStatusCode)
                return (false, await ReadErrorAsync(r) ?? "Error al marcar notificacion.");
            return (true, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[NotificacionService] PATCH Exception: {ex.Message}");
            return (false, "Error de conexión.");
        }
    }
}
