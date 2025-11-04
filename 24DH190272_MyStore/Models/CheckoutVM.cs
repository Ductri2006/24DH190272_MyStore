using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _24DH190272_MyStore.Models
{
    public class CheckoutVM
    {
        // Danh sách các sản phẩm trong giỏ hàng để hiển thị
        public List<CartItem> CartItems { get; set; }

        // Thông tin thu thập từ Form
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Phương thức giao hàng")]
        public string DeliveryMethod { get; set; }

        // Thông tin tính toán
        public decimal TotalAmount { get; set; }

        // Thêm các trường này để lấy thông tin Customer
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}