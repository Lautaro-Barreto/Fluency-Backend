using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interface;

namespace FluencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class OportunidadesController(IOportunidadService oportunidadService) : ControllerBase
{
    /// <summary>Devuelve todas las oportunidades agrupadas por etapa comercial (vista de embudo).</summary>
    [HttpGet("OportunidadesPorEtapa", Name = "OportunidadesPorEtapa")]
    [ProducesResponseType(typeof(IReadOnlyList<EtapaConOportunidadesResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EtapaConOportunidadesResponse>>> OportunidadesPorEtapa(
        CancellationToken cancellationToken)
        => Ok(await oportunidadService.GetOportunidadesPorEtapaAsync(cancellationToken));

    /// <summary>Mueve una oportunidad a una nueva etapa comercial y lo asienta en el historial.</summary>
    [HttpPost("UpdateEtapaOportunidad/{idOportunidad:int}", Name = "UpdateEtapaOportunidad")]
    [ProducesResponseType(typeof(OportunidadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OportunidadResponse>> UpdateEtapaOportunidad(
        int idOportunidad,
        [FromBody] UpdateEtapaOportunidadRequest request,
        CancellationToken cancellationToken)
    {
        var result = await oportunidadService.UpdateEtapaAsync(idOportunidad, request, cancellationToken);

        return result.Outcome switch
        {
            UpdateEtapaOportunidadOutcome.Success => Ok(result.Oportunidad),
            UpdateEtapaOportunidadOutcome.OportunidadNotFound
                => NotFound($"No existe la oportunidad {idOportunidad}."),
            UpdateEtapaOportunidadOutcome.EtapaNotFound
                => BadRequest($"No existe la etapa comercial {request.IdNuevaEtapa}."),
            _ => Problem()
        };
    }
}
