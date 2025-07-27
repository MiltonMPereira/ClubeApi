using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Domain.Interfaces;
using ClubAccessControl.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClubAccessControl.Infrastructure.Repositories
{
    public class SocioRepository : ISocioRepository
    {
        private readonly ClubeContext _context;

        public SocioRepository(ClubeContext context)
        {
            _context = context;
        }

        public async Task<Socio> ObterPorIdAsync(int id)
        {
            return await _context.Socios
                .Include(s => s.Plano)
                .ThenInclude(p => p.AreasPermitidas)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Socio>> ListarAsync()
        {
            return await _context.Socios
                .Include(s => s.Plano)
                .ThenInclude(p => p.AreasPermitidas)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Socio socio)
        {
            await _context.Socios.AddAsync(socio);
            //await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Socio socio)
        {
            _context.Socios.Update(socio);
            //await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Socio socio)
        {
            _context.Socios.Remove(socio);
            //await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Socios.AnyAsync(s => s.Id == id);
        }
    }
}
