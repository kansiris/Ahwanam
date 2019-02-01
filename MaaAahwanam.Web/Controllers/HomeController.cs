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
using Microsoft.AspNet.Identity;
using System.Configuration;
using MaaAahwanam.Web.Models;
using System.Net;
using System.Web.Script.Serialization;
using Facebook;
using System.Web.Security;

using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Portal;
using DotNetOpenAuth.GoogleOAuth2;
using Microsoft.AspNet.Membership.OpenAuth;
using System.Collections.Specialized;

namespace MaaAahwanam.Web.Controllers
{
    public class HomeController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        Vendormaster vendorMaster = new Vendormaster();
        cartservices cartserve = new cartservices();
        ResultsPageService resultsPageService = new ResultsPageService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorsCatering vendorsCatering = new VendorsCatering();
        VendorsDecorator vendorsDecorator = new VendorsDecorator();
        VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
        VendorsPhotography vendorsPhotography = new VendorsPhotography();
        VendorVenue vendorVenue = new VendorVenue();
        VendorsOther vendorsOther = new VendorsOther();
        VendorCateringService vendorCateringService = new VendorCateringService();
        VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
        VendorVenueService vendorVenueService = new VendorVenueService();
        VendorOthersService vendorOthersService = new VendorOthersService();
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();

        // GET: Home
        public ActionResult Index()
        {
            var data = resultsPageService.GetAllVendors("Venue").ToList();
            Random r = new Random();
            int rInt = r.Next(0, data.Count);
            ViewBag.venues = data.Skip(rInt).Take(3).ToList();
            string auth = checkAuthentication();
            if (auth == "")
                ViewBag.cartCount = cartserve.CartItemsCount(0);
            else
                ViewBag.username = auth;
            return View();
        }

        #region Cart
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
                            ViewBag.cartCount = cartserve.CartItemsCount(0);
                            return PartialView("ItemsCartViewBindingLayout");
                        }


                        ViewBag.cartCount = cartserve.CartItemsCount1((int)user.UserId);
                        //   List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                        var cartlist = cartserve.CartItemsList1(int.Parse(user.UserId.ToString()));
                        var cartlist1 = cartserve.CartItemsList1(int.Parse(user.UserId.ToString()));

                        //  decimal total = (cartlist1.TotalPrice) .Sum(s => s.TotalPrice);
                        ViewBag.cartitems = cartlist;
                        // ViewBag.Total = total;
                        ViewBag.Total = "0";
                    }
                }
                else
                {
                    ViewBag.cartCount = cartserve.CartItemsCount(0);
                }
                return PartialView("ItemsCartViewBindingLayout");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
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
                            ViewBag.cartCount = cartserve.CartItemsCount(0);
                            return PartialView("ItemsCartdetails");
                        }
                        ViewBag.cartCount = cartserve.CartItemsCount((int)user.UserId);
                        var cartlist1 = cartserve.CartItemsList1(int.Parse(user.UserId.ToString()));

                        // List<GetCartItems_Result> cartlist = cartService.CartItemsList(int.Parse(user.UserId.ToString()));
                        decimal total = cartlist1.Select(m => m.TotalPrice).Sum();
                        ViewBag.cartitems = cartlist1;
                        ViewBag.Total = total;
                        //  ViewBag.Total = "0";
                    }
                }
                else
                {
                    ViewBag.cartCount = cartserve.CartItemsCount(0);
                }


                return PartialView("ItemsCartdetails");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }

        }
        #endregion

        #region Authentication
        public JsonResult login(string Password, string Email, string url1)
        {
            UserLogin userLogin = new UserLogin();
            userLogin.UserName = Email;
            userLogin.Password = Password;
            //string ipaddress = HttpContext.Request.UserHostAddress;
            var userResponse1 = resultsPageService.GetUserLogin(userLogin);
            if (userResponse1 != null)
            {
                if (userResponse1.Status == "Active")
                {
                    vendorMaster = resultsPageService.GetVendorByEmail(userLogin.UserName);
                    string userData = JsonConvert.SerializeObject(userResponse1);
                    ValidUserUtility.SetAuthCookie(userData, userResponse1.UserLoginId.ToString());
                    ViewBag.userid = userResponse1.UserLoginId;

                    //string txtto = "amit.saxena@ahwanam.com,rameshsai@xsilica.com,sireesh.k@xsilica.com";
                    //int id = Convert.ToInt32(userResponse.UserLoginId);
                    //var userdetails = userLoginDetailsService.GetUser(id);

                    //string username = userdetails.FirstName;
                    //username = Capitalise(username);
                    //string emailid = userLogin.UserName;
                    //FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/login.html"));
                    //string readFile = File.OpenText().ReadToEnd();
                    //readFile = readFile.Replace("[ActivationLink]", url1);
                    //readFile = readFile.Replace("[name]", username);
                    //readFile = readFile.Replace("[Ipaddress]", ipaddress);
                    //readFile = readFile.Replace("[email]", Email);

                    //string txtmessage = readFile;//readFile + body;
                    //string subj = "User login from ahwanam";
                    //EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                    //emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                //int userlogintablecheck = (int)userResponse1.UserLoginId;
                return Json("success1", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult register(string CustomerPhoneNumber, string CustomerName, string Password, string Email)
        {
            UserLogin userLogin = new UserLogin();
            UserDetail userDetail = new UserDetail();
            userLogin.IPAddress = HttpContext.Request.UserHostAddress;
            userLogin.ActivationCode = Guid.NewGuid().ToString();
            userDetail.FirstName = CustomerName;
            userDetail.UserPhone = CustomerPhoneNumber;
            userLogin.Password = Password;
            userLogin.UserName = userDetail.AlternativeEmailID = Email;
            userLogin.Status = "InActive";
            var response = "";
            userLogin.UserType = "User";
            long data = userLoginDetailsService.GetLoginDetailsByEmail(Email);
            if (data == 0)
                response = userLoginDetailsService.AddUserDetails(userLogin, userDetail);
            else
                return Json("unique", JsonRequestBehavior.AllowGet);
            if (response == "sucess")
            {
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/home/ActivateEmail1?ActivationCode=" + userLogin.ActivationCode + "&&Email=" + userLogin.UserName;
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/welcome.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", Capitalise(userDetail.FirstName));
                readFile = readFile.Replace("[phoneno]", userDetail.UserPhone);
                TriggerEmail(userLogin.UserName, readFile, "Account Activation", null); // A Mail will be triggered
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
                return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Authentication(string type, [Bind(Prefix = "Item1")]UserLogin userLogin, [Bind(Prefix = "Item2")]UserDetail userDetail)
        {
            if (type == "login")
            {
                var userResponse1 = resultsPageService.GetUserLogin(userLogin);
                if (userResponse1 != null)
                {
                    if (userResponse1.Status == "Active")
                    {
                        vendorMaster = resultsPageService.GetVendorByEmail(userLogin.UserName);
                        string userData = JsonConvert.SerializeObject(userResponse1);
                        ValidUserUtility.SetAuthCookie(userData, userResponse1.UserLoginId.ToString());
                        ViewBag.userid = userResponse1.UserLoginId;
                        if (userResponse1.UserType == "Vendor")
                            return RedirectToAction("Index", "vdb");
                        else if (userResponse1.UserType == "Admin")
                            return RedirectToAction("Login", "Admin");
                        else if(userResponse1.UserType == "User")
                            return RedirectToAction("Index", "Home");
                    }
                    
                }
                return Content("<script>alert('Wrong Credentials,Check Username and password');location.href='/home';</script>");
            }
            else if (type == "register")
            {
                userLogin.IPAddress = HttpContext.Request.UserHostAddress;
                userLogin.Status = "InActive";
                var response = "";
                userLogin.UserType = "User";
                long data = userLoginDetailsService.GetLoginDetailsByEmail(userLogin.UserName);
                if (data == 0)
                    response = userLoginDetailsService.AddUserDetails(userLogin, userDetail);
                else
                    return Content("<script>alert('E-Mail ID Already Registered!!! Try Logging with your Password');location.href='/home';</script>");
                if (response == "sucess")
                {
                    string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/home/ActivateEmail1?ActivationCode=" + userLogin.ActivationCode + "&&Email=" + userLogin.UserName;
                    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/welcome.html"));
                    string readFile = File.OpenText().ReadToEnd();
                    readFile = readFile.Replace("[ActivationLink]", url);
                    readFile = readFile.Replace("[name]", Capitalise(userDetail.FirstName));
                    readFile = readFile.Replace("[phoneno]", userDetail.UserPhone);
                    TriggerEmail(userLogin.UserName, readFile, "Account Activation", null); // A Mail will be triggered
                    return Content("<script>alert('Check your email to active your account to login');location.href='/home';</script>");
                }
                else
                return Content("<script>alert('Registration Failed');location.href='/home';</script>");
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Email
        public ActionResult SendEmail(string name, string number, string city, string eventtype, string datepicker2, string Description)
        {
            string msg = "Name: " + name + ", Mobile Number : " + number + ",City : " + city + ",Event Type:" + eventtype + ",Event date:" + datepicker2 + ",Description:" + Description + ",IP:" + HttpContext.Request.UserHostAddress;
            string txtto = "rameshsai@xsilica.com,seema@xsilica.com,amit.saxena@ahwanam.com";
            TriggerEmail(txtto, msg.Replace(",", "<br/>"), "Request From Ahwanam Personalized assistance", null); // A mail will be triggered
            return Content("<script language='javascript' type='text/javascript'>alert('Details Sent Successfully!!!Click OK and Explore Ahwanam.com');location.href='" + @Url.Action("Index", "Home") + "'</script>");
        }

        public ActionResult ActivateEmail(string ActivationCode, string Email)
        {
            try
            {
                if (ActivationCode == "")
                { ActivationCode = null; }
                var userResponse = venorVenueSignUpService.GetUserdetails(Email);
                if (ActivationCode == userResponse.ActivationCode)
                {
                    return RedirectToAction("updatepassword", "Home", new { Email = Email });
                }
                //return Content("<script language='javascript' type='text/javascript'>alert('email not found');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                TempData["Active"] = "Email ID not found";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ActivateEmail1(string ActivationCode, string Email)
        {
            try
            {
                UserLogin userLogin = new UserLogin();
                UserDetail userDetails = new UserDetail();
                if (ActivationCode == "")
                { ActivationCode = null; }
                var userResponse = venorVenueSignUpService.GetUserdetails(Email);
                if (userResponse.Status != "Active")
                {
                    if (ActivationCode == userResponse.ActivationCode)
                    {
                        userLogin.Status = "Active";
                        userDetails.Status = "Active";
                        string email = userLogin.UserName;
                        var userid = userResponse.UserLoginId;
                        userLoginDetailsService.changestatus(userLogin, userDetails, (int)userid);
                        if (userResponse.UserType == "Vendor")
                        {
                            vendorMaster = vendorMasterService.GetVendorByEmail(email);
                            string vid = vendorMaster.Id.ToString();
                            if (vendorMaster.ServicType == "Catering")
                            {
                                var catering = vendorVenueSignUpService.GetVendorCatering(long.Parse(vid)).FirstOrDefault();
                                vendorsCatering.Status = vendorMaster.Status = "Active";
                                vendorsCatering = vendorCateringService.activeCatering(vendorsCatering, vendorMaster, long.Parse(catering.Id.ToString()), long.Parse(vid));
                            }
                            else if (vendorMaster.ServicType == "Decorator")
                            {
                                var decorators = vendorVenueSignUpService.GetVendorDecorator(long.Parse(vid)).FirstOrDefault();
                                vendorsDecorator.Status = vendorMaster.Status = "Active";
                                vendorsDecorator = vendorDecoratorService.activeDecorator(vendorsDecorator, vendorMaster, long.Parse(decorators.Id.ToString()), long.Parse(vid));
                            }
                            else if (vendorMaster.ServicType == "Photography")
                            {
                                var photography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(vid)).FirstOrDefault();
                                vendorsPhotography.Status = vendorMaster.Status = "Active";
                                vendorsPhotography = vendorPhotographyService.ActivePhotography(vendorsPhotography, vendorMaster, long.Parse(photography.Id.ToString()), long.Parse(vid));
                            }
                            else if (vendorMaster.ServicType == "Venue")
                            {
                                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(vid)).FirstOrDefault();
                                vendorVenue.Status = vendorMaster.Status = "Active";
                                vendorVenue = vendorVenueService.activeVenue(vendorVenue, vendorMaster, long.Parse(venues.Id.ToString()), long.Parse(vid));
                            }
                            else if (vendorMaster.ServicType == "Other")
                            {
                                var others = vendorVenueSignUpService.GetVendorOther(long.Parse(vid)).FirstOrDefault();
                                vendorsOther.Status = vendorMaster.Status = "Active";
                                vendorsOther = vendorOthersService.activationOther(vendorsOther, vendorMaster, long.Parse(others.Id.ToString()), long.Parse(vid));
                            }
                        }
                        TempData["Active"] = "Thanks for Verifying the Email";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Your Account is already Verified Please login');location.href='" + @Url.Action("Index", "Home") + "'</script>");
                    //TempData["Active"] = "Your Account is already Verified Please login";
                    //return RedirectToAction("Index", "NUserRegistration");
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Email not found');location.href='" + @Url.Action("Index", "Home") + "'</script>");
                //TempData["Active"] = "Email ID not found";
                //return RedirectToAction("Index", "NUserRegistration");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public void TriggerEmail(string txtto, string txtmsg, string subject, HttpPostedFileBase attachment)
        {
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(txtto, txtmsg, subject, attachment);
        }
        #endregion

        #region References
        public string checkAuthentication()
        {
            string name = "";
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                if (user.UserType == "User")
                {
                    var userdata = userLoginDetailsService.GetUser((int)user.UserId);
                    if (userdata.FirstName != "" && userdata.FirstName != null)
                        name = userdata.FirstName;
                    else if (userdata.FirstName != "" && userdata.FirstName != null && userdata.LastName != "" && userdata.LastName != null)
                        name = "" + userdata.FirstName + " " + userdata.LastName + "";
                    else
                        name = userLoginDetailsService.GetUserId((int)user.UserId).UserName; //userdata.AlternativeEmailID;
                    //if (user.UserType == "Admin")
                    //{
                    //    ViewBag.cartCount = cartserve.CartItemsCount(0);
                    //    return PartialView("ItemsCartViewBindingLayout");
                    //}
                    var cart = cartserve.CartItemsList((int)user.UserId).Where(m => m.Status == "Active");
                    ViewBag.cartCount = cart.Count();
                    ViewBag.cartitems = cart;
                    ViewBag.Total = "0";
                }
            }
            return name;
        }

        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
        #endregion

        #region Password
        public ActionResult updatepassword(string Email)
        {
            try
            {
                ViewBag.email = Email;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult changepassword(UserLogin userLogin)
        {
            try
            {
                var userResponse = venorVenueSignUpService.GetUserdetails(userLogin.UserName);
                userLoginDetailsService.changepassword(userLogin, (int)userResponse.UserLoginId);
                var userdetails = userLoginDetailsService.GetUser(int.Parse(userResponse.UserLoginId.ToString()));
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/home";
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/change-email.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", Capitalise(userdetails.FirstName));
                TriggerEmail(userLogin.UserName, readFile, "Your Password is changed", null); // A mail will be triggered
                return Json("success");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public JsonResult forgotpass(string Email)
        {
            UserLogin userLogin = new UserLogin();
            userLogin.UserName = Email;
            var userResponse = venorVenueSignUpService.GetUserLogdetails(userLogin);
            if (userResponse != null)
            {
                var userdetails = userLoginDetailsService.GetUser(int.Parse(userResponse.UserLoginId.ToString()));
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/ActivateEmail?ActivationCode=" + userResponse.ActivationCode + "&&Email=" + Email;
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/mailer.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", Capitalise(userdetails.FirstName));
                TriggerEmail(Email, readFile, "Password reset information", null); // A mail will be triggered
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            return Json("success1", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Signout
        public ActionResult SignOut()
        {
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}