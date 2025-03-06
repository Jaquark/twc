using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using System.Linq;
using System.Threading.Tasks;

[Route("api/reviews")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly LibraryContext _context;

    public ReviewsController(LibraryContext context)
    {
        _context = context;
    }

    // ✅ Get reviews for a specific book
    [HttpGet("{bookId}")]
    public async Task<IActionResult> GetReviewsForBook(int bookId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.BookId == bookId)
            .Select(r => new
            {
                r.Id,
                r.UserId,
                r.User.Username,
                r.Rating,
                r.Comment
            })
            .ToListAsync();

        return Ok(reviews);
    }
}