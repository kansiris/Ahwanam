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
                        ViewBag.username = userLoginDetailsService.GetUserId((int)user.UserId).UserName; //userdata.AlternativeEmailID;
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
                string phoneno = userDetail.UserPhone;

                username = Capitalise(username);
                string emailid = userLogin.UserName;
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/home/ActivateEmail1?ActivationCode=" + activationcode + "&&Email=" + emailid;
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/welcome.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", username);
                readFile = readFile.Replace("[phoneno]", phoneno);

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
                            var vsid = "";
                            if (vendorMaster.ServicType == "Catering")
                            {
                                var catering = vendorVenueSignUpService.GetVendorCatering(long.Parse(vid)).FirstOrDefault();
                                vsid = catering.Id.ToString();
                                vendorsCatering.Status = vendorMaster.Status = "Active";
                                vendorsCatering = vendorCateringService.activeCatering(vendorsCatering, vendorMaster, long.Parse(vsid), long.Parse(vid));
                            }
                            if (vendorMaster.ServicType == "Decorator")
                            {
                                var decorators = vendorVenueSignUpService.GetVendorDecorator(long.Parse(vid)).FirstOrDefault();
                                vsid = decorators.Id.ToString();
                                vendorsDecorator.Status = vendorMaster.Status = "Active";
                                vendorsDecorator = vendorDecoratorService.activeDecorator(vendorsDecorator, vendorMaster, long.Parse(vsid), long.Parse(vid));
                            }
                            if (vendorMaster.ServicType == "Photography")
                            {
                                var photography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(vid)).FirstOrDefault();
                                vsid = photography.Id.ToString();
                                vendorsPhotography.Status = vendorMaster.Status = "Active";
                                vendorsPhotography = vendorPhotographyService.ActivePhotography(vendorsPhotography, vendorMaster, long.Parse(vsid), long.Parse(vid));
                            }
                            if (vendorMaster.ServicType == "Venue")
                            {
                                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(vid)).FirstOrDefault();
                                vsid = venues.Id.ToString();
                                vendorVenue.Status = vendorMaster.Status = "Active";
                                vendorVenue = vendorVenueService.activeVenue(vendorVenue, vendorMaster, long.Parse(vsid), long.Parse(vid));
                            }
                            if (vendorMaster.ServicType == "Other")
                            {
                                var others = vendorVenueSignUpService.GetVendorOther(long.Parse(vid)).FirstOrDefault();
                                vsid = others.Id.ToString();
                                vendorsOther.Status = vendorMaster.Status = "Active";
                                vendorsOther = vendorOthersService.activationOther(vendorsOther, vendorMaster, long.Parse(vsid), long.Parse(vid));
                            }
                        }

                        TempData["Active"] = "Thanks for Verifying the Email";
                        return RedirectToAction("Index", "NUserRegistration");
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
                return RedirectToAction("Index", "home");
            }
        }
        public JsonResult login(string Password, string Email,string url1)
        {
            
            UserLogin userLogin = new UserLogin();
            UserDetail userDetail = new UserDetail();
            userLogin.UserName = Email;
            userLogin.Password = Password;
            string ipaddress = HttpContext.Request.UserHostAddress;
            var userResponse1 = resultsPageService.GetUserLogin(userLogin);
            var userResponse = resultsPageService.GetUserLogdetails(userLogin);
            if (userResponse1 != null)
            {
                if (userResponse1.Status == "Active")
                {
                    vendorMaster = resultsPageService.GetVendorByEmail(userLogin.UserName);
                    string userData = JsonConvert.SerializeObject(userResponse1);
                    ValidUserUtility.SetAuthCookie(userData, userResponse1.UserLoginId.ToString());
                        ViewBag.userid = userResponse1.UserLoginId;

                    string txtto = "amit.saxena@ahwanam.com,rameshsai@xsilica.com,sireesh.k@xsilica.com";
                    int id = Convert.ToInt32(userResponse.UserLoginId);
                    var userdetails = userLoginDetailsService.GetUser(id);
                    
                    string username = userdetails.FirstName;
                    username = Capitalise(username);
                    string emailid = userLogin.UserName;
                    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/login.html"));
                    string readFile = File.OpenText().ReadToEnd();
                    readFile = readFile.Replace("[ActivationLink]", url1);
                    readFile = readFile.Replace("[name]", username);
                    readFile = readFile.Replace("[Ipaddress]", ipaddress);
                    readFile = readFile.Replace("[email]", Email);

                    string txtmessage = readFile;//readFile + body;
                    string subj = "User login from ahwanam";
                    EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            else
            {
                    int userlogintablecheck = (int)userResponse1.UserLoginId;
                    return Json("success1", JsonRequestBehavior.AllowGet);

                }
            }

        public JsonResult forgotpass(string Email)
        {
            UserLogin userLogin = new UserLogin();
            UserDetail userDetail = new UserDetail();
            userLogin.UserName = Email;
            var userResponse = venorVenueSignUpService.GetUserLogdetails(userLogin);
            if (userResponse != null)
            {
                string emailid = userLogin.UserName;

                string activationcode = userResponse.ActivationCode;
                int id = Convert.ToInt32(userResponse.UserLoginId);
                var userdetails = userLoginDetailsService.GetUser(id);
                string name = userdetails.FirstName;

                name = Capitalise(name);
                string txtto = userLogin.UserName;
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/ActivateEmail?ActivationCode=" + activationcode + "&&Email=" + emailid;
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/mailer.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", name);
                string txtmessage = readFile;//readFile + body;
                string subj = "Password reset information";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                return Json("success", JsonRequestBehavior.AllowGet);

            }
            return Json("success1", JsonRequestBehavior.AllowGet);

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

        public ActionResult SignOut()
        {
            Response.Cookies.Clear();

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
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

        public ActionResult updatepassword(string Email)
        {
            try
            {
                ViewBag.email = Email;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }
        public ActionResult changepassword(UserLogin userLogin)
        {
            try
            {
                string email = userLogin.UserName;
                var userResponse = venorVenueSignUpService.GetUserdetails(email);
                var userid = userResponse.UserLoginId;
                userLoginDetailsService.changepassword(userLogin, (int)userid);
                string txtto = userLogin.UserName;
                int id = Convert.ToInt32(userResponse.UserLoginId);
                var userdetails = userLoginDetailsService.GetUser(id);
                string username = userdetails.FirstName;
                username = Capitalise(username);
                string emailid = userLogin.UserName;
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/home";
                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/change-email.html"));
                string readFile = File.OpenText().ReadToEnd();
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", username);
                string txtmessage = readFile;//readFile + body;
                string subj = "Your Password is changed";
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                return Json("success");
                // return Content("<script language='javascript' type='text/javascript'>alert('Password Updated Successfully');location.href='" + @Url.Action("Index", "ChangePassword") + "'</script>");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "home");
            }
        }


        public ActionResult SendEmail(string name, string number, string city, string eventtype, string datepicker2,string Description)
        {
            string ip = HttpContext.Request.UserHostAddress;
            string msg = "Name: " + name + ", Mobile Number : " + number + ",City : " + city + ",Event Type:" + eventtype + ",Event date:" + datepicker2 + ",Description:"+Description+",IP:" + ip;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam("rameshsai@xsilica.com", msg.Replace(",", "<br/>"), "Mail From Ahwanam");
            emailSendingUtility.Email_maaaahwanam("seema@xsilica.com", msg.Replace(",", "<br/>"), "Mail From Ahwanam");
            emailSendingUtility.Email_maaaahwanam("amit.saxena@ahwanam.com", msg.Replace(",", "<br/>"), "Mail From Ahwanam");
            return Content("<script language='javascript' type='text/javascript'>alert('Details Sent Successfully!!!Click OK and Explore Ahwanam.com');location.href='" + @Url.Action("Index", "Home") + "'</script>");
        }
    }
}