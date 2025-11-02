using _24DH190272_MyStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security; // Cần cho FormsAuthentication

namespace _24DH190272_MyStore.Controllers
{
    public class AccountController : Controller
    {

        // Khởi tạo kết nối Database
        private MyStoreEntities db = new MyStoreEntities();

        // GET: Account/Register
        // Hiển thị form đăng ký
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        // Xử lý khi người dùng nhấn nút Đăng ký
        [HttpPost]
        [ValidateAntiForgeryToken] // Giúp chống tấn công CSRF
        public ActionResult Register(RegisterVM model)
        {
            // Kiểm tra xem các Data Annotation (ví dụ: [Required]) đã hợp lệ chưa
            if (ModelState.IsValid)
            {
                // 1. KIỂM TRA TÊN ĐĂNG NHẬP TỒN TẠI [cite: 299]
                // Thay "Users" bằng tên Entity Bảng User của bạn
                var existingUser = db.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    // Trả lỗi về View nếu tên đã tồn tại [cite: 300]
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã tồn tại.");
                    return View(model);
                }

              // 2. TẠO BẢN GHI CHO BẢNG USER [cite: 302]
                User newUser = new User();
                newUser.Username = model.Username;
                newUser.Password = model.Password; // Quan trọng: Cần mã hóa mật khẩu này ở dự án thật!
                newUser.UserRole = "C"; // Gán vai trò là Customer

                db.Users.Add(newUser);

                // 3. TẠO BẢN GHI CHO BẢNG CUSTOMER
                Customer newCustomer = new Customer();
                newCustomer.CustomerName = model.FullName;
                newCustomer.CustomerEmail = model.Email;
                newCustomer.Username = model.Username;

                // THÊM DÒNG NÀY VÀO ĐÂY:
                newCustomer.CustomerPhone = model.CustomerPhone; // Gán số điện thoại

                db.Customers.Add(newCustomer);

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    // Khối này sẽ bắt lỗi và hiển thị chi tiết
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    // Nối các lỗi lại
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Tạo một thông báo lỗi dễ đọc hơn
                    var exceptionMessage = string.Concat(ex.Message, " Các lỗi validation là: ", fullErrorMessage);

                    // Ném (throw) lỗi mới để bạn có thể đọc nó
                    throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }

                // 5. CHUYỂN HƯỚNG VỀ TRANG CHỦ [cite: 305] (Đã sửa thành TrangChu)
                return RedirectToAction("TrangChu", "Home");
            }

            // Nếu Model không hợp lệ, hiển thị lại form với các thông báo lỗi
            return View(model);
        }

        // GET: Account/Login
        // Hiển thị form đăng nhập
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        // Xử lý khi người dùng nhấn nút Đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model, string returnUrl) // LoginVM là model bạn đã tạo
        {
            if (ModelState.IsValid)
            {
                // 1. KIỂM TRA TÀI KHOẢN TRONG DATABASE
                // (Nhớ mã hóa mật khẩu khi kiểm tra ở dự án thật)
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.UserRole == "C");

                if (user != null) // Nếu tìm thấy user
                {
                    // 2. LƯU COOKIE XÁC THỰC (Ghi nhớ đăng nhập)
                    // Đây là yêu cầu của cô bạn 
                    FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe); // RememberMe là checkbox "Ghi nhớ"

                    // 3. LƯU TRẠNG THÁI VÀO SESSION (Cho _Layout)
                    // Yêu cầu của cô bạn 
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;

                    // 4. CHUYỂN HƯỚNG
                    // (Chuyển về trang họ đang cố truy cập, hoặc về Trang chủ)
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("TrangChu", "Home");
                    }
                }
                else // Nếu tài khoản sai
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            // Nếu Model không hợp lệ, hiển thị lại form
            return View(model);
        }

        // GET: Account/Logout
        // Xử lý khi người dùng nhấn nút Đăng xuất
        public ActionResult Logout()
        {
            // 1. XÓA COOKIE XÁC THỰC
            FormsAuthentication.SignOut();

            // 2. XÓA SẠCH SESSION
            Session.Clear();
            Session.Abandon();

            // 3. CHUYỂN VỀ TRANG CHỦ
            return RedirectToAction("TrangChu", "Home");
        }



        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
    }
}