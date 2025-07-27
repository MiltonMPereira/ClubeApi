using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Domain.Common;

namespace ClubAccessControl.Application.Interfaces
{
    public interface IPlanoService
    {
        Task<Result<PlanoAcesso>> CriarAsync(PlanoAcesso plano, List<int> areasPermitidasIds);
        Task<Result<PlanoAcesso>> ObterPorIdAsync(int id);
        Task<Result<List<PlanoAcesso>>> ListarAsync();
        Task<Result<PlanoAcesso>> AtualizarAsync(int id, string? novoNome, List<int>? areasPermitidasIds);
        Task<Result> DeletarAsync(int id);
        Task<Result> AdicionarAreaAsync(int planoId, int areaId);
        Task<Result> RemoverAreaAsync(int planoId, int areaId);
    }
}
