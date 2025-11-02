using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH190272_MyStore.Models
{
    public class HomeProductVM
    {
        // Tiêu chí tìm kiếm (tên, mô tả, loại sản phẩm)
        public string SearchTerm { get; set; }

        // Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }  // Trang hiện tại
        public int PageSize { get; set; } = 10;  // Số sản phẩm mỗi trang

        // Danh sách sản phẩm nổi bật (Top 10)
        public List<Product> FeaturedProducts { get; set; }

        // Danh sách sản phẩm mới (có phân trang)
        public PagedList.IPagedList<Product> NewProducts { get; set; }

    }
}