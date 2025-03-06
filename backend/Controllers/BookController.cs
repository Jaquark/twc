using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(
            [FromQuery] string query,
            [FromQuery] string sortBy = "title",  // Default sort by title
            [FromQuery] string sortOrder = "asc")// Default ascending order
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var books = _context.Books
                    .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
                    .Select(b => new
                    {
                        b.Id,
                        b.Title,
                        b.Author,
                        b.Description,
                        b.CoverImage,
                        AverageRating = _context.Reviews
                            .Where(r => r.BookId == b.Id)
                            .DefaultIfEmpty()  // Handle empty reviews
                            .GroupBy(r => r.BookId)  // Group by BookId
                            .Select(g => g.Any() ? g.Average(r => r.Rating) : 0)  // Calculate average if not empty
                            .FirstOrDefault()
                    });

            // Sorting logic
            books = sortBy.ToLower() switch
            {
                "author" => sortOrder == "desc" ? books.OrderByDescending(b => b.Author) : books.OrderBy(b => b.Author),
                "rating" => sortOrder == "desc" ? books.OrderByDescending(b => b.AverageRating) : books.OrderBy(b => b.AverageRating),
                _ => sortOrder == "desc" ? books.OrderByDescending(b => b.Title) : books.OrderBy(b => b.Title),
            };

            var result = await books.Select(b => new
            {
                b.Id,
                b.Title,
                b.Author,
                b.Description,
                b.CoverImage,
                b.AverageRating
            }).ToListAsync();


            return Ok(result);
        }
    

        [HttpPost("add")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if ISBN already exists
            if (await _context.Books.AnyAsync(b => b.ISBN == book.ISBN))
            {
                return Conflict(new { message = "A book with this ISBN already exists." });
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Book added successfully", book });
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            var book = await _context.Books
                .Where(b => b.Id == bookId)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    b.Author,
                    b.Description,
                    b.CoverImage,
                    AverageRating = _context.Reviews
                        .Where(r => r.BookId == b.Id)
                        .DefaultIfEmpty()  // Handle empty reviews
                        .GroupBy(r => r.BookId)  // Group by BookId
                        .Select(g => g.Any() ? g.Average(r => r.Rating) : 0)  // Calculate average if not empty
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            return Ok(book);
        }

        [HttpPut("edit/{bookId}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> EditBook(int bookId, [FromBody] Book updatedBook)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            // Update book attributes
            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Description = updatedBook.Description;
            book.CoverImage = updatedBook.CoverImage;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Book updated successfully", book });
        }


        [HttpDelete("delete/{bookId}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound(new { message = "Book not found" });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Book deleted successfully" });
        }
    }
}

