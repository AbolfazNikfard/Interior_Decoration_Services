﻿using Interior_Decoration_Services.Data;
using Interior_Decoration_Services.Models;
using Interior_Decoration_Services.Models.View_Models;
using Interior_Decoration_Services.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Interior_Decoration_Services.Controllers
{
    public class HomeController : Controller
    {
        private ProjectContext _context;
        public HomeController(ProjectContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, int limit = 9, string sort = null, string search = null, string filter = null)
        {
            if (page < 1)
                return BadRequest(new { StatusCode = 400, message = "page number should be greater than 0" });
            if (limit < 1)
                return BadRequest(new { StatusCode = 400, message = "limit should be greater than 0" });
            int skip = (page - 1) * limit;
            double productCount, result;


            IQueryable<Product> products;
            if (search != null && filter == "available")
            {
                products = _context.products.Where(p => p.Name.Contains(search) && p.Stock == true);
                productCount = (double)_context.products.Where(p => p.Name.Contains(search) && p.Stock == true).Count();
            }
            else if (search != null && filter == null)
            {
                products = _context.products.Where(p => p.Name.Contains(search));
                productCount = (double)_context.products.Where(p => p.Name.Contains(search)).Count();
            }
            else if (search == null && filter != null)
            {
                products = _context.products.Where(p => p.Stock == true);
                productCount = (double)_context.products.Where(p => p.Stock == true).Count();
            }
            else
            {
                products = _context.products;
                productCount = (double)_context.products.Count();
            }

            ViewData["page"] = page;
            result = productCount / (double)limit;
            int pageCount = (int)Math.Ceiling(result);
            ViewData["pagesCount"] = pageCount;

            List<Product> productViewModel;
            if (sort != null)
                productViewModel = Sort.sorted_Products(products, sort, skip, limit);
            else
                productViewModel = products.Skip(skip).Take(limit).ToList();

            if (productViewModel == null) { return NotFound(); }
            return View(productViewModel);
        }
        [HttpGet]
        public IActionResult WorkSampleList(int groupId = 0, int page = 1)
        {
            if (page < 1)
                return BadRequest(new { StatusCode = 400, message = "page number should be greater than 0" });

            int limit = 4;
            int skip = (page - 1) * limit;
            double productCount, result;

            IQueryable<WorkSamples> workSampleList;

            if (groupId == 0)
            {
                workSampleList = _context.workSamples;
                productCount = workSampleList.Count();
            }
            else
            {
                workSampleList = _context.workSamples.Where(ws => ws.groupId == groupId);
                productCount = workSampleList.Count();
            }

            ViewData["page"] = page;
            result = productCount / (double)limit;
            int pageCount = (int)Math.Ceiling(result);
            ViewData["pagesCount"] = pageCount;

            List<Group> groups = _context.groups.ToList();
            WorkSampleListViewModel workSampleListViewModel = new WorkSampleListViewModel()
            {
                groups = groups,
                Samples = workSampleList.Skip(skip).Take(limit).ToList()
            };
            return View(workSampleListViewModel);
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Team()
        {
            return View();
        }
        public IActionResult OurService()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}