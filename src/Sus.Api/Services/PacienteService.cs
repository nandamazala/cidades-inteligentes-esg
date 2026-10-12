using Microsoft.EntityFrameworkCore;
using Sus.Api.Data;
using Sus.Api.Exceptions;
using Sus.Api.Models;
using Sus.Api.ViewModels;

namespace Sus.Api.Services;

public class PacienteService(SusDbContext context)
{
    public Task<PagedResultViewModel<PacienteResponseViewModel>> ListarAsync(int page, int pageSize)
    {
        return context.Pacientes
            .AsNoTracking()
            .OrderBy(paciente => paciente.Nome)
            .Select(paciente => new PacienteResponseViewModel
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Cpf = paciente.Cpf,
                Email = paciente.Email,
                Telefone = paciente.Telefone
            })
            .ToPagedResultAsync(page, pageSize);
    }

    public async Task<PacienteResponseViewModel> BuscarPorIdAsync(long id)
    {
        var paciente = await context.Pacientes.AsNoTracking().FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new ResourceNotFoundException($"Paciente nao encontrado. id={id}");

        return ToResponse(paciente);
    }

    public async Task<PacienteResponseViewModel> SalvarAsync(PacienteRequestViewModel request)
    {
        var paciente = new Paciente
        {
            Nome = request.Nome,
            Cpf = request.Cpf,
            Email = request.Email,
            Telefone = request.Telefone
        };

        context.Pacientes.Add(paciente);
        await context.SaveChangesAsync();

        return ToResponse(paciente);
    }

    public async Task<PacienteResponseViewModel> AtualizarAsync(long id, PacienteRequestViewModel request)
    {
        var paciente = await context.Pacientes.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Paciente nao encontrado. id={id}");

        paciente.Nome = request.Nome;
        paciente.Cpf = request.Cpf;
        paciente.Email = request.Email;
        paciente.Telefone = request.Telefone;

        await context.SaveChangesAsync();
        return ToResponse(paciente);
    }

    public async Task DeletarAsync(long id)
    {
        var paciente = await context.Pacientes.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Paciente nao encontrado. id={id}");

        var possuiAtendimentos = await context.Atendimentos.AnyAsync(atendimento => atendimento.PacienteId == id);
        if (possuiAtendimentos)
        {
            throw new ConflictException("Paciente nao pode ser removido porque possui atendimentos vinculados.");
        }

        context.Pacientes.Remove(paciente);
        await context.SaveChangesAsync();
    }

    private static PacienteResponseViewModel ToResponse(Paciente paciente)
    {
        return new PacienteResponseViewModel
        {
            Id = paciente.Id,
            Nome = paciente.Nome,
            Cpf = paciente.Cpf,
            Email = paciente.Email,
            Telefone = paciente.Telefone
        };
    }
}
