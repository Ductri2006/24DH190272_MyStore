using _24DH190272_MyStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _24DH190272_MyStore; // <-- THÊM DÒNG NÀY


namespace _24DH190272_MyStore.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        public ActionResult TrangChu(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products = db.Products.AsQueryable();

            // Nếu người dùng nhập từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                products = products.Where(p => p.ProductName.Contains(searchTerm) ||
                                               p.ProductDescription.Contains(searchTerm) ||
                                               p.Category.CategoryName.Contains(searchTerm));
            }

            // --- Phân trang ---
            int pageNumber = page ?? 1;     // Trang hiện tại (mặc định là 1)
            int pageSize = 6;               // Số sản phẩm mỗi trang

            // --- Lấy danh sách sản phẩm nổi bật ---
            model.FeaturedProducts = products
                .OrderByDescending(p => p.OrderDetails.Count())
                .Take(10)
                .ToList();          

            // --- Lấy danh sách sản phẩm mới (có phân trang) ---
            model.NewProducts = products
               .OrderByDescending(p => p.ProductID) // Hoặc p.CreateDate nếu bạn có cột đó
               .ToPagedList(pageNumber, pageSize);

            return View(model);
        }


        public ActionResult ProductDetails(int? id, int? quantity, int? page) // Thêm 2 tham số
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product pro = db.Products.Find(id);
            if (pro == null)
            {
                return HttpNotFound();
            }

            // 2. Khởi tạo ViewModel
            ProductDetailsVM model = new ProductDetailsVM { Product = pro };

            // 3. Lấy Sản phẩm Tương Tự (Không phân trang)
            model.RelatedProducts = db.Products
                .Where(p => p.CategoryID == pro.CategoryID && p.ProductID != pro.ProductID)
                .Take(8)
                .ToList();

            // 4. Lấy Sản phẩm Bán Chạy (CÓ PHÂN TRANG)
            // Cần thêm: using PagedList;
            int pageNumber = page ?? 1;
            int pageSize = 3; // Hoặc 6, tùy bạn

            model.TopProducts = db.Products
                .OrderByDescending(p => p.ProductID) // Thay bằng logic bán chạy
                .ToPagedList(pageNumber, pageSize); // Chuyển thành PagedList

            // 5. Cập nhật số lượng nếu người dùng thay đổi
            if (quantity.HasValue)
            {
                model.Quantity = quantity.Value;
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult _PVFeatureProduct()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult _PVNewProduct()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}