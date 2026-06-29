namespace VeterinariaAntioquia.Shared.Models;

public class Factura
{
    public int Id { get; set; }
    public int IdCita { get; set; }
    public int IdUsuario { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Impuestos { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = ""; // 'pendiente', 'pagada', 'cancelada'
    public string? MetodoPago { get; set; }
    public string? FechaEmision { get; set; }
    public string? FechaPago { get; set; }

    public List<FacturaDetalle> Detalles { get; set; } = new();
}

public class FacturaDetalle
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = "";
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class GenerarFacturaRequest
{
    public int IdCita { get; set; }
}

public class PagarFacturaRequest
{
    public string MetodoPago { get; set; } = "";
}
