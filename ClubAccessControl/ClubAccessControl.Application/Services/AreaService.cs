using ClubAccessControl.Domain.Interfaces;
using ClubAccessControl.Application.Interfaces;
using ClubAccessControl.Domain.Common;
using ClubAccessControl.Domain.Entidades;

namespace ClubAccessControl.Application.Services
{
    public class AreaService : IAreaService
    {
        private readonly IUnitOfWork _uow;

        public AreaService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<AreaClube>> CriarAsync(AreaClube area, List<int> planosPermitidosIds = null)
        {

            if (planosPermitidosIds!= null && planosPermitidosIds.Count>0)
            {
                var planos = await _uow.Planos.ListarPorIdsAsync(planosPermitidosIds);
                if (planos.Count>0 && planos.Count != planosPermitidosIds.Count)
                {
                    var idsNaoEncontrados = planosPermitidosIds.Except(planos.Select(p => p.Id)).ToList();
                    return Result<AreaClube>.Fail($"Planos não encontrados: {string.Join(", ", idsNaoEncontrados)}");
                }
                foreach (var plano in planos)
                {
                    area.AdicionarPlano(plano);
                }
            }

            await _uow.Areas.AdicionarAsync(area);
            await _uow.CommitAsync();

            return Result<AreaClube>.Ok(area);
        }

        public async Task<Result<AreaClube>> ObterPorIdAsync(int id)
        {
            var area = await _uow.Areas.ObterPorIdAsync(id);
            if (area == null)
                return Result<AreaClube>.Fail("Área não encontrada.");

            return Result<AreaClube>.Ok(area);
        }

        public async Task<Result<List<AreaClube>>> ListarAsync()
        {
            var areas = await _uow.Areas.ListarAsync();
            return Result<List<AreaClube>>.Ok(areas);
        }

        public async Task<Result<AreaClube>> AtualizarAsync(int areaId, string? novoNome, List<int>? planosPermitidosIds)
        {
            var area = await _uow.Areas.ObterPorIdAsync(areaId);
            if (area == null)
                return Result<AreaClube>.Fail("Área não encontrada.");

            if (!string.IsNullOrEmpty(novoNome))
                area.SetNome(novoNome);

            if (planosPermitidosIds != null && planosPermitidosIds.Count > 0)
            {
                var planos = await _uow.Planos.ListarPorIdsAsync(planosPermitidosIds);
                if (planos.Count > 0 && planos.Count != planosPermitidosIds.Count)
                {
                    var idsNaoEncontrados = planosPermitidosIds.Except(planos.Select(p => p.Id)).ToList();
                    return Result<AreaClube>.Fail($"Planos não encontrados: {string.Join(", ", idsNaoEncontrados)}");
                }
                area.PlanosPermitidos.Clear();
                foreach (var plano in planos)
                {
                    area.AdicionarPlano(plano);
                }
            }

            await _uow.Areas.AtualizarAsync(area);
            await _uow.CommitAsync();
            return Result<AreaClube>.Ok(area);
        }

        public async Task<Result> DeletarAsync(int id)
        {
            var area = await _uow.Areas.ObterPorIdAsync(id);
            if (area == null)
                return Result.Fail("Área não encontrada.");

            await _uow.Areas.RemoverAsync(area);
            await _uow.CommitAsync();
            return Result.Ok();
        }

        public async Task<Result> AdicionarPlanoAsync(int areaId, int planoId)
        {
            var area = await _uow.Areas.ObterPorIdAsync(areaId);
            if (area == null)
                return Result.Fail("Área não encontrada.");

            var plano = await _uow.Planos.ObterPorIdAsync(planoId);
            if (plano == null)
                return Result.Fail("Plano não encontrado.");

            if (area.PlanosPermitidos.Any(p => p.Id == planoId))
                return Result.Fail("Plano já está associado a esta área.");

            area.AdicionarPlano(plano);
            await _uow.Areas.AtualizarAsync(area);
            await _uow.CommitAsync();

            return Result.Ok();
        }

        public async Task<Result> RemoverPlanoAsync(int areaId, int planoId)
        {
            var area = await _uow.Areas.ObterPorIdAsync(areaId);
            if (area == null)
                return Result.Fail("Área não encontrada.");

            if (!area.PlanosPermitidos.Any(p => p.Id == planoId))
                return Result.Fail("Plano não está associado a esta área.");

            if (area.RemoverPlano(planoId))
            {
                await _uow.Areas.AtualizarAsync(area);
                await _uow.CommitAsync();
                return Result.Ok();
            }

            return Result.Fail("Falha ao remover plano da área.");
        }
    }
}