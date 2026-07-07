using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryNET.Api.Filters;

internal sealed class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // find request to validate
        var model = context.Arguments.OfType<T>().FirstOrDefault();

        if (model == null)
        {
            return await next(context);
        }
        
        // resolve validator, will throw if it was not registered
        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();

        var result = await validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            // map validation details for response
            return Results.ValidationProblem(result.ToDictionary());
        }

        return await next(context);
    }
}