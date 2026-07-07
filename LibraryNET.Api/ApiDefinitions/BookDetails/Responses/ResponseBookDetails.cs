using System;
using System.Linq.Expressions;

namespace LibraryNET.Api.ApiDefinitions.BookDetails.Responses;

public sealed class ResponseBookDetails
{
    public long Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Author { get; init; }

    public static Expression<Func<Data.Entities.BookDetails, ResponseBookDetails>> Map()
    {
        return bookDetail => new ResponseBookDetails
        {
            Id = bookDetail.Id,
            Name = bookDetail.Name,
            Description = bookDetail.Description,
            Author = bookDetail.Author!.Name
        };
    }
}