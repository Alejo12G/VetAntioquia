namespace VeterinariaAntioquia.Shared.Models;

public class Tratamiento
{
    public int Id { get; set; }
    public int IdDiagnostico { get; set; }
    public int IdMedicamento { get; set; }
    public string? MedicamentoNombre { get; set; }
    public string Dosis { get; set; } = "";
    public string Frecuencia { get; set; } = "";
    public int DuracionDias { get; set; }
    public string? Instrucciones { get; set; }
    public string Estado { get; set; } = ""; // 'activo', 'completado', 'cancelado'
    public string? FechaInicio { get; set; }
    public string? FechaFin { get; set; }
}

public class TratamientoRequest
{
    public int IdDiagnostico { get; set; }
    public int IdMedicamento { get; set; }
    public string Dosis { get; set; } = "";
    public string Frecuencia { get; set; } = "";
    public int DuracionDias { get; set; }
    public string? Instrucciones { get; set; }
    public string FechaInicio { get; set; } = ""; // Formato "yyyy-MM-dd"
}
