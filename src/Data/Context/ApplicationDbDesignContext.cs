using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Flashy.Data.Context;

public class ApplicationDbDesignContext : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // if (args.Length != 1)
        // {
        //     throw new ArgumentException("Expected connection string as argument");
        // }

        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>();

        // Logging
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        optionsBuilder.UseSqlServer("Data Source=tcp:flashy-app-server.database.windows.net,1433;Initial Catalog=SQLAzure;User Id=flashy-app-server-admin@flashy-app-server.database.windows.net;Password=701C73CHISD4AQ31$");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}