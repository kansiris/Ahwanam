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
            return View();
        }
        [HttpPost]
        public ActionResult Index(ManageUser mnguser, string id, string command)
        {
            //if (mnguser.type == "Corporate" && mnguser.Businessname != null || mnguser.type == "Individual")
            //{
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
            //  }
            // else { return Content("<script language='javascript' type='text/javascript'>alert('please enter businessname');location.href='/ManageUser'</script>"); }
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
        //[HttpPost]
        //public JsonResult UpdateUserDetails(ManageUser mnguser, string id)
        //{
        //    mnguser.updateddate = DateTime.Now;
        //    mnguser = mnguserservice.UpdateUser(mnguser, int.Parse(id));
        //    return Json("Sucess", JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public JsonResult booknow(string uid, string loc, string eventtype, string count, string date, string pid, string vid, string selectedp, string timeslot)
        //{
        //    int userid = Convert.ToInt32(uid);
        //    int price;
        //    string totalprice = "";
        //    string guest = "";
        //    string type = "";
        //    string pid1 = "";
        //    string etype1 = "";
        //    // string date1 = "";
           // HomeController home = new HomeController();
            //if (loc != "")
            //{//            var select1 = select.Split(',');

            //    pid1 = pid;
            //    var userdata = newmanageuse.GetUser(userid);
            //    //Payment Section
            //    var pkgs1 = newmanageuse.getpartpkgs(pid).FirstOrDefault();
            //    type = pkgs1.VendorType;
            //    guest = count;
            //    date1 = date;
            //    if (pkgs1.PackagePrice == null)
            //    { price = Convert.ToString(pkgs1.price1); }
            //    else { price = Convert.ToString(pkgs1.PackagePrice); }
            //    string totalprice1 = (Convert.ToInt32(price) * Convert.ToInt32(count)).ToString();
            // string id = Convert.ToString(pid);
            //string did = Convert.ToString(cartdetails.DealId);
            // string timeslot = cartdetails.attribute;
            //else
            //{
            //    var select1 = selectedp.Split(',');

            //    ViewBag.loc = select1[0];
            //    var guests = select1[1];
            //    DateTime ksdate = Convert.ToDateTime(select1[2]);
            //    date1 = ksdate.ToString("dd-MM-yyyy");
            //    ViewBag.date = date1;
            //    etype1 = ViewBag.eventtype = select1[3];
            //    string pid2 = select1[4];
            //    pid1 = pid2;
            //    var pkgs2 = newmanageuse.getpartpkgs(pid2).FirstOrDefault();
            //    if (pkgs2.PackagePrice == null)
            //    { price = Convert.ToString(pkgs2.price1); }
            //    else { price = Convert.ToString(pkgs2.PackagePrice); }

            //    totalprice = (Convert.ToInt64(guests) * Convert.ToInt64(price)).ToString();
            //    guest = guests;
            //}

            //  var pkgs = newmanageuse.getpartpkgs(pid1).FirstOrDefault();


    //        //  DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
    //        //Saving Record in order Table
    //        //  OrderService orderService = new OrderService();
    //        List<string> sdate = new List<string>();
    //        List<string> stimeslot = new List<string>();
    //        List<SPGETpartpkg_Result> package = new List<SPGETpartpkg_Result>();

    //        var pkgs = pid.Split(',');
    //        var date1 = date.Trim(',').Split(',');
    //        var timeslot1 = timeslot.Split(',');
    //        for (int i = 0; i < pkgs.Count(); i++)
    //        {

    //            var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();

    //            if (data.PackagePrice == null)
    //            { price = Convert.ToInt16(data.price1); }
    //            else { price = Convert.ToInt16(data.PackagePrice); }
    //            tprice = tprice + price;
    //            data.UpdatedDate = Convert.ToDateTime(date1[i].Split('~')[0]);
    //            data.timeslot = timeslot1[i].Split('~')[0];
    //            package.Add(data);
    //        }
    //        etype1 = eventtype;
    //        if (type == "Photography" || type == "Decorator" || type == "Other")
    //        {
    //            // totalprice = Convert.ToString(price);
    //            guest = "0";
    //        }
    //        else
    //        {
    //            totalprice = tprice.ToString();
    //        }

    //    }

    //    MaaAahwanam.Models.Order order = new MaaAahwanam.Models.Order();
    //    order.TotalPrice = Convert.ToDecimal(tprice);
    //        order.OrderDate = Convert.ToDateTime(updateddate); //Convert.ToDateTime(bookeddate);
    //        order.UpdatedBy = long.Parse(vid);
    //    order.OrderedBy = long.Parse(vid);
    //    order.UpdatedDate = Convert.ToDateTime(updateddate);
    //        order.Status = "Pending";
    //        order = newmanageuse.SaveOrder(order);

    //        //Saving Order Details
    //        //  OrderdetailsServices orderdetailsServices = new OrderdetailsServices();
    //        OrderDetail orderDetail = new OrderDetail();
    //    orderDetail.OrderId = order.OrderId;
    //        orderDetail.OrderBy = long.Parse(uid);
    //    orderDetail.PaymentId = '1';
    //        // orderDetail.ServiceType = type;
    //        orderDetail.ServicePrice = decimal.Parse(price);
    //    //  orderDetail.attribute = timeslot;
    //    orderDetail.TotalPrice = decimal.Parse(totalprice);
    //    orderDetail.PerunitPrice = decimal.Parse(price);
    //    orderDetail.Quantity = int.Parse(guest);
    //    orderDetail.OrderId = order.OrderId;
    //        orderDetail.VendorId = long.Parse(vid);
    //    orderDetail.Status = "Pending";
    //        orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
    //        orderDetail.UpdatedBy = userid;
    //        orderDetail.subid = pkgs.VendorSubId;
    //        orderDetail.BookedDate = Convert.ToDateTime(date1);
    //        ViewBag.orderdate = orderDetail.BookedDate;
    //        orderDetail.EventType = etype1;
    //        orderDetail.DealId = long.Parse(pid);


    //    newmanageuse.SaveOrderDetail(orderDetail);
    //        var userlogdetails = mnguserservice.getuserbyid(userid);
    //    string txtto = userlogdetails.email;
    //    string name = userlogdetails.firstname;
    //    name = home.Capitalise(name);
    //        string OrderId = Convert.ToString(order.OrderId);
    //    StringBuilder cds = new StringBuilder();
    //    cds.Append("<table style='border:1px black solid;'><tbody>");
    //        cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td> Quantity</td><td>Perunit Price</td><td>Total Price</td></tr>");
    //        cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + orderDetail.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderDetail.BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + orderDetail.EventType + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.Quantity + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.PerunitPrice + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.TotalPrice + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>         
    //        cds.Append("</tbody></table>");
    //        string url = Request.Url.Scheme + "://" + Request.Url.Authority;
    //    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/order.html"));
    //    string readFile = File.OpenText().ReadToEnd();
    //    readFile = readFile.Replace("[ActivationLink]", url);
    //        readFile = readFile.Replace("[name]", name);
    //        readFile = readFile.Replace("[orderid]", OrderId);
    //        readFile = readFile.Replace("[table]", cds.ToString());
    //        string txtmessage = readFile;//readFile + body;
    //    string subj = "Thanks for your order";
    //    EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
    //    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
    //        //emailSendingUtility.Email_maaaahwanam("seema@xsilica.com ", txtmessage, subj);
    //        string targetmails = "lakshmi.p@xsilica.com,seema.g@xsilica.com,rameshsai@xsilica.com";
    //    emailSendingUtility.Email_maaaahwanam(targetmails, txtmessage, subj);

    //        var vendordetails = newmanageuse.getvendor(Convert.ToInt32(vid));

    //    string txtto1 = vendordetails.EmailId;
    //    string vname = vendordetails.BusinessName;
    //    vname = home.Capitalise(vname);
    //        StringBuilder cds2 = new StringBuilder();
    //    cds2.Append("<table style='border:1px black solid;'><tbody>");
    //        cds2.Append("<tr><td>Order Id</td><td>Order Date</td><td>Customer Name</td><td>Customer Phone Number</td><td>flatno</td><td>Locality</td></tr>");
    //        cds2.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + orderDetail.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderDetail.BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + userlogdetails.firstname + " " + userlogdetails.lastname + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.phoneno + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress1 + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress2 + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
    //        cds2.Append("</tbody></table>");
    //        string url1 = Request.Url.Scheme + "://" + Request.Url.Authority;
    //    FileInfo file1 = new FileInfo(Server.MapPath("/mailtemplate/vorder.html"));
    //    string readfile1 = file1.OpenText().ReadToEnd();
    //    readfile1 = readfile1.Replace("[ActivationLink]", url1);
    //        readfile1 = readfile1.Replace("[name]", name);
    //        readfile1 = readfile1.Replace("[vname]", vname);
    //        readfile1 = readfile1.Replace("[msg]", cds2.ToString());
    //        readfile1 = readfile1.Replace("[orderid]", OrderId);
    //        string txtmessage1 = readfile1;
    //    string subj1 = "order has been placed";
    //    emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1);
    //        string msg = OrderId;


    //        return Json(msg, JsonRequestBehavior.AllowGet);
    //}
    [HttpPost]
    public JsonResult booknow(string uid, string loc, string eventtype, string guest, string date, string pid, string vid, string selectedp, string timeslot)
    {
        int userid = Convert.ToInt32(uid);
        int price;
        string totalprice = "";
        string type = "";
        string etype1 = "";
        // string date1 = "";
        HomeController home = new HomeController();
       

         DateTime updateddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        //Saving Record in order Table
        //  OrderService orderService = new OrderService();
        List<string> sdate = new List<string>();
        List<string> stimeslot = new List<string>();
        List<SPGETpartpkg_Result> package = new List<SPGETpartpkg_Result>();

        var pkgs = pid.Split(',');
        var date1 = date.Trim(',').Split(',');
        var timeslot1 = timeslot.Split(',');
        //for (int i = 0; i < pkgs.Count(); i++)
        //{

        //    var data = newmanageuse.getpartpkgs(pkgs[i]).FirstOrDefault();

        //    if (data.PackagePrice == null)
        //    { price = Convert.ToInt16(data.price1); }
        //    else { price = Convert.ToInt16(data.PackagePrice); }
        //    tprice = tprice + price;
        //    data.UpdatedDate = Convert.ToDateTime(date1[i].Split('~')[0]);
        //    data.timeslot = timeslot1[i].Split('~')[0];
        //    package.Add(data);
        //}
        etype1 = eventtype;
        if (type == "Photography" || type == "Decorator" || type == "Other")
        {
            // totalprice = Convert.ToString(price);
            guest = "0";
        }
        else
        {
            totalprice = tprice.ToString();
        }

    

    Order order = new Order();
        
    order.TotalPrice = Convert.ToDecimal(tprice);
            order.OrderDate = Convert.ToDateTime(updateddate); //Convert.ToDateTime(bookeddate);
            order.UpdatedBy = long.Parse(vid);
    order.OrderedBy = long.Parse(vid);
    order.UpdatedDate = Convert.ToDateTime(updateddate);
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
                data.UpdatedDate = Convert.ToDateTime(date1[i].Split('~')[0]);
                data.timeslot = timeslot1[i].Split('~')[0];
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order.OrderId;
                orderDetail.OrderBy = long.Parse(uid);
                orderDetail.PaymentId = '1';
                 orderDetail.ServiceType = type;
              orderDetail.ServicePrice = Convert.ToDecimal(price);
                  orderDetail.attribute = data.timeslot;
                orderDetail.TotalPrice = Convert.ToDecimal(tprice);
                orderDetail.PerunitPrice = Convert.ToDecimal(price);
                orderDetail.Quantity = Convert.ToInt32(guest);
                orderDetail.OrderId = order.OrderId;
                orderDetail.VendorId = long.Parse(vid);
                orderDetail.Status = "Pending";
                orderDetail.UpdatedDate = Convert.ToDateTime(updateddate);
                orderDetail.UpdatedBy = userid;
                orderDetail.subid = data.VendorSubId;
                orderDetail.BookedDate = Convert.ToDateTime(data.UpdatedDate);
                ViewBag.orderdate = orderDetail.BookedDate;
                orderDetail.EventType = etype1;
                orderDetail.DealId = long.Parse(pkgs[i]);


                newmanageuse.SaveOrderDetail(orderDetail);
            }
            var userlogdetails = mnguserservice.getuserbyid(userid);
    string txtto = userlogdetails.email;
    string name = userlogdetails.firstname;
    name = home.Capitalise(name);
            string OrderId = Convert.ToString(order.OrderId);
    StringBuilder cds = new StringBuilder();
    cds.Append("<table style='border:1px black solid;'><tbody>");
            cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td> Quantity</td><td>Perunit Price</td><td>Total Price</td></tr>");
            cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + updateddate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + userlogdetails.firstname + " " + userlogdetails.lastname + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.phoneno + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress1 + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress2 + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
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
    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
            //emailSendingUtility.Email_maaaahwanam("seema@xsilica.com ", txtmessage, subj);
            string targetmails = "lakshmi.p@xsilica.com,seema.g@xsilica.com,rameshsai@xsilica.com";
    emailSendingUtility.Email_maaaahwanam(targetmails, txtmessage, subj);

            var vendordetails = newmanageuse.getvendor(Convert.ToInt32(vid));

    string txtto1 = vendordetails.EmailId;
    string vname = vendordetails.BusinessName;
    vname = home.Capitalise(vname);
            StringBuilder cds2 = new StringBuilder();
    cds2.Append("<table style='border:1px black solid;'><tbody>");
            cds2.Append("<tr><td>Order Id</td><td>Order Date</td><td>Customer Name</td><td>Customer Phone Number</td><td>flatno</td><td>Locality</td></tr>");
            cds2.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + order.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + updateddate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + userlogdetails.firstname + " " + userlogdetails.lastname + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.phoneno + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress1 + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress2 + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
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
    emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1);
            string msg = OrderId;


            return Json(msg, JsonRequestBehavior.AllowGet);
}



[HttpPost]
        public JsonResult booknowss(string loc, string eventtype, string count, string date, string pid, string vid, string selectedp,
            string businessname, string firstname, string lastname, string email, string phoneno, string adress1, string adress2, string city, string state,
            string country, string pincode, string Status, string ctype)
        {
            string msg;
            if (Status != "InActive")
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
                mnguser.type = ctype;
                mnguser.vendorId = vid;
                mnguser = mnguserservice.AddUser(mnguser);
                var ksc = mnguserservice.getuserbyemail(email).FirstOrDefault();
                int userid = Convert.ToInt32(ksc.id);
                HomeController home = new HomeController();

                if (loc != "")
                {
                    pid1 = pid;
                    var userdata = newmanageuse.GetUser(userid);
                    //Payment Section
                    var pkgs1 = newmanageuse.getpartpkgs(pid).FirstOrDefault();
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
                    var pkgs2 = newmanageuse.getpartpkgs(pid2).FirstOrDefault();
                    if (pkgs2.PackagePrice == null)
                    { price = Convert.ToString(pkgs2.price1); }
                    else { price = Convert.ToString(pkgs2.PackagePrice); }

                    totalprice = (Convert.ToInt64(guests) * Convert.ToInt64(price)).ToString();
                    guest = guests;
                }
                var pkgs = newmanageuse.getpartpkgs(pid1).FirstOrDefault();


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
                orderDetail.DealId = long.Parse(pid);


                orderdetailsServices.SaveOrderDetail(orderDetail);
                var userlogdetails = mnguserservice.getuserbyid(userid);


                string txtto = userlogdetails.email;

                string name = userlogdetails.firstname;
                name = home.Capitalise(name);
                string OrderId = Convert.ToString(order.OrderId);
                StringBuilder cds = new StringBuilder();
                cds.Append("<table style='border:1px black solid;'><tbody>");
                cds.Append("<tr><td>Order Id</td><td>Order Date</td><td> Event Type </td><td> Quantity</td><td>Perunit Price</td><td>Total Price</td></tr>");

                cds.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + orderDetail.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderDetail.BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + orderDetail.EventType + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.Quantity + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.PerunitPrice + " </td><td style = 'width: 50px;border: 1px black solid;'> " + orderDetail.TotalPrice + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>

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
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                //emailSendingUtility.Email_maaaahwanam("seema@xsilica.com ", txtmessage, subj);
                string targetmails = "lakshmi.p@xsilica.com,seema.g@xsilica.com,rameshsai@xsilica.com";
                emailSendingUtility.Email_maaaahwanam(targetmails, txtmessage, subj);

                var vendordetails = newmanageuse.getvendor(Convert.ToInt32(vid));

                string txtto1 = vendordetails.EmailId;
                string vname = vendordetails.BusinessName;
                vname = home.Capitalise(vname);
                StringBuilder cds2 = new StringBuilder();
                cds2.Append("<table style='border:1px black solid;'><tbody>");
                cds2.Append("<tr><td>Order Id</td><td>Order Date</td><td>Customer Name</td><td>Customer Phone Number</td><td>flatno</td><td>Locality</td></tr>");
                cds2.Append("<tr><td style = 'width: 75px;border: 1px black solid;'> " + orderDetail.OrderId + "</td><td style = 'width: 75px;border: 1px black solid;' > " + orderDetail.BookedDate + " </td><td style = 'width: 75px;border: 1px black solid;'> " + userlogdetails.firstname + " " + userlogdetails.lastname + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.phoneno + " </td> <td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress1 + " </td><td style = 'width: 50px;border: 1px black solid;'> " + userlogdetails.adress2 + " </td></tr>");  //<td style = 'width: 50px;border: 2px black solid;'> " + item.eventstartdate + " </td><td> date </td>
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
                emailSendingUtility.Email_maaaahwanam(txtto1, txtmessage1, subj1);
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
                List<string> sdate = new List<string>();
                List<string> stimeslot = new List<string>();
                List<SPGETpartpkg_Result> package = new List<SPGETpartpkg_Result>();
               
                var select1 = select.Split(',');

                ViewBag.loc = select1[0];
                ViewBag.guests = select1[1];
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
                    data.UpdatedDate = Convert.ToDateTime(date1[i].Split('~')[0]);
                    data.timeslot = timeslot1[i].Split('~')[0];
                    package.Add(data);
                }
                ViewBag.package = package;
                ViewBag.tprice = tprice;

                //DateTime date = Convert.ToDateTime(select1[2]);
                //string date1 = date.ToString("dd-MM-yyyy");
                // ViewBag.date = date1;

                // //  var pid = select1[4];
                // //    
                // //     var vsid = pkgs.VendorSubId;
                // //var vid = pkgs.VendorId;
                // ViewBag.service = "non";//vendorVenueService.GetVendorVenue(vid, vsid);
                // string price = "";
                // //if (pkgs.PackagePrice == null)
                // //{
                // //    price = Convert.ToString(pkgs.price1);
                // //}
                // //else { price = Convert.ToString(pkgs.PackagePrice); }

                //// var total = Convert.ToInt64(guests) * Convert.ToInt64(price);
                // ViewBag.guest = guests;
                // ViewBag.total = "non";// total;
                // ViewBag.price = "non"; // price;
                // ViewBag.pid = "non";//pid;
                // ViewBag.pname = "non";//pkgs.PackageName;

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