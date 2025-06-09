using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;
using WebBanHang.Areas.Customer.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)

        {
            _db = db;
        }
        public IActionResult Index(int catid = 1)

        {
            var dsSanPham = _db.Products.Include(x => x.Category).Where(x => x.CategoryId == catid).ToList();
            return View(dsSanPham);
        }
        public IActionResult GetCategory()
        {
            var dsTheLoai = _db.Categories
      .Select(c => new CategoryWithCountVM
      {
          Id = c.Id,
          Name = c.Name,
          ProductCount = _db.Products.Count(p => p.CategoryId == c.Id)
      })
      .ToList();

            return PartialView("CategoryPartial", dsTheLoai);
        }


    }
}