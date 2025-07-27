using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISocioRepository Socios { get; }
        IPlanoRepository Planos { get; }
        IAcessoRepository Acessos { get; }
        IAreaRepository Areas { get; }
        Task<int> CommitAsync();
        void Rollback();
    }
}
