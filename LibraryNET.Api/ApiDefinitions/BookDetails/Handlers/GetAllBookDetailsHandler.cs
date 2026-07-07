using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.BookDetails.Responses;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.BookDetails.Handlers;

internal sealed class GetAllBookDetailsHandler(LibraryContext context)
{
    public async Task<IResult> Handle(CancellationToken cancellationToken)
    {
        return Results.Ok(await context.BookDetails
            .Select(ResponseBookDetails.Map())
            .ToArrayAsync(cancellationToken));
    }
}