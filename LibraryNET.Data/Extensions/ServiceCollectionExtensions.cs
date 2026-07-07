using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryNET.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSqliteDatabase(this IServiceCollection serviceCollection,
        IConfiguration configuration, int timeout = 5, bool isProduction = true)
    {
        return serviceCollection.AddDbContext<LibraryContext>(contextOptionsBuilder =>
        {
            contextOptionsBuilder.UseSqlite(configuration.GetConnectionString("LibraryContext"),
                sqliteOptionsBuilder => { sqliteOptionsBuilder.CommandTimeout(timeout); });

            if (!isProduction)
            {
                contextOptionsBuilder.EnableDetailedErrors().EnableSensitiveDataLogging();
            }
        });
    }
}