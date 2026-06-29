namespace VeterinariaAntioquia.Shared.Models;

public class Diagnostico
{
    public int Id { get; set; }
    public int CitaId { get; set; }
    public int IdVeterinario { get; set; }
    public string Sintomas { get; set; } = "";
    public string DiagnosticoText { get; set; } = ""; // Mapeado de "diagnostico" en JSON
    public string? Observaciones { get; set; }
    public string? Fecha { get; set; }

    // Lista de tratamientos que llegan en la misma petición GET
    public List<Tratamiento> Tratamientos { get; set; } = new();
}

public class DiagnosticoRequest
{
    public int CitaId { get; set; }
    public string Sintomas { get; set; } = "";
    public string Diagnostico { get; set; } = "";
    public string? Observaciones { get; set; }
}
