using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Website.Models;
using Website.Services;

namespace Website.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly OrderService _orderService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(OrderService orderService, ILogger<IndexModel> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public List<BookOrder> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                Orders = await _orderService.GetAllOrdersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders");
                ModelState.AddModelError(string.Empty, "Error loading orders. Please try again later.");
            }
        }
    }
}
