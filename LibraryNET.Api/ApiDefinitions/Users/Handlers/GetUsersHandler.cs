using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Users.Responses;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Users.Handlers;

internal sealed class GetAllUsersHandler(LibraryContext context)
{
    public async Task<IResult> Handle(CancellationToken cancellationToken)
    {
        return Results.Ok(await context.Users
            .Where(u => u.UserDetails != null)
            .Select(ResponseUser.Map())
            .ToArrayAsync(cancellationToken: cancellationToken));
    }
}