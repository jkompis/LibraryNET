using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Books.Handlers;

internal sealed class UpdateBookHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, long bookDetailsId, CancellationToken cancellationToken)
    {
        var book = await context.Books
            .Where(b => b.DecommissionReason == null)
            .Include(b => b.BookBorrowing)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (book is null)
        {
            return Results.NotFound();
        }

        var bookDetails = await context.BookDetails
            .FirstOrDefaultAsync(bd => bd.Id == bookDetailsId, cancellationToken);

        if (bookDetails is null)
        {
            return Results.BadRequest("Book details not found");
        }

        book.BookDetails = bookDetails;

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok();
    }
}