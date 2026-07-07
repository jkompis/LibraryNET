using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Data.Entities;

/// <summary>
/// Book author information
/// </summary>
public sealed class BookAuthor
{
    public long Id { get; set; }
    public required string Name { get; set; }

    // more info about the author

    public ICollection<BookDetails>? BookDetails { get; set; }

    internal static void Register(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookAuthor>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<BookAuthor>()
            .Property(e => e.Name)
            .HasMaxLength(Constraints.NameMaxLength)
            .IsRequired();

        modelBuilder.Entity<BookAuthor>()
            .HasMany(e => e.BookDetails)
            .WithOne(e => e.Author)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public static class Constraints
    {
        public const int NameMaxLength = 55;
    } 
}