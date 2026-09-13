using Microsoft.EntityFrameworkCore;
using Persistence.Models;
using Services.DTO;
using Services.Interface;

namespace Services.Implementations;

public sealed class EmpresaService(FluencyLocalDbContext db) : IEmpresaService
{
    public async Task<EmpresaResponse> CreateAsync(CreateEmpresaRequest request, CancellationToken cancellationToken)
    {
        var empresa = new Empresa
        {
            RazonSocial = request.RazonSocial,
            Cuit = request.Cuit,
            Industria = request.Industria,
            Correo = request.Correo,
            Telefono = request.Telefono,
            Direccion = request.Direccion,
            IdEstado = request.IdEstado,
            IdOrigen = request.IdOrigen,
            Observaciones = request.Observaciones
        };

        db.Empresas.Add(empresa);
        await db.SaveChangesAsync(cancellationToken);

        return ToResponse(empresa);
    }

    public async Task<EmpresaResponse?> GetByIdAsync(int idEmpresa, CancellationToken cancellationToken)
    {
        return await db.Empresas
            .AsNoTracking()
            .Where(e => e.Id == idEmpresa)
            .Select(e => new EmpresaResponse(
                e.Id,
                e.RazonSocial,
                e.Cuit,
                e.Industria,
                e.Correo,
                e.Telefono,
                e.Direccion,
                e.IdEstado,
                e.IdOrigen,
                e.Observaciones))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static EmpresaResponse ToResponse(Empresa empresa) => new(
        empresa.Id,
        empresa.RazonSocial,
        empresa.Cuit,
        empresa.Industria,
        empresa.Correo,
        empresa.Telefono,
        empresa.Direccion,
        empresa.IdEstado,
        empresa.IdOrigen,
        empresa.Observaciones);
}
