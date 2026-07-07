using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Data;
using LibraryNET.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Users.Handlers;

internal sealed class BorrowBookHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, long bookId, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);

        if (user is null)
        {
            return Results.NotFound();
        }

        if (user.UnregisteredAt.HasValue)
        {
            return Results.UnprocessableEntity("User is no longer active!");
        }

        var book = await context.Books
            .Include(b => b.BookBorrowing)
            .FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);

        if (book is null)
        {
            return Results.NotFound();
        }

        if (book.BookBorrowing != null)
        {
            return Results.UnprocessableEntity("Books is already borrowed");
        }

        if (book.DecommissionReason is not null)
        {
            return Results.UnprocessableEntity("Book decommissioned");
        }

        context.BookBorrowings.Add(new BookBorrowing
        {
            User = user,
            Book = book,
            BorrowedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok();
    }
}