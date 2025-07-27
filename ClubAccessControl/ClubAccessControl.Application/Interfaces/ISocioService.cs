using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Domain.Common;

namespace ClubAccessControl.Application.Interfaces
{
    public interface ISocioService
    {
        Task<Result<Socio>> CriarAsync(Socio socio);
        Task<Result<Socio>> ObterPorIdAsync(int id);
        Task<Result<List<Socio>>> ListarAsync();
        Task<Result<Socio>> AtualizarAsync(int id, string? novoNome, int? planoId);
        Task<Result> DeletarAsync(int id);
        Task<Result> AlterarPlanoAsync(int socioId, int novoPlanoId);
    }
}
