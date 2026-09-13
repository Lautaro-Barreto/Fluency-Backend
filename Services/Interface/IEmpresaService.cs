using Services.DTO;

namespace Services.Interface;

public interface IEmpresaService
{
    Task<EmpresaResponse> CreateAsync(CreateEmpresaRequest request, CancellationToken cancellationToken);

    Task<EmpresaResponse?> GetByIdAsync(int idEmpresa, CancellationToken cancellationToken);
}
