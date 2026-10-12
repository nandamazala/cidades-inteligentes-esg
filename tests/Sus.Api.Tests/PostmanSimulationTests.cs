using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Sus.Api.Tests;

public class PostmanSimulationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task PostmanCollectionFlow_ReturnsExpectedSuccessStatusCodes()
    {
        var token = await GetTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        await EnsureSuccessAsync(await _client.GetAsync("/pacientes?page=1&pageSize=10"));
        await EnsureSuccessAsync(await _client.GetAsync("/unidades-saude?page=1&pageSize=10"));
        await EnsureSuccessAsync(await _client.GetAsync("/atendimentos?page=1&pageSize=10"));

        var paciente = await PostAndReadJsonAsync("/pacientes", new
        {
            nome = "Joao Santos",
            cpf = "98765432109",
            email = "joao@email.com",
            telefone = "11988887777"
        });

        var unidade = await PostAndReadJsonAsync("/unidades-saude", new
        {
            nome = "UBS Norte",
            endereco = "Rua Norte, 200",
            capacidadeDiaria = 20
        });

        await EnsureSuccessAsync(await _client.PostAsJsonAsync("/atendimentos", new
        {
            pacienteId = paciente.RootElement.GetProperty("id").GetInt64(),
            unidadeId = unidade.RootElement.GetProperty("id").GetInt64(),
            status = "ABERTO"
        }));

        await EnsureSuccessAsync(await _client.GetAsync("/alertas?page=1&pageSize=10"));
    }

    private async Task<string> GetTokenAsync()
    {
        var response = await _client.PostAsJsonAsync("/auth/token", new
        {
            username = "admin",
            password = "admin123"
        });

        await EnsureSuccessAsync(response);

        var payload = await response.Content.ReadFromJsonAsync<TokenResult>();
        return payload!.AccessToken;
    }

    private async Task<JsonDocument> PostAndReadJsonAsync(string url, object body)
    {
        var response = await _client.PostAsJsonAsync(url, body);
        await EnsureSuccessAsync(response);

        var json = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(json);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status {(int)response.StatusCode}: {body}");
        }

        response.EnsureSuccessStatusCode();
    }

    private class TokenResult
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
