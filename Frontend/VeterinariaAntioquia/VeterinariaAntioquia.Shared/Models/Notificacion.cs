namespace VeterinariaAntioquia.Shared.Models;

public class Notificacion
{
    // En Firestore el ID suele ser string, si viene de MySQL es int,
    // usaremos string para cubrir ambos casos.
    public string Id { get; set; } = "";
    public int? IdUsuario { get; set; }
    public string Tipo { get; set; } = ""; // 'cita', 'factura', 'alerta'
    public string Titulo { get; set; } = "";
    public string Mensaje { get; set; } = "";
    public bool Leida { get; set; }
    public string? FechaEnvio { get; set; }
}
