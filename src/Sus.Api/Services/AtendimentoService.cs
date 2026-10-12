using Microsoft.EntityFrameworkCore;
using Sus.Api.Data;
using Sus.Api.Exceptions;
using Sus.Api.Models;
using Sus.Api.ViewModels;

namespace Sus.Api.Services;

public class AtendimentoService(SusDbContext context)
{
    public Task<PagedResultViewModel<AtendimentoResponseViewModel>> ListarAsync(int page, int pageSize)
    {
        return context.Atendimentos
            .AsNoTracking()
            .Include(atendimento => atendimento.Paciente)
            .Include(atendimento => atendimento.Unidade)
            .OrderByDescending(atendimento => atendimento.DataEntrada)
            .Select(atendimento => new AtendimentoResponseViewModel
            {
                Id = atendimento.Id,
                PacienteId = atendimento.PacienteId,
                PacienteNome = atendimento.Paciente.Nome,
                UnidadeId = atendimento.UnidadeId,
                UnidadeNome = atendimento.Unidade.Nome,
                DataEntrada = atendimento.DataEntrada,
                DataAtendimento = atendimento.DataAtendimento,
                Status = atendimento.Status
            })
            .ToPagedResultAsync(page, pageSize);
    }

    public async Task<AtendimentoResponseViewModel> BuscarPorIdAsync(long id)
    {
        var atendimento = await context.Atendimentos
            .AsNoTracking()
            .Include(item => item.Paciente)
            .Include(item => item.Unidade)
            .FirstOrDefaultAsync(item => item.Id == id)
            ?? throw new ResourceNotFoundException($"Atendimento nao encontrado. id={id}");

        return ToResponse(atendimento);
    }

    public async Task<AtendimentoResponseViewModel> SalvarAsync(AtendimentoRequestViewModel request)
    {
        await ValidarRelacionamentosAsync(request.PacienteId, request.UnidadeId);

        var atendimento = new Atendimento
        {
            PacienteId = request.PacienteId,
            UnidadeId = request.UnidadeId,
            DataEntrada = DateTime.UtcNow,
            DataAtendimento = request.DataAtendimento,
            Status = request.Status
        };

        context.Atendimentos.Add(atendimento);
        await context.SaveChangesAsync();
        await RegistrarEventosAsync(atendimento, atendimento.Status.Equals("FINALIZADO", StringComparison.OrdinalIgnoreCase));

        return await BuscarPorIdAsync(atendimento.Id);
    }

    public async Task<AtendimentoResponseViewModel> AtualizarAsync(long id, AtendimentoRequestViewModel request)
    {
        await ValidarRelacionamentosAsync(request.PacienteId, request.UnidadeId);

        var atendimento = await context.Atendimentos.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Atendimento nao encontrado. id={id}");

        var statusAnterior = atendimento.Status;

        atendimento.PacienteId = request.PacienteId;
        atendimento.UnidadeId = request.UnidadeId;
        atendimento.DataAtendimento = request.DataAtendimento;
        atendimento.Status = request.Status;

        await context.SaveChangesAsync();

        var finalizadoAgora =
            !statusAnterior.Equals("FINALIZADO", StringComparison.OrdinalIgnoreCase) &&
            atendimento.Status.Equals("FINALIZADO", StringComparison.OrdinalIgnoreCase);

        await RegistrarEventosAsync(atendimento, finalizadoAgora);

        return await BuscarPorIdAsync(atendimento.Id);
    }

    public async Task DeletarAsync(long id)
    {
        var atendimento = await context.Atendimentos.FindAsync(id)
            ?? throw new ResourceNotFoundException($"Atendimento nao encontrado. id={id}");

        context.Atendimentos.Remove(atendimento);
        await context.SaveChangesAsync();
    }

    private async Task ValidarRelacionamentosAsync(long pacienteId, long unidadeId)
    {
        if (!await context.Pacientes.AnyAsync(paciente => paciente.Id == pacienteId))
        {
            throw new ResourceNotFoundException($"Paciente nao encontrado. id={pacienteId}");
        }

        if (!await context.UnidadesSaude.AnyAsync(unidade => unidade.Id == unidadeId))
        {
            throw new ResourceNotFoundException($"Unidade de saude nao encontrada. id={unidadeId}");
        }
    }

    private async Task RegistrarEventosAsync(Atendimento atendimento, bool registrarAuditoria)
    {
        if (registrarAuditoria)
        {
            context.Auditorias.Add(new Auditoria
            {
                Acao = "Atendimento finalizado",
                DataEvento = DateTime.UtcNow,
                AtendimentoId = atendimento.Id
            });
        }

        var dataReferencia = atendimento.DataEntrada.Date;
        var totalNaUnidade = await context.Atendimentos.CountAsync(item =>
            item.UnidadeId == atendimento.UnidadeId &&
            item.DataEntrada.Date == dataReferencia);

        var capacidade = await context.UnidadesSaude
            .Where(unidade => unidade.Id == atendimento.UnidadeId)
            .Select(unidade => unidade.CapacidadeDiaria)
            .FirstAsync();

        var alertaJaRegistrado = await context.Alertas.AnyAsync(alerta =>
            alerta.AtendimentoId == atendimento.Id &&
            alerta.Mensagem == "Capacidade diaria da unidade excedida");

        if (totalNaUnidade > capacidade && !alertaJaRegistrado)
        {
            context.Alertas.Add(new Alerta
            {
                Mensagem = "Capacidade diaria da unidade excedida",
                DataAlerta = DateTime.UtcNow,
                AtendimentoId = atendimento.Id
            });
        }

        await context.SaveChangesAsync();
    }

    private static AtendimentoResponseViewModel ToResponse(Atendimento atendimento)
    {
        return new AtendimentoResponseViewModel
        {
            Id = atendimento.Id,
            PacienteId = atendimento.PacienteId,
            PacienteNome = atendimento.Paciente.Nome,
            UnidadeId = atendimento.UnidadeId,
            UnidadeNome = atendimento.Unidade.Nome,
            DataEntrada = atendimento.DataEntrada,
            DataAtendimento = atendimento.DataAtendimento,
            Status = atendimento.Status
        };
    }
}
