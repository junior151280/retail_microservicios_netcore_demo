using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Website.Models;
using Website.Services;

namespace Website.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(OrderService orderService, ILogger<CreateModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [BindProperty]
        public BookOrder Order { get; set; } = new BookOrder();

        [BindProperty]
        public OrderItem NewItem { get; set; } = new OrderItem();

        public void OnGet()
        {
            // Initialize with empty order
            Order = new BookOrder
            {
                Items = new List<OrderItem>()
            };
        }

        public IActionResult OnPostAddItem()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Add the new item to the order
            Order.Items.Add(new OrderItem
            {
                BookId = NewItem.BookId,
                BookTitle = NewItem.BookTitle,
                Price = NewItem.Price,
                Quantity = NewItem.Quantity
            });

            // Reset the new item
            NewItem = new OrderItem();
            
            // Keep the ModelState clean for the main form
            ModelState.Clear();
            
            return Page();
        }

        public IActionResult OnPostRemoveItem(int index)
        {
            if (index >= 0 && index < Order.Items.Count)
            {
                Order.Items.RemoveAt(index);
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!Order.Items.Any())
            {
                ModelState.AddModelError("Order.Items", "At least one item is required");
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var createdOrder = await _orderService.CreateOrderAsync(Order);
                return RedirectToPage("Details", new { id = createdOrder.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new order");
                ModelState.AddModelError(string.Empty, "Error creating order. Please try again.");
                return Page();
            }
        }
    }
}
