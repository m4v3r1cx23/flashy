using Flashy.API.Domain.Models.FlashCards;
using Flashy.API.Infrastructure.Data;
using Flashy.API.Models.Deck;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashy.API.Controllers;

[Authorize]
[Route("[controller]")]
public class DeckController(FlashyAPIDBContext context, UserManager<User> userManager) : Controller
{
  private readonly FlashyAPIDBContext _context = context;
  private readonly UserManager<User> _userManager = userManager;

  [HttpGet]
  public async Task<IActionResult> GetAllDecks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
  {
    var decks = await _context.Decks
      .Select(d => new
      {
        d.Id,
        d.Name,
        d.Category,
        d.Description,
        d.CreatedAt,
        d.UpdatedAt,
        CreatedBy = new
        {
          d.CreatedBy.Id,
          d.CreatedBy.FullName,
          d.CreatedBy.Email,
          d.CreatedBy.UserName,
        },
        FlashCards = d.FlashCards.Select(fc => new
        {
          fc.Id,
          fc.Question,
          fc.Answer,
          fc.Hint,
          fc.CreatedAt,
          fc.UpdatedAt,
          CreatedBy = new
          {
            fc.CreatedBy.Id,
            fc.CreatedBy.FullName,
            fc.CreatedBy.Email,
            fc.CreatedBy.UserName,
          },
        }),
      })
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();

    return Ok(decks);
  }

  [HttpPost]
  public async Task<IActionResult> CreateDeck([FromBody] DeckModel model)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Unauthorized();

    if (await _context.Decks.AnyAsync(d => d.Name == model.Name && d.Category == model.Category))
    {
      return BadRequest("Deck with the same name and category already exists.");
    }

    var flashCards = await _context.FlashCards.Where(fc => model.FlashCardIds.Contains(fc.Id)).ToListAsync();

    var deck = new Deck
    {
      Name = model.Name,
      Category = model.Category,
      Description = model.Description,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow,
      CreatedBy = user,
      FlashCards = flashCards,
    };

    _context.Decks.Add(deck);
    await _context.SaveChangesAsync();

    return Ok(new
    {
      deck.Id,
      deck.Name,
      deck.Category,
      deck.Description,
      deck.CreatedAt,
      deck.UpdatedAt,
      CreatedBy = new
      {
        deck.CreatedBy.Id,
        deck.CreatedBy.FullName,
        deck.CreatedBy.Email,
        deck.CreatedBy.UserName,
      },
      FlashCards = deck.FlashCards.Select(fc => new
      {
        fc.Id,
        fc.Question,
        fc.Answer,
        fc.Hint,
        fc.CreatedAt,
        fc.UpdatedAt,
        CreatedBy = new
        {
          fc.CreatedBy.Id,
          fc.CreatedBy.FullName,
          fc.CreatedBy.Email,
          fc.CreatedBy.UserName,
        },
      }),
    });
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateDeck(Guid id, [FromBody] DeckModel model)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Unauthorized();

    var deck = await _context.Decks.Include(t => t.FlashCards).FirstAsync(d => d.Id == id);
    if (deck == null) return NotFound();

    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
    if (deck.CreatedBy?.Id != user.Id && !isAdmin)
    {
      return Forbid();
    }

    var flashCards = await _context.FlashCards.Where(fc => model.FlashCardIds.Contains(fc.Id)).ToListAsync();

    deck.Name = model.Name;
    deck.Category = model.Category;
    deck.Description = model.Description;
    deck.UpdatedAt = DateTime.UtcNow;

    deck.FlashCards.Clear();
    deck.FlashCards.AddRange(flashCards);

    _context.Decks.Update(deck);
    await _context.SaveChangesAsync();

    return Ok(new
    {
      deck.Id,
      deck.Name,
      deck.Category,
      deck.Description,
      deck.CreatedAt,
      deck.UpdatedAt,
      CreatedBy = new
      {
        deck.CreatedBy.Id,
        deck.CreatedBy.FullName,
        deck.CreatedBy.Email,
        deck.CreatedBy.UserName,
      },
      FlashCards = deck.FlashCards.Select(fc => new
      {
        fc.Id,
        fc.Question,
        fc.Answer,
        fc.Hint,
        fc.CreatedAt,
        fc.UpdatedAt,
        CreatedBy = new
        {
          fc.CreatedBy.Id,
          fc.CreatedBy.FullName,
          fc.CreatedBy.Email,
          fc.CreatedBy.UserName,
        },
      }),
    });
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> DeleteDeck(Guid id)
  {
    var deck = await _context.Decks.FindAsync(id);
    if (deck == null) return NotFound();

    _context.Decks.Remove(deck);
    await _context.SaveChangesAsync();

    return Ok();
  }
}
