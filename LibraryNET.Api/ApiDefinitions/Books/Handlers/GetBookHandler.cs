using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Books.Responses;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Books.Handlers;

internal sealed class GetBookHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, CancellationToken cancellationToken)
    {
        var book = await context.Books
            .Where(b => b.DecommissionReason == null)
            .Select(ResponseBook.Map())
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (book is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(book);
    }
}