using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models 
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)] 
        public required string Title { get; set; }

        [Required]
        [StringLength(100)] 
        public required string Author { get; set; }

        [Required]
        [StringLength(500)] 
        public required string Description { get; set; }

        public required string CoverImage { get; set; }
        
        [Required]
        public required string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }

        [Required]
        public required string Category { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ISBN must be exactly 13 characters.")]
        public required string ISBN { get; set; }
        public int PageCount { get; set; }
        
        // Navigation properties
        public ICollection<Review> Reviews { get; set; }
        
        // This will represent the active checkout (if any)
        public Checkout CurrentCheckout { get; set; }
    }
}
