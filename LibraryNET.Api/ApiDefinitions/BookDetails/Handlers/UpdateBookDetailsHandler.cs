using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.BookDetails.Requests;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.BookDetails.Handlers;

internal sealed class UpdateBookDetailsHandler(LibraryContext context)
{
    public async Task<IResult> Handle(long id, RequestUpdateBookDetails request, CancellationToken cancellationToken)
    {
        var bookDetails = await context.BookDetails
            .Include(bd => bd.Author)
            .FirstOrDefaultAsync(bd => bd.Id == id, cancellationToken);

        if (bookDetails is null)
        {
            return Results.NotFound();
        }

        if (request.AuthorId.HasValue)
        {
            var author = await context.BookAuthors.FirstOrDefaultAsync(a => a.Id == request.AuthorId, cancellationToken);

            if (author is null)
            {
                return Results.BadRequest("Author not found");
            }

            bookDetails.Author = author;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            bookDetails.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            bookDetails.Description = request.Description;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok();
    }
}