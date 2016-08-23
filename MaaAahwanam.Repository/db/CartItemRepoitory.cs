using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using AutoMapper;

namespace MaaAahwanam.Repository.db
{
    public class CartItemRepoitory
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        GetCartItems_ResultModel getCartItems_ResultModel = new GetCartItems_ResultModel();
        public List<GetCartItems_Result> CartItemList(int vid)
        {
            return maaAahwanamEntities.GetCartItems(vid).ToList();
        }
        public int Updatecartitem(CartItem cartitemjson)
        {
            int updatestatus;
            CartItem cartItem = new CartItem();
            cartItem = _dbContext.CartItem.SingleOrDefault(i => i.CartId == cartitemjson.CartId);
            cartItem.attribute = cartitemjson.attribute;
            cartItem.Perunitprice = cartitemjson.Perunitprice;
            cartItem.Quantity = cartitemjson.Quantity;
            cartItem.TotalPrice = cartitemjson.TotalPrice;
            updatestatus = _dbContext.SaveChanges();
            return updatestatus;
        }
        public CartItem AddCartItem(CartItem cartItem)
        {
            cartItem=_dbContext.CartItem.Add(cartItem);
            _dbContext.SaveChanges();
            return cartItem;
        }


    }
}
