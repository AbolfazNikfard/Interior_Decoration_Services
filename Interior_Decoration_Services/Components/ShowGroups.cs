using Interior_Decoration_Services.Data;
using Interior_Decoration_Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Interior_Decoration_Services.Components
{
    public class ShowGroups : ViewComponent
    {
        private ProjectContext _context;
        public ShowGroups(ProjectContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string whichView)
        {
            var groups = _context.groups.Include(sg => sg.subGroups).Select(g =>
            new GroupAndSubGroupViewModel()
            {
                groupId = g.id,
                groupName = g.Name,
                subGroops = g.subGroups.ToList()
            }).ToList();
            string viewAddress = "~/Views/Component/ShowGroups.cshtml";
            if (whichView == "Responsive")
                viewAddress = "~/Views/Component/ShowGroupsResponsive.cshtml";
            else if (whichView == "Sidebar")
                viewAddress = "~/Views/Component/SidebarGroupBlock.cshtml";
            else if (whichView == "Navbar")
                viewAddress = "~/Views/Component/ShowGroupsNavbar.cshtml";
            return View(viewAddress, groups);
        }
    }
}
