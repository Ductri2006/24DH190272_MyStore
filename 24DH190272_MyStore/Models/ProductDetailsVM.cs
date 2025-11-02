using _24DH190272_MyStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;


namespace _24DH190272_MyStore.Models
{
    public class ProductDetailsVM
    {
        // 1. Sản phẩm chính đang xem
        public Product Product { get; set; } //

        // 2. Số lượng muốn đặt mua (Mặc định = 1)
        [Range(1, 99, ErrorMessage = "Số lượng phải từ 1 đến 99")]
        public int Quantity { get; set; } = 1; //

        // 3. Danh sách Sản phẩm Tương Tự (cùng danh mục)
        public ICollection<Product> RelatedProducts { get; set; } // [cite: 181]

        // 4. Danh sách Sản phẩm Bán Chạy (Top Deals)
        // THÀNH DÒNG NÀY:
        public IPagedList<Product> TopProducts { get; set; }
    }
}