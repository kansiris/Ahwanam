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

namespace MaaAahwanam.Web.Controllers
{
    public class UserRegistrationController : Controller
    {
        // GET: UserRegistration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string command, [Bind(Prefix = "Item1")] UserLogin userLogin, [Bind(Prefix = "Item2")] UserDetail userDetail)
        {
            UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
            userLogin.UserType = "User";
            var response = userLoginDetailsService.AddUserDetails(userLogin, userDetail);
            if (response == "sucess")
                return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Index", "SampleStorefront") + "'</script>");
            else
                return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Index", "UserRegistration") + "'</script>");
        }

        public ActionResult FacebookLogin()
        {
            Response.Redirect("https://www.facebook.com/v2.4/dialog/oauth/?client_id=" + ConfigurationManager.AppSettings["FacebookAppId"] + "&redirect_uri=http://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/UserRegistration/FacebookAuthentication?response_type=code&state=1");
            return View();
        }

        public ActionResult FacebookAuthentication()
        {
            //if (Request.QueryString["code"] != "")
            //{
            //    var obj = GetFacebookUserData(Request.QueryString["code"]);

            //    //ListView1.DataSource = obj;
            //    //ListView1.DataBind();
            //}
            return View();
        }

        protected List<Facebook.User> GetFacebookUserData(string code)
        {
            // Exchange the code for an access token
            Uri targetUri = new Uri("https://graph.facebook.com/oauth/access_token?client_id=" + ConfigurationManager.AppSettings["FacebookAppId"] + "&client_secret=" + ConfigurationManager.AppSettings["FacebookAppSecret"] + "&redirect_uri=http://" + Request.ServerVariables["SERVER_NAME"] + ":" + Request.ServerVariables["SERVER_PORT"] + "/UserRegistration/FacebookAuthentication?code=" + code);
            HttpWebRequest at = (HttpWebRequest)HttpWebRequest.Create(targetUri);

            System.IO.StreamReader str = new System.IO.StreamReader(at.GetResponse().GetResponseStream());
            string token = str.ReadToEnd().ToString().Replace("access_token=", "");

            // Split the access token and expiration from the single string
            string[] combined = token.Split('&');
            string accessToken = combined[0];

            // Exchange the code for an extended access token
            Uri eatTargetUri = new Uri("https://graph.facebook.com/oauth/access_token?grant_type=fb_exchange_token&client_id=" + ConfigurationManager.AppSettings["FacebookAppId"] + "&client_secret=" + ConfigurationManager.AppSettings["FacebookAppSecret"] + "&fb_exchange_token=" + accessToken);
            HttpWebRequest eat = (HttpWebRequest)HttpWebRequest.Create(eatTargetUri);

            StreamReader eatStr = new StreamReader(eat.GetResponse().GetResponseStream());
            string eatToken = eatStr.ReadToEnd().ToString().Replace("access_token=", "");

            // Split the access token and expiration from the single string
            string[] eatWords = eatToken.Split('&');
            string extendedAccessToken = eatWords[0];

            // Request the Facebook user information
            Uri targetUserUri = new Uri("https://graph.facebook.com/me?fields=first_name,last_name,gender,locale,link&access_token=" + accessToken);
            HttpWebRequest user = (HttpWebRequest)HttpWebRequest.Create(targetUserUri);

            // Read the returned JSON object response
            StreamReader userInfo = new StreamReader(user.GetResponse().GetResponseStream());
            string jsonResponse = string.Empty;
            jsonResponse = userInfo.ReadToEnd();

            // Deserialize and convert the JSON object to the Facebook.User object type
            JavaScriptSerializer sr = new JavaScriptSerializer();
            string jsondata = jsonResponse;
            Facebook.User converted = sr.Deserialize<Facebook.User>(jsondata);

            // Write the user data to a List
            List<Facebook.User> currentUser = new List<Facebook.User>();
            currentUser.Add(converted);

            // Return the current Facebook user
            return currentUser;
        }
    }
}