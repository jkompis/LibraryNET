using LibraryNET.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LibraryNET.Migrations;

// ReSharper disable once UnusedType.Global
public sealed class MigrationContext : IDesignTimeDbContextFactory<LibraryContext>
{
    public LibraryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();

        optionsBuilder.UseSqlite(configuration.GetConnectionString("LibraryContext"),
            sqliteOptionsBuilder =>
            {
                sqliteOptionsBuilder.CommandTimeout(60);
                sqliteOptionsBuilder.MigrationsAssembly(typeof(MigrationContext).Assembly.GetName().Name);
            });

        return new LibraryContext(optionsBuilder.Options);
    }
}