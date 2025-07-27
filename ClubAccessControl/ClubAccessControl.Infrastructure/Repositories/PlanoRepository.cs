using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Infrastructure.Repositories
{
    public class PlanoRepository : IPlanoRepository
    {
        private readonly ClubeContext _context;

        public PlanoRepository(ClubeContext context)
        {
            _context = context;
        }

        public async Task<PlanoAcesso> ObterPorIdAsync(int id)
        {
            return await _context.Planos
                .Include(p => p.AreasPermitidas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PlanoAcesso>> ListarAsync()
        {
            return await _context.Planos
                .Include(p => p.AreasPermitidas)
                .ToListAsync();
        }

        public async Task<List<PlanoAcesso>> ListarPorIdsAsync(List<int> ids)
        {
            return await _context.Planos
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
        }

        public async Task AdicionarAsync(PlanoAcesso plano)
        {
            await _context.Planos.AddAsync(plano);
            //await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(PlanoAcesso plano)
        {
            _context.Planos.Update(plano);
            //await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(PlanoAcesso plano)
        {
            _context.Planos.Remove(plano);
            //await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Planos.AnyAsync(p => p.Id == id);
        }
    }
}
