using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Books.Responses;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Users.Handlers;

internal sealed class ListBorrowedBooksHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, CancellationToken cancellationToken)
    {
        return Results.Ok(await context.BookBorrowings
            .Where(bb => bb.UserId == id)
            .Select(ResponseBorrowing.Map())
            .ToArrayAsync(cancellationToken));
    }
}