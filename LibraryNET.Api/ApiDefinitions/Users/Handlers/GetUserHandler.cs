using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Users.Responses;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Users.Handlers;

internal sealed class GetUserHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .Where(u => u.Id == id)
            .Select(ResponseUser.Map())
            .FirstOrDefaultAsync(cancellationToken);

        return user is null
            ? Results.NotFound()
            : Results.Ok(user);
    }
}