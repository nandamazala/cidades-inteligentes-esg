using Microsoft.EntityFrameworkCore;
using Sus.Api.Data;
using Sus.Api.ViewModels;

namespace Sus.Api.Services;

public class AlertaService(SusDbContext context)
{
    public Task<PagedResultViewModel<AlertaResponseViewModel>> ListarAsync(int page, int pageSize)
    {
        return context.Alertas
            .AsNoTracking()
            .OrderByDescending(alerta => alerta.DataAlerta)
            .Select(alerta => new AlertaResponseViewModel
            {
                Id = alerta.Id,
                Mensagem = alerta.Mensagem,
                DataAlerta = alerta.DataAlerta,
                AtendimentoId = alerta.AtendimentoId
            })
            .ToPagedResultAsync(page, pageSize);
    }
}
