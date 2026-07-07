using LibraryNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Data;

public sealed class LibraryContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<BookAuthor> BookAuthors { get; set; } = null!;
    public DbSet<BookBorrowing> BookBorrowings { get; set; } = null!;
    public DbSet<BookBorrowingHistory> BookBorrowingsHistory { get; set; } = null!;
    public DbSet<BookDetails> BookDetails { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserDetails> UserDetails { get; set; } = null!;

    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Book.Register(modelBuilder);
        BookAuthor.Register(modelBuilder);
        BookBorrowing.Register(modelBuilder);
        BookBorrowingHistory.Register(modelBuilder);
        Entities.BookDetails.Register(modelBuilder);
        User.Register(modelBuilder);
        Entities.UserDetails.Register(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }
}