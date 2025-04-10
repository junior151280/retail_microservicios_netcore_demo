using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Website.Models;
using Website.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Website.Pages.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly IOrderService _orderService;

        public DetailsModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public BookOrder? Order { get; set; }

        public List<BookOrder>? OrderItems { get; set; } = new List<BookOrder>();

        public async Task OnGetAsync()
        {
            OrderItems = await _orderService.GetOrdersAsync();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var createdOrder = await _orderService.CreateOrderAsync(Order);
            if (createdOrder != null)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error creating order. Please try again later.");
                return Page();
            }
        }
    }
}
