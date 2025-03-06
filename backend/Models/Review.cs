using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        // Foreign keys and navigation properties
        [Required]
        public required int BookId { get; set; }

        [Required]
        public required Book Book { get; set; }

        [Required]
        public required int UserId { get; set; }

        [Required]
        public required User User { get; set; }
        
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public required int Rating { get; set; } // 1 to 5

        [Required]
        [StringLength(500)] 
        public required string Comment { get; set; }

        [Required]
        public required DateTime CreatedAt { get; set; }
    }
}