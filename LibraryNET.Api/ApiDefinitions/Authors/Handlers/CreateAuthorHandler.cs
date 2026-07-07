using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Authors.Requests;
using LibraryNET.Data;
using LibraryNET.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace LibraryNET.Api.ApiDefinitions.Authors.Handlers;

internal sealed class CreateAuthorHandler(LibraryContext context)
{
    public async Task<IResult> Handle(RequestCreateAuthor request, CancellationToken cancellationToken)
    {
        var entityEntry = context.BookAuthors.Add(new BookAuthor
        {
            Name = request.Name!
        });

        await context.SaveChangesAsync(cancellationToken);

        return Results.Ok(entityEntry.Entity.Id);
    }
}