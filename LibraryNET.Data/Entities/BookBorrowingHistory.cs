using System;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Data.Entities;

/// <summary>
/// Information about historically borrowed book
/// </summary>
public sealed class BookBorrowingHistory
{
    public long Id { get; set; }
    public long BookId { get; set; }
    public long UserId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime ReturnedAt { get; set; }

    public Book? Book { get; set; }
    public User? User { get; set; }

    public static void Register(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookBorrowingHistory>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<BookBorrowingHistory>()
            .HasOne(e => e.Book)
            .WithMany(e => e.BookBorrowingHistory);

        modelBuilder.Entity<BookBorrowingHistory>()
            .HasOne(e => e.User)
            .WithMany(e => e.BookBorrowingHistory);
    }
}