using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sus.Api.Services;
using Sus.Api.ViewModels;

namespace Sus.Api.Controllers;

[ApiController]
[Route("unidades-saude")]
public class UnidadesSaudeController(UnidadeSaudeService service) : ControllerBase
{
    [HttpGet]
    public Task<PagedResultViewModel<UnidadeSaudeResponseViewModel>> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        return service.ListarAsync(page, pageSize);
    }

    [HttpGet("{id:long}")]
    public Task<UnidadeSaudeResponseViewModel> BuscarPorId(long id)
    {
        return service.BuscarPorIdAsync(id);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<UnidadeSaudeResponseViewModel>> Salvar([FromBody] UnidadeSaudeRequestViewModel request)
    {
        var response = await service.SalvarAsync(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = response.Id }, response);
    }

    [Authorize]
    [HttpPut("{id:long}")]
    public Task<UnidadeSaudeResponseViewModel> Atualizar(long id, [FromBody] UnidadeSaudeRequestViewModel request)
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
