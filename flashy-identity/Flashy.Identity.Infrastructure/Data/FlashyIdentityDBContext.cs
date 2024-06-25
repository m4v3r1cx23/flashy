using Microsoft.EntityFrameworkCore;

namespace Flashy.Identity.Infrastructure.Data
{
  public class FlashyIdentityDBContext(DbContextOptions<FlashyIdentityDBContext> options) : DbContext(options)
  {
  }
}
