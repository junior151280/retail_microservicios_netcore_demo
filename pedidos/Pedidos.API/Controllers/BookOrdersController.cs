using Microsoft.AspNetCore.Mvc;
using Pedidos.API.Models;
using Pedidos.API.Repositories;

namespace Pedidos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookOrdersController : ControllerBase
    {
        private readonly IBookOrderRepository _repository;
        private readonly ILogger<BookOrdersController> _logger;

        public BookOrdersController(IBookOrderRepository repository, ILogger<BookOrdersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookOrder>>> GetAllOrders()
        {
            _logger.LogInformation("Getting all book orders");
            var orders = await _repository.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookOrder>> GetOrderById(int id)
        {
            _logger.LogInformation("Getting book order by id: {OrderId}", id);
            var order = await _repository.GetOrderByIdAsync(id);
            
            if (order == null)
                return NotFound();
                
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<BookOrder>> CreateOrder(BookOrder order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            _logger.LogInformation("Creating new book order for customer: {CustomerName}", order.CustomerName);
            
            var createdOrder = await _repository.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }
    }
}
