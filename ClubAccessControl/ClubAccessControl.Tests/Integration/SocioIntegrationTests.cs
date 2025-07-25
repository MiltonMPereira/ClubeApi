using System.Threading.Tasks;
using ClubAccessControl.Tests.Fixtures;
using ClubAccessControl.Tests.Helpers;


namespace ClubAccessControl.Tests.Integration
{
    public class SocioIntegrationTests : IClassFixture<ClubeApiFactory>
    {
        private readonly HttpClient _client;

        public SocioIntegrationTests(ClubeApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CRUD_Socio_Completo()
        {
            // Cria area prévia
            var area = new { nome = "Piscina" };
            var areaCriada = await _client.PostJsonAsync<dynamic>("/Area", area);

            // Cria plano prévio com areasPermitidasIds
            var plano = new
            {
                nome = "Basico",
                areasPermitidasIds = new[] { (int)areaCriada.id }
            };

            var planoCriado = await _client.PostJsonAsync<dynamic>("/Plano", plano);

            // Create socio
            var socio = new { nome = "João", planoId = (int)planoCriado.id };
            var socioCriado = await _client.PostJsonAsync<dynamic>("/Socio", socio);
            Assert.NotNull(socioCriado.id);

            var id = (int)socioCriado.id;

            // Read
            var socioObtido = await _client.GetJsonAsync<dynamic>($"/Socio/{id}");
            Assert.Equal("João", (string)socioObtido.nome);

            // Update
            var socioAtualizado = new { nome = "João Silva" };
            await _client.PutJsonAsync<dynamic>($"/Socio/{id}", socioAtualizado);

            var socioAtualizadoResult = await _client.GetJsonAsync<dynamic>($"/Socio/{id}");
            Assert.Equal("João Silva", (string)socioAtualizadoResult.nome);

            // Delete
            await _client.DeleteAsync($"/Socio/{id}");

            // Confirm delete
            var socios = await _client.GetJsonAsync<dynamic[]>("/Socio");
            Assert.DoesNotContain(socios, s => s.id == id);
        }
    }
}
