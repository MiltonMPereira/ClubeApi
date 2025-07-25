using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Domain.Common;
using System;

namespace ClubAccessControl.Application.Interfaces
{
    public interface IAcessoService
    {
        Task<Result<TentativaAcesso>> RegistrarTentativaAsync(int socioId, int areaId);
        Task<Result<List<TentativaAcesso>>> ObterHistoricoSocioAsync(int socioId);
        Task<Result<List<TentativaAcesso>>> ObterHistoricoAreaAsync(int areaId);
    }
}
