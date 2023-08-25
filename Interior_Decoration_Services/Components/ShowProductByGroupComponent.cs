using Interior_Decoration_Services.Data;
using Interior_Decoration_Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace Interior_Decoration_Services.Components
{
    public class ShowProductByGroupComponent : ViewComponent
    {
        private ProjectContext _context;
        public ShowProductByGroupComponent(ProjectContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(List<Product> products)
        {
            return View("~/Views/Component/ShowProductByGroup.cshtml", products);
        }
    }
}
