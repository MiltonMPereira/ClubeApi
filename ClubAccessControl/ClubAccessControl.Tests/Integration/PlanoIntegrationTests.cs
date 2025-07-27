using ClubAccessControl.Tests.Fixtures;
using ClubAccessControl.Tests.Helpers;


namespace ClubAccessControl.Tests.Integration
{
    public class PlanoIntegrationTests : IClassFixture<ClubeApiFactory>
    {
        private readonly HttpClient _client;

        public PlanoIntegrationTests(ClubeApiFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CRUD_Plano_Completo()
        {
            // Cria area prévia
            var area = new { nome = "Piscina" };
            var areaCriada = await _client.PostJsonAsync<dynamic>("/Area", area);

            // Create plano com areasPermitidasIds
            var plano = new
            {
                nome = "Mensal",
                areasPermitidasIds = new[] { (int)areaCriada.id }
            };
            var planoCriado = await _client.PostJsonAsync<dynamic>("/Plano", plano);
            Assert.NotNull(planoCriado.id);

            var id = (int)planoCriado.id;

            // Read
            var planoObtido = await _client.GetJsonAsync<dynamic>($"/Plano/{id}");
            Assert.Equal("Mensal", (string)planoObtido.nome);

            // Update
            var planoAtualizado = new
            {
                nome = "Mensal Plus",
                areasPermitidasIds = new[] { (int)areaCriada.id }
            };
            await _client.PutJsonAsync<dynamic>($"/Plano/{id}", planoAtualizado);

            var planoAtualizadoResult = await _client.GetJsonAsync<dynamic>($"/Plano/{id}");
            Assert.Equal("Mensal Plus", (string)planoAtualizadoResult.nome);

            // Delete
            await _client.DeleteAsync($"/Plano/{id}");

            // Confirm delete
            var planos = await _client.GetJsonAsync<dynamic[]>("/Plano");
            Assert.DoesNotContain(planos, p => p.id == id);
        }
    }
}
