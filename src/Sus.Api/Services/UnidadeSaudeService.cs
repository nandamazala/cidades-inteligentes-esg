using Microsoft.EntityFrameworkCore;
using Sus.Api.Data;
using Sus.Api.Exceptions;
using Sus.Api.Models;
using Sus.Api.ViewModels;

namespace Sus.Api.Services;

public class UnidadeSaudeService(SusDbContext context)
{
    public Task<PagedResultViewModel<UnidadeSaudeResponseViewModel>> ListarAsync(int page, int pageSize)
    {
        return context.UnidadesSaude
            .AsNoTracking()
            .OrderBy(unidade => unidade.Nome)
            .Select(unidade => new UnidadeSaudeResponseViewModel
            {
                Id = unidade.Id,
                Nome = unidade.Nome,
                Endereco = unidade.Endereco,
                CapacidadeDiaria = unidade.CapacidadeDiaria
            })
            .ToPagedResultAsync(page, pageSize);
    }

    public async Task<UnidadeSaudeResponseViewModel> BuscarPorIdAsync(long id)
    {
        var unidade = await context.UnidadesSaude.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new ResourceNotFoundException($"Unidade de saude nao encontrada. id={id}");

        return ToResponse(unidade);
    }

    public async Task<UnidadeSaudeResponseViewModel> SalvarAsync(UnidadeSaudeRequestViewModel request)
    {
        var unidade = new UnidadeSaude
        {
            Nome = request.Nome,
            Endereco = request.Endereco,
            CapacidadeDiaria = request.CapacidadeDiaria
        };

        context.UnidadesSaude.Add(unidade);
        await context.SaveChangesAsync();

        return ToResponse(unidade);
    }

    public async Task<UnidadeSaudeResponseViewModel> AtualizarAsync(long id, UnidadeSaudeRequestViewModel request)
    {
        var unidade = await context.UnidadesSaude.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Unidade de saude nao encontrada. id={id}");

        unidade.Nome = request.Nome;
        unidade.Endereco = request.Endereco;
        unidade.CapacidadeDiaria = request.CapacidadeDiaria;

        await context.SaveChangesAsync();
        return ToResponse(unidade);
    }

    public async Task DeletarAsync(long id)
    {
        var unidade = await context.UnidadesSaude.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Unidade de saude nao encontrada. id={id}");

        var possuiAtendimentos = await context.Atendimentos.AnyAsync(atendimento => atendimento.UnidadeId == id);
        if (possuiAtendimentos)
        {
            throw new ConflictException("Unidade de saude nao pode ser removida porque possui atendimentos vinculados.");
        }

        context.UnidadesSaude.Remove(unidade);
        await context.SaveChangesAsync();
    }

    private static UnidadeSaudeResponseViewModel ToResponse(UnidadeSaude unidade)
    {
        return new UnidadeSaudeResponseViewModel
        {
            Id = unidade.Id,
            Nome = unidade.Nome,
            Endereco = unidade.Endereco,
            CapacidadeDiaria = unidade.CapacidadeDiaria
        };
    }
}
