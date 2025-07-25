using ClubAccessControl.Domain.Entidades;
using ClubAccessControl.Tests.Fixtures;
using ClubAccessControl.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubAccessControl.Tests.Integration
{
    public class AreaIntegrationTests : IClassFixture<ClubeApiFactory>
    {
        private readonly HttpClient _client;

        public AreaIntegrationTests(ClubeApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CRUD_Area_Completo_E_Vinculo_Com_Plano()
        {
            // ---------- CRUD SEM PLANO ----------

            // Create area
            var area = new { nome = "Piscina" };
            var areaCriada = await _client.PostJsonAsync<dynamic>("/Area", area);
            Assert.NotNull(areaCriada.id);
            var areaId = (int)areaCriada.id;

            // Read area
            var areaObtida = await _client.GetJsonAsync<dynamic>($"/Area/{areaId}");
            Assert.Equal("Piscina", (string)areaObtida.nome);

            // Update area
            var areaAtualizada = new { nome = "Piscina Olímpica" };
            await _client.PutJsonAsync<dynamic>($"/Area/{areaId}", areaAtualizada);

            var areaAtualizadaResult = await _client.GetJsonAsync<dynamic>($"/Area/{areaId}");
            Assert.Equal("Piscina Olímpica", (string)areaAtualizadaResult.nome);

            // ---------- VINCULO COM PLANO ----------

            // Cria outra area para vincular ao plano
            var areaAcademia = await _client.PostJsonAsync<AreaClube>("/Area", new { nome = "Academia" });

            // Cria plano com ambas areas
            var plano = new
            {
                nome = "Completo",
                areasPermitidasIds = new[] { areaId, (int)areaAcademia.Id }
            };
            var planoCriado = await _client.PostJsonAsync<dynamic>("/Plano", plano);
            Assert.NotNull(planoCriado.id);

            var planoId = (int)planoCriado.id;

            // Read plano e valida areas vinculadas
            var planoObtido = await _client.GetJsonAsync<dynamic>($"/Plano/{planoId}");

            var areasIds = planoObtido.areasPermitidasIds.ToObject<List<int>>();

            Assert.Contains(areaId, areasIds);
            Assert.Contains((int)areaAcademia.Id, areasIds);

            // ---------- DELETE AREA ----------

            await _client.DeleteAsync($"/Area/{areaId}");

            // Confirm delete
            var todasAreas = await _client.GetJsonAsync<dynamic[]>("/Area");
            Assert.DoesNotContain(todasAreas, a => a.id == areaId);
        }
    }
}
