using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using api_login.CustomExceptions;

namespace api_login.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UnAuthenticatedAccessException)
            {
                context.Result = new JsonResult(new
                {
                    message = "Você precisa estar logado para acessar este recurso."
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new JsonResult(new
                {
                    message = "Você não possui permissão para acessar este recurso."
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }

    }
}


