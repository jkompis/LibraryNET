using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Data.Entities;

/// <summary>
/// Detailed information for ore or more copies of the same physical book
/// </summary>
public sealed class BookDetails
{
    public long Id { get; set; }
    public long AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    // possibly more information such as publisher, year of publishing etc...

    public BookAuthor? Author { get; set; }
    public ICollection<Book>? Books { get; set; }

    internal static void Register(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookDetails>()
            .HasKey(e => e.Id);

        modelBuilder.Entity<BookDetails>()
            .HasMany(e => e.Books)
            .WithOne(e => e.BookDetails)
            // prevent DB accidents
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BookDetails>()
            .Property(e => e.Name)
            .HasMaxLength(Constraints.NameMaxLength)
            .IsRequired();

        modelBuilder.Entity<BookDetails>()
            .Property(e => e.Description)
            .HasMaxLength(Constraints.DescriptionMaxLength)
            .IsRequired();
    }

    public static class Constraints
    {
        public const int NameMaxLength = 55;
        public const int DescriptionMaxLength = 255;
    }
}