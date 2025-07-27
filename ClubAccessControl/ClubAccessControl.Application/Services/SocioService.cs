using ClubAccessControl.Domain.Interfaces;
using ClubAccessControl.Application.Interfaces;
using ClubAccessControl.Domain.Common;
using ClubAccessControl.Domain.Entidades;

namespace ClubAccessControl.Application.Services
{
    public class SocioService : ISocioService
    {
        private readonly IUnitOfWork _uow;

        public SocioService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<Socio>> CriarAsync(Socio socio)
        {
            var plano = await _uow.Planos.ObterPorIdAsync(socio.PlanoId);
            if (plano == null)
                return Result<Socio>.Fail("Plano inválido.");

            await _uow.Socios.AdicionarAsync(socio);
            await _uow.CommitAsync();
            return Result<Socio>.Ok(socio);
        }

        public async Task<Result<Socio>> ObterPorIdAsync(int id)
        {
            var socio = await _uow.Socios.ObterPorIdAsync(id);
            if (socio == null)
                return Result<Socio>.Fail("Sócio não encontrado.");

            return Result<Socio>.Ok(socio);
        }

        public async Task<Result<List<Socio>>> ListarAsync()
        {
            var socios = await _uow.Socios.ListarAsync();
            return Result<List<Socio>>.Ok(socios);
        }

        public async Task<Result<Socio>> AtualizarAsync(int socioId, string? novoNome, int? planoId)
        {
            var socio = await _uow.Socios.ObterPorIdAsync(socioId);
            if (socio == null)
                return Result<Socio>.Fail("Sócio não encontrado.");

            if (!string.IsNullOrEmpty(novoNome))
                socio.SetNome(novoNome);

            if (planoId != null && planoId.Value > 0)
            {
                var plano = await _uow.Planos.ObterPorIdAsync(planoId.Value);
                if (plano!= null)
                {
                    socio.AlterarPlano(plano.Id, plano);
                }
            }

            await _uow.Socios.AtualizarAsync(socio);
            await _uow.CommitAsync();
            return Result<Socio>.Ok(socio);
        }

        public async Task<Result> AtualizarAsync(int id, Socio socioAtualizado)
        {
            var socio = await _uow.Socios.ObterPorIdAsync(id);
            if (socio == null)
                return Result.Fail("Sócio não encontrado.");

            var plano = await _uow.Planos.ObterPorIdAsync(socioAtualizado.PlanoId);
            if (plano == null)
                return Result.Fail("Plano inválido.");

            socio.SetNome(socioAtualizado.Nome);
            socio.AlterarPlano(socioAtualizado.PlanoId, plano);

            await _uow.Socios.AtualizarAsync(socio);
            await _uow.CommitAsync();
            return Result.Ok();
        }

        public async Task<Result> DeletarAsync(int id)
        {
            var socio = await _uow.Socios.ObterPorIdAsync(id);
            if (socio == null)
                return Result.Fail("Sócio não encontrado.");

            await _uow.Socios.RemoverAsync(socio);
            await _uow.CommitAsync();
            return Result.Ok();
        }

        public async Task<Result> AlterarPlanoAsync(int socioId, int novoPlanoId)
        {
            var socio = await _uow.Socios.ObterPorIdAsync(socioId);
            if (socio == null)
                return Result.Fail("Sócio não encontrado.");

            var novoPlano = await _uow.Planos.ObterPorIdAsync(novoPlanoId);
            if (novoPlano == null)
                return Result.Fail("Plano inválido.");

            socio.AlterarPlano(novoPlanoId, novoPlano);
            await _uow.Socios.AtualizarAsync(socio);
            await _uow.CommitAsync();

            return Result.Ok();
        }
    }
}