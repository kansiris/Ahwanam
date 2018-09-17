using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace MaaAahwanam.Web.Controllers
{
    public class cartController : Controller
    {
        cartservices cartserve = new cartservices();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        decimal totalp, discount, servcharge, gst, nettotal, totalp2;
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
            //if (cartid == null)
            //{
            //    ViewBag.tamount = "000";
            //    ViewBag.discount = "0";
            //    ViewBag.service = "0";
            //    ViewBag.Gst = "0";
            //    ViewBag.netamount = "0";
            //}
            //else {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    List<GetCartItems_Result> cartlist = cartserve.CartItemsList(int.Parse(user.UserId.ToString())).ToList();
                    
                    if (cartlist.Count == 0)
                    {
                        ViewBag.tamount = "000";
                        ViewBag.discount = "0";
                        ViewBag.service = "0";
                        ViewBag.Gst = "0";
                        ViewBag.netamount = "0";
                        return PartialView("billing");
                    }
                    if (cartid != null)
                        cartid = cartid.Trim(',');
                    else
                        cartid = string.Join(",", cartlist.Select(m => m.CartId));
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
                        gst = 0;
                        nettotal = nettotal + totalp - Convert.ToDecimal(discount) + Convert.ToDecimal(servcharge) + Convert.ToDecimal(gst);
                    }
                    var totalp1 = totalp2;
                    var discount1 = "0";
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
            //}
            return PartialView("billing");
        }

        public JsonResult email(string selcartid)
        {
            selcartid = selcartid.Trim(',');
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userlogin = userLoginDetailsService.GetUserId(int.Parse(user.UserId.ToString()));
                    string Email = userlogin.UserName;
                    string txtto = "sireesh.k@xsilica.com,rameshsai@xsilica.com,seema@xsilica.com,amit.saxena@ahwanam.com,jm@dsc-usa.com";
                    int id = Convert.ToInt32(user.UserId);
                    var userdetails = userLoginDetailsService.GetUser(id);
                    string ipaddress = HttpContext.Request.UserHostAddress;
                    string username = userdetails.FirstName;
                    string phoneno = userdetails.UserPhone;
                    HomeController home = new HomeController();
                    username = home.Capitalise(username);
                    List<GetCartItems_Result> cartlist = cartserve.CartItemsList(int.Parse(user.UserId.ToString()));
                    var cartid1 = selcartid.Split(',');
                    //for (int i = 0; i < cartid1.Count(); i++)
                    //{
                    //    var cartdetails = cartlist.Where(m => m.CartId == Convert.ToInt64(cartid1[i])).FirstOrDefault();
                    //    var cartname1 = cartdetails.BusinessName;
                    //    cartname = cartname1 + ',' + cartname1;
                    //}
                    List<GetCartItems_Result> cartdetails = new List<GetCartItems_Result>();
                    for (int i = 0; i < cartid1.Count(); i++)
                    {
                        cartdetails.AddRange(cartlist.Where(m => m.CartId == Convert.ToInt64(cartid1[i])).ToList());
                    }
                    ViewBag.cartdetails = cartdetails;
                    StringBuilder cds = new StringBuilder();
                    cds.Append("<table style='border:1px;background: #0000;'><tbody><tb>");
                    foreach (var item in ViewBag.cartdetails)
                    {
                        cds.Append("<table style='border: 2px black solid'><tbody><tr><td> name </td><td> guest </td><td> amount </td><td> event </td><td> date </td></tr><tr><td style = 'width: 75px;border: 2px black solid;'> " + item.BusinessName + "</td><td style = 'width: 75px;border: 2px black solid;' > " + item.Quantity + " </td><td style = 'width: 75px;border: 2px black solid;'> " + item.TotalPrice + " </td><td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td style = 'width: 50px;border: 2px black solid;'> " + item.EventType + " </td></tr></tbody></table>");
                    }
                    cds.Append("</tb></table></tbody>");
                    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/login.html"));
                    string readFile = File.OpenText().ReadToEnd();
                    readFile = readFile.Replace("[carttable]", cds.ToString());
                    readFile = readFile.Replace("[name]", username);
                    readFile = readFile.Replace("[Ipaddress]", ipaddress);
                    readFile = readFile.Replace("[email]", Email);
                    readFile = readFile.Replace("[phoneno]", phoneno);
                    string txtmessage = readFile;//readFile + body;
                    string subj = "Get Quote From Cart Page";
                    EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                }
            }
            var message = "success";
            return Json(message);
        }
    }
}

