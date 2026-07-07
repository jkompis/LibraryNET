using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.BorrowingReminders.Handlers;

internal sealed class MarkRemindedHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long userId, long bookId, CancellationToken cancellationToken)
    {
        var bookBorrowing = await context.BookBorrowings
            .FirstOrDefaultAsync(bb => bb.UserId == userId && bb.BookId == bookId, cancellationToken);

        if (bookBorrowing is null)
        {
            return Results.NotFound();
        }

        bookBorrowing.RemindedAt = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok();
    }
}