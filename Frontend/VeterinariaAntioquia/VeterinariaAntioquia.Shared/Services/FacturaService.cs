namespace VeterinariaAntioquia.Shared.Services;
using System.Net;
using System.Text;
using System.Text.Json;
using VeterinariaAntioquia.Shared.Models;

public class FacturaService
{
    private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _http;

    public FacturaService(HttpClient http) => _http = http;

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

    public async Task<(Factura? data, string? error)> GenerarFacturaAsync(GenerarFacturaRequest req)
    {
        try
        {
            var r = await _http.PostAsync("api/facturas", Json(req));
            if (!r.IsSuccessStatusCode)
                return (null, await ReadErrorAsync(r) ?? "Error al generar la factura.");

            var body = await r.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Factura>(body, _jsonOpts);
            return (result, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FacturaService] POST Exception: {ex.Message}");
            return (null, "Error de conexión.");
        }
    }

    public async Task<(bool ok, string? error)> PagarFacturaAsync(int id, PagarFacturaRequest req)
    {
        try
        {
            var r = await _http.PatchAsync($"api/facturas/{id}/pagar", Json(req));
            if (!r.IsSuccessStatusCode)
                return (false, await ReadErrorAsync(r) ?? "Error al procesar el pago.");
            return (true, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[FacturaService] PATCH Pagar: {ex.Message}");
            return (false, "Error de conexión.");
        }
    }
}
