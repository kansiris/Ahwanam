using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;
using System.Configuration;
using MaaAahwanam.Web.Models;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Facebook;
using System.Web.Security;
using Newtonsoft.Json;
using MaaAahwanam.Utility;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Portal;
using DotNetOpenAuth.GoogleOAuth2;
using Microsoft.AspNet.Membership.OpenAuth;
using System.Collections.Specialized;

namespace MaaAahwanam.Web.Controllers
{
    public class NUserRegistrationController : Controller
    {

        static string perfecturl = "";

        string activationcode = "";
        string txtto = "";
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        Vendormaster vendorMaster = new Vendormaster();
        VendorMasterService vendorMasterService = new VendorMasterService();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        // GET: NUserRegistration

        public NUserRegistrationController()
        {
        }

        public NUserRegistrationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated == false)
            {
                if (TempData["Active"] != "")
                {
                    ViewBag.Active = TempData["Active"];
                }
                perfecturl = "";
              
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(string command, [Bind(Prefix = "Item1")] UserLogin userLogin, [Bind(Prefix = "Item2")] UserDetail userDetail, string ReturnUrl)
        {
            try
            {
                if (command == "UserReg")
                {
                    userLogin.IPAddress = HttpContext.Request.UserHostAddress;
                    userLogin.ActivationCode = Guid.NewGuid().ToString();
                    userLogin.Status = "InActive";
                    var response = "";
                    userLogin.UserType = "User";
                    long data = userLoginDetailsService.GetLoginDetailsByEmail(userLogin.UserName);
                    if (data == 0)
                    { response = userLoginDetailsService.AddUserDetails(userLogin, userDetail); }
                    else
                    {
                        TempData["Active"] = "E-Mail ID Already Registered!!! Try Logging with your Password";
                        return RedirectToAction("Index", "NUserRegistration");
                    }
                    if (response == "sucess")
                    {
                        activationcode = userLogin.ActivationCode;
                        txtto = userLogin.UserName;
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
                        TempData["Active"] = "Check your email to active your account to login";
                        return RedirectToAction("Index", "NUserRegistration");
                    }
                    else
                    {
                        TempData["Active"] = "Registration Failed";
                        return RedirectToAction("Index", "NUserRegistration");
                    }
                }
                if (command == "Login")
                {
                    var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
                    var userResponse1 = venorVenueSignUpService.GetUserLogdetails(userLogin);

                    if (userResponse != null)
                    {
                        if (userResponse1.Status == "Active")
                        {
                            vendorMaster = vendorMasterService.GetVendorByEmail(userLogin.UserName);
                            string userData = JsonConvert.SerializeObject(userResponse);
                            ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                            if (perfecturl != null && perfecturl != "")
                                return Redirect(perfecturl);
                            if (userResponse.UserType == "Vendor")
                            {
                                var vnid = userResponse.UserLoginId;

                                string vssid = Convert.ToString(vendorMaster.Id);
                                encptdecpt encript = new encptdecpt();

                                string encripted = encript.Encrypt(string.Format("Name={0}", vssid));

                                return RedirectToAction("Index", "NVendorDashboard", new { ks = encripted });
                            }
                            else
                                ViewBag.userid = userResponse.UserLoginId;
                            return RedirectToAction("Index", "NHomePage");
                        }
                        TempData["Active"] = "Please check Your email to verify Email ID";
                        return RedirectToAction("Index", "NUserRegistration");
                    }
                    else
                    {
                        //int query = vendorMasterService.checkemail(userLogin.UserName);
                        int userlogintablecheck = (int)userResponse1.UserLoginId;
                        if (userlogintablecheck == 0)
                            TempData["Active"] = "User Record Not Available"; //return Content("<script language='javascript' type='text/javascript'>alert('User Record Not Available');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                        else
                            TempData["Active"] = "Wrong Credentials,Check Username and password"; //return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                        return RedirectToAction("Index", "NUserRegistration");
                    }

                }
                if (command == "forgotpassword")
                {
                    var userResponse = venorVenueSignUpService.GetUserLogdetails(userLogin);
                    if (userResponse != null)
                    {
                        string emailid = userLogin.UserName;

                        activationcode = userResponse.ActivationCode;
                        int id = Convert.ToInt32(userResponse.UserLoginId);
                        var userdetails = userLoginDetailsService.GetUser(id);
                        string name = userdetails.FirstName;

                        name = Capitalise(name);
                        txtto = userLogin.UserName;
                        string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/NUserRegistration/ActivateEmail?ActivationCode=" + activationcode + "&&Email=" + emailid;
                        FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/mailer.html"));
                        string readFile = File.OpenText().ReadToEnd();
                        readFile = readFile.Replace("[ActivationLink]", url);
                        readFile = readFile.Replace("[name]", name);
                        string txtmessage = readFile;//readFile + body;
                        string subj = "Password reset information";


                        // vendor mail activation  begin
                        //txtto = userLogin.UserName;
                        //string mailid = userLogin.UserName;
                        //var userR = venorVenueSignUpService.GetUserdetails(mailid);
                        //string pas1 = userR.Password;
                        //string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/NUserRegistration/ActivateEmail1?ActivationCode=" + activationcode + "&&Email=" + emailid;
                        //FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/WelcomeMessage.html"));
                        //string readFile = File.OpenText().ReadToEnd();
                        //readFile = readFile.Replace("[ActivationLink]", url);
                        //readFile = readFile.Replace("[name]", name);
                        //readFile = readFile.Replace("[username]", mailid);
                        //readFile = readFile.Replace("[pass1]", pas1);
                        //string txtmessage = readFile;//readFile + body;
                        //string subj = "Welcome to Ahwanam";

                        // vendor mail activation  end

                        EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                        emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                        TempData["Active"] = "A mail is sent to your email to change password Please check your email";
                        return RedirectToAction("Index", "NUserRegistration");
                    }
                    else
                    {
                        int query = vendorMasterService.checkemail(userLogin.UserName);
                        if (query == 0)
                            TempData["Active"] = "User Record Not Available";//return Content("<script language='javascript' type='text/javascript'>alert('User Record Not Available');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                        else
                            TempData["Active"] = "Wrong Credentials,Check Username and password";//return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                        return RedirectToAction("Index", "NUserRegistration");
                    }

                }

                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }


        public ActionResult LoginBlocks(string email, string password, string typeid)
        {
            //if (userLogin != null)
            //    email = userLogin.UserName;
            if (password != null)
            {
                var userlogin = userLoginDetailsService.GetUserId(int.Parse(typeid));
                if (userlogin.Password.ToLower() == password || userlogin.Password == password)
                {
                    vendorMaster = vendorMasterService.GetVendorByEmail(email);
                    string userData = JsonConvert.SerializeObject(userlogin);
                    ValidUserUtility.SetAuthCookie(userData, userlogin.UserLoginId.ToString());
                    if (perfecturl != null && perfecturl != "")
                        return Redirect(perfecturl);
                    if (userlogin.UserType == "Vendor")
                    {
                        var vnid = userlogin.UserLoginId;
                        return RedirectToAction("Index", "NVendorDashboard", new { id = vendorMaster.Id });
                    }
                    else
                        ViewBag.userid = userlogin.UserLoginId;
                    return RedirectToAction("Index", "NHomePage");
                }
                else
                {
                    TempData["Active"] = "Wrong Credentials,Check Username and password";
                    return RedirectToAction("Index", "NUserRegistration");
                }
            }
            if (email != null)
            {
                var data = userLoginDetailsService.GetUserLoginTypes(email);
                ViewBag.userdata = data;
                if (data.Count() == 0)
                {
                    TempData["Active"] = "User Record Not Available";
                    return RedirectToAction("Index", "NUserRegistration");
                }
            }
            return PartialView();
        }


        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
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
                    return RedirectToAction("updatepassword", "NUserRegistration", new { Email = Email });
                }
                //return Content("<script language='javascript' type='text/javascript'>alert('email not found');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                TempData["Active"] = "Email ID not found";
                return RedirectToAction("Index", "NUserRegistration");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
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
                        TempData["Active"] = "Thanks for Verifying the Email";
                        return RedirectToAction("Index", "NUserRegistration");
                    }
                }
                else
                {
                    //return Content("<script language='javascript' type='text/javascript'>alert('Your Account is already Verified Please login');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                    TempData["Active"] = "Your Account is already Verified Please login";
                    return RedirectToAction("Index", "NUserRegistration");
                }
                //return Content("<script language='javascript' type='text/javascript'>alert('Email not found');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                TempData["Active"] = "Email ID not found";
                return RedirectToAction("Index", "NUserRegistration");
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
                txtto = userLogin.UserName;
                int id = Convert.ToInt32(userResponse.UserLoginId);
                var userdetails = userLoginDetailsService.GetUser(id);
                string username = userdetails.FirstName;
                username = Capitalise(username);
                string emailid = userLogin.UserName;
                string url = Request.Url.Scheme + "://" + Request.Url.Authority ;
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
                return RedirectToAction("Index", "Nhomepage");
            }
        }

        public JsonResult assignreturnurl(string ReturnUrl)
        {

            perfecturl = ReturnUrl;
            return Json(JsonRequestBehavior.AllowGet);
        }


        public ActionResult facebookLogin(string email, string id, string name, string gender, string firstname, string lastname, string picture, string currency, string timezone, string agerange)
        {
            try
            {
                //Write your code here to access these paramerters
                var response = "";
                UserLogin userLogin = new UserLogin();
                UserDetail userDetail = new UserDetail();
                userDetail.FirstName = name;
                userDetail.LastName = lastname;
                userDetail.UserImgName = firstname;
                userDetail.UserImgName = picture;
                userLogin.UserName = email;
                userLogin.Password = "Facebook";
                userLogin.UserType = "User";
                userLogin.Status = "Active";
                UserLogin userlogin1 = new UserLogin();

                userlogin1 = venorVenueSignUpService.GetUserLogdetails(userLogin); // checking where email id is registered or not.

                if (userlogin1 == null)
                {
                    response = userLoginDetailsService.AddUserDetails(userLogin, userDetail); // Adding user record to database
                }
                var userResponse = venorVenueSignUpService.GetUserdetails(email);

                if (userResponse.UserType == "User")
                {
                    FormsAuthentication.SetAuthCookie(email, false);
                    vendorMaster = vendorMasterService.GetVendorByEmail(userLogin.UserName);
                    string userData = JsonConvert.SerializeObject(userResponse); //creating identity
                    ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                    return Json("success");
                }
                else
                {
                    return Json("failed");
                    //  return Content("<script language='javascript' type='text/javascript'>alert('This email is registared as Vendor please login with Your Credentials');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Nhomepage");
            }
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "NUserRegistration", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }
        public ActionResult changeid(string id)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string email = userLoginDetailsService.Getusername(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendorByEmail(email);

                string vssid = Convert.ToString(vendorMaster.Id);
                encptdecpt encript = new encptdecpt();

                string encripted = encript.Encrypt(string.Format("Name={0}", vssid));

                vendorMaster = vendorMasterService.GetVendorByEmail(email);
                //return View("AvailableServices", vendorMaster.Id);
                return RedirectToAction("Index", "NVendorDashboard", new { ks = encripted });
            }
            return RedirectToAction("SignOut", "NUserRegistration");
        }
        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Response.Cookies.Clear();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "NHomePage");
        }


        public ActionResult SignOut()
        {
            Response.Cookies.Clear();

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "NHomePage");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";
        #endregion
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }


        }


    }
}