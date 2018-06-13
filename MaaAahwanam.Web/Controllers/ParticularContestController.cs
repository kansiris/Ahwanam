using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Web.Controllers
{
    public class ParticularContestController : Controller
    {
        ContestsService contestsService = new ContestsService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        // GET: ParticularContest
        public ActionResult Index(string id)
        {
            if (id != null)
            {
                var contests = contestsService.GetAllContests().Where(m => m.Status == "Active");
                ViewBag.contestname = contests.Where(m => m.ContentMasterID == long.Parse(id)).FirstOrDefault().ContestName;
            }
            else
                ViewBag.contestname = "Particular Contest";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string command, [Bind(Prefix = "Item2")]Contest contest, string id, HttpPostedFileBase file, string sample_input)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (command == "Add")
                {
                    contest.ContentMasterID = long.Parse(id);
                    contest.IPAddress = HttpContext.Request.UserHostAddress;
                    contest.SharedCount = 0;
                    string strm = sample_input.Replace("data:image/png;base64,", "");

                    //this is a simple white background image
                    var myfilename = "user@gmail.com" + "_" + id + "_.jpeg";
                    contest.UploadedImage = myfilename;
                    //Generate unique filename
                    string filepath = System.Web.HttpContext.Current.Server.MapPath(@"/ContestPics/" + myfilename);// "~/ProfilePictures/" + myfilename ;
                    var bytess = Convert.FromBase64String(strm);
                    using (var imageFile = new FileStream(filepath, FileMode.Create))
                    {
                        imageFile.Write(bytess, 0, bytess.Length);
                        imageFile.Flush();
                    }
                    contest = contestsService.EnterContest(contest);
                }
                if (contest.ContestId != 0)
                    return Content("<script language='javascript' type='text/javascript'>alert('Your Entry Recorded and Sent For Approval');location.href='/Contests/Index'</script>");
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed To Add New Entry!!! Try Again Later');location.href='/Contests/Index'</script>");
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Please Login');location.href='/NUserRegistration/Index'</script>");
        }

        [HttpPost]
        public ActionResult UserAuthentication(string command, [Bind(Prefix = "Item1")]UserLogin userLogin)
        {
            if (command == "Login")
            {
                var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
                var userResponse1 = venorVenueSignUpService.GetUserLogdetails(userLogin);

                if (userResponse != null)
                {
                    string userData = JsonConvert.SerializeObject(userResponse);
                    ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
                    return RedirectToAction("Index", "ParticularContest");
                }
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "ParticularContest") + "'</script>");
            }
            return View();
        }
    }
}