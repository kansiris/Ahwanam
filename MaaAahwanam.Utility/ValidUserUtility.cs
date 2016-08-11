using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Security;
using MaaAahwanam.Models;
//using MaaAahwanam.Repository;

namespace MaaAahwanam.Utility
{
    public class ValidUserUtility
    {
        public static int ValidUser()
        {
            int Userid = 0;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] == null) Userid = 0;
            //else if (authCookie.Name == ".ASPXAUTH") Userid = 0;
            else Userid = int.Parse(FormsAuthentication.Decrypt(HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name);
            return Userid;
        }
        public static string UserType()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string userdata = ticket.UserData;
            return userdata;
        }
        public static void SetAuthCookie(string userResponse, string userId)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                userId,
                DateTime.Now,
                DateTime.Now.AddMinutes(15),
                false, //pass here true, if  want to implement remember me functionality
                userResponse);     // the path for the cookie
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

    }
}
