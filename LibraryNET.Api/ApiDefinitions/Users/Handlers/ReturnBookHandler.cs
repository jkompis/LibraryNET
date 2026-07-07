using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Data;
using LibraryNET.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Users.Handlers;

internal sealed class ReturnBookHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, long bookId, CancellationToken cancellationToken)
    {
        var bookBorrowing = await context.BookBorrowings
            .FirstOrDefaultAsync(bb => bb.UserId == id && bb.BookId == bookId, cancellationToken);

        if (bookBorrowing is null)
        {
            return Results.NotFound();
        }

        var entityEntry = context.BookBorrowingsHistory.Add(new BookBorrowingHistory
        {
            UserId = bookBorrowing.UserId,
            BookId = bookBorrowing.BookId,
            BorrowedAt = bookBorrowing.BorrowedAt,
            ReturnedAt = DateTime.UtcNow
        });

        context.BookBorrowings.Remove(bookBorrowing);

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok(entityEntry.Entity.Id);
    }
}