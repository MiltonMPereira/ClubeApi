using ClubAccessControl.Tests.Fixtures;
using ClubAccessControl.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Tests.Integration
{
    public class AcessoIntegrationTests : IClassFixture<ClubeApiFactory>
    {
        private readonly HttpClient _client;

        public AcessoIntegrationTests(ClubeApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Deve_Registrar_Tentativa_Acesso()
        {
            // Cria area
            var area = await _client.PostJsonAsync<dynamic>("/Area", new { nome = "Academia" });

            // Cria plano com area permitida
            var plano = new
            {
                nome = "Premium",
                areasPermitidasIds = new[] { (int)area.id }
            };
            var planoCriado = await _client.PostJsonAsync<dynamic>("/Plano", plano);

            // Cria socio vinculado ao plano
            var socio = await _client.PostJsonAsync<dynamic>("/Socio", new { nome = "Maria", planoId = (int)planoCriado.id });

            // Realiza tentativa de acesso
            var tentativa = new { socioId = (int)socio.id, areaClubeId = (int)area.id };
            var result = await _client.PostJsonAsync<dynamic>("/Acesso/acessar", tentativa);

            Assert.True((bool)result.autorizado);
        }
    }
}
