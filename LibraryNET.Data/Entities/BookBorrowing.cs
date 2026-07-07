using System;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Data.Entities;

/// <summary>
/// Information about currently borrowed book
/// </summary>
public class BookBorrowing
{
    public long BookId { get; set; }
    public long UserId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime? RemindedAt { get; set; }

    public Book? Book { get; set; }
    public User? User { get; set; }

    internal static void Register(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookBorrowing>()
            .HasKey(e => new { e.BookId, e.UserId });

        modelBuilder.Entity<BookBorrowing>()
            .HasOne(e => e.Book)
            .WithOne(e => e.BookBorrowing);

        modelBuilder.Entity<BookBorrowing>()
            .HasOne(e => e.User)
            .WithMany(e => e.BookBorrowings);
    }
}