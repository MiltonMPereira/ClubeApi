using ClubAccessControl.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Domain.Interfaces
{
    public interface IAcessoRepository
    {
        Task<TentativaAcesso> ObterPorIdAsync(int id);
        Task<List<TentativaAcesso>> ListarAsync();
        Task AdicionarAsync(TentativaAcesso tentativa);
        Task<List<TentativaAcesso>> ObterPorSocioAsync(int socioId);
        Task<List<TentativaAcesso>> ObterPorAreaAsync(int areaId);
    }
}
