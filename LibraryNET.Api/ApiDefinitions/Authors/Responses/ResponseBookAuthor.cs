using System;
using System.Linq.Expressions;
using LibraryNET.Data.Entities;

namespace LibraryNET.Api.ApiDefinitions.Authors.Responses;

internal sealed class ResponseBookAuthor
{
    public long Id { get; init; }
    public required string Name { get; init; }

    public static Expression<Func<BookAuthor, ResponseBookAuthor>> Map()
    {
        return author => new ResponseBookAuthor
        {
            Id = author.Id,
            Name = author.Name
        };
    }
}