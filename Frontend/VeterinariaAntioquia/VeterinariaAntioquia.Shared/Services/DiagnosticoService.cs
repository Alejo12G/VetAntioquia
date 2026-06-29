namespace VeterinariaAntioquia.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;
using VeterinariaAntioquia.Shared.Models;

public class DiagnosticoService
{
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _http;

    public DiagnosticoService(HttpClient http) => _http = http;

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

    public async Task<(Diagnostico? data, string? error)> RegistrarDiagnosticoAsync(DiagnosticoRequest req)
    {
        try
        {
            var r = await _http.PostAsync("api/diagnosticos", Json(req));
            if (!r.IsSuccessStatusCode)
                return (null, await ReadErrorAsync(r) ?? "Error al registrar el diagnóstico.");

            var body = await r.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Diagnostico>(body, _jsonOpts);
            return (result, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DiagnosticoService] POST Exception: {ex.Message}");
            return (null, "Error de conexión.");
        }
    }

    public async Task<(Diagnostico? data, string? error)> GetDiagnosticoDeCitaAsync(int citaId)
    {
        try
        {
            var r = await _http.GetAsync($"api/diagnosticos/cita/{citaId}");
            if (r.StatusCode == HttpStatusCode.NotFound)
                return (null, null); // Sin error, solo no hay diagnóstico aún
            if (!r.IsSuccessStatusCode)
                return (null, await ReadErrorAsync(r) ?? "Error al obtener el diagnóstico.");

            var body = await r.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(body) || body == "null")
                return (null, null);

            var result = JsonSerializer.Deserialize<Diagnostico>(body, _jsonOpts);
            return (result, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DiagnosticoService] GET Exception: {ex.Message}");
            return (null, "Error de conexión.");
        }
    }
}
