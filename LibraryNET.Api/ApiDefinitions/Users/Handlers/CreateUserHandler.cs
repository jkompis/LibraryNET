using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Users.Requests;
using LibraryNET.Data;
using LibraryNET.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace LibraryNET.Api.ApiDefinitions.Users.Handlers;

internal sealed class CreateUserHandler(LibraryContext context)
{
    public async Task<IResult> Handle(RequestCreateUser request, CancellationToken cancellationToken)
    {
        var entityEntry = context.Users.Add(new User
        {
            RegisteredAt = DateTime.UtcNow,
            UserDetails = new UserDetails
            {
                Name = request.Name!,
                Address = request.Address!,
                PhoneNumber = request.PhoneNumber,
                EmailAddress = request.EmailAddress!
            }
        });

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok(entityEntry.Entity.Id);
    }
}