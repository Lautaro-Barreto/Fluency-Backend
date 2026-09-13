using Microsoft.EntityFrameworkCore;
using Persistence.Models;
using Services.DTO;
using Services.Interface;

namespace Services.Implementations;

public sealed class OportunidadService(FluencyLocalDbContext db) : IOportunidadService
{
    public async Task<IReadOnlyList<EtapaConOportunidadesResponse>> GetOportunidadesPorEtapaAsync(
        CancellationToken cancellationToken)
    {
        return await db.EtapaComercials
            .AsNoTracking()
            .OrderBy(e => e.Orden)
            .Select(e => new EtapaConOportunidadesResponse(
                e.Id,
                e.Nombre,
                e.Orden,
                e.Oportunidads
                    .Select(o => new OportunidadResumenResponse(
                        o.Id,
                        o.Titulo,
                        o.IdEmpresa,
                        o.IdContacto,
                        o.IdUsuario,
                        o.FechaEstimadaCierre))
                    .ToList()))
            .ToListAsync(cancellationToken);
    }

    public async Task<UpdateEtapaOportunidadResult> UpdateEtapaAsync(
        int idOportunidad,
        UpdateEtapaOportunidadRequest request,
        CancellationToken cancellationToken)
    {
        var oportunidad = await db.Oportunidads
            .FirstOrDefaultAsync(o => o.Id == idOportunidad, cancellationToken);

        if (oportunidad is null)
        {
            return new UpdateEtapaOportunidadResult(UpdateEtapaOportunidadOutcome.OportunidadNotFound, null);
        }

        var nuevaEtapaExiste = await db.EtapaComercials
            .AnyAsync(e => e.Id == request.IdNuevaEtapa, cancellationToken);

        if (!nuevaEtapaExiste)
        {
            return new UpdateEtapaOportunidadResult(UpdateEtapaOportunidadOutcome.EtapaNotFound, null);
        }

        var idEtapaAnterior = oportunidad.IdEtapa;
        oportunidad.IdEtapa = request.IdNuevaEtapa;

        db.HistorialEtapas.Add(new HistorialEtapa
        {
            IdOportunidad = oportunidad.Id,
            IdEtapaAnterior = idEtapaAnterior,
            IdNuevaEtapa = request.IdNuevaEtapa,
            IdUsuario = request.IdUsuario,
            Fecha = DateTime.UtcNow,
            Observacion = request.Observacion
        });

        await db.SaveChangesAsync(cancellationToken);

        var response = new OportunidadResponse(
            oportunidad.Id,
            oportunidad.Titulo,
            oportunidad.IdUsuario,
            oportunidad.IdEmpresa,
            oportunidad.IdContacto,
            oportunidad.IdServicio,
            oportunidad.IdEtapa,
            oportunidad.FechaEstimadaCierre,
            oportunidad.FechaCierre,
            oportunidad.IdOrigen,
            oportunidad.IdEstado,
            oportunidad.Observaciones);

        return new UpdateEtapaOportunidadResult(UpdateEtapaOportunidadOutcome.Success, response);
    }
}
