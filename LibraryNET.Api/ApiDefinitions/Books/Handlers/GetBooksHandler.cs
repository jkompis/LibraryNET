using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Books.Responses;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Books.Handlers;

internal sealed class GetBooksHandler(LibraryContext context)
{
    public async Task<IResult> Handle(CancellationToken cancellationToken)
    {
        return Results.Ok(await context.Books
            .Where(b => b.DecommissionReason == null)
            .Select(ResponseBook.Map())
            .ToArrayAsync(cancellationToken));
    }
}