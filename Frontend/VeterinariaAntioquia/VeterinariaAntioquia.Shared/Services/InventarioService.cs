namespace VeterinariaAntioquia.Shared.Services;

// ═══════════════════════════════════════════════════════════════
//  InventarioService.cs — Servicio del módulo Inventario
//
//  Cubre todos los endpoints de /api/inventario:
//    GET    /api/inventario                      → lista completa
//    GET    /api/inventario/alertas              → stock bajo / vencidos
//    POST   /api/inventario/medicamentos         → crear medicamento + stock
//    PUT    /api/inventario/medicamentos/:id     → editar datos del medicamento
//    PUT    /api/inventario/stock/:idMedicamento → ajustar stock
//    DELETE /api/inventario/medicamentos/:id     → eliminar medicamento
// ═══════════════════════════════════════════════════════════════

using System.Net;
using System.Text;
using System.Text.Json;
using VeterinariaAntioquia.Shared.Models;

public class InventarioService
{
    private readonly JsonSerializerOptions _jsonOpts = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
    };

    private readonly HttpClient _http;

    public InventarioService(HttpClient http) => _http = http;

    // ── Helpers internos ──────────────────────────────────────

    private StringContent Json(object payload) =>
        new(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

    private async Task<string?> ReadErrorAsync(System.Net.Http.HttpResponseMessage r)
    {
        try
        {
            var body = await r.Content.ReadAsStringAsync();
            var doc  = JsonSerializer.Deserialize<JsonElement>(body, _jsonOpts);
            return doc.TryGetProperty("error", out var e) ? e.GetString() : null;
        }
        catch { return null; }
    }

    // ═══════════════════════════════════════════════════════════
    //  GET /api/inventario
    // ═══════════════════════════════════════════════════════════
    public async Task<(List<MedicamentoInventario> data, string? error)> GetInventarioAsync()
    {
        try
        {
            var r = await _http.GetAsync("api/inventario");

            if (r.StatusCode == HttpStatusCode.Unauthorized)
                return (new(), "Sesión expirada.");
            if (r.StatusCode == HttpStatusCode.Forbidden)
                return (new(), "No tienes permiso para ver el inventario.");

            var body = await r.Content.ReadAsStringAsync();

            if (!r.IsSuccessStatusCode)
            {
                Console.WriteLine($"[InventarioService.Get] {(int)r.StatusCode}: {body}");
                return (new(), "No se pudo cargar el inventario.");
            }

            var result = JsonSerializer.Deserialize<List<MedicamentoInventario>>(body, _jsonOpts);
            return (result ?? new(), null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[InventarioService.Get] EXCEPTION: {ex.Message}");
            return (new(), "Error de conexión al cargar el inventario.");
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  GET /api/inventario/alertas
    // ═══════════════════════════════════════════════════════════
    public async Task<(List<MedicamentoInventario> data, string? error)> GetAlertasAsync()
    {
        try
        {
            var r    = await _http.GetAsync("api/inventario/alertas");
            var body = await r.Content.ReadAsStringAsync();

            if (!r.IsSuccessStatusCode)
                return (new(), "No se pudieron cargar las alertas.");

            var result = JsonSerializer.Deserialize<List<MedicamentoInventario>>(body, _jsonOpts);
            return (result ?? new(), null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[InventarioService.Alertas] EXCEPTION: {ex.Message}");
            return (new(), "Error de conexión.");
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  POST /api/inventario/medicamentos
    // ═══════════════════════════════════════════════════════════
    public async Task<(MedicamentoInventario? data, string? error)> CrearMedicamentoAsync(MedicamentoRequest req)
    {
        try
        {
            var r    = await _http.PostAsync("api/inventario/medicamentos", Json(req));
            var body = await r.Content.ReadAsStringAsync();

            if (r.StatusCode == HttpStatusCode.Unauthorized) return (null, "Sesión expirada.");
            if (r.StatusCode == HttpStatusCode.Conflict)     return (null, await ReadErrorAsync(r) ?? "Nombre duplicado.");
            if (r.StatusCode == HttpStatusCode.BadRequest)   return (null, await ReadErrorAsync(r) ?? "Datos inválidos.");

            if (!r.IsSuccessStatusCode)
            {
                Console.WriteLine($"[InventarioService.Crear] {(int)r.StatusCode}: {body}");
                return (null, "No se pudo crear el medicamento.");
            }

            var result = JsonSerializer.Deserialize<MedicamentoInventario>(body, _jsonOpts);
            return (result, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[InventarioService.Crear] EXCEPTION: {ex.Message}");
            return (null, "Error de conexión al crear el medicamento.");
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  PUT /api/inventario/medicamentos/:id
    // ═══════════════════════════════════════════════════════════
    public async Task<(Medicamento? data, string? error)> EditarMedicamentoAsync(int id, MedicamentoRequest req)
    {
        try
        {
            var r = await _http.PutAsync($"api/inventario/medicamentos/{id}", Json(req));

            if (r.StatusCode == HttpStatusCode.Unauthorized) return (null, "Sesión expirada.");
            if (r.StatusCode == HttpStatusCode.NotFound)     return (null, "Medicamento no encontrado.");
            if (r.StatusCode == HttpStatusCode.Conflict)     return (null, await ReadErrorAsync(r) ?? "Nombre duplicado.");
            if (r.StatusCode == HttpStatusCode.BadRequest)   return (null, await ReadErrorAsync(r) ?? "Datos inválidos.");

            if (!r.IsSuccessStatusCode)
                return (null, "No se pudo actualizar el medicamento.");

            var body   = await r.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Medicamento>(body, _jsonOpts);
            return (result, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[InventarioService.Editar] EXCEPTION: {ex.Message}");
            return (null, "Error de conexión al actualizar.");
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  PUT /api/inventario/stock/:idMedicamento
    // ═══════════════════════════════════════════════════════════
    public async Task<(bool ok, string? error)> AjustarStockAsync(int idMedicamento, AjusteStockRequest req)
    {
        try
        {
            var r = await _http.PutAsync($"api/inventario/stock/{idMedicamento}", Json(req));

            if (r.StatusCode == HttpStatusCode.Unauthorized) return (false, "Sesión expirada.");
            if (r.StatusCode == HttpStatusCode.NotFound)     return (false, "Medicamento no encontrado.");
            if (r.StatusCode == HttpStatusCode.BadRequest)   return (false, await ReadErrorAsync(r) ?? "Datos inválidos.");

            if (!r.IsSuccessStatusCode)
                return (false, "No se pudo actualizar el stock.");

            return (true, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[InventarioService.AjustarStock] EXCEPTION: {ex.Message}");
            return (false, "Error de conexión al ajustar el stock.");
        }
    }

    // ═══════════════════════════════════════════════════════════
    //  DELETE /api/inventario/medicamentos/:id
    // ═══════════════════════════════════════════════════════════
    public async Task<(bool ok, string? error)> EliminarMedicamentoAsync(int id)
    {
        try
        {
            var r = await _http.DeleteAsync($"api/inventario/medicamentos/{id}");

            if (r.StatusCode == HttpStatusCode.Unauthorized) return (false, "Sesión expirada.");
            if (r.StatusCode == HttpStatusCode.NotFound)     return (false, "Medicamento no encontrado.");
            if (r.StatusCode == HttpStatusCode.Conflict)     return (false, await ReadErrorAsync(r) ?? "No se puede eliminar.");

            if (!r.IsSuccessStatusCode)
                return (false, "No se pudo eliminar el medicamento.");

            return (true, null);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[InventarioService.Eliminar] EXCEPTION: {ex.Message}");
            return (false, "Error de conexión al eliminar.");
        }
    }
}
