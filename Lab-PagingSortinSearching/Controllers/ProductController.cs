using DAL;

using Lab_PagingSortinSearching.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab_PagingSortinSearching.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string SearchText="", int page=1, int pageSize = 4, string sortCol = "ProductId", string sortDir = "asc")
        {
            ViewData["sortCol"] = sortCol;
            ViewData["sortDir"] = sortDir;

            var query = _context.Products.AsQueryable();

            if(page < 1) 
                page = 1;


            if(!string.IsNullOrEmpty(SearchText) )
            {
                query = _context.Products.Where(p => 
                p.Name.Contains(SearchText)
                ).AsQueryable();
            }
            switch (sortCol)
            {
                case "Name": 
                    query = sortDir == "asc" ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name); break;
                case "Description": 
                    query = sortDir == "asc" ? query.OrderBy(p => p.Description) : query.OrderByDescending(p => p.Description); break;
                case "Color": 
                    query = sortDir == "asc" ? query.OrderBy(p => p.Color) : query.OrderByDescending(p => p.Color); break;
                case "UnitPrice": 
                    query = sortDir == "asc" ? query.OrderBy(p => p.UnitPrice) : query.OrderByDescending(p => p.UnitPrice); break;
                default:
                    query = sortDir == "asc" ? query.OrderBy(p => p.ProductId) : query.OrderByDescending(p => p.ProductId);

                break;
            }


            int totalRecords = query.Count();
            var data = query.Skip((page-1) * pageSize).Take(pageSize).ToList();
            Pager pager = new Pager(totalRecords,page, pageSize);
            ViewBag.Pager = pager;


            return View(data);
        }
    }
}
