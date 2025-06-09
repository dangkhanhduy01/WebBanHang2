using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebBanHang.Areas.Customer.Models;
using WebBanHang.Models;

namespace WebBanHang.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("CART") ?? new Cart();
            ViewBag.CART = cart;
            return View();
        }

        public IActionResult ProcessOrder(Order order)
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("CART");

            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                order.Total = cart.Total;
                order.State = "Pending";

                _db.Orders.Add(order);
                _db.SaveChanges();

                foreach (var item in cart.Items)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id,
                        ProductId = item.Product.Id,
                        Quantity = item.Quantity
                    };
                    _db.OrderDetails.Add(orderDetail);
                    _db.SaveChanges();
                }

                HttpContext.Session.Remove("CART");

                TempData["Success"] = "Đặt hàng thành công! Cảm ơn bạn đã mua sắm tại cửa hàng.";
                return RedirectToAction("Result");
            }

            ViewBag.CART = cart;
            return View("Index", order);
        }

        // ✅ Action cần bổ sung để tránh lỗi 404
        public IActionResult Result()
        {
            return View();
        }
    }
}
