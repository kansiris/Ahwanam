using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Service;
using System.Net;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.Text.RegularExpressions;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Web.Controllers
{
    public class NUserProfileController : Controller
    {

        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        CartService cartService = new CartService();

        // GET: NUserProfile
        public ActionResult Index()
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var userdata = userLoginDetailsService.GetUser((int)user.UserId);
            if (userdata.FirstName != "" && userdata.FirstName != null)
                ViewBag.username = userdata.FirstName;
            else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                ViewBag.username = "" + userdata.FirstName + " " + userdata.LastName + "";
            else
                ViewBag.username = userdata.AlternativeEmailID;
            ViewBag.phoneno = userdata.UserPhone;
            var userdata1 = userLoginDetailsService.GetUserId((int)user.UserId);
            ViewBag.emailid = userdata1.UserName;

            List<GetCartItemsnew_Result> cartlist = cartService.CartItemsListnew(int.Parse(user.UserId.ToString()));
            decimal total = cartlist.Sum(s => s.TotalPrice);
            ViewBag.Cartlist = cartlist;
            ViewBag.Total = total;





            return View();
        }

        public ActionResult changepassword(UserLogin userLogin)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            var userdata12 = userLoginDetailsService.GetUserId((int)user.UserId);
            userLoginDetailsService.changepassword(userLogin, (int)user.UserId);
            return Json("success");
            //return Content("<script language='javascript' type='text/javascript'>alert('Password Updated Successfully');location.href='" + @Url.Action("Index", "ChangePassword") + "'</script>");

        }

        public ActionResult updatedetails(UserDetail userdetail)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            userLoginDetailsService.UpdateUserdetailsnew(userdetail, (int)user.UserId);

            return Json("success");

            //  return Content("<script language='javascript' type='text/javascript'>alert('userdetails Updated Successfully');location.href='" + @Url.Action("Index", "ChangePassword") + "'</script>");

        }
    }
}