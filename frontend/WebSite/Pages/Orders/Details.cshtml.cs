using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Website.Models;
using Website.Services;

namespace Website.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(OrderService orderService, ILogger<DetailsModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public BookOrder? Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Order = await _orderService.GetOrderByIdAsync(id.Value);
                
                if (Order == null)
                {
                    return NotFound();
                }
                
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order with ID {OrderId}", id);
                ModelState.AddModelError(string.Empty, "Error loading order details. Please try again later.");
                return RedirectToPage("./Index");
            }
        }
    }
}
