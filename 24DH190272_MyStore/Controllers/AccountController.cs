using _24DH190272_MyStore;
using _24DH190272_MyStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security; // Cần cho FormsAuthentication
using System.Data.Entity;

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
        //public ActionResult Login(LoginVM model, string returnUrl) // LoginVM là model bạn đã tạo
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // 1. KIỂM TRA TÀI KHOẢN TRONG DATABASE
        //        // (Nhớ mã hóa mật khẩu khi kiểm tra ở dự án thật)
        //        var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password && u.UserRole == "C");

        //        if (user != null) // Nếu tìm thấy user
        //        {
        //            // 2. LƯU COOKIE XÁC THỰC (Ghi nhớ đăng nhập)
        //            // Đây là yêu cầu của cô bạn 
        //            FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe); // RememberMe là checkbox "Ghi nhớ"

        //            // 3. LƯU TRẠNG THÁI VÀO SESSION (Cho _Layout)
        //            // Yêu cầu của cô bạn 
        //            Session["Username"] = user.Username;
        //            Session["UserRole"] = user.UserRole;

        //            // 4. CHUYỂN HƯỚNG
        //            // (Chuyển về trang họ đang cố truy cập, hoặc về Trang chủ)
        //            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
        //                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
        //            {
        //                return Redirect(returnUrl);
        //            }
        //            else
        //            {
        //                return RedirectToAction("TrangChu", "Home");
        //            }
        //        }
        //        else // Nếu tài khoản sai
        //        {
        //            ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
        //        }
        //    }

        //    // Nếu Model không hợp lệ, hiển thị lại form
        //    return View(model);
        //}

        public ActionResult Login(LoginVM model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // 1. KIỂM TRA TÀI KHOẢN (Đã sửa: Bỏ điều kiện UserRole == "C" để tìm cả Admin)
                // Lưu ý: Ở dự án thật nên mã hóa mật khẩu (MD5/BCrypt) trước khi so sánh
                //var user = db.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                var user = db.Users.FirstOrDefault(u => u.Username == model.Username
                                      && u.Password == model.Password
                                      && (u.UserRole == "C" || u.UserRole == "A"));

                if (user != null) // Đăng nhập thành công
                {
                    // 2. LƯU COOKIE XÁC THỰC
                    FormsAuthentication.SetAuthCookie(user.Username, model.RememberMe);

                    // 3. LƯU SESSION
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole; // Lưu quyền vào Session để check

                    // 4. PHÂN QUYỀN CHUYỂN HƯỚNG (LOGIC MỚI)

                    // TRƯỜNG HỢP 1: Nếu là Admin (Role A) -> Chuyển sang khu vực Admin (Areas)
                    if (user.UserRole == "A")
                    {
                        // Dựa vào ảnh: Areas -> Admin -> Views -> Home -> Index.cshtml
                        // Cú pháp: RedirectToAction("Action", "Controller", new { area = "TênArea" })
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                    // TRƯỜNG HỢP 2: Nếu là Khách (Role C) -> Về trang chủ
                    if (user.UserRole == "C")
                    {
                        // Có thể kiểm tra returnUrl nếu muốn người dùng quay lại trang họ đang xem dở
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            // Về trang chủ Client
                            return RedirectToAction("TrangChu", "Home");
                        }
                    }

                    // TRƯỜNG HỢP KHÁC (Nếu có role khác thì mặc định về trang chủ)
                    return RedirectToAction("TrangChu", "Home");
                }
                else // Sai tài khoản hoặc mật khẩu
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            // Nếu Model không hợp lệ hoặc đăng nhập sai, hiển thị lại form
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

        [Authorize] // bắt buộc người dùng phải có trang này mới mua hàng được
        public ActionResult ProfileInfo()
        {
            // 1. Lấy Username của người dùng đã đăng nhập từ cookie
            string username = User.Identity.Name;

            // 2. Lấy thông tin Customer từ Database
            // (Thay "Customers" và "Username" bằng tên cột/bảng của bạn)
            var customer = db.Customers.FirstOrDefault(c => c.Username == username);

            if (customer == null)
            {
                // Nếu có User (đăng nhập) nhưng không có Customer (lỗi dữ liệu)
                return HttpNotFound("Không tìm thấy thông tin khách hàng.");
            }

            // 3. Gửi Model "Customer" sang View
            return View(customer);
        }



        // GET: Account
        public ActionResult Index()
        {
            return View();
        }



        // GET: Account/EditProfile
        // Hiển thị form chỉnh sửa thông tin
        [Authorize]
        public ActionResult EditProfile()
        {
            // 1. Lấy Username của người dùng đã đăng nhập
            string username = User.Identity.Name;

            // 2. Lấy thông tin Customer hiện tại từ Database
            var customer = db.Customers.FirstOrDefault(c => c.Username == username);

            if (customer == null)
            {
                return HttpNotFound("Không tìm thấy thông tin khách hàng để chỉnh sửa.");
            }

            // 3. Trả về View với thông tin Customer hiện tại
            return View(customer);
        }

        // POST: Account/EditProfile
        // Xử lý lưu thông tin người dùng đã chỉnh sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditProfile(Customer customerModel)
        {
            // 1. Kiểm tra tính hợp lệ của Model (các quy tắc [Required], Email,...)
            if (ModelState.IsValid)
            {
                // 2. TÌM BẢN GHI HIỆN CÓ CẦN CẬP NHẬT
                // Lấy lại username từ FormsAuthentication, KHÔNG DÙNG model.Username (vì nó có thể đã bị giả mạo/thay đổi)
                string currentUsername = User.Identity.Name;

                // Dùng FirstOrDefault vì ta chỉ cần tìm 1 Customer của Username đó
                var existingCustomer = db.Customers.FirstOrDefault(c => c.Username == currentUsername);

                if (existingCustomer != null)
                {
                    // 3. CẬP NHẬT THÔNG TIN: Gán dữ liệu mới từ Form (customerModel) vào bản ghi cũ (existingCustomer)
                    existingCustomer.CustomerName = customerModel.CustomerName;
                    existingCustomer.CustomerEmail = customerModel.CustomerEmail;
                    existingCustomer.CustomerPhone = customerModel.CustomerPhone;
                    existingCustomer.CustomerAddress = customerModel.CustomerAddress;

                    // 4. Báo cho Entity Framework biết đối tượng này đã được sửa
                    // Cần using System.Data.Entity;
                    db.Entry(existingCustomer).State = EntityState.Modified;

                    // 5. LƯU THAY ĐỔI VÀO DATABASE
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi khi lưu
                        ModelState.AddModelError("", "Lỗi khi lưu thông tin: " + ex.Message);
                        return View(customerModel);
                    }

                    // 6. THÔNG BÁO THÀNH CÔNG VÀ CHUYỂN HƯỚNG
                    // Dùng TempData để gửi thông báo sang View khác
                    TempData["SuccessMessage"] = "Hồ sơ cá nhân đã được cập nhật thành công!";

                    // Chuyển hướng về trang ProfileInfo để load lại thông tin mới
                    return RedirectToAction("ProfileInfo");
                }
                else
                {
                    // Trường hợp không tìm thấy Customer (lỗi hệ thống)
                    ModelState.AddModelError("", "Lỗi hệ thống: Không tìm thấy hồ sơ của bạn.");
                }
            }

            // Nếu Model không hợp lệ hoặc có lỗi, hiển thị lại Form
            return View(customerModel);
        }
    }
}