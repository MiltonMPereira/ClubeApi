using ClubAccessControl.Domain.Interfaces;
using ClubAccessControl.Application.Interfaces;
using ClubAccessControl.Domain.Common;
using ClubAccessControl.Domain.Entidades;

namespace ClubAccessControl.Application.Services
{
    public class AcessoService : IAcessoService
    {
        private readonly IUnitOfWork _uow; 

        public AcessoService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<Result<TentativaAcesso>> RegistrarTentativaAsync(int socioId, int areaId)
        {
            var socio = await _uow.Socios.ObterPorIdAsync(socioId);
            if (socio == null)
                return Result<TentativaAcesso>.Fail("Sócio não encontrado.");

            var area = await _uow.Areas.ObterPorIdAsync(areaId);
            if (area == null)
                return Result<TentativaAcesso>.Fail("Área não encontrada.");

            var autorizado = socio.PodeAcessar(area);
            var tentativa = new TentativaAcesso(socioId, areaId, autorizado);

            await _uow.Acessos.AdicionarAsync(tentativa);
            await _uow.CommitAsync();

            if (!autorizado)
                return Result<TentativaAcesso>.Fail("Acesso negado pelo plano do sócio.");

            return Result<TentativaAcesso>.Ok(tentativa);
        }

        public async Task<Result<List<TentativaAcesso>>> ObterHistoricoSocioAsync(int socioId)
        {
            var existeSocio = await _uow.Socios.ExisteAsync(socioId);
            if (!existeSocio)
                return Result<List<TentativaAcesso>>.Fail("Sócio não encontrado.");

            var historico = await _uow.Acessos.ObterPorSocioAsync(socioId);
            return Result<List<TentativaAcesso>>.Ok(historico);
        }

        public async Task<Result<List<TentativaAcesso>>> ObterHistoricoAreaAsync(int areaId)
        {
            var existeArea = await _uow.Areas.ExisteAsync(areaId);
            if (!existeArea)
                return Result<List<TentativaAcesso>>.Fail("Área não encontrada.");

            var historico = await _uow.Acessos.ObterPorAreaAsync(areaId);
            return Result<List<TentativaAcesso>>.Ok(historico);
        }
    }
}