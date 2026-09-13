using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interface;

namespace FluencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class EmpresaController(IEmpresaService empresaService) : ControllerBase
{
    /// <summary>Da de alta una nueva empresa.</summary>
    [HttpPost("AltaEmpresa", Name = "AltaEmpresa")]
    [ProducesResponseType(typeof(EmpresaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EmpresaResponse>> AltaEmpresa(
        [FromBody] CreateEmpresaRequest request,
        CancellationToken cancellationToken)
    {
        var empresa = await empresaService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(DatosEmpresa), new { idEmpresa = empresa.Id }, empresa);
    }

    /// <summary>Obtiene los datos de una empresa por id.</summary>
    [HttpGet("DatosEmpresa/{idEmpresa:int}", Name = "DatosEmpresa")]
    [ProducesResponseType(typeof(EmpresaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmpresaResponse>> DatosEmpresa(
        int idEmpresa,
        CancellationToken cancellationToken)
    {
        var empresa = await empresaService.GetByIdAsync(idEmpresa, cancellationToken);
        return empresa is null ? NotFound($"No existe la empresa {idEmpresa}.") : Ok(empresa);
    }
}
