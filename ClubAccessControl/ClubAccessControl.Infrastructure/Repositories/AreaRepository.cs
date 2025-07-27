using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace ClubAccessControl.Infrastructure.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly ClubeContext _context;

        public AreaRepository(ClubeContext context)
        {
            _context = context;
        }

        public async Task<AreaClube> ObterPorIdAsync(int id)
        {
            return await _context.Areas
                .Include(a => a.PlanosPermitidos)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<AreaClube>> ListarAsync()
        {
            return await _context.Areas
                .Include(a => a.PlanosPermitidos)
                .ToListAsync();
        }

        public async Task<List<AreaClube>> ListarPorIdsAsync(List<int> ids)
        {
            return await _context.Areas
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
        }

        public async Task AdicionarAsync(AreaClube area)
        {
            await _context.Areas.AddAsync(area);
            //await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(AreaClube area)
        {
            _context.Areas.Update(area);
            //await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(AreaClube area)
        {
            _context.Areas.Remove(area);
            //await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Areas.AnyAsync(a => a.Id == id);
        }
    }
}
