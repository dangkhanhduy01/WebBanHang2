using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Areas.Customer.Models;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<Cart>("CART") ?? new Cart();
            return View(cart);
        }

        public IActionResult AddToCart(int productId)
        {
            var product = _db.Products.FirstOrDefault(x => x.Id == productId);
            if (product != null)
            {
                var cart = HttpContext.Session.GetJson<Cart>("CART") ?? new Cart();
                cart.Add(product, 1);
                HttpContext.Session.SetJson("CART", cart);
            }
            return RedirectToAction("Index");
        }


        public IActionResult GetQuantityOfCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("CART");
            if (cart != null)
            {
                return Json(new { qty = cart.Quantity });

            }
            return Json(new { qty = 0 });
        }


        public IActionResult Update(int productId, int qty)
        {
            var cart = HttpContext.Session.GetJson<Cart>("CART");
            if (cart != null)
            {
                cart.Update(productId, qty);
                HttpContext.Session.SetJson("CART", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int productId)
        {
            var cart = HttpContext.Session.GetJson<Cart>("CART");
            if (cart != null)
            {
                cart.Remove(productId);
                HttpContext.Session.SetJson("CART", cart);
            }
            return RedirectToAction("Index");
        }
    }

}
