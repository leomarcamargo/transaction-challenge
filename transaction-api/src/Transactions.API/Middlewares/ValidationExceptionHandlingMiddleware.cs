using FluentValidation;
using Transactions.API.Models;

namespace Transactions.API.Middlewares;

public sealed class ValidationExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            var validationErrors = exception.Errors.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);

            var error = new ExceptionResponse(nameof(ValidationException),
                StatusCodes.Status400BadRequest,
                "One or more validation errors has occurred", 
                validationErrors);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}