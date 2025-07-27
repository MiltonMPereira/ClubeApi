using ClubAccessControl.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Domain.Interfaces
{
    public interface IPlanoRepository
    {
        Task<PlanoAcesso> ObterPorIdAsync(int id);
        Task<List<PlanoAcesso>> ListarAsync();
        Task AdicionarAsync(PlanoAcesso plano);
        Task AtualizarAsync(PlanoAcesso plano);
        Task RemoverAsync(PlanoAcesso plano);
        Task<bool> ExisteAsync(int id);
        Task<List<PlanoAcesso>> ListarPorIdsAsync(List<int> ids);
    }
}
