using ClubAccessControl.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClubeContext _context;
        private bool _disposed;

        public UnitOfWork(ClubeContext context)
        {
            _context = context;
        }

        private ISocioRepository _socios;
        public ISocioRepository Socios => _socios ??= new SocioRepository(_context);

        private IPlanoRepository _planos;
        public IPlanoRepository Planos => _planos ??= new PlanoRepository(_context);

        private IAreaRepository _areas;
        public IAreaRepository Areas => _areas ??= new AreaRepository(_context);

        private IAcessoRepository _acessos;
        public IAcessoRepository Acessos => _acessos ??= new AcessoRepository(_context);

        public async Task<int> CommitAsync()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public void Rollback()
        {
            _context.ChangeTracker.Entries()
                .ToList()
                .ForEach(e => e.State = EntityState.Unchanged);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
