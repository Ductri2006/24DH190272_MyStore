using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _24DH190272_MyStore.Models;
using System.Net;
using _24DH190272_MyStore;


namespace _24DH190272_MyStore.Controllers
{
    [Authorize] // Bắt buộc người dùng phải đăng nhập mới vào được Controller này
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();

        // Hàm trợ giúp lấy CartService
        private CartService GetCartService()
        {
            return new CartService(this.Session);
        }

        // Hàm trợ giúp lấy giỏ hàng
        private Cart GetCart()
        {
            return GetCartService().GetCart();
        }

        // GET: Order/Checkout

        // Hiển thị trang thanh toán
        public ActionResult Checkout()
        {
            // 1. Kiểm tra giỏ hàng
            var cart = GetCart();
            if (cart.Items.Count() == 0)
            {
                // Nếu giỏ hàng rỗng, quay về trang chủ
                return RedirectToAction("TrangChu", "Home");
            }

            // 2. Lấy thông tin khách hàng đã đăng nhập
            var username = User.Identity.Name;
            var customer = db.Customers.FirstOrDefault(c => c.Username == username);
            if (customer == null)
            {
                // Lỗi hiếm gặp: đã đăng nhập nhưng không có thông tin customer
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Không tìm thấy thông tin khách hàng.");
            }

            // 3. Tạo CheckoutVM và gán thông tin
            var model = new CheckoutVM
            {
                CartItems = cart.Items.ToList(),
                TotalAmount = cart.TotalValue(),
                // Tự động điền thông tin khách hàng
                CustomerID = customer.CustomerID,
                CustomerName = customer.CustomerName,
                CustomerEmail = customer.CustomerEmail,
                CustomerPhone = customer.CustomerPhone,
                ShippingAddress = customer.CustomerAddress // Dùng địa chỉ mặc định 
            };

            return View(model);
        }

        // POST: Order/Checkout
        // Xử lý khi người dùng nhấn nút "Đặt Hàng"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVM model)
        {
            var cart = GetCart();

            if (!ModelState.IsValid)
            {
                // Nếu model không hợp lệ (ví dụ: thiếu địa chỉ),
                // gán lại CartItems và TotalAmount (vì nó sẽ bị mất khi POST)
                model.CartItems = cart.Items.ToList();
                model.TotalAmount = cart.TotalValue();
                return View(model);
            }

            // --- Logic lưu đơn hàng vào Database ---

            // 1. TẠO ĐƠN HÀNG (Order)
            Order newOrder = new Order
            {
                CustomerID = model.CustomerID,
                OrderDate = DateTime.Now,
                TotalAmount = model.TotalAmount,
                PaymentStatus = "Pending", // Trạng thái chờ thanh toán 
                // Lấy các trường mới từ Database (đã cập nhật)
                ShippingAddress = model.ShippingAddress,
                PaymentMethod = model.PaymentMethod,
                DeliveryMethod = model.DeliveryMethod
            };
            db.Orders.Add(newOrder);
            db.SaveChanges(); // Phải lưu để lấy OrderID

            // 2. TẠO CHI TIẾT ĐƠN HÀNG (OrderDetail)
            // Lặp qua các sản phẩm trong giỏ hàng
            foreach (var item in cart.Items)
            {
                OrderDetail detail = new OrderDetail
                {
                    OrderID = newOrder.OrderID, // Lấy ID của đơn hàng vừa tạo
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                    // Cột TotalPrice (đã thêm) sẽ được Database tự động tính toán
                };
                db.OrderDetails.Add(detail);
            }
            db.SaveChanges();

            // 3. XÓA GIỎ HÀNG KHỎI SESSION
            GetCartService().ClearCart();

            // 4. CHUYỂN HƯỚNG TỚI TRANG XÁC NHẬN
            return RedirectToAction("OrderSuccess", new { id = newOrder.OrderID });
        }


        // GET: Order/OrderSuccess
        // Hiển thị trang xác nhận đặt hàng thành công
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Orders.Include("OrderDetails.Product")
                            .FirstOrDefault(o => o.OrderID == id);

            if (order == null || order.Customer.Username != User.Identity.Name)
            {
                // Nếu không tìm thấy đơn hàng HOẶC đơn hàng không phải của user này
                return HttpNotFound();
            }

            return View(order);
        }
    }
}