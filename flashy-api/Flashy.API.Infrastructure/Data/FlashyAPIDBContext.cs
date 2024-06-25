using Microsoft.EntityFrameworkCore;

namespace Flashy.API.Infrastructure.Data
{
  public class FlashyAPIDBContext(DbContextOptions<FlashyAPIDBContext> options) : DbContext(options)
  {
  }
}
