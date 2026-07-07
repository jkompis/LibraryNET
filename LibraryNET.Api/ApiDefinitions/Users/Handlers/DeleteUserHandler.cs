using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Users.Handlers;

internal sealed class DeleteUserHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Include(u => u.UserDetails)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);

        if (user is null)
        {
            return Results.NotFound("User not found");
        }

        if (user.UserDetails is null)
        {
            return Results.UnprocessableEntity("User already deleted!");
        }

        if (await context.BookBorrowings.AnyAsync(bb => bb.UserId == user.Id, cancellationToken))
        {
            return Results.BadRequest("User has borrowed books!");
        }

        user.UnregisteredAt = DateTime.UtcNow;
        context.UserDetails.Remove(user.UserDetails!);
        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok();
    }
}