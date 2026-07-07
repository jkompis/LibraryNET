using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.BorrowingReminders.Responses;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.BorrowingReminders.Handlers;

internal sealed class GetPendingBorrowingRemindersHandler(LibraryContext context)
{
    public async Task<IResult> Handle(CancellationToken cancellationToken)
    {
        // 90 days period remind 7 days before
        var cutoffTime = DateTime.UtcNow.AddDays(-83);

        return Results.Ok(await context.BookBorrowings
            .Where(bb => bb.RemindedAt == null && bb.BorrowedAt <= cutoffTime)
            .Select(ResponsePendingBorrowingReminder.Map())
            .ToArrayAsync(cancellationToken));
    }
}