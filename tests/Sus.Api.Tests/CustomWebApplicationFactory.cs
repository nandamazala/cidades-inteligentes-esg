using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sus.Api.Data;
using Sus.Api.Models;

namespace Sus.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly InMemoryDatabaseRoot _databaseRoot = new();
    private readonly string _databaseName = $"SusTests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Database:DisableAutoMigrations"] = "true"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<SusDbContext>>();
            services.AddDbContext<SusDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName, _databaseRoot));

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SusDbContext>();
            Seed(context);
        });
    }

    private static void Seed(SusDbContext context)
    {
        if (context.Pacientes.Any())
        {
            return;
        }

        var paciente = new Paciente
        {
            Id = 1,
            Nome = "Maria Silva",
            Cpf = "12345678901",
            Email = "maria@email.com",
            Telefone = "11999999999"
        };

        var unidade = new UnidadeSaude
        {
            Id = 1,
            Nome = "UBS Central",
            Endereco = "Rua Central, 100",
            CapacidadeDiaria = 50
        };

        var atendimento = new Atendimento
        {
            Id = 1,
            PacienteId = 1,
            Paciente = paciente,
            UnidadeId = 1,
            Unidade = unidade,
            DataEntrada = DateTime.UtcNow,
            Status = "ABERTO"
        };

        var alerta = new Alerta
        {
            Id = 1,
            AtendimentoId = 1,
            Atendimento = atendimento,
            DataAlerta = DateTime.UtcNow,
            Mensagem = "Capacidade diaria da unidade excedida"
        };

        context.Pacientes.Add(paciente);
        context.UnidadesSaude.Add(unidade);
        context.Atendimentos.Add(atendimento);
        context.Alertas.Add(alerta);
        context.SaveChanges();
    }
}
