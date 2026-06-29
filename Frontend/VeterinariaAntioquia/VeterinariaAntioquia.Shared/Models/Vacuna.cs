namespace VeterinariaAntioquia.Shared.Models;

public class Vacuna
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string? Descripcion { get; set; }
    public string? Fabricante { get; set; }
    public int DiasInmunidad { get; set; }
}

public class HistorialVacuna
{
    public int Id { get; set; }
    public string VacunaNombre { get; set; } = "";
    public string? Fabricante { get; set; }
    public string? FechaAplicacion { get; set; }
    public string? FechaProxima { get; set; }
    public string? Lote { get; set; }
    public string? VeterinarioNombre { get; set; }
}

public class AplicarVacunaRequest
{
    public int IdMascota { get; set; }
    public int IdVacuna { get; set; }
    public string FechaAplicacion { get; set; } = "";
    public string? Lote { get; set; }
}
