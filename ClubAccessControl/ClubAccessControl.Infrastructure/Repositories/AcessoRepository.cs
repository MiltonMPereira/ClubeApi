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
    public class AcessoRepository : IAcessoRepository
    {
        private readonly ClubeContext _context;

        public AcessoRepository(ClubeContext context)
        {
            _context = context;
        }

        public async Task<TentativaAcesso> ObterPorIdAsync(int id)
        {
            return await _context.TentativasAcesso
                .Include(t => t.Socio)
                .Include(t => t.AreaClube)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TentativaAcesso>> ListarAsync()
        {
            return await _context.TentativasAcesso
                .Include(t => t.Socio)
                .Include(t => t.AreaClube)
                .ToListAsync();
        }

        public async Task AdicionarAsync(TentativaAcesso tentativa)
        {
            await _context.TentativasAcesso.AddAsync(tentativa);
            //await _context.SaveChangesAsync();
        }

        public async Task<List<TentativaAcesso>> ObterPorSocioAsync(int socioId)
        {
            return await _context.TentativasAcesso
                .Where(t => t.SocioId == socioId)
                .Include(t => t.AreaClube)
                .ToListAsync();
        }

        public async Task<List<TentativaAcesso>> ObterPorAreaAsync(int areaId)
        {
            return await _context.TentativasAcesso
                .Where(t => t.AreaClubeId == areaId)
                .Include(t => t.Socio)
                .ToListAsync();
        }
    }
}
