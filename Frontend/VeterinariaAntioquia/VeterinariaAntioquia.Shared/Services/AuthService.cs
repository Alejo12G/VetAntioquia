namespace VeterinariaAntioquia.Shared.Services;

using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using VeterinariaAntioquia.Shared.Models;
public class AuthService
{
    private readonly HttpClient _http;
    private readonly IStorageService _storage;
    private readonly AuthenticationStateProvider _authStateProvider;
    
    public AuthService(HttpClient http, IStorageService storage, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _storage = storage;
        _authStateProvider = authStateProvider;
    }

    // Guardar ambos tokens en el almacenamiento
    public async Task SetAuthTokensAsync(string accessToken, string refreshToken)
    {
        await _storage.SaveItemAsync("accessToken", accessToken);
        await _storage.SaveItemAsync("refreshToken", refreshToken);
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(accessToken);
    }

    // Funcion para registrar un usuario
    public async Task<string?> Register(string nombre, string email, string password)
    {
        try
        {
            var request = new RegisterRequest { Nombre = nombre, Email = email, Password = password };
            var response = await _http.PostAsJsonAsync("api/auth/register", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                if (result != null)
                {
                    await SetAuthTokensAsync(result.AccessToken, result.RefreshToken);
                    return null; //Registro exitoso, no hay error
                }
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) return "Todos los campos son obligatorios";
            return "Error al conectar con el servidor";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DEBUG] ERROR: {ex.Message}");
            return ex.Message;
        }
        
    }
    public async Task<string?> Login(string email, string password)
    {
        try
        {
            var request = new LoginRequest { Email = email, Password = password };

            var response = await _http.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null)
                {
                    await SetAuthTokensAsync(result.AccessToken, result.RefreshToken);
                    return null; // Login exitoso, no hay error
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return "Credenciales incorrectas";
            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                return "Esta cuenta ha sido desactivada";

            return "Error al conectar con el servidor";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DEBUG] ERROR: {ex.Message}");
            return ex.Message;
        }
    }

    /// <summary>
    /// Intenta renovar el access token usando el refresh token almacenado.
    /// Retorna true si la renovación fue exitosa, false si falló (sesión expirada).
    /// </summary>
    public async Task<bool> RefreshAccessTokenAsync()
    {
        try
        {
            var refreshToken = await _storage.GetItemAsync("refreshToken");
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            var response = await _http.PostAsJsonAsync("api/auth/refresh", new { refreshToken });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
                if (result != null && !string.IsNullOrWhiteSpace(result.AccessToken))
                {
                    await _storage.SaveItemAsync("accessToken", result.AccessToken);
                    ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.AccessToken);
                    return true;
                }
            }

            // Si el refresh token es inválido o expirado, hacemos logout
            await Logout();
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService.Refresh] ERROR: {ex.Message}");
            return false;
        }
    }

    // Funcion cerrar sesion
    public async Task Logout()
    {
        await _storage.RemoveItemAsync("accessToken");
        await _storage.RemoveItemAsync("refreshToken");
        ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
    }
}