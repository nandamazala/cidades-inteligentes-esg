using Microsoft.EntityFrameworkCore;

namespace Sus.Api.Exceptions;

public class GlobalExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ResourceNotFoundException exception)
        {
            await WriteErrorAsync(context, StatusCodes.Status404NotFound, exception.Message);
        }
        catch (ConflictException exception)
        {
            await WriteErrorAsync(context, StatusCodes.Status409Conflict, exception.Message);
        }
        catch (DbUpdateException)
        {
            await WriteErrorAsync(context, StatusCodes.Status409Conflict, "Nao foi possivel concluir a operacao no banco de dados.");
        }
        catch (Exception exception)
        {
            var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
            var message = environment.IsDevelopment() || environment.IsEnvironment("Testing")
                ? exception.Message
                : "Erro interno ao processar a requisicao.";

            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, message);
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var error = new ApiError
        {
            StatusCode = statusCode,
            Message = message
        };

        await context.Response.WriteAsJsonAsync(error);
    }
}
