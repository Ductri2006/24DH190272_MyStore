using _24DH190272_MyStore.Models;
using System.Linq;
using System.Web.Mvc;

namespace TenProjectCuaBan.Controllers
{
    public class CartController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities(); // Thay bằng tên Context của bạn

        // Hàm trợ giúp để lấy CartService (dùng Session)
        private CartService GetCartService()
        {
            return new CartService(this.Session);
        }

        // GET: Cart/Index
        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCartService().GetCart();
            return View(cart); // Trả về View Model "Cart"
        }

        // POST: Cart/AddToCart
        // Thêm sản phẩm vào giỏ (được gọi từ trang ProductDetails)
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity = 1)
        {
            // 1. Tìm sản phẩm trong DB
            var product = db.Products.Find(id); // Thay "Products" bằng tên Entity Bảng sản phẩm
            if (product == null)
            {
                return HttpNotFound();
            }

            // 2. Lấy giỏ hàng từ Session
            var cart = GetCartService().GetCart();

            // 3. Thêm sản phẩm vào giỏ hàng (logic nằm trong Cart.cs)
            cart.AddItem(product, quantity);

            // 4. Quay về trang Chi tiết sản phẩm (hoặc trang Giỏ hàng)
            // Tạm thời quay về trang chủ, bạn có thể đổi thành "ProductDetails"
            return RedirectToAction("TrangChu", "Home");
        }

        // POST: Cart/RemoveFromCart
        // Xóa một sản phẩm khỏi giỏ
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = GetCartService().GetCart();
            cart.RemoveItem(id);
            return RedirectToAction("Index"); // Tải lại trang giỏ hàng
        }

        // POST: Cart/UpdateQuantity
        // Cập nhật số lượng
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            if (quantity > 0)
            {
                var cart = GetCartService().GetCart();
                cart.UpdateQuantity(id, quantity);
            }
            return RedirectToAction("Index"); // Tải lại trang giỏ hàng
        }

        // GET: Cart/ClearCart
        // Xóa sạch giỏ hàng
        public ActionResult ClearCart()
        {
            var cart = GetCartService().GetCart();
            cart.ClearCart();
            return RedirectToAction("Index"); // Tải lại trang giỏ hàng
        }
    }
}