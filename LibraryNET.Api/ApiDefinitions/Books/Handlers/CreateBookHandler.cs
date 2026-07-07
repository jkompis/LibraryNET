using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Data;
using LibraryNET.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Books.Handlers;

internal sealed class CreateBookHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long bookDetailsId, CancellationToken cancellationToken)
    {
        var bookDetails = await context.BookDetails
            .FirstOrDefaultAsync(bd => bd.Id == bookDetailsId, cancellationToken);

        if (bookDetails is null)
        {
            return Results.BadRequest("Book details not found");
        }

        var entityEntry = context.Add(new Book
        {
            CommissionedAt = DateTime.UtcNow,
            BookDetails = bookDetails
        });

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok(entityEntry.Entity.Id);
    }
}