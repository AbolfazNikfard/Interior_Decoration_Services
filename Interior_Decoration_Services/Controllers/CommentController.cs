using Interior_Decoration_Services.Data;
using Interior_Decoration_Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interior_Decoration_Services.Controllers
{
    [Authorize(Roles ="Buyer")]
    public class CommentController : Controller
    {
        private ProjectContext _context;
        public CommentController(ProjectContext context)
        {
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult addComment(int productId,string userName,string comment)
        {
            try
            {
                Comment newComment = new Comment
                {
                    productId = productId,
                    userName = userName,
                    comment = comment,
                };
                _context.comments.Add(newComment);
                _context.SaveChanges();
                return RedirectToAction("ProductDetails", "Product", new { productId = productId });
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
    }
}
