using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Exceptions;

namespace WorkbenchAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);

            }
            catch(ForbidException fe)
            {
                context.Response.StatusCode = 403;
            }
            catch(BadRequestException bre)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(bre.Message);
            }
            catch(NotFoundExceptions nfe)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(nfe.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.StatusCode = 500;
                await  context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
