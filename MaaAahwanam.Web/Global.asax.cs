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

namespace MaaAahwanam.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
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
                    HttpContext.Current.User = (IPrincipal)newUser;

                }
            }

        }
    }
}
