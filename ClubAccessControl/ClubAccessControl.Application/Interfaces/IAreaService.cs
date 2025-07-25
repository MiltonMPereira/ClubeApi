using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Domain.Common;

namespace ClubAccessControl.Application.Interfaces
{
    public interface IAreaService
    {
        Task<Result<AreaClube>> CriarAsync(AreaClube area, List<int> planosPermitidosIds = null);
        Task<Result<AreaClube>> ObterPorIdAsync(int id);
        Task<Result<List<AreaClube>>> ListarAsync();
        Task<Result<AreaClube>> AtualizarAsync(int id, string? novoNome, List<int>? planosPermitidosIds);
        Task<Result> DeletarAsync(int id);
        Task<Result> AdicionarPlanoAsync(int areaId, int planoId);
        Task<Result> RemoverPlanoAsync(int areaId, int planoId);
    }
}
