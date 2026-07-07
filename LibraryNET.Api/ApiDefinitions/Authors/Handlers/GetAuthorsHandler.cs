using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Authors.Responses;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.Authors.Handlers;

internal sealed class GetAuthorsHandler(LibraryContext context)
{
    public async Task<IResult> Handle(CancellationToken cancellationToken)
    {
        return Results.Ok(await context.BookAuthors
            .Select(ResponseBookAuthor.Map())
            .ToArrayAsync(cancellationToken));
    }
}