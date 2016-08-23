using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class CartService
    {
        CartItemRepoitory cartItemRepoitory = new CartItemRepoitory();

        public List<GetCartItems_Result> CartItemsList(int vid)
        {
            List<GetCartItems_Result> l1 = cartItemRepoitory.CartItemList(vid);
            return l1;
        }
        public GetCartItems_Result editcartitem(int vid, int cartID)
        {
            GetCartItems_Result l1 = cartItemRepoitory.CartItemList(vid).Where(i => i.CartId == cartID).FirstOrDefault();
            return l1;
        }
        public string Updatecartitem(CartItem CartItemjson)
        {
            int l1 = cartItemRepoitory.Updatecartitem(CartItemjson);
            if (l1 != 0)
            {
                return "Success";
            }
            return "Failure";
        }
        public int CartItemsCount(int UserId)
        {
            var l1 = 0;
            if (UserId != 0)
                l1 = cartItemRepoitory.CartItemList(UserId).Count();
            return l1;
        }
        public string AddCartItem(CartItem cartItem)
        {
            string message = "";
            cartItem = cartItemRepoitory.AddCartItem(cartItem);
            if (cartItem != null)
            {
                message = "Success";
            }
            else
            {
                message = "Failed";
            }
            return message;
        }
    }
}
