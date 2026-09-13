using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interface;

namespace FluencyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class ContactoController(IContactoService contactoService) : ControllerBase
{
    /// <summary>Da de alta un nuevo contacto.</summary>
    [HttpPost("AltaContacto", Name = "AltaContacto")]
    [ProducesResponseType(typeof(ContactoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactoResponse>> AltaContacto(
        [FromBody] CreateContactoRequest request,
        CancellationToken cancellationToken)
    {
        var contacto = await contactoService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(DatosContacto), new { idContacto = contacto.Id }, contacto);
    }

    /// <summary>Obtiene los datos de un contacto por id.</summary>
    [HttpGet("DatosContacto/{idContacto:int}", Name = "DatosContacto")]
    [ProducesResponseType(typeof(ContactoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactoResponse>> DatosContacto(
        int idContacto,
        CancellationToken cancellationToken)
    {
        var contacto = await contactoService.GetByIdAsync(idContacto, cancellationToken);
        return contacto is null ? NotFound($"No existe el contacto {idContacto}.") : Ok(contacto);
    }

    /// <summary>Obtiene el historial de cambios de etapa de las oportunidades del contacto.</summary>
    [HttpGet("HistorialEtapas/{idContacto:int}", Name = "HistorialEtapasContacto")]
    [ProducesResponseType(typeof(IReadOnlyList<HistorialEtapaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<HistorialEtapaResponse>>> HistorialEtapas(
        int idContacto,
        CancellationToken cancellationToken)
    {
        var historial = await contactoService.GetHistorialEtapasAsync(idContacto, cancellationToken);
        return historial is null ? NotFound($"No existe el contacto {idContacto}.") : Ok(historial);
    }
}
