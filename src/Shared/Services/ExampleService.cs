using Flashy.Data.Context;

using Microsoft.EntityFrameworkCore;

namespace flashy.src.Shared.Services
{
    public class ExampleService
    {
        ApplicationDbContext _context;

        public ExampleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetExample()
        {
            var flashCards = await _context.Flashcards.ToListAsync();

            var userNames = flashCards.Select(f => f.CreatedBy.UserName).Distinct().ToList();

            return userNames;
        }
    }
}