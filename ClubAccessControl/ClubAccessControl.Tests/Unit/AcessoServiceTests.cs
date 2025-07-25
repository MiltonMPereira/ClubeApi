using ClubAccessControl.Application.Services;
using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClubAccessControl.Tests.Unit
{
    public class AcessoServiceTests
    {
        private ClubeContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ClubeContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ClubeContext(options);

            // Seed inicial
            var areaPiscina = new AreaClube("Piscina");
            var areaAcademia = new AreaClube("Academia");

            var planoBasico = new PlanoAcesso("Básico");
            planoBasico.AreasPermitidas.Add(areaPiscina);

            context.Planos.Add(planoBasico);
            var socio = new Socio("João", planoBasico.Id);

            context.Areas.AddRange(areaPiscina, areaAcademia);
            context.Socios.Add(socio);
            context.SaveChanges();

            return context;
        }

        private AcessoService GetService(ClubeContext context)
        {
            var socioRepo = new SocioRepository(context);
            var areaRepo = new AreaRepository(context);
            var tentativaRepo = new AcessoRepository(context);
            return new AcessoService(socioRepo, areaRepo, tentativaRepo);
        }

        [Fact]
        public async Task RegistrarTentativa_DevePermitirQuandoAreaPermitida()
        {
            // Arrange  
            using var context = GetContext();
            var service = GetService(context);

            // Act
            var resultado = await service.RegistrarTentativaAsync(1, 1); // socioId=1, areaId=1 (Piscina)

            // Assert
            Assert.True(resultado.Valor.Autorizado);
            Assert.Equal("Piscina", resultado.Valor.AreaClube.Nome);
        }

        [Fact]
        public async Task RegistrarTentativa_DeveNegarQuandoAreaNaoPermitida()
        {
            // Arrange 
            using var context = GetContext();
            var service = GetService(context);

            // Act
            var resultado = await service.RegistrarTentativaAsync(1, 2); // socioId=1, areaId=2 (Academia)

            // Assert
            Assert.True(resultado.Erro== "Acesso negado pelo plano do sócio.");
        }

        [Fact]
        public async Task RegistrarTentativa_DeveLancarExcecao_QuandoSocioNaoExiste()
        {
            using var context = GetContext();
            var service = GetService(context);

            var tentativa = await service.RegistrarTentativaAsync(99, 1);

            Assert.True(tentativa.Erro == "Sócio não encontrado.");
            Assert.Null(tentativa.Valor);
        }

        [Fact]
        public async Task RegistrarTentativa_DeveLancarExcecao_QuandoAreaNaoExiste()
        {
            using var context = GetContext();
            var service = GetService(context);

            var tentativa = await service.RegistrarTentativaAsync(1, 99);

            Assert.True(tentativa.Erro == "Área não encontrada.");
            Assert.Null(tentativa.Valor);
        }
    }
}
