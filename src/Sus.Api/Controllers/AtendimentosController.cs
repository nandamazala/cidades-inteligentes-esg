using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sus.Api.Services;
using Sus.Api.ViewModels;

namespace Sus.Api.Controllers;

[ApiController]
[Route("atendimentos")]
public class AtendimentosController(AtendimentoService service) : ControllerBase
{
    [HttpGet]
    public Task<PagedResultViewModel<AtendimentoResponseViewModel>> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        return service.ListarAsync(page, pageSize);
    }

    [HttpGet("{id:long}")]
    public Task<AtendimentoResponseViewModel> BuscarPorId(long id)
    {
        return service.BuscarPorIdAsync(id);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AtendimentoResponseViewModel>> Salvar([FromBody] AtendimentoRequestViewModel request)
    {
        var response = await service.SalvarAsync(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = response.Id }, response);
    }

    [Authorize]
    [HttpPut("{id:long}")]
    public Task<AtendimentoResponseViewModel> Atualizar(long id, [FromBody] AtendimentoRequestViewModel request)
    {
        return service.AtualizarAsync(id, request);
    }

    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Deletar(long id)
    {
        await service.DeletarAsync(id);
        return NoContent();
    }
}
