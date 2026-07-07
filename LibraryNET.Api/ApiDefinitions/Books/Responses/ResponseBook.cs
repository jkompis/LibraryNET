using System;
using System.Linq.Expressions;
using LibraryNET.Data.Entities;

namespace LibraryNET.Api.ApiDefinitions.Books.Responses;

internal sealed class ResponseBook
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Author { get; set; }
    public bool Available { get; set; }

    public static Expression<Func<Book, ResponseBook>> Map()
    {
        return book => new ResponseBook
        {
            Id = book.Id,
            Name = book.BookDetails!.Name,
            Description = book.BookDetails!.Description,
            Author = book.BookDetails!.Author!.Name,
            Available = book.BookBorrowing == null
        };
    }
}