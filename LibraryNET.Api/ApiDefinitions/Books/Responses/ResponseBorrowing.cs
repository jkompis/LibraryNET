using System;
using System.Linq.Expressions;
using LibraryNET.Data.Entities;

namespace LibraryNET.Api.ApiDefinitions.Books.Responses;

internal sealed class ResponseBorrowing
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Author { get; set; }
    public DateTime BorrowDate { get; set; }

    public static Expression<Func<BookBorrowing, ResponseBorrowing>> Map()
    {
        return borrowing => new ResponseBorrowing
        {
            Name = borrowing.Book!.BookDetails!.Name,
            Description = borrowing.Book.BookDetails.Description,
            Author = borrowing.Book.BookDetails.Author!.Name,
            BorrowDate = borrowing.BorrowedAt
        };
    }
}