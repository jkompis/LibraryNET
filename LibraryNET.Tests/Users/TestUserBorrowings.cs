using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Users.Handlers;
using LibraryNET.Data;
using LibraryNET.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LibraryNET.Tests.Users;

public sealed class TestUserBorrowings
{
    private const long UserId = 1;

    private const long FreeBookId = 1;
    private const long BorrowedBookId = 2;
    private const long LostBookId = 3;

    private static async Task SeedDatabase(DbContextOptions<LibraryContext> options)
    {
        await using var context = new LibraryContext(options);
        await context.Database.EnsureCreatedAsync();

        context.Users.Add(new User
        {
            Id = UserId,
            UserDetails = new UserDetails
            {
                Name = "Test User",
                Address = "Test Address",
                EmailAddress = "test@email.com"
            }
        });

        context.BookDetails.Add(new BookDetails
        {
            Id = 1,
            Name = "Test Book",
            Description = "Test Description",
            Author = new BookAuthor
            {
                Id = 1,
                Name = "Test Author"
            },
            Books = new List<Book>
            {
                new Book
                {
                    Id = FreeBookId
                },
                new Book
                {
                    Id = BorrowedBookId,
                    BookBorrowing = new BookBorrowing
                    {
                        UserId = 1
                    }
                },
                new Book
                {
                    Id = LostBookId,
                    DecommissionReason = Book.DecommissioningReason.Lost,
                    DecommissionedAt = new DateTime(2020, 1, 1)
                }
            }
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task TestBorrow_Success()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseSqlite(connection)
            .Options;

        await SeedDatabase(options);

        await using var context = new LibraryContext(options);
        await context.Database.EnsureCreatedAsync();

        var handler = new BorrowBookHandler(context);

        // Act
        var result = await handler.Handle(UserId, FreeBookId, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<Ok>();
        await using var assertContext = new LibraryContext(options);
        var bookBorrowing = await assertContext.BookBorrowings.FirstOrDefaultAsync(bb => bb.UserId == UserId && bb.BookId == FreeBookId);
        bookBorrowing.ShouldNotBeNull();
    }

    [Fact]
    public async Task TestBorrow_Fail_BookBorrowed()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseSqlite(connection)
            .Options;

        await SeedDatabase(options);

        await using var context = new LibraryContext(options);
        await context.Database.EnsureCreatedAsync();

        var handler = new BorrowBookHandler(context);

        // Act
        var result = await handler.Handle(UserId, BorrowedBookId, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<UnprocessableEntity<string>>();
    }

    [Fact]
    public async Task TestBorrow_Fail_BookLost()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseSqlite(connection)
            .Options;

        await SeedDatabase(options);

        await using var context = new LibraryContext(options);
        await context.Database.EnsureCreatedAsync();

        var handler = new BorrowBookHandler(context);

        // Act
        var result = await handler.Handle(UserId, LostBookId, CancellationToken.None);

        // Assert
        result.ShouldBeOfType<UnprocessableEntity<string>>();
    }
}