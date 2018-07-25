using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Models;
using System.IO;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class NVendorOrdersController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        OrderService orderService = new OrderService();
        Payment_orderServices payment_orderServices = new Payment_orderServices();
        // GET: NVendorOrders
        public ActionResult Index(string ks)
        {
            try
            {
                if (TempData["Active"] != "")
                {
                    ViewBag.Active = TempData["Active"];
                }
                ViewBag.id = ks;
                string strReq = "";
                encptdecpt encript = new encptdecpt();
                strReq = encript.Decrypt(ks);
                //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                string[] arrMsgs = strReq.Split('&');
                string[] arrIndMsg;
                string id = "";
                arrIndMsg = arrMsgs[0].Split('='); //Get the id
                id = arrIndMsg[1].ToString().Trim();
                var orders = orderService.userOrderList().Where(m => m.Id == int.Parse(id));
                ViewBag.order = orders.OrderByDescending(m => m.OrderId);
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult OrderApproval(string ks, string orderid, string command)
        {
            try
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    if (user.UserType == "Vendor")
                    {
                        string strReq = "";
                        encptdecpt encript = new encptdecpt();
                        strReq = encript.Decrypt(ks);
                        //Parse the value... this is done is very raw format.. you can add loops or so to get the values out of the query string...
                        string[] arrMsgs = strReq.Split('&');
                        string[] arrIndMsg;
                        string id = "";
                        arrIndMsg = arrMsgs[0].Split('='); //Get the id
                        id = arrIndMsg[1].ToString().Trim();
                        //var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                        var orders = orderService.userOrderList().Where(m => m.OrderId == Convert.ToInt64(orderid));
                        Order order = orderService.GetParticularOrder(long.Parse(orderid));
                        OrderDetail orderdetail = new OrderDetail();
                        int paymentchecking = payment_orderServices.GetPaymentOrderService(long.Parse(orderid)).Count();
                        //if (paymentchecking > 0)
                        //{
                        //    TempData["Active"] = "Cannot Update Partial/Full Payement Orders Status";
                        //    return RedirectToAction("Index", "NVendorOrders", new { ks = ks });
                        //}
                        if (command == "Accept")
                        {
                            order.Status = "Active";
                            order.UpdatedBy = long.Parse(id);
                            orderdetail.Status = "Active";
                            orderdetail.UpdatedBy = long.Parse(id);
                            order = orderService.updateOrderstatus(order, orderdetail, Convert.ToInt64(orderid));
                            TempData["Active"] = "Order Accepted";
                            return RedirectToAction("Index", "NVendorOrders", new { ks = ks });
                        }
                        else
                        {
                            order.Status = "Vendor Declined";
                            orderdetail.Status = "Vendor Declined";
                            order = orderService.updateOrderstatus(order, orderdetail, Convert.ToInt64(orderid));
                            TempData["Active"] = "Order Cancelled";
                            return RedirectToAction("Index", "NVendorOrders", new { ks = ks });
                        }
                        
                        //SendEmail(int.Parse(orders.FirstOrDefault().UserLoginId.ToString()), orderid, id, command, orders.FirstOrDefault().BusinessName);
                        // return RedirectToAction("Index", "NVendorOrders", new { id = id });
                        //return Content("<script language='javascript' type='text/javascript'>alert('Order Cancelled');location.href='" + @Url.Action("Index", "NVendorOrders", new { ks = ks }) + "'</script>");

                    }
                }
                //TempData["Active"] = "Please Login";
                //return RedirectToAction("Index", "NVendorOrders", new { id = id });
                return Content("<script language='javascript' type='text/javascript'>alert('Please Login');location.href='" + @Url.Action("Index", "NVendorOrders", new { ks = ks }) + "'</script>");

            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }

        public void SendEmail(int userid, string orderid, string id, string command, string BusinessName)
        {
            var userlogdetails = userLoginDetailsService.GetUserId(userid);
            string txtto = userlogdetails.UserName;
            var userdetails = userLoginDetailsService.GetUser(userid);

            string name = userdetails.FirstName;
            string umessage = "";
            if (command == "Accept") umessage = "" + BusinessName + " Accepted Your Order";
            else if (command == "Decline") umessage = "" + BusinessName + " Declined Your Order Request Due to inavailablity of Dates";
            name = Capitalise(name);

            string OrderId = orderid;

            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/userorderconfirmation.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[message]", umessage);
            readFile = readFile.Replace("[ActivationLink]", url);
            readFile = readFile.Replace("[name]", name);
            readFile = readFile.Replace("[orderid]", OrderId);

            string txtmessage = readFile;
            string subj = "" + BusinessName + " Order update";
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
            var vendordetails = userLoginDetailsService.getvendor(Convert.ToInt16(id));


            txtto = vendordetails.EmailId;
            string vname = vendordetails.BusinessName;
            vname = Capitalise(vname);

            string vmessage = "";
            if (command == "Accept") vmessage = "You have Accepted " + userdetails.FirstName + " Order Request";
            else if (command == "Decline") vmessage = "You have Declined " + userdetails.FirstName + " Order Request";

            url = Request.Url.Scheme + "://" + Request.Url.Authority;
            File = new FileInfo(Server.MapPath("/mailtemplate/vendororderconfirmation.html"));
            readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[ActivationLink]", url);
            readFile = readFile.Replace("[message]", vmessage);
            //readFile = readFile.Replace("[name]", name);
            readFile = readFile.Replace("[vname]", vname);
            readFile = readFile.Replace("[orderid]", OrderId);
            txtmessage = readFile;
            subj = "Order Status Update";
            emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
        }
    }
}