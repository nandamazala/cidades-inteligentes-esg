using System.Net.Http.Json;
using Xunit;

namespace Sus.Api.Tests;

public class StatusCodeTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task AuthController_ReturnsHttpStatusCode200()
    {
        var response = await _client.PostAsJsonAsync("/auth/token", new
        {
            username = "admin",
            password = "admin123"
        });

        await EnsureSuccessAsync(response);
    }

    [Fact]
    public async Task PacientesController_ReturnsHttpStatusCode200()
    {
        var response = await _client.GetAsync("/pacientes?page=1&pageSize=10");

        await EnsureSuccessAsync(response);
    }

    [Fact]
    public async Task UnidadesSaudeController_ReturnsHttpStatusCode200()
    {
        var response = await _client.GetAsync("/unidades-saude?page=1&pageSize=10");

        await EnsureSuccessAsync(response);
    }

    [Fact]
    public async Task AtendimentosController_ReturnsHttpStatusCode200()
    {
        var response = await _client.GetAsync("/atendimentos?page=1&pageSize=10");

        await EnsureSuccessAsync(response);
    }

    [Fact]
    public async Task AlertasController_ReturnsHttpStatusCode200()
    {
        var tokenResponse = await _client.PostAsJsonAsync("/auth/token", new
        {
            username = "admin",
            password = "admin123"
        });
        await EnsureSuccessAsync(tokenResponse);

        var token = await tokenResponse.Content.ReadFromJsonAsync<TokenResult>();
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token!.AccessToken);

        var response = await _client.GetAsync("/alertas?page=1&pageSize=10");

        await EnsureSuccessAsync(response);
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
