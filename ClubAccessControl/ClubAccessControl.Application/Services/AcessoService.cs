using ClubAccessControl.Domain.Interfaces;
using ClubAccessControl.Application.Interfaces;
using ClubAccessControl.Domain.Common;
using ClubAccessControl.Domain.Entidades;

namespace ClubAccessControl.Application.Services
{
    public class AcessoService : IAcessoService
    {
        private readonly ISocioRepository _socioRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IAcessoRepository _tentativaRepository;

        public AcessoService(
            ISocioRepository socioRepository,
            IAreaRepository areaRepository,
            IAcessoRepository tentativaRepository)
        {
            _socioRepository = socioRepository;
            _areaRepository = areaRepository;
            _tentativaRepository = tentativaRepository;
        }

        public async Task<Result<TentativaAcesso>> RegistrarTentativaAsync(int socioId, int areaId)
        {
            var socio = await _socioRepository.ObterPorIdAsync(socioId);
            if (socio == null)
                return Result<TentativaAcesso>.Fail("Sócio não encontrado.");

            var area = await _areaRepository.ObterPorIdAsync(areaId);
            if (area == null)
                return Result<TentativaAcesso>.Fail("Área não encontrada.");

            var autorizado = socio.PodeAcessar(area);
            var tentativa = new TentativaAcesso(socioId, areaId, autorizado);

            await _tentativaRepository.AdicionarAsync(tentativa);

            if (!autorizado)
                return Result<TentativaAcesso>.Fail("Acesso negado pelo plano do sócio.");

            return Result<TentativaAcesso>.Ok(tentativa);
        }

        public async Task<Result<List<TentativaAcesso>>> ObterHistoricoSocioAsync(int socioId)
        {
            var existeSocio = await _socioRepository.ExisteAsync(socioId);
            if (!existeSocio)
                return Result<List<TentativaAcesso>>.Fail("Sócio não encontrado.");

            var historico = await _tentativaRepository.ObterPorSocioAsync(socioId);
            return Result<List<TentativaAcesso>>.Ok(historico);
        }

        public async Task<Result<List<TentativaAcesso>>> ObterHistoricoAreaAsync(int areaId)
        {
            var existeArea = await _areaRepository.ExisteAsync(areaId);
            if (!existeArea)
                return Result<List<TentativaAcesso>>.Fail("Área não encontrada.");

            var historico = await _tentativaRepository.ObterPorAreaAsync(areaId);
            return Result<List<TentativaAcesso>>.Ok(historico);
        }
    }
}