using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class CheckoutController : ControllerBase
{
    private readonly LibraryContext _context;

    public CheckoutController(LibraryContext context)
    {
        _context = context;
    }

    // Get checked-out books for the logged-in user
    [HttpGet("my-checkouts")]
    [Authorize]
    public async Task<IActionResult> GetUserCheckouts()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var roleClaim = User.FindFirstValue(ClaimTypes.Role);

        if (userIdClaim == null)
            return Unauthorized("User not authenticated");

        int userId = int.Parse(userIdClaim);
        bool isLibrarian = roleClaim == "Librarian";

        var checkoutsQuery = _context.Checkouts
            .Include(c => c.Book)
            .Include(c => c.User)
            .AsQueryable();

        if (!isLibrarian)
        {
            // Regular users can only see their own checkouts
            checkoutsQuery = checkoutsQuery.Where(c => c.UserId == userId);
        }

        var checkouts = await checkoutsQuery
            .Where(c => c.ReturnDate == null) // Only show active checkouts
            .Select(c => new
            {
                c.Book.Id,
                c.Book.Title,
                BorrowedBy = c.User.Username,
                c.CheckoutDate,
                DueDate = c.CheckoutDate.AddDays(5)
            })
            .ToListAsync();

        return Ok(checkouts);
    }

    // ✅ Allow only librarians to see all checked-out books
    [HttpGet("all-checkouts")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> GetAllCheckouts()
    {
        var checkouts = await _context.Checkouts
            .Include(c => c.Book)
            .Include(c => c.User)
            .Where(c => c.ReturnDate == null) // Only active checkouts
            .Select(c => new
            {
                c.Book.Id,
                c.Book.Title,
                c.User.Username,
                c.CheckoutDate,
                DueDate = c.CheckoutDate.AddDays(5)
            })
            .ToListAsync();

        return Ok(checkouts);
    }

    [HttpPost("checkout/{bookId}")]
    [Authorize]
    public async Task<IActionResult> CheckoutBook(int bookId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized("User not authenticated");

        int userId = int.Parse(userIdClaim);

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Check if the book exists
        var book = await _context.Books.FindAsync(bookId);
        if (book == null)
            return NotFound("Book not found");

        // Check if the book is already checked out
        var existingCheckout = await _context.Checkouts
            .FirstOrDefaultAsync(c => c.BookId == bookId && c.ReturnDate == null);

        if (existingCheckout != null)
            return BadRequest("This book is already checked out");

        // Create a new checkout record
        var checkout = new Checkout
        {
            BookId = bookId,
            UserId = userId,
            CheckoutDate = DateTime.UtcNow,
            Book = book,
            User = user,
            DueDate = DateTime.UtcNow.AddDays(5) // Assume 5-day checkout period
        };

        _context.Checkouts.Add(checkout);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Book checked out successfully", checkout });
    }
}