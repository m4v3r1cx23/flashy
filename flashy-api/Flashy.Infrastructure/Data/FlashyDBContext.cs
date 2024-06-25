using Microsoft.EntityFrameworkCore;

namespace Flashy.Infrastructure.Data
{
  public class FlashyDBContext(DbContextOptions<FlashyDBContext> options) : DbContext(options)
  {

  }
}