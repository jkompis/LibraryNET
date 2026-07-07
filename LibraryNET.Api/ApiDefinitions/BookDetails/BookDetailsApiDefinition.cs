using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LibraryNET.Api.ApiDefinitions.BookDetails.Handlers;
using LibraryNET.Api.ApiDefinitions.BookDetails.Requests;
using LibraryNET.Api.Extensions;
using LibraryNET.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryNET.Api.ApiDefinitions.BookDetails;

// ReSharper disable once UnusedType.Global
public sealed class BookDetailsApiDefinition : IApiDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var groupBuilder = app.MapGroup("book-details");

        groupBuilder.MapGet("", GetAllBookDetails)
            .WithName("Get all book details")
            .WithDescription("Gets all book details");

        groupBuilder.MapGet("{id:long}", GetBookDetails)
            .WithName("Get book details")
            .WithDescription("Gets book details");

        groupBuilder.MapPost("", CreateBookDetails)
            .WithName("Create book details")
            .WithDescription("Creates book details")
            .AddEndpointFilter<ValidationFilter<RequestCreateBookDetails>>();

        groupBuilder.MapPost("{id:long}", UpdateBookDetails)
            .WithName("Update book details")
            .WithDescription("Updates book details")
            .AddEndpointFilter<ValidationFilter<RequestUpdateBookDetails>>();
    }

    public void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetAllBookDetailsHandler>();
        serviceCollection.AddScoped<GetBookDetailsHandler>();
        serviceCollection.AddScoped<CreateBookDetailsHandler>();
        serviceCollection.AddScoped<UpdateBookDetailsHandler>();
        
        serviceCollection.AddScoped<IValidator<RequestCreateBookDetails>, RequestCreateBookDetails.Validator>();
        serviceCollection.AddScoped<IValidator<RequestUpdateBookDetails>, RequestUpdateBookDetails.Validator>();
    }

    private static async Task<IResult> GetAllBookDetails(GetAllBookDetailsHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(cancellationToken);

    private static async Task<IResult> GetBookDetails([FromRoute] long id, GetBookDetailsHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(id, cancellationToken);

    private static async Task<IResult> CreateBookDetails([FromBody] RequestCreateBookDetails request, CreateBookDetailsHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(request, cancellationToken);

    private static async Task<IResult> UpdateBookDetails([FromRoute] long id, RequestUpdateBookDetails request, UpdateBookDetailsHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(id, request, cancellationToken);
}