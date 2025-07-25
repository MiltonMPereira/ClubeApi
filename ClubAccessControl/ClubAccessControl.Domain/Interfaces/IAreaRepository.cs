using ClubAccessControl.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Domain.Interfaces
{
    public interface IAreaRepository
    {
        Task<AreaClube> ObterPorIdAsync(int id);
        Task<List<AreaClube>> ListarAsync();
        Task AdicionarAsync(AreaClube area);
        Task AtualizarAsync(AreaClube area);
        Task RemoverAsync(AreaClube area);
        Task<bool> ExisteAsync(int id);
        Task<List<AreaClube>> ListarPorIdsAsync(List<int> ids);
    }
}
