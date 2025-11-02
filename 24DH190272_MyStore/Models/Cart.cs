using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH190272_MyStore.Models
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items => items;

        // Thêm sản phẩm vào giỏ
        // (Lưu ý: Tham số 'category' được truyền vào từ Controller 
        // nhưng không được sử dụng trong việc tạo CartItem ở bước này)
        // THÊM HÀM MỚI NÀY VÀO:
        public void AddItem(Product product, int quantity)
        {
            // Tìm xem sản phẩm đã có trong giỏ hàng (items) chưa
            CartItem item = items.FirstOrDefault(i => i.ProductID == product.ProductID);

            if (item == null) // Nếu sản phẩm chưa có trong giỏ
            {
                // Tạo mới một CartItem và thêm vào danh sách
                items.Add(new CartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,     // Tên cột tên SP của bạn
                    ProductImage = product.ProductImage,    // Tên cột ảnh SP của bạn
                    UnitPrice = product.ProductPrice,   // Tên cột giá SP của bạn
                    Quantity = quantity
                });
            }
            else // Nếu sản phẩm đã có
            {
                // Chỉ tăng số lượng
                item.Quantity += quantity;
            }
        }

        // Xóa sản phẩm khỏi giỏ
        public void RemoveItem(int productId)
        {
            items.RemoveAll(i => i.ProductID == productId);
        }

        // Tính tổng giá trị giỏ hàng
        public decimal TotalValue()
        {
            return items.Sum(i => i.TotalPrice);
        }

        // Làm trống giỏ hàng
        public void clear()
        {
            items.Clear();
        }

        // Cập nhật số lượng của sản phẩm đã chọn
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = items.FirstOrDefault(i => i.ProductID == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }

        public void ClearCart()
        {
            items.Clear();
        }
    }
}
