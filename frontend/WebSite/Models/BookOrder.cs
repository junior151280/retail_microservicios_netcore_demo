using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class BookOrder
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Customer name is required")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; } = string.Empty;
        
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        
        [Display(Name = "Total Amount")]
        public decimal TotalAmount => Items.Sum(item => item.Price * item.Quantity);
    }

    public class OrderItem
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Book ID")]
        public int BookId { get; set; }
        
        [Required]
        [Display(Name = "Book Title")]
        public string BookTitle { get; set; } = string.Empty;
        
        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1;
        
        [Required]
        [Range(0.01, 1000.00, ErrorMessage = "Price must be between 0.01 and 1000.00")]
        public decimal Price { get; set; }
    }
}
