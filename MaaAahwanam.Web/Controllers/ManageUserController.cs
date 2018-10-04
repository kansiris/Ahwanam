using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MaaAahwanam.Web.Controllers
{
    public class ManageUserController : Controller
    {
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorDashBoardService mnguserservice = new VendorDashBoardService();
        viewservicesservice viewservicesss = new viewservicesservice();
        VendorProductsService vendorProductsService = new VendorProductsService();



        // GET: ManageUser
        public ActionResult Index(string VendorId, string select)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = userLoginDetailsService.Getusername(long.Parse(uid));
                vendorMaster = vendorMasterService.GetVendorByEmail(vemail);
                VendorId = vendorMaster.Id.ToString();
                ViewBag.masterid = VendorId;
                ViewBag.Userlist = mnguserservice.getuser(VendorId);
                if (select != null)
                {
                    var select1 = select.Split(',');

                    ViewBag.loc = select1[0];
                    var guests = select1[1];
                    DateTime date = Convert.ToDateTime(select1[2]);
                    string date1 = date.ToString("dd-MM-yyyy");
                    ViewBag.date = date1;
                    ViewBag.eventtype = select1[3];
                    var pid = select1[4];
                    var pkgs = vendorProductsService.getpartpkgs(pid).FirstOrDefault();
                    string price = "";
                    if (pkgs.PackagePrice == null)
                    { price = Convert.ToString(pkgs.price1); }
                    else { price = Convert.ToString(pkgs.PackagePrice); }

                    var total = Convert.ToInt64(guests) * Convert.ToInt64(price);
                    ViewBag.guest = guests;
                    ViewBag.total = total;
                    ViewBag.price = price;
                    ViewBag.pid = pid;
                    ViewBag.pname = pkgs.PackageName;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(ManageUser mnguser, string id, string command)
        {
            string msg = string.Empty;
            mnguser.registereddate = DateTime.Now;
            mnguser.updateddate = DateTime.Now;
            if (command == "Save")
            {
                mnguser = mnguserservice.AddUser(mnguser);
                msg = "Added New User";
            }
            else if (command == "Update")
            {
                mnguser = mnguserservice.UpdateUser(mnguser, int.Parse(id));
                msg = "Updated User";
            }
            return Content("<script language='javascript' type='text/javascript'>alert('" + msg + "');location.href='/ManageUser'</script>");
        }
        public JsonResult checkemail(string email, string id)
        {
            int query = mnguserservice.checkuseremail(email, id);
            if (query == 0)
                return Json("valid email");
            else
                return Json("already email is added");
        }
        [HttpPost]
        public JsonResult GetUserDetails(string id)
        {
            var data = mnguserservice.getuserbyid(int.Parse(id));
            return Json(data);
        }
        //[HttpPost]
        //public JsonResult UpdateUserDetails(ManageUser mnguser, string id)
        //{
        //    mnguser.updateddate = DateTime.Now;
        //    mnguser = mnguserservice.UpdateUser(mnguser, int.Parse(id));
        //    return Json("Sucess", JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult booknow(string uid, string loc, string eventtype, string count, string date, string pid, string vid, string selectedp)
        {
            int userid = Convert.ToInt32(uid);
            string price = "";
            string totalprice = "";
            string guest = "";
            string type = "";
            string pid1 = "";
            string etype1 = "";
            string date1 = "";

            HomeController home = new HomeController();

            if (loc != "")
            {
                pid1 = pid;
                var userdata = userLoginDetailsService.GetUser(userid);
                //Payment Section
                var pkgs1 = vendorProductsService.getpartpkgs(pid).FirstOrDefault();
                type = pkgs1.VendorType;
                guest = count;
                date1 = date;
                if (pkgs1.PackagePrice == null)
                { price = Convert.ToString(pkgs1.price1); }
                else { price = Convert.ToString(pkgs1.PackagePrice); }
                string totalprice1 = (Convert.ToInt32(price) * Convert.ToInt32(count)).ToString();
                // string id = Convert.ToString(pid);
                //string did = Convert.ToString(cartdetails.DealId);
                // string timeslot = cartdetails.attribute;
                etype1 = eventtype;
                if (type == "Photography" || type == "Decorator" || type == "Other")
                {
                    totalprice = price;
                    guest = "0";
                }
                else
                {
                    totalprice = totalprice1;
                }

            }
            else
            {
                var select1 = selectedp.Split(',');

                ViewBag.loc = select1[0];
                var guests = select1[1];
                DateTime ksdate = Convert.ToDateTime(select1[2]);
                date1 = ksdate.ToString("dd-MM-yyyy");
                ViewBag.date = date1;
                etype1 = ViewBag.eventtype = select1[3];
                string pid2 = select1[4];
                pid1 = pid2;
                var pkgs2 = vendorProductsService.getpartpkgs(pid2).FirstOrDefault();
                if (pkgs2.PackagePrice == null)
                { price = Convert.ToString(pkgs2.price1); }
                else { price = Convert.ToString(pkgs2.PackagePrice); }

                totalprice = (Convert.ToInt64(guests) * Convert.ToInt64(price)).ToString();
                guest = guests;
            }
            var pkgs = vendorProductsService.getpartpkgs(pid1).FirstOrDefault();


            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            //Saving Record in order Table
            OrderService orderService = new OrderService();
            MaaAahwanam.Models.Order order = new MaaAahwanam.Models.Order();
            order.TotalPrice = Convert.ToDecimal(totalprice);
            order.OrderDate = Convert.ToDateTime(updateddate); //Convert.ToDateTime(bookeddate);
            order.UpdatedBy = long.Parse(vid);
            order.OrderedBy = long.Parse(vid);
            order.UpdatedDate = Convert.ToDateTime(updateddate);
            order.Status = "Pending";
            order = orderService.SaveOrder(order);

            //Saving Order Details
            OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderId = order.OrderId;
            orderDetail.OrderBy = userid;
            orderDetail.PaymentId = '1';
            // orderDetail.ServiceType = type;
            orderDetail.ServicePrice = decimal.Parse(price);
            //  orderDetail.attribute = timeslot;
            orderDetail.TotalPrice = decimal.Parse(totalprice);
            orderDetail.PerunitPrice = decimal.Parse(price);
            orderDetail.Quantity = int.Parse(guest);
            orderDetail.OrderId = order.OrderId;
            orderDetail.VendorId = long.Parse(vid);
            orderDetail.Status = "Pending";
            orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
            orderDetail.UpdatedBy = userid;
            orderDetail.subid = pkgs.VendorSubId;
            orderDetail.BookedDate = Convert.ToDateTime(date1);
            orderDetail.EventType = etype1;
            //                orderDetail.DealId = long.Parse(did);


            orderdetailsServices.SaveOrderDetail(orderDetail);
            var userlogdetails = mnguserservice.getuserbyid(userid);


            string txtto = userlogdetails.email;

            string name = userlogdetails.firstname;
            name = home.Capitalise(name);
            string OrderId = Convert.ToString(order.OrderId);
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[ActivationLink]", url);
            readFile = readFile.Replace("[name]", name);
            readFile = readFile.Replace("[orderid]", OrderId);
            string txtmessage = readFile;//readFile + body;
            string subj = "Thanks for your order";
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
            emailSendingUtility.Email_maaaahwanam("seema@xsilica.com ", txtmessage, subj);

            var vendordetails = userLoginDetailsService.getvendor(Convert.ToInt32(vid));

            string txtto1 = vendordetails.EmailId;
            string vname = vendordetails.BusinessName;
            vname = home.Capitalise(vname);

            string url1 = Request.Url.Scheme + "://" + Request.Url.Authority;
            FileInfo file1 = new FileInfo(Server.MapPath("/mailtemplate/vorder.html"));
            string readfile1 = file1.OpenText().ReadToEnd();
            readfile1 = readfile1.Replace("[ActivationLink]", url1);
            readfile1 = readfile1.Replace("[name]", name);
            readfile1 = readfile1.Replace("[vname]", vname);
            readfile1 = readfile1.Replace("[orderid]", OrderId);
            string txtmessage1 = readfile1;
            string subj1 = "order has been placed";
            emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1);
            string msg = OrderId;


            return Json(msg, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult booknowss(string loc, string eventtype, string count, string date, string pid, string vid, string selectedp,
            string businessname, string firstname, string lastname, string email, string phoneno, string adress1, string adress2, string city, string state,
            string country, string pincode, string Status)
        {
            string price = "";
            string totalprice = "";
            string guest = "";
            string type = "";
            string pid1 = "";
            string etype1 = "";
            string date1 = "";
            ManageUser mnguser = new ManageUser();
            mnguser.registereddate = DateTime.Now;
            mnguser.updateddate = DateTime.Now;
            mnguser.Businessname = businessname;
            mnguser.firstname = firstname;
            mnguser.lastname = lastname;
            mnguser.email = email;
            mnguser.phoneno = phoneno;
            mnguser.adress1 = adress1;
            mnguser.adress2 = adress2;
            mnguser.city = city;
            mnguser.state = state;
            mnguser.country = country;
            mnguser.pincode = pincode;
            mnguser.Status = Status;

            mnguser = mnguserservice.AddUser(mnguser);
            var ksc = mnguserservice.getuserbyemail(email).FirstOrDefault();
            int userid = Convert.ToInt32(ksc.id);
            HomeController home = new HomeController();

            if (loc != "")
            {
                pid1 = pid;
                var userdata = userLoginDetailsService.GetUser(userid);
                //Payment Section
                var pkgs1 = vendorProductsService.getpartpkgs(pid).FirstOrDefault();
                type = pkgs1.VendorType;
                guest = count;
                date1 = date;
                if (pkgs1.PackagePrice == null)
                { price = Convert.ToString(pkgs1.price1); }
                else { price = Convert.ToString(pkgs1.PackagePrice); }
                string totalprice1 = (Convert.ToInt32(price) * Convert.ToInt32(count)).ToString();
                // string id = Convert.ToString(pid);
                //string did = Convert.ToString(cartdetails.DealId);
                // string timeslot = cartdetails.attribute;
                etype1 = eventtype;
                if (type == "Photography" || type == "Decorator" || type == "Other")
                {
                    totalprice = price;
                    guest = "0";
                }
                else
                {
                    totalprice = totalprice1;
                }

            }
            else
            {
                var select1 = selectedp.Split(',');

                ViewBag.loc = select1[0];
                var guests = select1[1];
                DateTime ksdate = Convert.ToDateTime(select1[2]);
                date1 = ksdate.ToString("dd-MM-yyyy");
                ViewBag.date = date1;
                etype1 = ViewBag.eventtype = select1[3];
                string pid2 = select1[4];
                pid1 = pid2;
                var pkgs2 = vendorProductsService.getpartpkgs(pid2).FirstOrDefault();
                if (pkgs2.PackagePrice == null)
                { price = Convert.ToString(pkgs2.price1); }
                else { price = Convert.ToString(pkgs2.PackagePrice); }

                totalprice = (Convert.ToInt64(guests) * Convert.ToInt64(price)).ToString();
                guest = guests;
            }
            var pkgs = vendorProductsService.getpartpkgs(pid1).FirstOrDefault();


            DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            //Saving Record in order Table
            OrderService orderService = new OrderService();
            MaaAahwanam.Models.Order order = new MaaAahwanam.Models.Order();
            order.TotalPrice = Convert.ToDecimal(totalprice);
            order.OrderDate = Convert.ToDateTime(updateddate); //Convert.ToDateTime(bookeddate);
            order.UpdatedBy = long.Parse(vid);
            order.OrderedBy = long.Parse(vid);
            order.UpdatedDate = Convert.ToDateTime(updateddate);
            order.Status = "Pending";
            order = orderService.SaveOrder(order);

            //Saving Order Details
            OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderId = order.OrderId;
            orderDetail.OrderBy = userid;
            orderDetail.PaymentId = '1';
            // orderDetail.ServiceType = type;
            orderDetail.ServicePrice = decimal.Parse(price);
            //  orderDetail.attribute = timeslot;
            orderDetail.TotalPrice = decimal.Parse(totalprice);
            orderDetail.PerunitPrice = decimal.Parse(price);
            orderDetail.Quantity = int.Parse(guest);
            orderDetail.OrderId = order.OrderId;
            orderDetail.VendorId = long.Parse(vid);
            orderDetail.Status = "Pending";
            orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
            orderDetail.UpdatedBy = userid;
            orderDetail.subid = pkgs.VendorSubId;
            orderDetail.BookedDate = Convert.ToDateTime(date1);
            orderDetail.EventType = etype1;
            //                orderDetail.DealId = long.Parse(did);


            orderdetailsServices.SaveOrderDetail(orderDetail);
            var userlogdetails = mnguserservice.getuserbyid(userid);


            string txtto = userlogdetails.email;

            string name = userlogdetails.firstname;
            name = home.Capitalise(name);
            string OrderId = Convert.ToString(order.OrderId);
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[ActivationLink]", url);
            readFile = readFile.Replace("[name]", name);
            readFile = readFile.Replace("[orderid]", OrderId);
            string txtmessage = readFile;//readFile + body;
            string subj = "Thanks for your order";
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
            emailSendingUtility.Email_maaaahwanam("seema@xsilica.com ", txtmessage, subj);

            var vendordetails = userLoginDetailsService.getvendor(Convert.ToInt32(vid));

            string txtto1 = vendordetails.EmailId;
            string vname = vendordetails.BusinessName;
            vname = home.Capitalise(vname);

            string url1 = Request.Url.Scheme + "://" + Request.Url.Authority;
            FileInfo file1 = new FileInfo(Server.MapPath("/mailtemplate/vorder.html"));
            string readfile1 = file1.OpenText().ReadToEnd();
            readfile1 = readfile1.Replace("[ActivationLink]", url1);
            readfile1 = readfile1.Replace("[name]", name);
            readfile1 = readfile1.Replace("[vname]", vname);
            readfile1 = readfile1.Replace("[orderid]", OrderId);
            string txtmessage1 = readfile1;
            string subj1 = "order has been placed";
            emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1);
            string msg = OrderId;

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult orderdetails(string select)
        {
            if (select != null)
            {
                var select1 = select.Split(',');

                ViewBag.loc = select1[0];
                var guests = select1[1];
                DateTime date = Convert.ToDateTime(select1[2]);
                string date1 = date.ToString("dd-MM-yyyy");
                ViewBag.date = date1;
                ViewBag.eventtype = select1[3];
                var pid = select1[4];
                var pkgs = vendorProductsService.getpartpkgs(pid).FirstOrDefault();
                string price = "";
                if (pkgs.PackagePrice == null)
                {
                    price = Convert.ToString(pkgs.price1);
                }
                else { price = Convert.ToString(pkgs.PackagePrice); }

                var total = Convert.ToInt64(guests) * Convert.ToInt64(price);
                ViewBag.guest = guests;
                ViewBag.total = total;
                ViewBag.price = price;
                ViewBag.pid = pid;
                ViewBag.pname = pkgs.PackageName;

            }
            return View("orderdetails");
        }
    }
}