using Microsoft.EntityFrameworkCore;

namespace LibraryNET.Data.Entities;

/// <summary>
/// Personal information of the user, deleted afterward to comply with GDPR
/// </summary>
public sealed class UserDetails
{
    public long UserId { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public string? PhoneNumber { get; set; }
    public required string EmailAddress { get; set; }

    public User? User { get; set; }

    internal static void Register(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDetails>()
            .HasKey(e => e.UserId);

        modelBuilder.Entity<UserDetails>()
            .HasOne(e => e.User)
            .WithOne(e => e.UserDetails);

        modelBuilder.Entity<UserDetails>()
            .Property(e => e.Name)
            .HasMaxLength(Constraints.NameMaxLength)
            .IsRequired();

        modelBuilder.Entity<UserDetails>()
            .Property(e => e.Address)
            .HasMaxLength(Constraints.AddressMaxLength)
            .IsRequired();

        modelBuilder.Entity<UserDetails>()
            .Property(e => e.PhoneNumber)
            .HasMaxLength(Constraints.PhoneNumberMaxLength);

        modelBuilder.Entity<UserDetails>()
            .Property(e => e.EmailAddress)
            .HasMaxLength(Constraints.EmailAddressMaxLength)
            .IsRequired();
    }

    public static class Constraints
    {
        public const int NameMaxLength = 50;
        public const int AddressMaxLength = 255;
        public const int PhoneNumberMaxLength = 55;
        public const int EmailAddressMaxLength = 55;
    }
}