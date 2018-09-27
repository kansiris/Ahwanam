using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class PaymentsController : Controller
    {
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        OrderService orderService = new OrderService();
        OrderdetailsServices orderdetailService = new OrderdetailsServices();
        // GET: Payments
        public ActionResult Index(string Oid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string id = user.UserId.ToString();
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                Vendormaster Vendormaster = vendorMasterService.GetVendorByEmail(email);
                ViewBag.Vendor = vendorMasterService.GetVendor(Convert.ToInt64(Vendormaster.Id));
                if (Oid != null && Oid != "")
                {
                    var orderdetails = orderService.userOrderList().Where(m => m.OrderId == long.Parse(Oid)).ToList();
                    ViewBag.orderid = Oid;
                    ViewBag.username = orderdetails.FirstOrDefault().FirstName + " " + orderdetails.FirstOrDefault().LastName;
                    ViewBag.vendorname = orderdetails.FirstOrDefault().BusinessName;
                    ViewBag.vendoraddress = orderdetails.FirstOrDefault().Address + "," + orderdetails.FirstOrDefault().Landmark + "," + orderdetails.FirstOrDefault().City;
                    ViewBag.vendorcontact = orderdetails.FirstOrDefault().ContactNumber;
                    ViewBag.orderdate = Convert.ToDateTime(orderdetails.FirstOrDefault().BookedDate).ToString("MMM d,yyyy");
                    ViewBag.orderdetails = orderdetails;
                    ViewBag.totalprice = orderdetails.FirstOrDefault().TotalPrice;
                }
            }
            return View();
        }
    }
}