using Interior_Decoration_Services.Convertor;
using Interior_Decoration_Services.Data;
using Interior_Decoration_Services.Enum;
using Interior_Decoration_Services.Models;
using Interior_Decoration_Services.Models.ApiModel;
using Interior_Decoration_Services.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace Interior_Decoration_Services.Controllers
{
    [Authorize(Roles = "Buyer")]
    public class IndentController : Controller
    {
        private ProjectContext _context;
        private readonly UserManager<User> _userManager;
        public IndentController(ProjectContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddToOrder model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) { return NotFound(); }

                var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
                if (buyer == null) { return NotFound(); }

                var product = _context.products.SingleOrDefault(p => p.id == model.productId);
                if (product == null) { return NotFound(); }

                if (product.Stock)
                {
                    var cartItem = _context.orders.Where(o => o.productId == product.id && o.buyerId == buyer.id && (o.Status == OrderStatus.Pending || o.Status == OrderStatus.reffered || o.Status == OrderStatus.doing)).SingleOrDefault();
                    #region Validation

                    #endregion
                    if (cartItem == null)
                    {
                        _context.orders.Add(new Order
                        {
                            productId = product.id,
                            Description = model.Description,
                            buyerId = buyer.id,
                            createdAt = DateTime.Now
                        });
                    }
                    else
                        return Ok(new { message = "The Product Added already" });
                    _context.SaveChanges();
                }
                else
                    return Ok(new { message = "The product is not available" });

                return Ok(new { message = "Success" });
            }
            catch (Exception e)
            {
                Console.WriteLine($"catche error: {e.Message}");
                return StatusCode(500);
            }
        }
        [HttpPost]
        public IActionResult EditOrder([FromBody] EditOrder order)
        {
            try
            {
                var Order = _context.orders.SingleOrDefault(o => o.Id == order.OrderId);
                if (Order == null)
                    return NotFound();
                Order.Description = order.Description;
                _context.orders.Update(Order);
                _context.SaveChanges();
                return RedirectToAction("UserOrders", "Indent");
            }
            catch (Exception e)
            {
                Console.WriteLine("Catched Error: ", e.Message);
                return StatusCode(500);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UserOrders()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) { return NotFound(); }

            var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
            if (buyer == null) { return NotFound(); }

            var userCart = _context.orders.Where(o => o.buyerId == buyer.id).IgnoreQueryFilters()
                .Include(p => p.product)
                .Select(o => new UserOrderViewModel
                {
                    orderId = o.Id,
                    orderDescription = o.Description,
                    productId = o.productId,
                    productName = o.product.Name,
                    productImage = o.product.productImage,
                    productPrice = o.product.Price,
                    productColor = o.product.Color,
                    productMaterial = o.product.Material,
                    registerDateTime = o.createdAt,
                    OrderStat = o.Status
                }
                ).ToList();
            return View(userCart);
        }
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null) { return NotFound(); }

                var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
                if (buyer == null) { return NotFound(); }

                var deleteItem = _context.orders.Where(o => o.Id == orderId).SingleOrDefault();
                if (deleteItem == null) { return NotFound(); }
                deleteItem.Status = OrderStatus.canceled;
                _context.orders.Update(deleteItem);
                _context.SaveChanges();
                return RedirectToAction("UserOrders", "Indent");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
    }
}
