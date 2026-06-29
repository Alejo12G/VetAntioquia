namespace VeterinariaAntioquia.Shared.Models;

// ═══════════════════════════════════════════════════════════════
//  Medicamento.cs — Modelos del módulo Inventario
//
//  Tablas cubiertos:
//    · medicamentos  → Medicamento
//    · inventarios   → campos incrustados en MedicamentoInventario (JOIN)
//
//  MedicamentoInventario es el DTO principal que usa la UI:
//    combina los datos del medicamento con su stock actual.
// ═══════════════════════════════════════════════════════════════

// ── Medicamento (datos del producto) ─────────────────────────
public class Medicamento
{
    public int     Id                { get; set; }
    public string  Nombre            { get; set; } = "";
    public string  Tipo              { get; set; } = "";
    public string? Descripcion       { get; set; }
    public string? Contraindicaciones { get; set; }
    public string? Fabricante        { get; set; }
    public decimal Precio            { get; set; }
}

// ── MedicamentoInventario (JOIN medicamentos + inventarios) ───
// DTO devuelto por GET /api/inventario — incluye el stock.
public class MedicamentoInventario
{
    public int      Id                  { get; set; }
    public int?     InventarioId        { get; set; }
    public string   Nombre              { get; set; } = "";
    public string   Tipo                { get; set; } = "";
    public string?  Descripcion         { get; set; }
    public string?  Contraindicaciones  { get; set; }
    public string?  Fabricante          { get; set; }
    public decimal  Precio              { get; set; }

    // Stock
    public int      Cantidad            { get; set; }
    public int      CantidadMinima      { get; set; } = 5;
    public DateTime? FechaVencimiento   { get; set; }
    public string?  Lote                { get; set; }
    public DateTime? FechaActualizacion { get; set; }

    // Calculados en backend, útiles para alertas en UI
    public bool StockBajo { get; set; }
    public bool Vencido   { get; set; }
}

// ── Petición de ajuste de stock ───────────────────────────────
// Body para PUT /api/inventario/stock/:id
public class AjusteStockRequest
{
    public int      Cantidad           { get; set; }
    public int?     CantidadMinima     { get; set; }
    public string?  FechaVencimiento   { get; set; }
    public string?  Lote               { get; set; }
}

// ── Petición crear/editar medicamento ─────────────────────────
// Body para POST y PUT /api/inventario/medicamentos
public class MedicamentoRequest
{
    public string   Nombre             { get; set; } = "";
    public string   Tipo               { get; set; } = "";
    public string?  Descripcion        { get; set; }
    public string?  Contraindicaciones { get; set; }
    public string?  Fabricante         { get; set; }
    public decimal  Precio             { get; set; }

    // Solo para creación (POST)
    public int      Cantidad           { get; set; } = 0;
    public int      CantidadMinima     { get; set; } = 5;
    public string?  FechaVencimiento   { get; set; }
    public string?  Lote               { get; set; }
}
