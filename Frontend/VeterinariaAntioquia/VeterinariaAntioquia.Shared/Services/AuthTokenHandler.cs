namespace VeterinariaAntioquia.Shared.Services;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VeterinariaAntioquia.Shared.Models;

/// <summary>
/// DelegatingHandler que intercepta cada request HTTP para:
/// 1. Adjuntar automáticamente el Access Token como header Bearer.
/// 2. Si el API responde 401/403, intentar renovar el token usando
///    el Refresh Token vía POST /api/auth/refresh.
/// 3. Si la renovación es exitosa, reintentar el request original
///    con el nuevo Access Token (transparente para el caller).
/// 4. Si la renovación falla, la respuesta 401/403 original se
///    propaga al caller para que maneje el logout.
/// </summary>
public class AuthTokenHandler : DelegatingHandler
{
    private readonly IStorageService _storage;

    // Semáforo para evitar múltiples refreshes simultáneos
    // cuando varios requests fallan al mismo tiempo.
    private static readonly SemaphoreSlim _refreshLock = new(1, 1);

    public AuthTokenHandler(IStorageService storage)
    {
        _storage = storage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        try
        {
            // ── 1. Adjuntar el access token al request ──
            var accessToken = await _storage.GetItemAsync("accessToken");

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }
        catch (InvalidOperationException)
        {
            // JS Interop no disponible en prerender — ignorar y continuar sin token
        }

        // ── 2. Enviar el request original ──
        var response = await base.SendAsync(request, cancellationToken);

        // ── 3. Si 401/403, intentar renovar el token ──
        // No intentamos refresh en rutas de autenticación para evitar loops.
        var path = request.RequestUri?.AbsolutePath ?? "";
        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden
            && !path.Contains("/auth/refresh")
            && !path.Contains("/auth/login")
            && !path.Contains("/auth/register"))
        {
            var refreshed = await TryRefreshTokenAsync(cancellationToken);
            if (refreshed)
            {
                // ── 4. Reintentar el request con el nuevo token ──
                var newAccessToken = await _storage.GetItemAsync("accessToken");
                var retryRequest = await CloneHttpRequestMessageAsync(request);
                retryRequest.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", newAccessToken);

                // Descartamos la respuesta original y retornamos la del retry
                response.Dispose();
                response = await base.SendAsync(retryRequest, cancellationToken);
            }
        }

        return response;
    }

    /// <summary>
    /// Intenta renovar el access token usando el refresh token almacenado.
    /// Usa un semáforo para evitar que múltiples requests simultáneos
    /// disparen múltiples llamadas al endpoint /refresh.
    /// </summary>
    private async Task<bool> TryRefreshTokenAsync(CancellationToken ct)
    {
        // Esperar máximo 10 segundos para adquirir el lock
        if (!await _refreshLock.WaitAsync(TimeSpan.FromSeconds(10), ct))
            return false;

        try
        {
            var refreshToken = await _storage.GetItemAsync("refreshToken");
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            // Construimos el request manualmente para enviarlo a través de
            // base.SendAsync (que usa el InnerHandler), evitando recursión
            // infinita ya que este handler no volverá a interceptar el refresh.
            var refreshRequest = new HttpRequestMessage(HttpMethod.Post, "api/auth/refresh")
            {
                Content = JsonContent.Create(new { refreshToken })
            };

            var response = await base.SendAsync(refreshRequest, ct);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>(ct);
                if (result != null && !string.IsNullOrWhiteSpace(result.AccessToken))
                {
                    await _storage.SaveItemAsync("accessToken", result.AccessToken);
                    return true;
                }
            }

            // Refresh token inválido o expirado — limpiar storage
            // El caller recibirá la respuesta 401 original y deberá manejar el logout
            await _storage.RemoveItemAsync("accessToken");
            await _storage.RemoveItemAsync("refreshToken");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthTokenHandler.Refresh] ERROR: {ex.Message}");
            return false;
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    /// <summary>
    /// Clona un HttpRequestMessage para poder reenviar el request
    /// después de renovar el token. El request original ya fue consumido
    /// y no puede reutilizarse directamente.
    /// </summary>
    private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(
        HttpRequestMessage original)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri)
        {
            Version = original.Version
        };

        // Clonar el body si existe
        if (original.Content != null)
        {
            var stream = new MemoryStream();
            await original.Content.CopyToAsync(stream);
            stream.Position = 0;
            clone.Content = new StreamContent(stream);

            // Copiar headers del content (Content-Type, etc.)
            foreach (var header in original.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        // Copiar headers del request (excepto Authorization que se reemplazará)
        foreach (var header in original.Headers)
        {
            if (!header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        // Copiar Options (.NET 5+)
        foreach (var option in original.Options)
        {
            clone.Options.TryAdd(option.Key, option.Value);
        }

        return clone;
    }
}