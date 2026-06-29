namespace VeterinariaAntioquia.Shared.Models;

// ═══════════════════════════════════════════════════════════════
//  CitaModels.cs — Modelos del módulo de Citas
//
//  Contiene:
//    · Servicio          — catálogo de servicios disponibles
//    · Cita              — entidad principal de cita
//    · CitaResumen       — DTO enriquecido para la vista de listado
//    · SlotOcupado       — horario ocupado para bloqueo de calendario
//    · CrearCitaRequest  — payload para POST /api/citas
//    · CrearCitaResponse — respuesta tras crear una cita
// ═══════════════════════════════════════════════════════════════

// ──────────────────────────────────────────────────────────────
//  Servicio
//  Tabla: servicios
// ──────────────────────────────────────────────────────────────
public class Servicio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string? Descripcion { get; set; }

    /// <summary>
    /// Categoría del servicio.
    /// Valores posibles: "clinica" | "estetica" | "cirugia" | "vacunacion" | "laboratorio" | "otro"
    /// </summary>
    public string Categoria { get; set; } = "";

    /// <summary>Precio base en pesos colombianos (COP).</summary>
    public decimal PrecioBase { get; set; }

    /// <summary>Duración estimada en minutos. Usado para bloquear slots de calendario.</summary>
    public int DuracionEstimadaMin { get; set; }

    public int Activo { get; set; }
}

// ──────────────────────────────────────────────────────────────
//  Cita
//  Tabla: citas
// ──────────────────────────────────────────────────────────────
public class Cita
{
    public int Id { get; set; }

    /// <summary>
    /// ID de la mascota. Nullable: una cita puede agendarse sin mascota registrada
    /// (el veterinario toma los datos en consulta).
    /// </summary>
    public int? IdMascota { get; set; }

    public int IdVeterinario { get; set; }
    public int IdServicio { get; set; }
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Estado actual de la cita.
    /// Valores posibles: "programada" | "confirmada" | "completada" | "cancelada" | "no_asistio"
    /// </summary>
    public string Estado { get; set; } = "programada";

    /// <summary>Motivo o descripción de síntomas. Opcional.</summary>
    public string? Motivo { get; set; }

    /// <summary>Duración real o estimada de la cita en minutos.</summary>
    public int DuracionMinutos { get; set; }
}

// ──────────────────────────────────────────────────────────────
//  CitaResumen
//  DTO enriquecido para la vista de listado de citas (/citas).
//
//  El API debe hacer JOIN entre citas, mascotas, usuarios y servicios
//  y devolver este objeto aplanado — evita múltiples requests
//  desde el cliente.
//
//  Endpoint productor: GET /api/citas/mias
// ──────────────────────────────────────────────────────────────
public class CitaResumen
{
    // ── Datos de la cita ──────────────────────────────────────
    public int Id { get; set; }
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Estado de la cita.
    /// Valores: "programada" | "confirmada" | "completada" | "cancelada" | "no_asistio"
    /// </summary>
    public string Estado { get; set; } = "";

    /// <summary>Motivo de la consulta. Puede ser null si no se especificó.</summary>
    public string? Motivo { get; set; }

    public int DuracionMinutos { get; set; }

    // ── Datos del servicio (JOIN servicios) ───────────────────
    public string ServicioNombre { get; set; } = "";
    public string ServicioCategoria { get; set; } = "";
    public decimal ServicioPrecioBase { get; set; }

    // ── Datos del veterinario (JOIN usuarios) ─────────────────
    public string VeterinarioNombre { get; set; } = "";

    // ── Datos de la mascota (LEFT JOIN mascotas) ──────────────
    /// <summary>
    /// Nombre de la mascota. Null si la cita se agendó sin mascota registrada.
    /// </summary>
    public string? MascotaNombre { get; set; }

    public string? MascotaFotoUrl { get; set; }

    // ── Propiedades calculadas en cliente ─────────────────────

    /// <summary>True si la cita está en el futuro y su estado es "programada" o "confirmada".</summary>
    public bool EsProxima =>
        Fecha > DateTime.Now &&
        (Estado == "programada" || Estado == "confirmada");

    /// <summary>True si la cita ya ocurrió o fue cancelada/no_asistio.</summary>
    public bool EsPasada => !EsProxima;
}

// ──────────────────────────────────────────────────────────────
//  CitaVeterinarioResumen
//  DTO para la vista del veterinario (GET /api/citas/asignadas).
//  Muestra ClienteNombre en lugar de VeterinarioNombre.
// ──────────────────────────────────────────────────────────────
public class CitaVeterinarioResumen
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = "";
    public string? Motivo { get; set; }
    public int DuracionMinutos { get; set; }

    // ── Datos del servicio ─────────────────────────────────────
    public string ServicioNombre { get; set; } = "";
    public string ServicioCategoria { get; set; } = "";
    public decimal ServicioPrecioBase { get; set; }

    // ── Datos del cliente (dueño de la cita) ──────────────────
    public string ClienteNombre { get; set; } = "";

    // ── Datos de la mascota ───────────────────────────────────
    public string? MascotaNombre { get; set; }
    public string? MascotaFotoUrl { get; set; }

    // ── Propiedades calculadas ────────────────────────────────
    public bool EsProxima =>
        Fecha > DateTime.Now &&
        (Estado == "programada" || Estado == "confirmada");

    public bool EsPasada => !EsProxima;
}

// ──────────────────────────────────────────────────────────────
//  CitaDetalle
//  DTO completo para la vista de consulta (GET /api/citas/:id/detalle).
//  Incluye todos los datos relacionados: servicio, cliente, mascota, especie.
// ──────────────────────────────────────────────────────────────
public class CitaDetalle
{
    // ── Datos de la cita ──────────────────────────────────────
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = "";
    public string? Motivo { get; set; }
    public int DuracionMinutos { get; set; }

    // ── Datos del servicio ────────────────────────────────────
    public string ServicioNombre { get; set; } = "";
    public string ServicioCategoria { get; set; } = "";
    public decimal ServicioPrecioBase { get; set; }
    public string? ServicioDescripcion { get; set; }
    public int ServicioDuracionEstimada { get; set; }

    // ── Datos del cliente (dueño) ─────────────────────────────
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; } = "";
    public string? ClienteEmail { get; set; }
    public string? ClienteTelefono { get; set; }

    // ── Datos de la mascota (nullable) ────────────────────────
    public string? MascotaNombre { get; set; }
    public string? MascotaSexo { get; set; }
    public bool? MascotaEsterilizado { get; set; }
    public DateTime? MascotaFechaNacimiento { get; set; }
    public string? MascotaFotoUrl { get; set; }
    public string? EspecieNombre { get; set; }

    // ── Propiedades calculadas ────────────────────────────────
    public int? MascotaEdadAnios => MascotaFechaNacimiento.HasValue
        ? (int)((DateTime.Today - MascotaFechaNacimiento.Value).TotalDays / 365.25)
        : null;

    public bool EsProxima =>
        Fecha > DateTime.Now &&
        (Estado == "programada" || Estado == "confirmada");
}

// ──────────────────────────────────────────────────────────────
//  SlotOcupado
//  DTO para verificar disponibilidad de horarios.
//
//  Endpoint productor: GET /api/citas/disponibilidad
// ──────────────────────────────────────────────────────────────
public class SlotOcupado
{
    /// <summary>Inicio del slot ocupado (fecha + hora).</summary>
    public DateTime Inicio { get; set; }

    /// <summary>Fin del slot ocupado = Inicio + duracion_minutos de la cita.</summary>
    public DateTime Fin { get; set; }
}

// ──────────────────────────────────────────────────────────────
//  CrearCitaRequest
//  Payload para POST /api/citas
// ──────────────────────────────────────────────────────────────
public class CrearCitaRequest
{
    /// <summary>
    /// ID de la mascota. Nullable: enviar null si el usuario no tiene
    /// mascota registrada — el veterinario toma los datos en consulta.
    /// </summary>
    public int? IdMascota { get; set; }

    public int IdVeterinario { get; set; }
    public int IdServicio { get; set; }

    /// <summary>Fecha y hora combinadas de la cita (UTC recomendado).</summary>
    public DateTime Fecha { get; set; }

    /// <summary>Motivo de la consulta. Opcional.</summary>
    public string? Motivo { get; set; }

    /// <summary>
    /// Duración en minutos. Debe coincidir con duracion_estimada_min
    /// del servicio seleccionado para un correcto bloqueo de calendario.
    /// </summary>
    public int DuracionMinutos { get; set; }
}

// ──────────────────────────────────────────────────────────────
//  CrearCitaResponse
//  Respuesta de POST /api/citas
// ──────────────────────────────────────────────────────────────
public class CrearCitaResponse
{
    /// <summary>ID asignado por la base de datos a la nueva cita.</summary>
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    /// <summary>Estado inicial asignado por el backend. Siempre "programada".</summary>
    public string Estado { get; set; } = "";
}

// ──────────────────────────────────────────────────────────────
//  CancelarCitaRequest
//  Payload para PATCH /api/citas/{id}/cancelar
// ──────────────────────────────────────────────────────────────
public class CancelarCitaRequest
{
    /// <summary>Motivo de cancelación. Opcional pero recomendado.</summary>
    public string? Motivo { get; set; }
}

// ──────────────────────────────────────────────────────────────
//  CompletarCitaRequest
//  Payload para PATCH /api/citas/{id}/completar
// ──────────────────────────────────────────────────────────────
public class CompletarCitaRequest
{
    /// <summary>Diagnóstico emitido por el veterinario. Obligatorio.</summary>
    public string Diagnostico { get; set; } = "";

    /// <summary>Tratamiento indicado. Obligatorio.</summary>
    public string Tratamiento { get; set; } = "";

    /// <summary>Observaciones adicionales. Opcional.</summary>
    public string? Observaciones { get; set; }

    /// <summary>Peso actual de la mascota en kg. Opcional.</summary>
    public decimal? PesoKg { get; set; }
}

// ──────────────────────────────────────────────────────────────
//  AsignarMascotaRequest
//  Payload para PATCH /api/citas/{id}/asignar-mascota
// ──────────────────────────────────────────────────────────────
public class AsignarMascotaRequest
{
    public int IdMascota { get; set; }
}

// ──────────────────────────────────────────────────────────────
//  CrearMascotaRequest
//  Payload para POST /api/mascotas/cliente/{clienteId}
// ──────────────────────────────────────────────────────────────
public class CrearMascotaRequest
{
    public string Nombre { get; set; } = "";
    public string? FechaNacimiento { get; set; }

    /// <summary>Valores posibles: "macho" | "hembra".</summary>
    public string Sexo { get; set; } = "";

    public bool Esterilizado { get; set; }
    public int IdEspecie { get; set; }
}