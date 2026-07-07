using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Data;
using LibraryNET.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Books.Handlers;

internal sealed class DeleteBookHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, Book.DecommissioningReason decommissioningReason, CancellationToken cancellationToken)
    {
        var book = await context.Books
            .Where(b => b.DecommissionReason == null)
            .Include(b => b.BookBorrowing)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        if (book is null)
        {
            return Results.NotFound();
        }

        if (book.BookBorrowing is not null)
        {
            return Results.UnprocessableEntity("Books is borrowed!");
        }

        book.DecommissionReason = decommissioningReason;
        book.DecommissionedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok();
    }
}