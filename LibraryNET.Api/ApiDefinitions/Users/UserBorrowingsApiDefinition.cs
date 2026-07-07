using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Users.Handlers;
using LibraryNET.Api.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryNET.Api.ApiDefinitions.Users;

// ReSharper disable once UnusedType.Global
public sealed class UserBorrowingsApiDefinition : IApiDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var groupBuilder = app.MapGroup("users/{id:long}/borrowings");
        
        groupBuilder.MapPost("", ListBorrowedBooks)
            .WithName("List books borrowed to user")
            .WithDescription("Lists books borrowed to user");

        groupBuilder.MapPost("{bookId:long}", BorrowBook)
            .WithName("Borrow user a book")
            .WithDescription("Borrows user a book");

        groupBuilder.MapDelete("{bookId:long}", ReturnBook)
            .WithName("Return book borrowed to user")
            .WithDescription("Returns book borrowed to user");
    }

    public void RegisterServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ListBorrowedBooksHandler>();
        serviceCollection.AddScoped<BorrowBookHandler>();
        serviceCollection.AddScoped<ReturnBookHandler>();
    }
    
    private static async Task<IResult> ListBorrowedBooks([FromRoute] long id, ListBorrowedBooksHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(id, cancellationToken);

    private static async Task<IResult> BorrowBook([FromRoute] long id, [FromRoute] long bookId, BorrowBookHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(id, bookId, cancellationToken);

    private static async Task<IResult> ReturnBook([FromRoute] long id, [FromRoute] long bookId, ReturnBookHandler handler, CancellationToken cancellationToken)
        => await handler.Handle(id, bookId, cancellationToken);
}