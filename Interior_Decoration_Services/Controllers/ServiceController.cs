using Interior_Decoration_Services.Enum;
using Interior_Decoration_Services.Models.View_Models;
using Interior_Decoration_Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Interior_Decoration_Services.Data;
using Microsoft.AspNetCore.Identity;

namespace Interior_Decoration_Services.Controllers
{
    [Authorize(Roles = "Service")]
    public class ServiceController : Controller
    {
        private ProjectContext _context;
        private readonly UserManager<User> _userManager;
        public ServiceController(ProjectContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrdersList(string WhichOrders = "reffered")
        {
            try
            {
                ViewData["OrdersType"] = WhichOrders;
                IQueryable<Order> whichOrders;
                List<OrderViewModel> orders;
                if (WhichOrders == "reffered")
                    whichOrders = _context.orders.IgnoreQueryFilters().Where(c => c.Status == OrderStatus.reffered);

                else if (WhichOrders == "doing")
                    whichOrders = _context.orders.IgnoreQueryFilters().Where(c => c.Status == OrderStatus.doing);

                else
                    whichOrders = _context.orders.IgnoreQueryFilters().Where(c => c.Status == OrderStatus.finished);

                orders = whichOrders.Include(p => p.product)
                .Include(b => b.buyer).ThenInclude(u => u.user)
                .Select(o => new OrderViewModel
                {
                    orderId = o.Id,
                    productId = o.productId,
                    productName = o.product.Name,
                    productImage = o.product.productImage,
                    productPrice = o.product.Price,
                    userName = o.buyer.user.UserName,
                    orderDate = o.createdAt
                })
                .ToList();
                return View(orders);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public async Task<IActionResult> OrderDetail(int orderId)
        {
            var order = _context.orders.SingleOrDefault(o => o.Id == orderId);
            if (order == null)
                return NotFound();

            var product = _context.products.IgnoreQueryFilters().SingleOrDefault(p => p.id == order.productId);
            if (product == null)
                return NotFound();

            var buyer = _context.buyers.IgnoreQueryFilters().SingleOrDefault(b => b.id == order.buyerId);
            if (buyer == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(buyer.userId);
            if (user == null)
                return NotFound();

            OrderDetailViewModel orderDetail = new OrderDetailViewModel
            {
                orderId = orderId,
                orderDescription = order.Description,
                orderRegisterDateTime = order.createdAt,
                userFirstName = user.Name,
                userLastName = user.Family,
                userAddress = user.Address,
                userPhone = user.PhoneNumber,
                productId = order.productId,
                productImage = product.productImage,
                productName = product.Name,
                productPrice = product.Price,
                productUnit = product.Unit,
                productSize = product.Size,
                productUnitOfMeasurement = product.UnitOFMeasurement
            };
            return View(orderDetail);
        }
        public IActionResult DeterminingOrderStatus(int orderId, int status = 0, string wichOrdersList = "reffered")
        {
            try
            {
                Order order = _context.orders.SingleOrDefault(o => o.Id == orderId);
                if (order == null)
                    return NotFound();
                order.Status = (OrderStatus)status;
                _context.orders.Update(order);
                _context.SaveChanges();
                return RedirectToAction("OrdersList", "Service", new { WhichOrders = wichOrdersList });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
    }
}
