﻿using Interior_Decoration_Services.Data;
using Interior_Decoration_Services.Models;
using Interior_Decoration_Services.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Interior_Decoration_Services.Controllers
{
    public class ProductController : Controller
    {
        private ProjectContext _context;
        public ProductController(ProjectContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult ShowProductByGroupId(int groupId, int page = 1, int limit = 9, string sort = null, string search = null, string filter = null)
        {
            try
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
                    products = _context.products.Where(p => p.Name.Contains(search) && p.Stock == true && p.groupId == groupId);
                    productCount = (double)_context.products.Where(p => p.Name.Contains(search) && p.Stock == true && p.groupId == groupId).Count();
                }
                else if (search != null && filter == null)
                {
                    products = _context.products.Where(p => p.Name.Contains(search) && p.groupId == groupId);
                    productCount = (double)_context.products.Where(p => p.Name.Contains(search) && p.groupId == groupId).Count();
                }
                else if (search == null && filter != null)
                {
                    products = _context.products.Where(p => p.Stock == true && p.groupId == groupId);
                    productCount = (double)_context.products.Where(p => p.Stock == true && p.groupId == groupId).Count();
                }
                else
                {
                    products = _context.products.Where(p => p.groupId == groupId);
                    productCount = (double)_context.products.Count(p => p.groupId == groupId);
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

                return View(productViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public IActionResult ShowProductBySubGroupId(int subGroupId, int page = 1, int limit = 9, string sort = null, string search = null, string filter = null)
        {
            try
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
                    products = _context.products.Where(p => p.Name.Contains(search) && p.Stock == true && p.subGroupId == subGroupId);
                    productCount = (double)_context.products.Where(p => p.Name.Contains(search) && p.Stock == true && p.subGroupId == subGroupId).Count();
                }
                else if (search != null && filter == null)
                {
                    products = _context.products.Where(p => p.Name.Contains(search) && p.subGroupId == subGroupId);
                    productCount = (double)_context.products.Where(p => p.Name.Contains(search) && p.subGroupId == subGroupId).Count();
                }
                else if (search == null && filter != null)
                {
                    products = _context.products.Where(p => p.Stock == true && p.subGroupId == subGroupId);
                    productCount = (double)_context.products.Where(p => p.Stock == true && p.subGroupId == subGroupId).Count();
                }
                else
                {
                    products = _context.products.Where(p => p.subGroupId == subGroupId);
                    productCount = (double)_context.products.Count(p => p.subGroupId == subGroupId);
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

                return View(productViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
        public IActionResult ProductDetails(int productId)
        {
            try
            {
                var product = _context.products.Where(p => p.id == productId).SingleOrDefault();
                if (product == null) { return NotFound(); }
                return View(product);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Catched Error: {e.Message}");
                return StatusCode(500);
            }
        }
    }
}
