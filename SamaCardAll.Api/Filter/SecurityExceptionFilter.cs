using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SecurityExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Intercepta a exceção específica de segurança
        if (context.Exception is UnauthorizedAccessException)
        {
            // 1. Define o código de status HTTP para 401 Unauthorized
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

            // 2. Cria a resposta JSON que o Blazor espera
            context.Result = new JsonResult(new
            {
                error = "Unauthorized",
                message = context.Exception.Message // Passa sua mensagem amigável ("Usuário não autenticado...")
            });

            // 3. Indica que a exceção foi tratada (impede o 500)
            context.ExceptionHandled = true;
        }
        // Você pode adicionar mais 'else if' para outras exceções (ex: Forbidden, NotFound)
    }
}