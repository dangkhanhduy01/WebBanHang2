using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)

        {
            _db = db;
        }
        public IActionResult Index()
        {
            var pageSize = 4;
            var dsSanPham = _db.Products.Include(x => x.Category).ToList();
            return View(dsSanPham.Skip(0).Take(pageSize).ToList());
        }
        public IActionResult LoadMore(int page = 1)
        {
            int pageSize = 4;
            var data = _db.Products.Include(x => x.Category)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            return PartialView("_ProductPartial", data);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
