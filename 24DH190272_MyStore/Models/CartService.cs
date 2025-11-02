using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH190272_MyStore.Models
{
    public class CartService
    {
        private readonly HttpSessionStateBase session;

        public CartService(HttpSessionStateBase session)
        {
            this.session = session;
        }

        // Lấy giỏ hàng từ Session
        public Cart GetCart()
        {
            var cart = (Cart)session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                session["Cart"] = cart;
            }
            return cart;
        }

        // Xóa giỏ hàng khỏi Session
        public void ClearCart()
        {
            session["Cart"] = null;
        }
    }
}
