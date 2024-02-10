using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Flashy.Data.Context;

public class ApplicationDbDesignContext : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        if (args.Length != 1)
        {
            throw new ArgumentException("Expected connection string as argument");
        }

        DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder =
            new DbContextOptionsBuilder<ApplicationDbContext>();

        // Logging
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

        optionsBuilder.UseSqlServer(args[0]);

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}