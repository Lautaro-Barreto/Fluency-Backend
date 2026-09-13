using System.ComponentModel.DataAnnotations;

namespace Services.DTO;

/// <summary>Datos de un contacto.</summary>
public sealed record ContactoResponse(
    int Id,
    string Nombre,
    string Apellido,
    string? Documento,
    string? Cargo,
    string Correo,
    string? Telefono,
    int? IdEstado,
    int? IdOrigen,
    int? IdEmpresa,
    string? Observaciones);

/// <summary>Payload para dar de alta un contacto.</summary>
public sealed record CreateContactoRequest
{
    [Required, MaxLength(100)]
    public required string Nombre { get; init; }

    [Required, MaxLength(100)]
    public required string Apellido { get; init; }

    [Required, MaxLength(150), EmailAddress]
    public required string Correo { get; init; }

    [MaxLength(20)]
    public string? Documento { get; init; }

    [MaxLength(100)]
    public string? Cargo { get; init; }

    [MaxLength(50)]
    public string? Telefono { get; init; }

    public int? IdEstado { get; init; }

    public int? IdOrigen { get; init; }

    public int? IdEmpresa { get; init; }

    public string? Observaciones { get; init; }
}

/// <summary>Un cambio de etapa comercial registrado en el historial de una oportunidad del contacto.</summary>
public sealed record HistorialEtapaResponse(
    int Id,
    int? IdOportunidad,
    int? IdEtapaAnterior,
    int? IdNuevaEtapa,
    DateTime? Fecha,
    int? IdUsuario,
    string? Observacion);
