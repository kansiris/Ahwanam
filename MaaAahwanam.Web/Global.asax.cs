//using MaaAahwanam.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;
using MaaAahwanam.Models;
using MaaAahwanam.Web.Custom;
using System.Security.Principal;
using System.Web.Helpers;
using MaaAahwanam.Utility;
using Microsoft.Owin.Security;

namespace MaaAahwanam.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
            //AuthConfig.RegisterAuth();
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null)
                {
                    var serializeModel = JsonConvert.DeserializeObject<UserResponse>(authTicket.UserData);
                    var newUser = new CustomPrincipal(authTicket.Name)
                    {
                        UserId = serializeModel.UserLoginId,
                        FirstName = serializeModel.FirstName,
                        LastName = serializeModel.LastName,
                        UserType = serializeModel.UserType
                    };
                    HttpContext.Current.User = newUser;

                }
            }
            //else
            //{
            //    var newUser = new CustomPrincipal("0")
            //    {
            //        UserId = 0,
            //        FirstName = null,
            //        LastName = null,
            //        UserType = "",
            //    };
            //    HttpContext.Current.User = newUser;
            //}

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            string txt = Convert.ToString(httpException);
            if (httpException != null)
            {
                string action;
                string email = "sireesh.k@xsilica.com,maaaahwanamtest@gmail.com,rameshsai@xsilica.com";  //,info @ahwanam.com";
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = " Error occured in application HttpError404";
                        break;
                    case 500:
                        // server error
                        action = "Error occured in application HttpError500";
                        break;
                    default:
                        action = "Error occured in application General";
                        break;
                }
                EmailSendingUtility EmailSend = new EmailSendingUtility();
                EmailSend.Email_maaaahwanam(email,txt , action);
                // clear error on server
                Server.ClearError();
                //Response.Cookies.Clear();

                //FormsAuthentication.SignOut();

                //  Response.Redirect(String.Format("~/Error/{0}/?message={1}", action, exception.Message));
                // return Content("<script language='javascript' type='text/javascript'>alert('User Record Not Available');location.href='" + @Url.Action("Index", "NUserRegistration") + "'</script>");
                Response.Write("<script language='javascript'>window.alert('Some thing went wrong please try again after some time click OK to continue!!!');window.location='/nhomepage/index';</script>");


            }
        }
    }
}
