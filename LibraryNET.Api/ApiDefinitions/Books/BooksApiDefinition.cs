using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Books.Handlers;
using LibraryNET.Api.Extensions;
using LibraryNET.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryNET.Api.ApiDefinitions.Books;

// ReSharper disable once UnusedType.Global
public sealed class BooksApiDefinition : IApiDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var group = app.MapGroup("books");

        group.MapGet("", GetAll)
            .WithName("Get all books")
            .WithDescription("Gets all books");

        group.MapGet("{id:long}", GetById)
            .WithName("Get book by id")
            .WithDescription("Gets book by id");

        group.MapPost("", CreateBook)
            .WithName("Create new book")
            .WithDescription("Creates new book")
            .Produces<long>();

        group.MapPut("{id:long}", UpdateBook)
            .WithName("Update book")
            .WithDescription("Updates book");

        group.MapDelete("{id:long}", DecommissionBook)
            .WithName("Delete book")
            .WithDescription("Deletes new book");
    }

    public void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<GetBooksHandler>();
        serviceCollection.AddScoped<GetBookHandler>();
        serviceCollection.AddScoped<CreateBookHandler>();
        serviceCollection.AddScoped<UpdateBookHandler>();
        serviceCollection.AddScoped<DeleteBookHandler>();
    }

    private static async Task<IResult> GetAll(GetBooksHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(cancellationToken);

    private static async Task<IResult> GetById([FromRoute] long id, GetBookHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(id, cancellationToken);

    private static async Task<IResult> CreateBook([FromQuery] long bookDetailsId, CreateBookHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(bookDetailsId, cancellationToken);

    private static async Task<IResult> UpdateBook([FromRoute] long id, [FromQuery] long bookDetailsId, UpdateBookHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(id, bookDetailsId, cancellationToken);

    private static async Task<IResult> DecommissionBook([FromRoute] long id, [FromQuery] Book.DecommissioningReason decommissioningReason, DeleteBookHandler handler,
        CancellationToken cancellationToken)
        => await handler.Handle(id, decommissioningReason, cancellationToken);
}