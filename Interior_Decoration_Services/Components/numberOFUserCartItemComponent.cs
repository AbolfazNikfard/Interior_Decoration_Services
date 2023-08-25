using Interior_Decoration_Services.Data;
using Interior_Decoration_Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Interior_Decoration_Services.Components
{
    public class numberOFUserCartItemComponent : ViewComponent
    {
        private ProjectContext _context;
        private UserManager<User> _userManager;
        public numberOFUserCartItemComponent(ProjectContext context, UserManager<User> userManage)
        {
            _context = context;
            _userManager = userManage;
        }
        public async Task<IViewComponentResult> InvokeAsync(string ForWhere)
        {
            string viewAdress = "~/Views/Component/numberOfUserCartItem.cshtml";
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                ViewData["NumberOfCartItem"] = 0;
                ViewData["ForWhere"] = ForWhere;
                return View(viewAdress);
            }
            var buyer = _context.buyers.SingleOrDefault(b => b.userId == user.Id);
            if (buyer == null)
            {
                ViewData["NumberOfCartItem"] = 0;
                ViewData["ForWhere"] = ForWhere;
                return View(viewAdress);
            }
            int userCart = _context.carts.Where(c => c.buyerId == buyer.id).Count();
            ViewData["NumberOfCartItem"] = userCart;
            ViewData["ForWhere"] = ForWhere;
            return View(viewAdress);
        }
    }
}
