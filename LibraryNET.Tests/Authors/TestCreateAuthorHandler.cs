using System.Threading;
using System.Threading.Tasks;
using LibraryNET.Api.ApiDefinitions.Authors.Handlers;
using LibraryNET.Api.ApiDefinitions.Authors.Requests;
using LibraryNET.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace LibraryNET.Tests.Authors;

public sealed class TestCreateAuthorHandler
{
    [Fact]
    public async Task TestCreateAuthor_Success()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseSqlite(connection)
            .Options;
        
        await using var context = new LibraryContext(options);
        await context.Database.EnsureCreatedAsync();
        
        var handler = new CreateAuthorHandler(context);
        
        // Act
        await handler.Handle(new RequestCreateAuthor
        {
            Name = "Test"
        }, CancellationToken.None);

        // Assert
        await using var assertContext = new LibraryContext(options);
        var bookAuthor = await assertContext.BookAuthors.FirstOrDefaultAsync();
        bookAuthor.ShouldNotBeNull();
        bookAuthor.Name.ShouldNotBeNull();
        bookAuthor.Name.ShouldBe("Test");
    }
    
    [Fact]
    public async Task TestCreateAuthor_Throws_ShouldNotBeNull()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseSqlite(connection)
            .Options;
        
        await using var context = new LibraryContext(options);
        await context.Database.EnsureCreatedAsync();
        
        var handler = new CreateAuthorHandler(context);
        
        // Act / Assert
        await handler.Handle(new RequestCreateAuthor
        {
            Name = null
        }, CancellationToken.None).ShouldThrowAsync<DbUpdateException>();
    }
    
    [Fact(Skip = "SQLite won't throw on this, other DBs would")]
    public async Task TestCreateAuthor_Throws_TooLong()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseSqlite(connection)
            .Options;
        
        await using var context = new LibraryContext(options);
        await context.Database.EnsureCreatedAsync();
        
        var handler = new CreateAuthorHandler(context);
        
        // Act / Assert
        await handler.Handle(new RequestCreateAuthor
        {
            Name = "This name is way too long to be saved into db............................................................."
        }, CancellationToken.None).ShouldThrowAsync<DbUpdateException>();
    }
}