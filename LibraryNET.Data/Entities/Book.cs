using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Data.Entities;

/// <summary>
/// Represents physical book
/// </summary>
public sealed class Book
{
    public long Id { get; set; }
    public long BookDetailsId { get; set; }
    public DateTime CommissionedAt { get; set; }
    public DateTime? DecommissionedAt { get; set; }
    public DecommissioningReason? DecommissionReason { get; set; }

    // possibly also purchase price etc...

    public BookDetails? BookDetails { get; set; }
    public BookBorrowing? BookBorrowing { get; set; }
    public ICollection<BookBorrowingHistory>? BookBorrowingHistory { get; set; }

    public enum DecommissioningReason : byte
    {
        None = 0,
        Lost = 1,
        Old = 2,
        Destroyed = 3
    }

    internal static void Register(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasKey(e => e.Id);
    }
}