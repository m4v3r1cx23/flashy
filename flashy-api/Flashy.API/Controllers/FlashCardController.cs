using Flashy.API.Domain.Models.FlashCards;
using Flashy.API.Infrastructure.Data;
using Flashy.API.Models.FlashCard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

[Authorize]
[Route("[controller]")]
public class FlashCardController(FlashyAPIDBContext context, UserManager<User> userManager) : Controller
{
  private readonly FlashyAPIDBContext _context = context;
  private readonly UserManager<User> _userManager = userManager;

  [HttpGet]
  public async Task<IActionResult> GetAllFlashCards([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
  {
    var flashcards = await _context.FlashCards
      .Select(fc => new
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
      })
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();

    return Ok(flashcards);
  }

  [HttpPost]
  public async Task<IActionResult> CreateFlashCard([FromBody] FlashCardModel model)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Unauthorized();

    var hash = ComputeSha256Hash(model.Question);
    if (await _context.FlashCards.AnyAsync(fc => fc.Hash == hash))
    {
      return BadRequest("FlashCard with the same question already exists.");
    }

    var flashCard = new FlashCard
    {
      Question = model.Question,
      Answer = model.Answer,
      Hint = model.Hint,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow,
      CreatedBy = user,
      Hash = hash,
    };

    _context.FlashCards.Add(flashCard);
    await _context.SaveChangesAsync();

    return Ok(new
    {
      flashCard.Id,
      flashCard.Question,
      flashCard.Answer,
      flashCard.Hint,
      flashCard.CreatedAt,
      flashCard.UpdatedAt,
      CreatedBy = new
      {
        flashCard.CreatedBy.Id,
        flashCard.CreatedBy.FullName,
        flashCard.CreatedBy.Email,
        flashCard.CreatedBy.UserName,
      },
    });
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateFlashCard(Guid id, [FromBody] FlashCardModel model)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Unauthorized();

    var flashCard = await _context.FlashCards.FindAsync(id);
    if (flashCard == null) return NotFound();

    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

    if (flashCard.CreatedBy?.Id != user.Id && !isAdmin)
    {
      return Forbid();
    }

    flashCard.Question = model.Question;
    flashCard.Answer = model.Answer;
    flashCard.Hint = model.Hint;
    flashCard.UpdatedAt = DateTime.UtcNow;
    flashCard.Hash = ComputeSha256Hash(model.Question);

    _context.FlashCards.Update(flashCard);
    await _context.SaveChangesAsync();

    return Ok(new
    {
      flashCard.Id,
      flashCard.Question,
      flashCard.Answer,
      flashCard.Hint,
      flashCard.CreatedAt,
      flashCard.UpdatedAt,
      CreatedBy = new
      {
        flashCard.CreatedBy.Id,
        flashCard.CreatedBy.FullName,
        flashCard.CreatedBy.Email,
        flashCard.CreatedBy.UserName,
      },
    });
  }

  [HttpDelete("{id}")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> DeleteFlashCard(Guid id)
  {
    var flashCard = await _context.FlashCards.FindAsync(id);
    if (flashCard == null) return NotFound();

    _context.FlashCards.Remove(flashCard);
    await _context.SaveChangesAsync();

    return Ok();
  }

  private static string ComputeSha256Hash(string rawData)
  {
    byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));
    StringBuilder builder = new();

    for (int i = 0; i < bytes.Length; i++)
    {
      builder.Append(bytes[i].ToString("x2"));
    }

    return builder.ToString();
  }
}
