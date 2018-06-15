using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using MaaAahwanam.Models;
using System.Web.Security;
using Newtonsoft.Json;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class ContestsController : Controller
    {
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        ContestsService contestsService = new ContestsService();
        // GET: Contests
        public ActionResult Index()
        {
            var contests = contestsService.GetAllContests().Where(m => m.Status == "Active");
            ViewBag.contests = contests;
            return View();
        }

        public PartialViewResult Authenticate()
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
                }
            }
            return PartialView("Authenticate");
        }
        [HttpPost]
        public JsonResult SubmittingSubscriber(Subscription Subscription)
        {
            string message = string.Empty;
            //try
            //{
                SubscriptionService subscriptionService = new SubscriptionService();
                subscriptionService.addsubscription(Subscription);
                EmailSendingUtility EmailSend = new EmailSendingUtility();
                EmailSend.Email_maaaahwanam(Subscription.EmailId, "Thank you for subscribing to Maa-Aahwanam", "Confirmation Subscription");
                message = "subscribed successfully";
            //}
            //catch
            //{
            //    message = "subscription failed";
            //}
            return Json(String.Format(message));
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
    }
}