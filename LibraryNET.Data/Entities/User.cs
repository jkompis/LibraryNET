using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Data.Entities;

/// <summary>
/// Permanent user information
/// </summary>
public sealed class User
{
    public long Id { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime? UnregisteredAt { get; set; }
    
    public UserDetails? UserDetails { get; set; }
    public ICollection<BookBorrowing>? BookBorrowings { get; set; }
    public ICollection<BookBorrowingHistory>? BookBorrowingHistory { get; set; }

    internal static void Register(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(e => e.Id);
    }
}