using Microsoft.AspNetCore.Components.Authorization;
using VeterinariaAntioquia.Web.Components;
using VeterinariaAntioquia.Shared.Services;
using VeterinariaAntioquia.Shared.Services.Styles;
using VeterinariaAntioquia.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Razor / Blazor ────────────────────────────────────────
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── Auth: solo el core de Blazor, sin middleware ASP.NET ──
// NO llamamos AddAuthentication() porque no tenemos esquema
// server-side (cookies/JWT). Blazor maneja el estado de auth
// completamente a través de CustomAuthStateProvider.
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

// ── AuthTokenHandler ──────────────────────────────────────
builder.Services.AddTransient<AuthTokenHandler>();

// ── HttpClient con el handler en el pipeline ─────────────
builder.Services.AddHttpClient("VetApi", client =>
{
    client.BaseAddress = new Uri(VeterinariaAntioquia.Shared.ApiConfig.BaseUrl);
})
.AddHttpMessageHandler<AuthTokenHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("VetApi"));

// ── Servicios de negocio ──────────────────────────────────
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CitaService>();
builder.Services.AddScoped<EspecieService>();
builder.Services.AddScoped<InventarioService>();
builder.Services.AddScoped<DiagnosticoService>();
builder.Services.AddScoped<TratamientoService>();
builder.Services.AddScoped<VacunaService>();
builder.Services.AddScoped<FacturaService>();
builder.Services.AddScoped<NotificacionService>();

// ── Servicios de plataforma ───────────────────────────────
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<CustomToastService>();

// ── Pipeline HTTP ─────────────────────────────────────────
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/404");
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(VeterinariaAntioquia.Shared._Imports).Assembly);

app.Run();