using Bogus;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Library.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(LibraryContext context)
        {
            if (await context.Books.AnyAsync()) return;

            var passwordHasher = new PasswordHasher<User>();

            var faker = new Faker();

            var predefinedUsers = new List<User>
            {
                new User { Username = "admin", Email = "admin@example.com", PasswordHash = passwordHasher.HashPassword(null, "hashedpassword123"), Role = UserRole.Librarian },
                new User { Username = "customer", Email = "customer@example.com", PasswordHash = passwordHasher.HashPassword(null, "hashedpassword123"), Role = UserRole.Customer }
            };

            var users = new Faker<User>()
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor( u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
                .RuleFor(u => u.Role, (f, u) => f.PickRandom<UserRole>())
                .Generate(10);

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();


            var books = new Faker<Book>()
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
                .RuleFor(b => b.Author, f => f.Name.FullName())
                .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
                .RuleFor(b => b.CoverImage, f => f.Image.PicsumUrl())
                .RuleFor(b => b.Publisher, f => f.Name.FullName())
                .RuleFor(b => b.Category, f => f.Name.FullName())
                .RuleFor(b => b.ISBN, f => f.Random.Long(1000000000000, 9999999999999).ToString())
                .Generate(20);

            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();

            var reviews = new Faker<Review>()
                .RuleFor(r => r.BookId, (f, r) => f.PickRandom(books).Id!)
                .RuleFor(r => r.UserId, (f, r) => f.PickRandom(users).Id!)
                .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
                .RuleFor(r => r.Comment, f => f.Lorem.Sentence())
                .Generate(50); 

            await context.Reviews.AddRangeAsync(reviews);
            await context.SaveChangesAsync();

            Console.WriteLine("✅ Database seeded successfully!");
        }
    }
}