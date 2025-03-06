using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models 
{
    public class Checkout
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public required int BookId { get; set; }
        
        [Required]
        public required Book Book { get; set; }
        
        [Required]
        public required int UserId { get; set; }

        [Required]
        public required User User { get; set; }
        
        [Required]
        public required DateTime CheckoutDate { get; set; }

        [Required]
        public required DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }  
        public bool IsReturned => ReturnDate != null;
    }

}