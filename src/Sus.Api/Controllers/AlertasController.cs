using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sus.Api.Services;
using Sus.Api.ViewModels;

namespace Sus.Api.Controllers;

[ApiController]
[Route("alertas")]
public class AlertasController(AlertaService service) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public Task<PagedResultViewModel<AlertaResponseViewModel>> Listar([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        return service.ListarAsync(page, pageSize);
    }
}
