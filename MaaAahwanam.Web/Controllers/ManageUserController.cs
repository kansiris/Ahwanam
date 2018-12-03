using DotNetOpenAuth.Messaging;
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
    public class ManageUserController : Controller
    {
        VendorVenueService vendorVenueService = new VendorVenueService();

        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        PartnerService partnerservice = new PartnerService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        VendorDashBoardService mnguserservice = new VendorDashBoardService();

        // VendorProductsService vendorProductsService = new VendorProductsService();
        int tprice;
        int price1;

        // GET: ManageUser
        public ActionResult Index(string VendorId, string select, string packageid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
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
                    // var pid = select1[4];
                    if (packageid != "" || packageid != null)
                    {
                        var pakageid = packageid.Split(',');
                        int price; StringBuilder pakg = new StringBuilder();
                        for (int i = 0; i < pakageid.Count(); i++)
                        {
                            if (pakageid[i] == "" || pakageid[i] == null)
                            {
                                price1 = 0;
                            }
                            else
                            {
                                var pkgs = newmanageuse.getpartpkgs(pakageid[i]).FirstOrDefault();
                                if (pkgs.PackagePrice == null)
                                {
                                    price = Convert.ToInt32(pkgs.price1);
                                }
                                else { price = Convert.ToInt32(pkgs.PackagePrice); }
                                price1 = price1 + price;

                                pakg.Append("pkgs.PackageName + ',' +");
                            }

                        }


                        var total = Convert.ToInt64(guests) * Convert.ToInt64(price1);
                        ViewBag.guest = guests;
                        ViewBag.total = total;
                        ViewBag.price = price1;
                        ViewBag.pid = packageid;
                        ViewBag.pname = pakg;
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult Index(ManageUser mnguser, string id, string command)
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
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult checkemail(string email, string id)
        {
            int query = mnguserservice.checkuseremail(email, id);
            if (query == 0)
                return Json("success");
            else
                return Json("sucess1");

        }
        [HttpPost]
        public JsonResult GetUserDetails(string id)
        {
            var data = mnguserservice.getuserbyid(int.Parse(id));
            return Json(data);
        }

        public ActionResult customerdetails(string id)
        {
            if (id != null)
            {
                var data = mnguserservice.getuserbyid(int.Parse(id));
                ViewBag.customer = data;
            }
            else { ViewBag.customer = null; }
            return View();
        }
        public ActionResult mnguserdetails()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
               string VendorId = vendorMaster.Id.ToString();
                ViewBag.masterid = VendorId;
                ViewBag.Userlist = mnguserservice.getuser(VendorId);
            }
            else
            {
                ViewBag.Userlist = "";
            }
            return PartialView("mnguserdetails");
        }
        //[HttpPost]
        //public JsonResult UpdateUserDetails(ManageUser mnguser, string id)
        //{
        //    mnguser.updateddate = DateTime.Now;
        //    mnguser = mnguserservice.UpdateUser(mnguser, int.Parse(id));
        //    return Json("Sucess", JsonRequestBehavior.AllowGet);
        //}

      
    [HttpPost]
    public JsonResult booknow(string uid, string loc, string eventtype, string guest, string date, string pid, string vid, string timeslot,string booktype, string alltprice, string alldiscounttype, string discountprice, string fpkgprice)
    {
        int userid = Convert.ToInt32(uid);
        int price;
        string totalprice = "";
        string type = "";
        string etype1 = "";
        HomeController home = new HomeController();
        DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        //Saving Record in order Table
        //  OrderService orderService = new OrderService();
        List<string> sdate = new List<string>();
        List<string> stimeslot = new List<string>();
        List<SPGETpartpkg_Result> package = new List<SPGETpartpkg_Result>();
            var alltprice1 = alltprice.Trim(',').Split(',');
            var alldiscounttype1 = alldiscounttype.Trim(',').Split(',');
            var discountprice1 = discountprice.Trim(',').Split(',');
            var fpkgprice1 = fpkgprice.Trim(',').Split(',');

        var pkgs = pid.Split(',');
        var date1 = date.Trim(',').Split(',');
        var timeslot1 = timeslot.Split(',');
            OrderDetail orderDetail = new OrderDetail();

            etype1 = eventtype;
            for (int i = 0; i < pkgs.Count(); i++)
            {

                var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();
                if (data.PackagePrice == null)
                {
                    price = Convert.ToInt16(data.price1);
                }
                else { price = Convert.ToInt16(data.PackagePrice); }
                tprice = tprice + price;
                if (type == "Photography" || type == "Decorator" || type == "Other")
                {
                    totalprice = Convert.ToString(price);
                    guest = "0";
                }
                else
                {
                    totalprice = Convert.ToString(tprice * Convert.ToInt16(guest));
                }
            }
                Order order = new Order();
            order.TotalPrice = Convert.ToDecimal(totalprice);
            order.OrderDate = Convert.ToDateTime(updateddate); //Convert.ToDateTime(bookeddate);
            order.UpdatedBy = long.Parse(vid);
            order.OrderedBy = long.Parse(vid);
            order.UpdatedDate = Convert.ToDateTime(updateddate);
            if (booktype == "Quote") { order.type = "Quote"; }
            else
            {
                order.type = "Order";
            }
            order.bookingtype = "Vendor";
            order.Status = "Pending";
            order = newmanageuse.SaveOrder(order);

            //Saving Order Details
            //OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
            for (int i = 0; i < pkgs.Count(); i++)
            {

                var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();
                if (data.PackagePrice == null)
                {
                    price = Convert.ToInt16(data.price1);
                }
                else { price = Convert.ToInt16(data.PackagePrice); }
                tprice = tprice + price;
                if (type == "Photography" || type == "Decorator" || type == "Other")
                {
                    totalprice = Convert.ToString(price);
                    guest = "0";
                }
                else
                {
                    totalprice = Convert.ToString(price* Convert.ToInt16(guest));
                }

                for (int j = 0; j < date1.Count(); j++)
                {
                    if (date1[j].Split('~')[1] == data.VendorSubId.ToString())
                    {var allto = alltprice1[j].Split('₹')[1].ToString();
                       
                        if (allto.Split('~')[0].ToString() == null || allto.Split('~')[0].ToString() == "") { data.price1 = "0"; } else { data.price1 = allto.Split('~')[0].ToString(); }
                        if (alldiscounttype1[j].Split('~')[0] == null|| alldiscounttype1[j].Split('~')[0] == "") { data.price2 = "0"; } else { data.price2 = alldiscounttype1[j].Split('~')[0]; }
                        if (discountprice1[j].Split('~')[0] == null|| discountprice1[j].Split('~')[0] == "") { data.price3 = "0"; } else { data.price3 = discountprice1[j].Split('~')[0]; }
                        if (fpkgprice1[j].Split('~')[0] == null|| fpkgprice1[j].Split('~')[0] == "") { data.price4 = "0"; } else { data.price4 = fpkgprice1[j].Split('~')[0]; }
                        data.UpdatedDate = Convert.ToDateTime(date1[j].Split('~')[0]);
                        data.timeslot = timeslot1[j].Split('~')[0];
                    }
                }

               // data.UpdatedDate = Convert.ToDateTime(date1[i].Split('~')[0]);
                //data.timeslot = timeslot1[i].Split('~')[0];
               
                orderDetail.OrderId = order.OrderId;
                orderDetail.OrderBy = long.Parse(uid);
                orderDetail.PaymentId = '1';
                orderDetail.ServiceType = type;
          //      orderDetail.ServicePrice = Convert.ToDecimal(price);
                orderDetail.attribute = data.timeslot;
                orderDetail.TotalPrice = (Convert.ToDecimal(data.price1));
                orderDetail.ServicePrice = Convert.ToDecimal(data.price4);
                orderDetail.PerunitPrice = Convert.ToDecimal(price);
                orderDetail.Quantity = Convert.ToInt32(guest);
                orderDetail.OrderId = order.OrderId;
                orderDetail.VendorId = long.Parse(vid);

                if (booktype == "Quote") { orderDetail.type = "Quote"; }
                else
                {
                    orderDetail.type = "Order";
                }
                orderDetail.DiscountType = data.price2;
                orderDetail.DiscountPrice = Convert.ToDecimal(data.price3);
                //orderDetail.Discount = Convert.ToDecimal(data.price2);
                orderDetail.OrderType = "online";
                orderDetail.Status = "Pending";
                orderDetail.bookingtype = "Vendor";
                orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
                orderDetail.UpdatedBy = userid;
                orderDetail.subid = data.VendorSubId;
                orderDetail.BookedDate = Convert.ToDateTime(data.UpdatedDate);
                ViewBag.orderdate = orderDetail.BookedDate;
                orderDetail.EventType = etype1;
                orderDetail.ServiceType = data.VendorType;
                orderDetail.DealId = long.Parse(pkgs[i]);
                orderDetail =   newmanageuse.SaveOrderDetail(orderDetail);
            }
            var userlogdetails = mnguserservice.getuserbyid(userid);
    string txtto = userlogdetails.email;
    string name = userlogdetails.firstname;
    name = home.Capitalise(name);
            string OrderId = Convert.ToString(order.OrderId);
    StringBuilder cds = new StringBuilder();
    cds.Append("<table style='border:1px black solid;'><tbody>");
            cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td> Quantity</td><td>Perunit Price</td><td>Total Price</td></tr>");
            cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderDetail.BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + orderDetail.EventType + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.Quantity + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.PerunitPrice+ " </td><td style = 'width: 50px;border: 1px black solid;'> " +orderDetail.TotalPrice+ " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
            cds.Append("</tbody></table>");
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
    string readFile = File.OpenText().ReadToEnd();
    readFile = readFile.Replace("[ActivationLink]", url);
            readFile = readFile.Replace("[name]", name);
            readFile = readFile.Replace("[orderid]", OrderId);
            readFile = readFile.Replace("[table]", cds.ToString());
            string txtmessage = readFile;//readFile + body;
    string subj = "Thanks for your order";
    EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj, null);
            //emailSendingUtility.Email_maaaahwanam("seema@xsilica.com ", txtmessage, subj);
            string targetmails = "lakshmi.p@xsilica.com,seema.g@xsilica.com,rameshsai@xsilica.com";
    emailSendingUtility.Email_maaaahwanam(targetmails, txtmessage, subj, null);

            var vendordetails = newmanageuse.getvendor(Convert.ToInt32(vid));

    string txtto1 = vendordetails.EmailId;
    string vname = vendordetails.BusinessName;
    vname = home.Capitalise(vname);
            StringBuilder cds2 = new StringBuilder();
    cds2.Append("<table style='border:1px black solid;'><tbody>");
            cds2.Append("<tr><td>Order Id</td><td>Order Date</td><td>Customer Name</td><td>Customer Phone Number</td><td>flatno</td><td>Locality</td></tr>");
            cds2.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderDate + "</td><td style = 'width: 75px;border: 1px black solid;'> " + userlogdetails.firstname + " " + userlogdetails.lastname + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.phoneno + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress1 + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress2 + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
            cds2.Append("</tbody></table>");
            string url1 = Request.Url.Scheme + "://" + Request.Url.Authority;
    FileInfo file1 = new FileInfo(Server.MapPath("/mailtemplate/vorder.html"));
    string readfile1 = file1.OpenText().ReadToEnd();
    readfile1 = readfile1.Replace("[ActivationLink]", url1);
            readfile1 = readfile1.Replace("[name]", name);
            readfile1 = readfile1.Replace("[vname]", vname);
            readfile1 = readfile1.Replace("[msg]", cds2.ToString());
            readfile1 = readfile1.Replace("[orderid]", OrderId);
            string txtmessage1 = readfile1;
    string subj1 = "order has been placed";
    emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1, null);
            string msg = OrderId;


            return Json(msg, JsonRequestBehavior.AllowGet);
}


        [HttpPost]
        public JsonResult booknowss(string loc, string eventtype, string guest, string date, string pid, string vid, string selectedp,
            string businessname, string firstname, string lastname, string email, string phoneno, string adress1, string adress2, string city, string state,
            string country, string pincode, string Status, string ctype,string timeslot)
        {
            string msg; string totalprice = ""; int price;
            if (Status != "InActive")
            {

             
               
             
                string type = "";
                string etype1 = "";
                DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                //Saving Record in order Table
                //  OrderService orderService = new OrderService();
                List<string> sdate = new List<string>();
                List<string> stimeslot = new List<string>();
                List<SPGETpartpkg_Result> package = new List<SPGETpartpkg_Result>();
             
                    var pkgs = pid.Split(',');
                for (int i = 0; i < pkgs.Count(); i++)
                {

                    var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();
                    if (data.PackagePrice == null)
                    {
                        price = Convert.ToInt16(data.price1);
                    }
                    else { price = Convert.ToInt16(data.PackagePrice); }
                    tprice = tprice + price;
                    if (type == "Photography" || type == "Decorator" || type == "Other")
                    {
                        totalprice = Convert.ToString(price);
                        guest = "0";
                    }
                    else
                    {
                        totalprice = Convert.ToString(tprice * Convert.ToInt16(guest));
                    }
                }
                var date1 = date.Trim(',').Split(',');
                var timeslot1 = timeslot.Split(',');

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
                mnguser.type = ctype;
                mnguser.vendorId = vid;
                mnguser = mnguserservice.AddUser(mnguser);
                var ksc = mnguserservice.getuserbyemail(email).FirstOrDefault();
                int userid = Convert.ToInt32(ksc.id);
                HomeController home = new HomeController();


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
                for (int i = 0; i < pkgs.Count(); i++)
                {

                    var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();
                    if (data.PackagePrice == null)
                    {
                        price = Convert.ToInt16(data.price1);
                    }
                    else { price = Convert.ToInt16(data.PackagePrice); }
                    tprice = tprice + price;
                    if (type == "Photography" || type == "Decorator" || type == "Other")
                    {
                        totalprice = Convert.ToString(price);
                        guest = "0";
                    }
                    else
                    {
                        totalprice = Convert.ToString(price * Convert.ToInt16(guest));
                    }
                    for (int j = 0; j < date1.Count(); j++)
                    {
                        if (date1[j].Split('~')[1] == data.VendorSubId.ToString())
                        {
                            data.UpdatedDate = Convert.ToDateTime(date1[j].Split('~')[0]);
                            data.timeslot = timeslot1[j].Split('~')[0];
                        }
                    }
                    //data.UpdatedDate = Convert.ToDateTime(date1[i].Split('~')[0]);
                    //data.timeslot = timeslot1[i].Split('~')[0];
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.OrderBy = userid;
                    orderDetail.PaymentId = '1';
                    orderDetail.ServiceType = type;
                    orderDetail.ServicePrice = Convert.ToDecimal(price);
                    orderDetail.attribute = data.timeslot;
                    orderDetail.TotalPrice = Convert.ToDecimal(totalprice);
                    orderDetail.PerunitPrice = Convert.ToDecimal(price);
                    orderDetail.Quantity = Convert.ToInt32(guest);
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.VendorId = long.Parse(vid);
                    orderDetail.Status = "Pending";
                    orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
                    orderDetail.UpdatedBy = long.Parse(vid);
                    orderDetail.subid = data.VendorSubId;
                    orderDetail.BookedDate = Convert.ToDateTime(data.UpdatedDate);
                    ViewBag.orderdate = orderDetail.BookedDate;
                    orderDetail.EventType = etype1;
                    orderDetail.DealId = long.Parse(pkgs[i]);


                    newmanageuse.SaveOrderDetail(orderDetail);
                }
                OrderDetail orderDetail1 = new OrderDetail();
                var userlogdetails = mnguserservice.getuserbyid(userid);


                string txtto = userlogdetails.email;

                string name = userlogdetails.firstname;
                name = home.Capitalise(name);
                string OrderId = Convert.ToString(order.OrderId);
                StringBuilder cds = new StringBuilder();
                cds.Append("<table style='border:1px black solid;'><tbody>");
                cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td> Quantity</td><td>Perunit Price</td><td>Total Price</td></tr>");

                cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderDetail1.BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + orderDetail1.EventType+ " </td><td style = 'width: 50px;border: 1px black solid;'> " +orderDetail1.Quantity + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + orderDetail1.PerunitPrice + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderDetail1.TotalPrice + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>

                cds.Append("</tbody></table>");
                string url = Request.Url.Scheme + "://" + Request.Url.Authority;
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", name);
                readFile = readFile.Replace("[orderid]", OrderId);
                readFile = readFile.Replace("[table]", cds.ToString());
                string txtmessage = readFile;//readFile + body;
                string subj = "Thanks for your order";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj, null);
                //emailSendingUtility.Email_maaaahwanam("seema@xsilica.com ", txtmessage, subj);
                string targetmails = "lakshmi.p@xsilica.com,seema.g@xsilica.com,rameshsai@xsilica.com";
                emailSendingUtility.Email_maaaahwanam(targetmails, txtmessage, subj, null);

                var vendordetails = newmanageuse.getvendor(Convert.ToInt32(vid));

                string txtto1 = vendordetails.EmailId;
                string vname = vendordetails.BusinessName;
                vname = home.Capitalise(vname);
                StringBuilder cds2 = new StringBuilder();
                cds2.Append("<table style='border:1px black solid;'><tbody>");
                cds2.Append("<tr><td>Order Id</td><td>Order Date</td><td>Customer Name</td><td>Customer Phone Number</td><td>flatno</td><td>Locality</td></tr>");
                cds2.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderDate+ "</td><td style = 'width: 75px;border: 1px black solid;'> " + userlogdetails.firstname + " " + userlogdetails.lastname + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.phoneno + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress1 + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress2 + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
                cds2.Append("</tbody></table>");
                string url1 = Request.Url.Scheme + "://" + Request.Url.Authority;
                FileInfo file1 = new FileInfo(Server.MapPath("/mailtemplate/vorder.html"));
                string readfile1 = file1.OpenText().ReadToEnd();
                readfile1 = readfile1.Replace("[ActivationLink]", url1);
                readfile1 = readfile1.Replace("[name]", name);
                readfile1 = readfile1.Replace("[vname]", vname);
                readfile1 = readfile1.Replace("[msg]", cds2.ToString());
                readfile1 = readfile1.Replace("[orderid]", OrderId);
                string txtmessage1 = readfile1;
                string subj1 = "order has been placed";
                emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1, null);
                msg = OrderId;
            }
            else
            {
                msg = "please active the user";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


       

        public ActionResult orderdetails(string select, string packageid, string date, string timeslot)
        {
            if (select != null && select != "null" && select != "")
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    string uid = user.UserId.ToString();
                    string vemail = newmanageuse.Getusername(long.Parse(uid));
                    vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                   var VendorId = vendorMaster.Id.ToString();

                    List<string> sdate = new List<string>();
                    List<string> stimeslot = new List<string>();
                    List<SPGETpartpkg_Result> package = new List<SPGETpartpkg_Result>();

                    var select1 = select.Split(',');
                    ViewBag.resellername = partnerservice.GetPartners(Convert.ToString(VendorId));
                    ViewBag.loc = select1[0];
                    var guest = ViewBag.guests = select1[1];
                    ViewBag.eventtype = select1[2];
                    var pkgs = packageid.Split(',');
                    var date1 = date.Trim(',').Split(',');
                    var timeslot1 = timeslot.Split(',');
                    for (int i = 0; i < pkgs.Count(); i++)
                    {

                        var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();
                        int price;
                        if (data.PackagePrice == null)
                        { price = Convert.ToInt16(data.price1); }
                        else { price = Convert.ToInt16(data.PackagePrice); }
                        tprice = tprice + price;
                        for (int j = 0; j < date1.Count(); j++)
                        {
                            if (date1[j].Split('~')[1] == data.VendorSubId.ToString())
                            {
                                data.UpdatedDate = Convert.ToDateTime(date1[j].Split('~')[0]);
                                data.timeslot = timeslot1[j].Split('~')[0];
                            }
                            else {
                              //  data.UpdatedDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                                data.timeslot = "";

                            }
                        }
                        package.Add(data);
                    }
                    var tot = tprice * Convert.ToInt32(guest);
                    var gtot = tot + (tot * 0.18);
                    ViewBag.package = package;
                    ViewBag.tprice = tot;
                    ViewBag.gtot = gtot;
                   
                }
            }
            return View("orderdetails");
        }

        public JsonResult GetParticularPackage(string pid)
        {
            if (pid != null && pid != "")
            {
                SPGETpartpkg_Result package = newmanageuse.getpartpkgs(pid).FirstOrDefault();
                return Json(package, JsonRequestBehavior.AllowGet);
            }
            return Json("Failed!!!");
        }
    }
}