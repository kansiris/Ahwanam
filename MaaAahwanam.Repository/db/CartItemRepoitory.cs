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

        public List<GetCartItemsnew_Result> CartItemListnew(int vid)
        {
            return maaAahwanamEntities.GetCartItemsnew(vid).ToList();
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
            cartItem.Status = cartitemjson.Status;
            updatestatus = _dbContext.SaveChanges();
            return updatestatus;
        }

        public int adddatecartitem(CartItem cartitemjson)
        {
            int adddatecart;
            CartItem cartItem = new CartItem();
            cartItem = _dbContext.CartItem.SingleOrDefault(i => i.CartId == cartitemjson.CartId);
        
            cartItem.TotalPrice = cartitemjson.TotalPrice;
            cartItem.ExtraDate1 = cartitemjson.ExtraDate1;
            cartItem.ExtraDate2 = cartitemjson.ExtraDate2;
            cartItem.ExtraDate3 = cartitemjson.ExtraDate3;
            adddatecart = _dbContext.SaveChanges();
            return adddatecart;
        }
        public CartItem AddCartItem(CartItem cartItem)
        {
            cartItem = _dbContext.CartItem.Add(cartItem);
            _dbContext.SaveChanges();
            return cartItem;
        }

        public string DeletecartItem(long cartId)
        {
            try
            {
                var list = _dbContext.CartItem.FirstOrDefault(m => m.CartId == cartId);
                _dbContext.CartItem.Remove(list);
                _dbContext.SaveChanges();
                return "Success";
            }
            catch
            {
                return "Failed";
            }
        }

        public List<cartcount_Result> cartcount(long id)
        {
           return  maaAahwanamEntities.cartcount(id).ToList();
        }

    }
}
