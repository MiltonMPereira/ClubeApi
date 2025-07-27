using ClubAccessControl.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Domain.Interfaces
{
    public interface ISocioRepository
    {
        Task<Socio> ObterPorIdAsync(int id);
        Task<List<Socio>> ListarAsync();
        Task AdicionarAsync(Socio socio);
        Task AtualizarAsync(Socio socio);
        Task RemoverAsync(Socio socio);
        Task<bool> ExisteAsync(int id);
    }
}
