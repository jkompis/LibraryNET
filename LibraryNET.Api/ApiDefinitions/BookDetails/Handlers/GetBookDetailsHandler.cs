using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.BookDetails.Responses;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.BookDetails.Handlers;

internal sealed class GetBookDetailsHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, CancellationToken cancellationToken)
    {
        var bookDetails = await context.BookDetails
            .Select(ResponseBookDetails.Map())
            .FirstOrDefaultAsync(bd => bd.Id == id, cancellationToken);

        if (bookDetails is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(bookDetails);
    }
}