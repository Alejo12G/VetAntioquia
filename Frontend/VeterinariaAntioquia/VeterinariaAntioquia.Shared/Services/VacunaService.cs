namespace VeterinariaAntioquia.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;
using VeterinariaAntioquia.Shared.Models;

public class VacunaService
{
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _http;

    public VacunaService(HttpClient http) => _http = http;

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

    public async Task<(List<Vacuna> data, string? error)> GetCatalogoAsync()
    {
        try
        {
            var r = await _http.GetAsync("api/vacunas");
            if (!r.IsSuccessStatusCode)
                return (new(), await ReadErrorAsync(r) ?? "Error al obtener vacunas.");

            var body = await r.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<Vacuna>>(body, _jsonOpts);
            return (result ?? new(), null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VacunaService] GET Catalogo: {ex.Message}");
            return (new(), "Error de conexión.");
        }
    }

    public async Task<(List<HistorialVacuna> data, string? error)> GetHistorialAsync(int idMascota)
    {
        try
        {
            var r = await _http.GetAsync($"api/vacunas/mascota/{idMascota}");
            if (!r.IsSuccessStatusCode)
                return (new(), await ReadErrorAsync(r) ?? "Error al obtener el historial.");

            var body = await r.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<HistorialVacuna>>(body, _jsonOpts);
            return (result ?? new(), null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VacunaService] GET Historial: {ex.Message}");
            return (new(), "Error de conexión.");
        }
    }

    public async Task<(bool ok, string? error)> AplicarVacunaAsync(AplicarVacunaRequest req)
    {
        try
        {
            var r = await _http.PostAsync("api/vacunas/aplicar", Json(req));
            if (!r.IsSuccessStatusCode)
                return (false, await ReadErrorAsync(r) ?? "Error al registrar la vacunación.");
            return (true, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VacunaService] POST Aplicar: {ex.Message}");
            return (false, "Error de conexión.");
        }
    }
}
