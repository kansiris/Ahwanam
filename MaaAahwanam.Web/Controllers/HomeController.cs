using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Models;
using System.IO;
using Newtonsoft.Json;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Controllers
{
    public class HomeController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        Vendormaster vendorMaster = new Vendormaster();
        CartService cartService = new CartService();

        VendorMasterService vendorMasterService = new VendorMasterService();

        ResultsPageService resultsPageService = new ResultsPageService();
        // GET: Home
        public ActionResult Index()
        {
            var data = resultsPageService.GetAllVendors("Venue").ToList();
            Random r = new Random();
            int rInt = r.Next(0, data.Count);
            ViewBag.venues = data.Skip(rInt).Take(3).ToList();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;

                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    if (userdata.FirstName != "" && userdata.FirstName != null)
                        ViewBag.username = userdata.FirstName;
                    else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                        ViewBag.username = "" + userdata.FirstName + " " + userdata.LastName + "";
                    else
                        ViewBag.username = userdata.AlternativeEmailID;
                    if (user.UserType == "Admin")
                    {
                        ViewBag.cartCount = cartService.CartItemsCount(0);
                        return PartialView("ItemsCartViewBindingLayout");
                    }


                    ViewBag.cartCount = cartService.CartItemsCount1((int)user.UserId);
                    //   List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                    var cartlist = cartService.CartItemsList1(int.Parse(user.UserId.ToString()));
                    var cartlist1 = cartService.CartItemsList1(int.Parse(user.UserId.ToString()));

                    //  decimal total = (cartlist1.TotalPrice) .Sum(s => s.TotalPrice);
                    ViewBag.cartitems = cartlist;
                    // ViewBag.Total = total;
                    ViewBag.Total = "0";
                }
            }
            else
            {
                ViewBag.cartCount = cartService.CartItemsCount(0);
            }
            return View();
        }

        //[HttpPost]
        //public ActionResult Index(string command,string loc,string selectevent, string guests,string datetimepicker1)
        //{
        //    return RedirectToAction("Index", "results");
        //}

        public JsonResult register(string CustomerPhoneNumber, string CustomerName, string Password, string Email)
        {
            UserLogin userLogin = new UserLogin();
            UserDetail userDetail = new UserDetail();
            userLogin.IPAddress = HttpContext.Request.UserHostAddress;
            userLogin.ActivationCode = Guid.NewGuid().ToString();
            userDetail.FirstName = CustomerName;
            userDetail.UserPhone = CustomerPhoneNumber;
            userLogin.Password = Password;
            userLogin.UserName = Email;
            
            userLogin.Status = "InActive";
            var response = "";
            userLogin.UserType = "User";
            long data = userLoginDetailsService.GetLoginDetailsByEmail(Email);
            if (data == 0)
            { response = userLoginDetailsService.AddUserDetails(userLogin, userDetail); }
            else
            {
                return Json("unique", JsonRequestBehavior.AllowGet);

            }
            if (response == "sucess")
            {
              var  activationcode = userLogin.ActivationCode;
               var txtto = userLogin.UserName;
                string username = userDetail.FirstName;
                username = Capitalise(username);
                string emailid = userLogin.UserName;
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/NUserRegistration/ActivateEmail1?ActivationCode=" + activationcode + "&&Email=" + emailid;
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/welcome.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", username);
                string txtmessage = readFile;//readFile + body;
                string subj = "Account Activation";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                return Json("success", JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);

            }


        }
        public JsonResult login(string Password, string Email)
        {
            UserLogin userLogin = new UserLogin();
            UserDetail userDetail = new UserDetail();
            userLogin.UserName = Email;
            userLogin.Password = Password;


            var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
            var userResponse1 = venorVenueSignUpService.GetUserLogdetails(userLogin);

            if (userResponse1 != null)
            {
                if (userResponse1.Status == "Active")
                {
                    vendorMaster = vendorMasterService.GetVendorByEmail(userLogin.UserName);
                    string userData = JsonConvert.SerializeObject(userResponse1);
                    ValidUserUtility.SetAuthCookie(userData, userResponse1.UserLoginId.ToString());
                    //if (perfecturl != null && perfecturl != "")
                    //    return Redirect(perfecturl);
                    //if (userResponse.UserType == "Vendor" || userResponse == null)
                    //{
                    //    var vnid = userResponse.UserLoginId;

                    //    string vssid = Convert.ToString(vendorMaster.Id);
                    //    encptdecpt encript = new encptdecpt();

                    //    string encripted = encript.Encrypt(string.Format("Name={0}", vssid));

                    //    return Json("unique", JsonRequestBehavior.AllowGet);
                    //}
                    //else
                        ViewBag.userid = userResponse1.UserLoginId;
                    return Json("success", JsonRequestBehavior.AllowGet);

                }
                return Json("Failed", JsonRequestBehavior.AllowGet);

            //    TempData["Active"] = "Please check Your email to verify Email ID";
            //    return RedirectToAction("Index", "NUserRegistration");
            }
            else
            {
                    //int query = vendorMasterService.checkemail(userLogin.UserName);
                    int userlogintablecheck = (int)userResponse1.UserLoginId;
                    //if (userlogintablecheck == 0)
                    //    TempData["Active"] = "User Record Not Available"; //return Content("<script language='javascript' type='text/javascript'>alert('User Record Not Available');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                    //else
                    //    TempData["Active"] = "Wrong Credentials,Check Username and password"; //return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                    //return RedirectToAction("Index", "NUserRegistration");
                    return Json("success1", JsonRequestBehavior.AllowGet);

                }
            }
        

        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
        public ActionResult ItemsCartViewBindingLayout()
        {
            try
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;

                    if (user.UserType == "User")
                    {
                        var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                        if (userdata.FirstName != "" && userdata.FirstName != null)
                            ViewBag.username = userdata.FirstName;
                        else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                            ViewBag.username = "" + userdata.FirstName + " " + userdata.LastName + "";
                        else
                            ViewBag.username = userdata.AlternativeEmailID;
                        if (user.UserType == "Admin")
                        {
                            ViewBag.cartCount = cartService.CartItemsCount(0);
                            return PartialView("ItemsCartViewBindingLayout");
                        }


                        ViewBag.cartCount = cartService.CartItemsCount1((int)user.UserId);
                        //   List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                        var cartlist = cartService.CartItemsList1(int.Parse(user.UserId.ToString()));
                        var cartlist1 = cartService.CartItemsList1(int.Parse(user.UserId.ToString()));

                        //  decimal total = (cartlist1.TotalPrice) .Sum(s => s.TotalPrice);
                        ViewBag.cartitems = cartlist;
                        // ViewBag.Total = total;
                        ViewBag.Total = "0";
                    }
                }
                else
                {
                    ViewBag.cartCount = cartService.CartItemsCount(0);
                }
                return PartialView("ItemsCartViewBindingLayout");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }


        public ActionResult ItemsCartdetails()
        {
            try
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    if (user.UserType == "User")
                    {
                        var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                        if (user.UserType == "Admin")
                        {
                            ViewBag.cartCount = cartService.CartItemsCount(0);
                            return PartialView("ItemsCartdetails");
                        }
                        ViewBag.cartCount = cartService.CartItemsCount((int)user.UserId);
                        var cartlist1 = cartService.CartItemsList1(int.Parse(user.UserId.ToString()));

                        // List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                        decimal total = cartlist1.Select(m => m.TotalPrice).Sum();
                        ViewBag.cartitems = cartlist1;
                        ViewBag.Total = total;
                        //  ViewBag.Total = "0";
                    }
                }
                else
                {
                    ViewBag.cartCount = cartService.CartItemsCount(0);
                }


                return PartialView("ItemsCartdetails");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public ActionResult SendEmail(string name, string number, string city, string eventtype, string datepicker2)
        {
            string ip = HttpContext.Request.UserHostAddress;
            string msg = "Name: " + name + ", Mobile Number : " + number + ",City : " + city + ",Event Type:" + eventtype + ",Event date:" + datepicker2 + ",IP:" + ip;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam("rameshsai@xsilica.com", msg.Replace(",", "<br/>"), "Mail From Ahwanam");
            emailSendingUtility.Email_maaaahwanam("seema@xsilica.com", msg.Replace(",", "<br/>"), "Mail From Ahwanam");
            emailSendingUtility.Email_maaaahwanam("amit.saxena@ahwanam.com", msg.Replace(",", "<br/>"), "Mail From Ahwanam");
            return Content("<script language='javascript' type='text/javascript'>alert('Details Sent Successfully!!!Click OK and Explore Ahwanam.com');location.href='" + @Url.Action("Index", "Home") + "'</script>");
        }
    }
}