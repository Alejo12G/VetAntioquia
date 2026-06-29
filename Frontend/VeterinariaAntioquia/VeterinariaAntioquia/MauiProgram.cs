using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using VeterinariaAntioquia.Shared.Services;
using VeterinariaAntioquia.Shared.Services.Styles;
using VeterinariaAntioquia.Services;

namespace VeterinariaAntioquia;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        // ── Autenticación ─────────────────────────────────────
        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<IStorageService, SecureStorageService>();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

        // ── AuthTokenHandler ──────────────────────────────────
        builder.Services.AddTransient<AuthTokenHandler>();

        // ── HttpClient con el handler en el pipeline ──────────
        builder.Services.AddHttpClient("VetApi", client =>
    {
        client.BaseAddress = new Uri(VeterinariaAntioquia.Shared.ApiConfig.BaseUrl);
    })
            .AddHttpMessageHandler<AuthTokenHandler>();

        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient("VetApi"));

        // ── Servicios de negocio ──────────────────────────────
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<CitaService>();
        builder.Services.AddScoped<EspecieService>();
        builder.Services.AddScoped<InventarioService>();
        builder.Services.AddScoped<DiagnosticoService>();
        builder.Services.AddScoped<TratamientoService>();
        builder.Services.AddScoped<VacunaService>();
        builder.Services.AddScoped<FacturaService>();
        builder.Services.AddScoped<NotificacionService>();

        // ── Servicios de plataforma ───────────────────────────
        builder.Services.AddSingleton<IFormFactor, FormFactor>();
        builder.Services.AddScoped<CustomToastService>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}