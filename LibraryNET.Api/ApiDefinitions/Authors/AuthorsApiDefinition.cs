using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LibraryNET.Api.ApiDefinitions.Authors.Handlers;
using LibraryNET.Api.ApiDefinitions.Authors.Requests;
using LibraryNET.Api.Extensions;
using LibraryNET.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryNET.Api.ApiDefinitions.Authors;

// ReSharper disable once UnusedType.Global
public sealed class AuthorsApiDefinition : IApiDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var groupBuilder = app.MapGroup("authors");

        groupBuilder.MapGet("", GetAuthors)
            .WithName("Get all authors")
            .WithDescription("Gets all authors");

        groupBuilder.MapPost("", CreateAuthor)
            .WithName("Create author")
            .WithDescription("Creates author")
            .AddEndpointFilter<ValidationFilter<RequestCreateAuthor>>();
    }

    public void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetAuthorsHandler>();
        serviceCollection.AddScoped<CreateAuthorHandler>();
        
        serviceCollection.AddScoped<IValidator<RequestCreateAuthor>, RequestCreateAuthor.Validator>();
    }

    private static async Task<IResult> GetAuthors(GetAuthorsHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(cancellationToken);

    private static async Task<IResult> CreateAuthor(CreateAuthorHandler handler, RequestCreateAuthor request, CancellationToken cancellationToken)
        => await handler.Handle(request, cancellationToken);
}