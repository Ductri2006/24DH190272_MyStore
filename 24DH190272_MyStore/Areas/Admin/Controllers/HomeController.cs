//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace _24DH190272_MyStore.Areas.Admin.Controllers
//{
//    public class HomeController : Controller
//    {
//        // GET: Admin/Home
//        public ActionResult Index()
//        {
//            return View();
//        }
//    }
//}

using _24DH190272_MyStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// Thêm namespace chứa Entity Framework Context của bạn
// Ví dụ: using _24DH190272_MyStore.Models; 

namespace _24DH190272_MyStore.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // Khai báo đối tượng Context (thay MyStoreContext bằng tên thật)
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            // 1. Tổng số Danh mục
            ViewBag.TotalCategories = db.Categories.Count();

            // 2. Tổng số Sản phẩm
            ViewBag.TotalProducts = db.Products.Count();

            // 3. Đơn hàng (Đếm tất cả đơn hàng)
            ViewBag.NewOrdersCount = db.Orders.Count();

            // 4. Tổng số Khách hàng
            ViewBag.TotalCustomers = db.Customers.Count();

            // Trả về View với các giá trị đã được gán vào ViewBag
            return View();
        }

        // Đảm bảo giải phóng tài nguyên khi Controller bị hủy
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}