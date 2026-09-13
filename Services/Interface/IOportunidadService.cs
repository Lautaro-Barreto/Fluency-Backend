using Services.DTO;

namespace Services.Interface;

public interface IOportunidadService
{
    Task<IReadOnlyList<EtapaConOportunidadesResponse>> GetOportunidadesPorEtapaAsync(
        CancellationToken cancellationToken);

    Task<UpdateEtapaOportunidadResult> UpdateEtapaAsync(
        int idOportunidad,
        UpdateEtapaOportunidadRequest request,
        CancellationToken cancellationToken);

    Task<CreateOportunidadResult> CreateAsync(
        CreateOportunidadRequest request,
        CancellationToken cancellationToken);
}
