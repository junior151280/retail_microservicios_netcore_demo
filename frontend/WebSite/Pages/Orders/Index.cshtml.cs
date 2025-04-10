using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Website.Models;
using Website.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Website.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public List<BookOrder> Orders { get; set; } = new List<BookOrder>();

        public async Task OnGetAsync()
        {
            Orders = await _orderService.GetOrdersAsync();
        }
    }
}
