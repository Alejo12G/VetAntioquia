namespace VeterinariaAntioquia.Shared.Models;

// ── Mascota ──────────────────────────────────────────
public class Mascota
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";

    /// <summary>Fecha de nacimiento. Opcional — puede ser null si no se conoce.</summary>
    public DateTime? FechaNacimiento { get; set; }

    /// <summary>Valores posibles: "macho" | "hembra".</summary>
    public string Sexo { get; set; } = "";

    public bool Esterilizado { get; set; }
    public int IdEspecie { get; set; }

    /// <summary>URL absoluta o relativa a la foto de perfil de la mascota.</summary>
    public string? FotoUrl { get; set; }

    /// <summary>
    /// Edad calculada en años a partir de FechaNacimiento.
    /// Devuelve null si FechaNacimiento no está definida.
    /// </summary>
    public int? EdadAnios => FechaNacimiento.HasValue
        ? (int)((DateTime.Today - FechaNacimiento.Value).TotalDays / 365.25)
        : null;
}

// ── Especie ──────────────────────────────────────────
public class Especie
{
    public int    Id                { get; set; }
    public string Nombre            { get; set; } = "";
    public string? NombreCientifico { get; set; }
    public string? Caracteristicas  { get; set; }
}

// ── MascotaCliente ───────────────────────────────────
// DTO enriquecido con nombre de especie, usado para
// mostrar mascotas del cliente en la consulta veterinaria.
public class MascotaCliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public DateTime? FechaNacimiento { get; set; }
    public string Sexo { get; set; } = "";
    public bool Esterilizado { get; set; }
    public int IdEspecie { get; set; }
    public string? FotoUrl { get; set; }
    public string? EspecieNombre { get; set; }

    public int? EdadAnios => FechaNacimiento.HasValue
        ? (int)((DateTime.Today - FechaNacimiento.Value).TotalDays / 365.25)
        : null;
}

