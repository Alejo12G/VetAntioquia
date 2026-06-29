using VeterinariaAntioquia.Shared.Utils;

namespace VeterinariaAntioquia.Shared.Models;
using VeterinariaAntioquia.Shared.Utils;

// ──────────────────────────────────────────────────────────────
//  Veterinario
//  Subconjunto de la tabla: usuarios (rol = 'veterinario')
// ──────────────────────────────────────────────────────────────
public class Veterinario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Email { get; set; } = "";

    /// <summary>Teléfono de contacto. Opcional.</summary>
    public string? Telefono { get; set; }
}
