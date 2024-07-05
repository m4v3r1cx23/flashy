using Flashy.API.Domain.Models.FlashCards;
using Flashy.API.Infrastructure.Data;
using Flashy.API.Models.Trial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Flashy.API.Controllers;

[Authorize]
[Route("[controller]")]
public class TrialController(FlashyAPIDBContext context, UserManager<User> userManager) : Controller
{
  private readonly FlashyAPIDBContext _context = context;
  private readonly UserManager<User> _userManager = userManager;

  [HttpGet("user-trials")]
  public async Task<IActionResult> GetUserTrials([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Unauthorized();

    var trials = await _context.Trials
      .Select(t => new
      {
        t.Id,
        t.StartedAt,
        t.CompletedAt,
        t.LastFlashCardIndex,
        User = new
        {
          t.User.Id,
          t.User.FullName,
          t.User.Email,
          t.User.UserName
        },
        Answers = t.Answers.Select(a => new
        {
          a.Id,
          FlashCardId = a.FlashCard.Id,
          a.IsCorrect
        }),
        FlashCards = t.FlashCards.Select(fc => new
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
      .Where(t => t.User.Id == user.Id)
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();

    return Ok(trials);
  }

  [HttpGet("all-trials")]
  [Authorize(Roles = "Admin")]
  public async Task<IActionResult> GetAllTrials([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
  {
    var trials = await _context.Trials
      .Select(t => new
      {
        t.Id,
        t.StartedAt,
        t.CompletedAt,
        t.LastFlashCardIndex,
        User = new
        {
          t.User.Id,
          t.User.FullName,
          t.User.Email,
          t.User.UserName
        },
        Answers = t.Answers.Select(a => new
        {
          a.Id,
          FlashCardId = a.FlashCard.Id,
          a.IsCorrect
        }),
        FlashCards = t.FlashCards.Select(fc => new
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

    return Ok(trials);
  }

  [HttpPost]
  public async Task<IActionResult> StartTrial([FromBody] Guid deckId)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Unauthorized();

    var deck = await _context.Decks.Include(d => d.FlashCards).FirstOrDefaultAsync(d => d.Id == deckId);
    if (deck == null) return NotFound("Deck not found.");

    var trial = new Trial
    {
      Id = Guid.NewGuid(),
      User = user,
      FlashCards = deck.FlashCards,
      Answers = [],
      LastFlashCardIndex = 0,
      StartedAt = DateTime.UtcNow
    };

    _context.Trials.Add(trial);
    await _context.SaveChangesAsync();

    return Ok(new
    {
      trial.Id,
      trial.StartedAt,
      trial.CompletedAt,
      trial.LastFlashCardIndex,
      User = new
      {
        trial.User.Id,
        trial.User.FullName,
        trial.User.Email,
        trial.User.UserName
      },
      Answers = trial.Answers.Select(a => new
      {
        a.Id,
        FlashCardId = a.FlashCard.Id,
        a.IsCorrect
      }),
      FlashCards = trial.FlashCards.Select(fc => new
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
  public async Task<IActionResult> UpdateTrial(Guid id, [FromBody] TrialModel model)
  {
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Unauthorized();

    var trial = await _context.Trials.Include(t => t.FlashCards).Include(t => t.Answers).FirstOrDefaultAsync(t => t.Id == id);
    if (trial == null) return NotFound();

    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
    if (trial.User.Id != user.Id && !isAdmin)
    {
      return Forbid();
    }

    trial.LastFlashCardIndex = model.LastFlashCardIndex;
    trial.CompletedAt = model.CompletedAt;

    foreach (var answer in model.Answers)
    {
      var existingAnswer = trial.Answers.FirstOrDefault(a => a.FlashCard.Id == answer.Id);
      if (existingAnswer != null)
      {
        existingAnswer.IsCorrect = answer.IsCorrect;
      }
      else
      {
        trial.Answers.Add(new Answer
        {
          FlashCard = trial.FlashCards.First(fc => fc.Id == answer.Id),
          IsCorrect = answer.IsCorrect,
          Trial = trial
        });
      }
    }

    _context.Trials.Update(trial);
    await _context.SaveChangesAsync();

    return Ok(new
    {
      trial.Id,
      trial.StartedAt,
      trial.CompletedAt,
      trial.LastFlashCardIndex,
      User = new
      {
        trial.User.Id,
        trial.User.FullName,
        trial.User.Email,
        trial.User.UserName
      },
      Answers = trial.Answers.Select(a => new
      {
        a.Id,
        FlashCardId = a.FlashCard.Id,
        a.IsCorrect
      }),
      FlashCards = trial.FlashCards.Select(fc => new
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
  public async Task<IActionResult> DeleteTrial(Guid id)
  {
    var trial = await _context.Trials.FindAsync(id);
    if (trial == null) return NotFound();

    _context.Trials.Remove(trial);
    await _context.SaveChangesAsync();

    return Ok();
  }
}
