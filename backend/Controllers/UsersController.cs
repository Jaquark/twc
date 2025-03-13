using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly LibraryContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public UsersController(LibraryContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (user == null)
        {
            return BadRequest("User data is required.");
        }

        // Hash the password
        user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

        // Check if the email already exists in the database
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            return Conflict("Email is already taken.");
        }

        user.Reviews = null;
        user.Checkouts = null;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Return the created user
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }
}