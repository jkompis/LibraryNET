using System;
using System.Linq.Expressions;
using LibraryNET.Data.Entities;

namespace LibraryNET.Api.ApiDefinitions.BorrowingReminders.Responses;

public sealed class ResponsePendingBorrowingReminder
{
    public long UserId { get; set; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public long BookId { get; set; }
    public required string BookName { get; init; }

    public static Expression<Func<BookBorrowing, ResponsePendingBorrowingReminder>> Map()
    {
        return borrowing => new ResponsePendingBorrowingReminder
        {
            UserId = borrowing.UserId,
            UserName = borrowing.User!.UserDetails!.Name,
            Email = borrowing.User!.UserDetails!.EmailAddress,
            BookId = borrowing.BookId,
            BookName = borrowing.Book!.BookDetails!.Name
        };
    }
}