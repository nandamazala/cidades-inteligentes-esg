using Microsoft.AspNetCore.Mvc;
using Sus.Api.Security;
using Sus.Api.ViewModels;

namespace Sus.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IConfiguration configuration, JwtService jwtService) : ControllerBase
{
    [HttpPost("token")]
    public ActionResult<TokenResponseViewModel> GerarToken([FromBody] LoginRequestViewModel request)
    {
        var username = configuration["AppUser:Username"];
        var password = configuration["AppUser:Password"];

        if (request.Username != username || request.Password != password)
        {
            return Unauthorized();
        }

        return Ok(new TokenResponseViewModel
        {
            AccessToken = jwtService.GenerateToken(request.Username)
        });
    }
}
