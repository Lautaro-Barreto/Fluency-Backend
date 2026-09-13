using System.ComponentModel.DataAnnotations;

namespace Services.DTO;

/// <summary>Resumen de una oportunidad para las vistas de embudo comercial.</summary>
public sealed record OportunidadResumenResponse(
    int Id,
    string Titulo,
    int? IdEmpresa,
    int? IdContacto,
    int? IdUsuario,
    DateOnly? FechaEstimadaCierre);

/// <summary>Una etapa comercial junto con las oportunidades que se encuentran en ella.</summary>
public sealed record EtapaConOportunidadesResponse(
    int IdEtapa,
    string Nombre,
    int Orden,
    IReadOnlyList<OportunidadResumenResponse> Oportunidades);

/// <summary>Detalle completo de una oportunidad.</summary>
public sealed record OportunidadResponse(
    int Id,
    string Titulo,
    int? IdUsuario,
    int? IdEmpresa,
    int? IdContacto,
    int? IdServicio,
    int? IdEtapa,
    DateOnly? FechaEstimadaCierre,
    DateTime? FechaCierre,
    int? IdOrigen,
    int? IdEstado,
    string? Observaciones);

/// <summary>Payload para mover una oportunidad a una nueva etapa comercial.</summary>
public sealed record UpdateEtapaOportunidadRequest
{
    /// <summary>Id de la nueva etapa comercial.</summary>
    public required int IdNuevaEtapa { get; init; }

    /// <summary>Usuario que realiza el cambio, para dejarlo asentado en el historial.</summary>
    public int? IdUsuario { get; init; }

    /// <summary>Observación opcional sobre el cambio de etapa.</summary>
    public string? Observacion { get; init; }
}

/// <summary>Resultado posible de intentar actualizar la etapa de una oportunidad.</summary>
public enum UpdateEtapaOportunidadOutcome
{
    Success,
    OportunidadNotFound,
    EtapaNotFound
}

/// <summary>Envuelve el resultado de la operación y, si tuvo éxito, la oportunidad ya actualizada.</summary>
public sealed record UpdateEtapaOportunidadResult(
    UpdateEtapaOportunidadOutcome Outcome,
    OportunidadResponse? Oportunidad);

/// <summary>Payload para registrar una nueva oportunidad.</summary>
public sealed record CreateOportunidadRequest
{
    [Required, MaxLength(150)]
    public required string Titulo { get; init; }

    /// <summary>Usuario responsable de la oportunidad (toda oportunidad debe tener un responsable).</summary>
    public required int IdUsuario { get; init; }

    /// <summary>Etapa comercial inicial de la oportunidad.</summary>
    public required int IdEtapa { get; init; }

    /// <summary>Empresa asociada. La oportunidad debe tener empresa y/o contacto.</summary>
    public int? IdEmpresa { get; init; }

    /// <summary>Contacto asociado. La oportunidad debe tener empresa y/o contacto.</summary>
    public int? IdContacto { get; init; }

    public int? IdServicio { get; init; }

    public DateOnly? FechaEstimadaCierre { get; init; }

    public int? IdOrigen { get; init; }

    public int? IdEstado { get; init; }

    public string? Observaciones { get; init; }
}

/// <summary>Resultado posible de intentar registrar una nueva oportunidad.</summary>
public enum CreateOportunidadOutcome
{
    Success,
    ValidationFailed
}

/// <summary>
/// Envuelve el resultado de dar de alta una oportunidad: si falló la validación de negocio,
/// <see cref="Errors"/> detalla cada motivo; si tuvo éxito, <see cref="Oportunidad"/> trae la oportunidad creada.
/// </summary>
public sealed record CreateOportunidadResult(
    CreateOportunidadOutcome Outcome,
    OportunidadResponse? Oportunidad,
    IReadOnlyList<string> Errors);
