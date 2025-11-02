using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _24DH190272_MyStore.Models
{
  
    //metada: lớp trừu tượng hóa
    public class UserMetadata
    {
        [Required(ErrorMessage = "Username is required!")]
        [StringLength(30, MinimumLength = 5)]
        [RegularExpression(@"\r\n ^[a-zA-Z0-9][[._-](?![._-])|[a-zA-Z0-9]]{3,18}[a-zA-Z0-9]$\r\n")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }

    public class CategoryMetadata
    {
        [HiddenInput] // ẩn trường dữ liệu (người dùng không nhập dữ liệu chp trường này
        public int CategoryID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string CategoryName { get; set; }
    }

    public class CustomerMetadata 
    {
        [HiddenInput(DisplayValue = false)]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Tên khách hàng không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên khách hàng tối đa 100 ký tự.")]
        [Display(Name = "Tên khách hàng")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [StringLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự.")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ!")]
        [Display(Name = "Số điện thoại")]
        public string CustomerPhone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        [Display(Name = "Địa chỉ Email")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "Username là bắt buộc!")]
        [Display(Name = "Tài khoản liên kết")]
        public string Username { get; set; }
    }

    public class ProductMetadata 
    {
        [Required(ErrorMessage = "Mã sản phẩm không được để trống!")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm bắt buộc!")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm bắt buộc!")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm tối đa 200 ký tự.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm bắt buộc!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0!")]
        [DataType(DataType.Currency)]
        public decimal ProductPrice { get; set; }

        [StringLength(200, ErrorMessage = "Tên file ảnh tối đa 200 ký tự.")]
        public string ProductImage { get; set; }

        [Required(ErrorMessage = "Mô tả sản phẩm bắt buộc!")]
        [StringLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự.")]
        public string ProductDescription { get; set; }
    }

    public class OrderMetadata 
    {
        [HiddenInput(DisplayValue = false)]
        public int OrderID { get; set; } // khóa chính , không hiển thị khi nhập form


        [Required(ErrorMessage = "Mã khách hàng là bắt buộc!")]
        [Display(Name = "Khách hàng")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Ngày đặt hàng là bắt buộc!")]
        [DataType(DataType.Date)]
        [Display(Name = "Ngày đặt hàng")]
        public System.DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Tổng tiền là bắt buộc!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn 0!")]
        [DataType(DataType.Currency)]
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [StringLength(100, ErrorMessage = "Trạng thái thanh toán tối đa 100 ký tự.")]
        [Display(Name = "Trạng thái thanh toán")]
        public string PaymentStatus { get; set; }

        [Required(ErrorMessage = "Địa chỉ giao hàng là bắt buộc!")]
        [StringLength(500, ErrorMessage = "Địa chỉ giao hàng tối đa 500 ký tự.")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string AddressDelivery { get; set; }

    }

    public class OrderDetailMetadata 
    {
        [Display(Name = "Mã chi tiết đơn hàng")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Mã sản phẩm")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn đơn hàng")]
        [Display(Name = "Mã đơn hàng")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá không hợp lệ")]
        [Display(Name = "Đơn giá")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }


}