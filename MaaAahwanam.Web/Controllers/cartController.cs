using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MaaAahwanam.Web.Controllers
{
    public class cartController : Controller
    {
        cartservices cartserve = new cartservices();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        decimal totalp, discount,servcharge,gst,nettotal, totalp2;
        // GET: cart
        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    if (user.UserType == "Admin")
                    {
                        ViewBag.cartCount = cartserve.CartItemsCount(0);
                        return PartialView("ItemsCartdetails");
                    }
                    ViewBag.cartCount = cartserve.CartItemsCount((int)user.UserId);

                    List<GetCartItems_Result> cartlist = cartserve.CartItemsList(int.Parse(user.UserId.ToString()));
                    decimal total = cartlist.Where(m => m.Status == "Active").Sum(s => s.TotalPrice);
                    ViewBag.cartitems = cartlist.OrderByDescending(m => m.UpdatedDate).Where(m => m.Status == "Active");
                    ViewBag.Total = total;

                }
            }
            else
            {
                ViewBag.cartCount = cartserve.CartItemsCount(0);
            }


            return View();
        }

        public JsonResult DeletecartItem(long cartId)
        {
            var message = cartserve.Deletecartitem(cartId);
            return Json(message);
        }


        public ActionResult billing(string cartid)
        {
            if (cartid == null)
            {

                ViewBag.tamount = "000";
                ViewBag.discount = "0";
                ViewBag.service = "0";
                ViewBag.Gst = "0";
                ViewBag.netamount = "0";

            }
            else {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    if (user.UserType == "User")
                    {
                        List<GetCartItems_Result> cartlist = cartserve.CartItemsList(int.Parse(user.UserId.ToString()));


                        var cartid1 = cartid.Split(',');
                        for (int i = 0; i < cartid1.Count(); i++)
                        {
                            if (cartid1[i] == "" || cartid1[i] == null)
                            {

                                totalp = 0;
                            }
                            else
                            {
                                var cartdetails = cartlist.Where(m => m.CartId == Convert.ToInt64(cartid1[i])).FirstOrDefault();
                                totalp = cartdetails.TotalPrice;
                            }
                            totalp2 = totalp2 + totalp;
                            discount = 0;
                            servcharge = 0;
                            gst = 0 ;
                            nettotal = nettotal + totalp - Convert.ToDecimal(discount) + Convert.ToDecimal(servcharge) + Convert.ToDecimal(gst);
                            
                        }
                        var totalp1 = totalp2;
                        var discount1 = "0.00";
                        var servcharge1 = servcharge;
                        var gst1 = gst;
                        var nettotal1 = Convert.ToString(nettotal);
                        ViewBag.tamount = totalp1;
                        ViewBag.discount = discount1;
                        ViewBag.service = servcharge1;
                        ViewBag.Gst = gst1;
                        ViewBag.netamount = nettotal1;
                    }
                }

            }


            return PartialView("billing");

        }

    }
}

