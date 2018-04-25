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
    public class UserRegistrationController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        Vendormaster vendorMaster = new Vendormaster();
        VendorMasterService vendorMasterService = new VendorMasterService();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserRegistrationController()
        {
        }

        public UserRegistrationController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: UserRegistration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string command, [Bind(Prefix = "Item1")] UserLogin userLogin, [Bind(Prefix = "Item2")] UserDetail userDetail)
        {
            if (command == "UserReg")
            {
                var response = "";
                userLogin.UserType = "User";
                long data = userLoginDetailsService.GetLoginDetailsByEmail(userLogin.UserName);
                if (data == 0)
                    response = userLoginDetailsService.AddUserDetails(userLogin, userDetail);
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('E-Mail ID Already Registered!!! Try Logging with your Password');location.href='" + @Url.Action("Index", "UserRegistration") + "'</script>");
                if (response == "sucess")
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Index", "UserRegistration") + "'</script>");
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Index", "UserRegistration") + "'</script>");
            }
            if (command == "Login")
            {
                var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
                if (userResponse != null)
                {
                    vendorMaster = vendorMasterService.GetVendorByEmail(userLogin.UserName);
                    string userData = JsonConvert.SerializeObject(userResponse);
                    ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                    //ValidUserUtility.SetAuthCookie(userData, userLogin.UserLoginId.ToString());
                    if (userResponse.UserType == "Vendor")
                         return RedirectToAction("Index", "NewVendorDashboard", new { id = vendorMaster.Id });
                       
                    
                    else
                        return RedirectToAction("Index", "HomePage");
                }
                else
                {
                    int query = vendorMasterService.checkemail(userLogin.UserName);
                    if (query == 0)
                        return Content("<script language='javascript' type='text/javascript'>alert('User Record Not Available');location.href='" + @Url.Action("Index", "UserRegistration") + "'</script>");
                    else
                        return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "UserRegistration") + "'</script>");
                }
            }
            return View();
        }

        //public ActionResult FacebookLogin()
        //{
        //    Response.Redirect("https://www.facebook.com/v2.4/dialog/oauth/?client_id=" + ConfigurationManager.AppSettings["FacebookAppId"] + "&redirect_uri=http://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/UserRegistration/FacebookAuthentication?response_type=code&state=1");
        //    return View();
        //}

        public ActionResult FacebookAuthentication()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {

                client_id = "152565978688349",
                client_secret = "e94b2cf9672b78b7ef552d2097d3c605",
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email"

            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult GoogleLogin(string email, string name, string firstname, string lastname, string Picture)
        {               //Write your code here to access these paramerters
            var response = "";

            FormsAuthentication.SetAuthCookie(email, false);
            UserLogin userLogin = new UserLogin();
            UserDetail userDetail = new UserDetail();
            userDetail.FirstName = name;
            userDetail.LastName = lastname;
            userDetail.UserImgName = firstname;
            userDetail.UserImgName = Picture;
            userLogin.UserName = email;
            userLogin.Password = "Google";
            userLogin.UserType = "User";
            UserLogin userlogin1 = new UserLogin();

            userlogin1 = venorVenueSignUpService.GetUserLogin(userLogin); // checking where email id is registered or not.

            if (userlogin1 == null)
                response = userLoginDetailsService.AddUserDetails(userLogin, userDetail); // Adding user record to database
            else
                response = "sucess";
            if (response == "sucess")
            {
                var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
                if (userResponse != null)
                {
                    vendorMaster = vendorMasterService.GetVendorByEmail(userLogin.UserName);
                    string userData = JsonConvert.SerializeObject(userResponse); //creating identity
                    ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                    return RedirectToAction("Index", "HomePage");
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Authentication Failed');location.href='" + @Url.Action("Index", "UserRegistration") + "'</script>");
            }
            return RedirectToAction("Index", "UserRegistration");
        }
        
        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        public ActionResult FacebookCallback(string code)
        {
            try {
                var fb = new FacebookClient();
                dynamic result = fb.Post("oauth/access_token", new
                {
                    client_id = "152565978688349",
                    client_secret = "e94b2cf9672b78b7ef552d2097d3c605",
                    redirect_uri = RediredtUri.AbsoluteUri,
                    code = code

                });
                var accessToken = result.access_token;
                Session["AccessToken"] = accessToken;
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
                string email = me.email;
                TempData["email"] = me.email;
                TempData["first_name"] = me.first_name;
                TempData["lastname"] = me.last_name;
                TempData["picture"] = me.picture.data.url;
                FormsAuthentication.SetAuthCookie(email, false);
                UserLogin userLogin = new UserLogin();
                UserDetail userDetail = new UserDetail();
                userDetail.FirstName = me.first_name;
                userDetail.LastName = me.last_name;
                userDetail.UserImgName = me.picture.data.url;
                userDetail.Url = me.link;
                userDetail.Gender = me.gender;
                userLogin.UserName = email;
                userLogin.Password = "Facebook";
                userLogin.UserType = "User";

                UserLogin userlogin1 = new UserLogin();

                userlogin1 = venorVenueSignUpService.GetUserLogin(userLogin); // checking where email id is registered or not.
                var response = "";
                if (userlogin1 == null)
                    response = userLoginDetailsService.AddUserDetails(userLogin, userDetail); // Adding user record to database
                else
                    response = "sucess";
                if (response == "sucess")
                {
                    var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
                    if (userResponse != null)
                    {
                        vendorMaster = vendorMasterService.GetVendorByEmail(userLogin.UserName);
                        string userData = JsonConvert.SerializeObject(userResponse); //creating identity
                        ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                        return RedirectToAction("Index", "HomePage");
                    }
                }
                else
                { return Content("<script language='javascript' type='text/javascript'>alert('Authentication Failed');location.href='" + @Url.Action("Index", "UserRegistration") + "'</script>"); }
                return RedirectToAction("Index", "UserRegistration");
            }
        catch(Exception)
            {
                return RedirectToAction("Index", "UserRegistration");
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "UserRegistration", new { ReturnUrl = returnUrl }));
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
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
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
        #endregion



        //protected List<Facebook.User> GetFacebookUserData(string code)
        //{
        //    // Exchange the code for an access token
        //    Uri targetUri = new Uri("https://graph.facebook.com/oauth/access_token?client_id=" + ConfigurationManager.AppSettings["FacebookAppId"] + "&client_secret=" + ConfigurationManager.AppSettings["FacebookAppSecret"] + "&redirect_uri=http://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/UserRegistration/FacebookAuthentication?code=" + code);
        //    HttpWebRequest at = (HttpWebRequest)HttpWebRequest.Create(targetUri);

        //    System.IO.StreamReader str = new System.IO.StreamReader(at.GetResponse().GetResponseStream());
        //    string token = str.ReadToEnd().ToString().Replace("access_token=", "");

        //    // Split the access token and expiration from the single string
        //    string[] combined = token.Split('&');
        //    string accessToken = combined[0];

        //    // Exchange the code for an extended access token
        //    Uri eatTargetUri = new Uri("https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id=" + ConfigurationManager.AppSettings["FacebookAppId"] + "&client_secret=" + ConfigurationManager.AppSettings["FacebookAppSecret"] + "&fb_exchange_token=" + accessToken);
        //    HttpWebRequest eat = (HttpWebRequest)HttpWebRequest.Create(eatTargetUri);

        //    StreamReader eatStr = new StreamReader(eat.GetResponse().GetResponseStream());
        //    string eatToken = eatStr.ReadToEnd().ToString().Replace("access_token=", "");

        //    // Split the access token and expiration from the single string
        //    string[] eatWords = eatToken.Split('&');
        //    string extendedAccessToken = eatWords[0];

        //    // Request the Facebook user information
        //    Uri targetUserUri = new Uri("https://graph.facebook.com/me?fields=first_name,last_name,gender,locale,link&access_token=" + accessToken);
        //    HttpWebRequest user = (HttpWebRequest)HttpWebRequest.Create(targetUserUri);

        //    // Read the returned JSON object response
        //    StreamReader userInfo = new StreamReader(user.GetResponse().GetResponseStream());
        //    string jsonResponse = string.Empty;
        //    jsonResponse = userInfo.ReadToEnd();

        //    // Deserialize and convert the JSON object to the Facebook.User object type
        //    JavaScriptSerializer sr = new JavaScriptSerializer();
        //    string jsondata = jsonResponse;
        //    Facebook.User converted = sr.Deserialize<Facebook.User>(jsondata);

        //    // Write the user data to a List
        //    List<Facebook.User> currentUser = new List<Facebook.User>();
        //    currentUser.Add(converted);

        //    // Return the current Facebook user
        //    return currentUser;
        //}
    }
}