using Microsoft.EntityFrameworkCore;
using Persistence.Models;
using Services.DTO;
using Services.Interface;

namespace Services.Implementations;

public sealed class ContactoService(FluencyLocalDbContext db) : IContactoService
{
    public async Task<ContactoResponse> CreateAsync(CreateContactoRequest request, CancellationToken cancellationToken)
    {
        var contacto = new Contacto
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Correo = request.Correo,
            Documento = request.Documento,
            Cargo = request.Cargo,
            Telefono = request.Telefono,
            IdEstado = request.IdEstado,
            IdOrigen = request.IdOrigen,
            IdEmpresa = request.IdEmpresa,
            Observaciones = request.Observaciones
        };

        db.Contactos.Add(contacto);
        await db.SaveChangesAsync(cancellationToken);

        return ToResponse(contacto);
    }

    public async Task<ContactoResponse?> GetByIdAsync(int idContacto, CancellationToken cancellationToken)
    {
        return await db.Contactos
            .AsNoTracking()
            .Where(c => c.Id == idContacto)
            .Select(c => new ContactoResponse(
                c.Id,
                c.Nombre,
                c.Apellido,
                c.Documento,
                c.Cargo,
                c.Correo,
                c.Telefono,
                c.IdEstado,
                c.IdOrigen,
                c.IdEmpresa,
                c.Observaciones))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<HistorialEtapaResponse>?> GetHistorialEtapasAsync(
        int idContacto, CancellationToken cancellationToken)
    {
        var existeContacto = await db.Contactos.AnyAsync(c => c.Id == idContacto, cancellationToken);
        if (!existeContacto)
        {
            return null;
        }

        return await db.HistorialEtapas
            .AsNoTracking()
            .Where(h => h.IdOportunidadNavigation!.IdContacto == idContacto)
            .OrderByDescending(h => h.Fecha)
            .Select(h => new HistorialEtapaResponse(
                h.Id,
                h.IdOportunidad,
                h.IdEtapaAnterior,
                h.IdNuevaEtapa,
                h.Fecha,
                h.IdUsuario,
                h.Observacion))
            .ToListAsync(cancellationToken);
    }

    private static ContactoResponse ToResponse(Contacto contacto) => new(
        contacto.Id,
        contacto.Nombre,
        contacto.Apellido,
        contacto.Documento,
        contacto.Cargo,
        contacto.Correo,
        contacto.Telefono,
        contacto.IdEstado,
        contacto.IdOrigen,
        contacto.IdEmpresa,
        contacto.Observaciones);
}
