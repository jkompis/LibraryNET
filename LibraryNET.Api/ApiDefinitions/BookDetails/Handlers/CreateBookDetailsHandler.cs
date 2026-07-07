using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.BookDetails.Requests;
using LibraryNET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Api.ApiDefinitions.BookDetails.Handlers;

internal sealed class CreateBookDetailsHandler(LibraryContext context)
{
    public async Task<IResult> Handle(RequestCreateBookDetails request, CancellationToken cancellationToken)
    {
        var author = await context.BookAuthors.FirstOrDefaultAsync(a => a.Id == request.AuthorId, cancellationToken);

        if (author is null)
        {
            return Results.BadRequest("Author not found");
        }

        var entityEntry = context.BookDetails.Add(new Data.Entities.BookDetails
        {
            Name = request.Name!,
            Description = request.Description!,
            Author = author
        });

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok(entityEntry.Entity.Id);
    }
}