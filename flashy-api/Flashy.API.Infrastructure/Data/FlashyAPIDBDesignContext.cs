using Flashy.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

public class ApplicationDbDesignContext : IDesignTimeDbContextFactory<FlashyAPIDBContext>
{
  public FlashyAPIDBContext CreateDbContext(string[] args)
  {
    DbContextOptionsBuilder<FlashyAPIDBContext> optionsBuilder = new();

    // Logging
    optionsBuilder.EnableSensitiveDataLogging();
    optionsBuilder.EnableDetailedErrors();
    optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);

    optionsBuilder.UseSqlServer("Server=localhost,5150;Database=FlashyAPI;User Id=FlashyAPI;Password=Fl@shy@PI#062024!;TrustServerCertificate=True;");

    return new FlashyAPIDBContext(optionsBuilder.Options);
  }
}
