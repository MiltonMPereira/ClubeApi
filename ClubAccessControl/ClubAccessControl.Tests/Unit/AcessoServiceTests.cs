using ClubAccessControl.Application.Services;
using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Domain.Interfaces;
using ClubAccessControl.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

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

        private (AcessoService, Mock<IUnitOfWork>) GetService(ClubeContext context)
        {
            var uowMock = new Mock<IUnitOfWork>();

            // Configurar os repositórios mockados para usar o contexto em memória
            var acessoRepo = new AcessoRepository(context);
            var areaRepo = new AreaRepository(context);
            var socioRepo = new SocioRepository(context);
            var planoRepo = new PlanoRepository(context);

            uowMock.Setup(u => u.Acessos).Returns(acessoRepo);
            uowMock.Setup(u => u.Areas).Returns(areaRepo);
            uowMock.Setup(u => u.Planos).Returns(planoRepo);
            uowMock.Setup(u => u.Socios).Returns(socioRepo);

            // Configurar o CommitAsync para chamar SaveChanges no contexto real
            uowMock.Setup(u => u.CommitAsync()).ReturnsAsync(() => context.SaveChanges());

            var service = new AcessoService(uowMock.Object);

            return (service, uowMock);
        }

        [Fact]
        public async Task RegistrarTentativa_DevePermitirQuandoAreaPermitida()
        {
            // Arrange  
            using var context = GetContext();
            var (service, uowMock) = GetService(context);

            // Act
            var resultado = await service.RegistrarTentativaAsync(1, 1); // socioId=1, areaId=1 (Piscina)

            // Assert
            Assert.True(resultado.Valor.Autorizado);
            Assert.Equal("Piscina", resultado.Valor.AreaClube.Nome);

            // Verificar se o acesso foi registrado
            var acessos = await context.TentativasAcesso.ToListAsync();
            Assert.Single(acessos);
            Assert.True(acessos[0].Autorizado);

            // Verificar se o Commit foi chamado
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task RegistrarTentativa_DeveNegarQuandoAreaNaoPermitida()
        {
            // Arrange 
            using var context = GetContext();
            var (service, uowMock) = GetService(context);

            // Act
            var resultado = await service.RegistrarTentativaAsync(1, 2); // socioId=1, areaId=2 (Academia)

            // Assert
            Assert.Equal("Acesso negado pelo plano do sócio.", resultado.Erro);

            // Verificar que o acesso foi registrado como negado
            var acessos = await context.TentativasAcesso.ToListAsync();
            Assert.Single(acessos);
            Assert.False(acessos[0].Autorizado);

            // Verificar se o Commit foi chamado
            uowMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task RegistrarTentativa_DeveLancarExcecao_QuandoSocioNaoExiste()
        {
            using var context = GetContext();
            var (service, uowMock) = GetService(context);

            var tentativa = await service.RegistrarTentativaAsync(99, 1);

            // Assert
            Assert.Equal("Sócio não encontrado.", tentativa.Erro);
            Assert.Null(tentativa.Valor);

            // Verificar que nenhum acesso foi registrado
            var acessos = await context.TentativasAcesso.ToListAsync();
            Assert.Empty(acessos);

            // Verificar que o Commit não foi chamado
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task RegistrarTentativa_DeveLancarExcecao_QuandoAreaNaoExiste()
        {
            using var context = GetContext();
            var (service, uowMock) = GetService(context);

            var tentativa = await service.RegistrarTentativaAsync(1, 99);
             
            // Assert
            Assert.Equal("Área não encontrada.", tentativa.Erro);
            Assert.Null(tentativa.Valor);

            // Verificar que nenhum acesso foi registrado
            var acessos = await context.TentativasAcesso.ToListAsync();
            Assert.Empty(acessos);

            // Verificar que o Commit não foi chamado
            uowMock.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
