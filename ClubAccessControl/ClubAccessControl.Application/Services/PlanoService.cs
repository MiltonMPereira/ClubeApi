using ClubAccessControl.Domain.Interfaces;
using ClubAccessControl.Application.Interfaces;
using ClubAccessControl.Domain.Common;
using ClubAccessControl.Domain.Entidades;

namespace ClubAccessControl.Application.Services
{
    public class PlanoService : IPlanoService
    {
        private readonly IUnitOfWork _uow;

        public PlanoService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<PlanoAcesso>> CriarAsync(PlanoAcesso plano, List<int> areasPermitidasIds)
        {

            if (areasPermitidasIds == null || !areasPermitidasIds.Any())
                return Result<PlanoAcesso>.Fail("Plano deve ter pelo menos uma área permitida.");

            var areas = await _uow.Areas.ListarPorIdsAsync(areasPermitidasIds);

            if (areas.Count != areasPermitidasIds.Count)
            {
                var idsNaoEncontrados = areasPermitidasIds.Except(areas.Select(a => a.Id)).ToList();
                return Result<PlanoAcesso>.Fail($"Áreas não encontradas: {string.Join(", ", idsNaoEncontrados)}");
            }

            foreach (var item in areas)
            {
                plano.AdicionarArea(item);
            } 

            await _uow.Planos.AdicionarAsync(plano);
            await _uow.CommitAsync();

            return Result<PlanoAcesso>.Ok(plano);
        }

        public async Task<Result<PlanoAcesso>> ObterPorIdAsync(int id)
        {
            var plano = await _uow.Planos.ObterPorIdAsync(id);
            if (plano == null)
                return Result<PlanoAcesso>.Fail("Plano não encontrado.");

            return Result<PlanoAcesso>.Ok(plano);
        }

        public async Task<Result<List<PlanoAcesso>>> ListarAsync()
        {
            var planos = await _uow.Planos.ListarAsync();
            return Result<List<PlanoAcesso>>.Ok(planos);
        }

        public async Task<Result<PlanoAcesso>> AtualizarAsync(int planoId, string? novoNome, List<int>? areasPermitidasIds)
        {
            var plano = await _uow.Planos.ObterPorIdAsync(planoId);
            if (plano == null)
                return Result<PlanoAcesso>.Fail("Plano não encontrado.");

            if (!string.IsNullOrEmpty(novoNome))
                plano.SetNome(novoNome);

            if (areasPermitidasIds != null && areasPermitidasIds.Count > 0)
            {
                var areas = await _uow.Areas.ListarPorIdsAsync(areasPermitidasIds);
                if (areas.Count > 0 && areas.Count != areasPermitidasIds.Count)
                {
                    var idsNaoEncontrados = areasPermitidasIds.Except(areas.Select(p => p.Id)).ToList();
                    return Result<PlanoAcesso>.Fail($"Areas não encontradas: {string.Join(", ", idsNaoEncontrados)}");
                }
                plano.AreasPermitidas.Clear();
                foreach (var area in areas)
                {
                    plano.AdicionarArea(area);
                }
            }

            await _uow.Planos.AtualizarAsync(plano);
            await _uow.CommitAsync();
            return Result<PlanoAcesso>.Ok(plano);
        }


        public async Task<Result> DeletarAsync(int id)
        {
            var plano = await _uow.Planos.ObterPorIdAsync(id);
            if (plano == null)
                return Result.Fail("Plano não encontrado.");

            await _uow.Planos.RemoverAsync(plano);
            await _uow.CommitAsync();
            return Result.Ok();
        }

        public async Task<Result> AdicionarAreaAsync(int planoId, int areaId)
        {
            var plano = await _uow.Planos.ObterPorIdAsync(planoId);
            if (plano == null)
                return Result.Fail("Plano não encontrado.");

            var area = await _uow.Areas.ObterPorIdAsync(areaId);
            if (area == null)
                return Result.Fail("Área não encontrada.");

            if (plano.AreasPermitidas.Any(a => a.Id == areaId))
                return Result.Fail("Área já está associada a este plano.");

            plano.AdicionarArea(area);
            await _uow.Planos.AtualizarAsync(plano);
            await _uow.CommitAsync();

            return Result.Ok();
        }

        public async Task<Result> RemoverAreaAsync(int planoId, int areaId)
        {
            var plano = await _uow.Planos.ObterPorIdAsync(planoId);
            if (plano == null)
                return Result.Fail("Plano não encontrado.");

            if (!plano.AreasPermitidas.Any(a => a.Id == areaId))
                return Result.Fail("Área não está associada a este plano.");

            if (plano.RemoverArea(areaId))
            {
                await _uow.Planos.AtualizarAsync(plano);
                await _uow.CommitAsync();
                return Result.Ok();
            }

            return Result.Fail("Falha ao remover área do plano.");
        }
    }
}