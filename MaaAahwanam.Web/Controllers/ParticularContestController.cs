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
using MaaAahwanam.Web.Custom;
using System.Web.Security;

namespace MaaAahwanam.Web.Controllers
{
    public class ParticularContestController : Controller
    {
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        ContestsService contestsService = new ContestsService();
        VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();

        // GET: ParticularContest
        public ActionResult Index(string id, string csid)
        {
            ViewBag.id = id;
            if (id != null && csid == null)
            {
                var contests = contestsService.GetAllContests().Where(m => m.Status == "Active");
                ViewBag.contestname = contests.Where(m => m.ContentMasterID == long.Parse(id)).FirstOrDefault().ContestName;
                var allrecords = contestsService.GetAllEntries(long.Parse(id));
                var AvailableContestEntries = allrecords.Where(m => m.Status == "Active").ToList();
                List<string> contestentries = new List<string>();
                List<string> votecount = new List<string>();
                List<string> votedornot = new List<string>();
                foreach (var item in AvailableContestEntries)
                {
                    var date = TimeAgo(item.UpdatedDate);
                    contestentries.Add(date);
                    var count = contestsService.GetAllVotes(item.ContestId).Where(m => m.Status == "Active").Count();
                    votecount.Add(count.ToString());
                    if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                        var userlogin = userLoginDetailsService.GetUser((int)user.UserId);
                        if (userlogin.AlternativeEmailID == null)
                        {
                            var getdata = userLoginDetailsService.GetUserId((int)user.UserId);
                            userlogin.AlternativeEmailID = getdata.UserName;
                        }
                        var getVote = contestsService.GetAllVotes(item.ContestId).Where(m => m.Email == userlogin.AlternativeEmailID && m.Status == "Active").Count();
                        if (getVote == 0) votedornot.Add("1"); //ViewBag.vote = "1";
                        else votedornot.Add("0");//ViewBag.vote = "0";
                    }
                }
                ViewBag.AvailableContestEntries = AvailableContestEntries;
                ViewBag.count = AvailableContestEntries.Count();
                ViewBag.time = contestentries;
                ViewBag.votecount = votecount;
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    var userlogin = userLoginDetailsService.GetUser((int)user.UserId);
                    if (userlogin.AlternativeEmailID == null)
                    {
                        var getdata = userLoginDetailsService.GetUserId((int)user.UserId);
                        userlogin.AlternativeEmailID = getdata.UserName;
                    }
                    var userenties = allrecords.Where(m => m.UserLoginID == user.UserId).ToList();
                    ViewBag.userenties = userenties;
                    List<string> myuploadedtime = new List<string>();
                    List<string> myvotes = new List<string>();
                    List<string> myvotedornot = new List<string>();
                    foreach (var item in ViewBag.userenties)
                    {
                        var date1 = TimeAgo(item.UpdatedDate);
                        myuploadedtime.Add(date1);
                        var count1 = contestsService.GetAllVotes(long.Parse(id)).Where(m => m.Status == "Active").Count();
                        myvotes.Add(count1.ToString());
                        var mygetVote = contestsService.GetAllVotes(long.Parse(id)).Where(m => m.Email == userlogin.AlternativeEmailID && m.Status == "Active").Count();
                        if (mygetVote == 0) myvotedornot.Add("1"); //ViewBag.vote = "1";
                        else myvotedornot.Add("0");//ViewBag.vote = "0";
                    }
                    ViewBag.mytime = myuploadedtime;
                    ViewBag.myvotes = myvotes;
                    ViewBag.mycount = userenties.Count();
                    ViewBag.myvotedornot = myvotedornot;
                    ViewBag.uploadimage = userenties.Where(m => m.ContentMasterID == long.Parse(id) && m.UserLoginID == user.UserId).Count();
                    //ViewBag.uploadimage = uploadimage;

                }
                var fburl = "http://www.ahwanam.com/ParticularContest?id=id&csid=csid";

                ViewBag.fburl = "http://tinyurl.com/api-create.php?url=" + fburl;
                ViewBag.votestatus = votedornot;
            }
            else if (id != null && csid != null)
            {
                var contests = contestsService.GetAllContests().Where(m => m.Status == "Active");
                ViewBag.contestname = contests.Where(m => m.ContentMasterID == long.Parse(id)).FirstOrDefault().ContestName;
                var allrecords = contestsService.GetAllEntries(long.Parse(id));
                var AvailableContestEntries1 = contestsService.GetAllEntries(long.Parse(id));
                var AvailableContestEntries = AvailableContestEntries1.Where(m => m.ContestId == long.Parse(csid)).ToList();

                List<string> contestentries = new List<string>();
                List<string> votecount = new List<string>();
                List<string> votedornot = new List<string>();
                foreach (var item in AvailableContestEntries)
                {
                    var date = TimeAgo(item.UpdatedDate);
                    contestentries.Add(date);
                    var count = contestsService.GetAllVotes(item.ContestId).Where(m => m.Status == "Active").Count();
                    votecount.Add(count.ToString());
                    if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                        var userlogin = userLoginDetailsService.GetUser((int)user.UserId);
                        if (userlogin.AlternativeEmailID == null)
                        {
                            var getdata = userLoginDetailsService.GetUserId((int)user.UserId);
                            userlogin.AlternativeEmailID = getdata.UserName;
                        }
                        var getVote = contestsService.GetAllVotes(long.Parse(id)).Where(m => m.Email == userlogin.AlternativeEmailID && m.Status == "Active").Count();
                        if (getVote == 0) votedornot.Add("1"); //ViewBag.vote = "1";
                        else votedornot.Add("0");//ViewBag.vote = "0";
                    }
                }
                ViewBag.AvailableContestEntries = AvailableContestEntries;
                ViewBag.count = AvailableContestEntries.Count();
                ViewBag.time = contestentries;
                ViewBag.votecount = votecount;
                ViewBag.csid = csid;
                ViewBag.cssid = id;
                var fburl = "http://www.ahwanam.com/ParticularContest?id=id&csid=csid";

                ViewBag.fburl = "http://tinyurl.com/api-create.php?url=" + fburl;
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    var userlogin = userLoginDetailsService.GetUser((int)user.UserId);
                    if (userlogin.AlternativeEmailID == null)
                    {
                        var getdata = userLoginDetailsService.GetUserId((int)user.UserId);
                        userlogin.AlternativeEmailID = getdata.UserName;
                    }
                    var userenties = allrecords.Where(m => m.UserLoginID == user.UserId).ToList();
                    ViewBag.userenties = userenties;
                    List<string> myuploadedtime = new List<string>();
                    List<string> myvotes = new List<string>();
                    List<string> myvotedornot = new List<string>();
                    foreach (var item in ViewBag.userenties)
                    {
                        var date1 = TimeAgo(item.UpdatedDate);
                        myuploadedtime.Add(date1);
                        var count1 = contestsService.GetAllVotes(long.Parse(id)).Where(m => m.Status == "Active").Count();
                        myvotes.Add(count1.ToString());
                        var mygetVote = contestsService.GetAllVotes(long.Parse(id)).Where(m => m.Email == userlogin.AlternativeEmailID && m.Status == "Active").Count();
                        if (mygetVote == 0) myvotedornot.Add("1"); //ViewBag.vote = "1";
                        else myvotedornot.Add("0");//ViewBag.vote = "0";
                    }
                    ViewBag.mytime = myuploadedtime;
                    ViewBag.myvotes = myvotes;
                    ViewBag.mycount = userenties.Count();
                    ViewBag.myvotedornot = myvotedornot;
                    ViewBag.uploadimage = userenties.Where(m => m.ContentMasterID == long.Parse(id) && m.UserLoginID == user.UserId).Count();

                }
                ViewBag.votestatus = votedornot;
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
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    //var response = userLoginDetailsService.GetUser((int)user.UserId);
                    var userlogin = userLoginDetailsService.GetUserId((int)user.UserId);
                    contest.ContentMasterID = long.Parse(id);
                    contest.IPAddress = HttpContext.Request.UserHostAddress;
                    contest.SharedCount = 0;
                    contest.UserLoginID = user.UserId;
                    string strm = sample_input.Replace("data:image/png;base64,", "");

                    //this is a simple white background image
                    var myfilename = userlogin.UserName + "_" + id + "_.jpeg";
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

        //public PartialViewResult LoadMore(string id, string L1)
        //{
        //    var contests = contestsService.GetAllContests().Where(m => m.Status == "Active");
        //    ViewBag.contestname = contests.Where(m => m.ContentMasterID == long.Parse(id)).FirstOrDefault().ContestName;
        //    var allrecords = contestsService.GetAllEntries(long.Parse(id));
        //    var AvailableContestEntries = allrecords.Where(m => m.Status == "Active").ToList();
        //    List<string> contestentries = new List<string>();
        //    List<string> votecount = new List<string>();
        //    List<string> votedornot = new List<string>();
        //    foreach (var item in AvailableContestEntries)
        //    {
        //        var date = TimeAgo(item.UpdatedDate);
        //        contestentries.Add(date);
        //        var count = contestsService.GetAllVotes(item.ContestId).Where(m => m.Status == "Active").Count();
        //        votecount.Add(count.ToString());
        //        if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        //        {
        //            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
        //            var userlogin = userLoginDetailsService.GetUser((int)user.UserId);
        //            if (userlogin.AlternativeEmailID == null)
        //            {
        //                var getdata = userLoginDetailsService.GetUserId((int)user.UserId);
        //                userlogin.AlternativeEmailID = getdata.UserName;
        //            }
        //            var getVote = contestsService.GetAllVotes(long.Parse(id)).Where(m => m.Email == userlogin.AlternativeEmailID && m.Status == "Active").Count();
        //            if (getVote == 0) votedornot.Add("1"); //ViewBag.vote = "1";
        //            else votedornot.Add("0");//ViewBag.vote = "0";
        //        }
        //    }
        //    ViewBag.AvailableContestEntries = AvailableContestEntries;
        //    ViewBag.count = AvailableContestEntries.Count();
        //    ViewBag.time = contestentries;
        //    ViewBag.votecount = votecount;
        //    if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        //    {
        //        var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
        //        var userenties = allrecords.Where(m => m.UserLoginID == user.UserId).ToList();
        //        ViewBag.userenties = userenties;
        //        List<string> myuploadedtime = new List<string>();
        //        List<string> myvotes = new List<string>();
        //        foreach (var item in ViewBag.userenties)
        //        {
        //            var date1 = TimeAgo(item.UpdatedDate);
        //            myuploadedtime.Add(date1);
        //            var count1 = contestsService.GetAllVotes(long.Parse(id)).Where(m => m.Status == "Active").Count();
        //            myvotes.Add(count1.ToString());
        //        }
        //        ViewBag.mytime = myuploadedtime;
        //        ViewBag.myvotes = myvotes;
        //        ViewBag.mycount = userenties.Count();
        //        ViewBag.uploadimage = userenties.Where(m => m.ContentMasterID == long.Parse(id) && m.UserLoginID == user.UserId).Count();
        //        //ViewBag.uploadimage = uploadimage;
        //    }
        //    return PartialView("LoadMore");
        //}

        //[HttpPost]
        //public ActionResult UserAuthentication(string command, [Bind(Prefix = "Item1")]UserLogin userLogin)
        //{
        //    if (command == "Login")
        //    {
        //        var userResponse = venorVenueSignUpService.GetUserLogin(userLogin);
        //        var userResponse1 = venorVenueSignUpService.GetUserLogdetails(userLogin);

        //        if (userResponse != null)
        //        {
        //            string userData = JsonConvert.SerializeObject(userResponse);
        //            ValidUserUtility.SetAuthCookie(userData, userResponse.UserLoginId.ToString());
        //            return RedirectToAction("Index", "ParticularContest");
        //        }
        //        else
        //            return Content("<script language='javascript' type='text/javascript'>alert('Wrong Credentials,Check Username and password');location.href='" + @Url.Action("Index", "ParticularContest") + "'</script>");
        //    }
        //    return View();
        //}

        public static string TimeAgo(DateTime dt)
        {
            //DateTime uploadeddate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, INDIAN_ZONE);
            //TimeSpan span = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, INDIAN_ZONE) - dt;
            TimeSpan span = DateTime.Now.AddHours(12).AddMinutes(30) - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }

        public JsonResult Voting(string id)
        {
            ContestVote contestVote = new ContestVote();
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                //var response = userLoginDetailsService.GetUser((int)user.UserId);
                var userlogin = userLoginDetailsService.GetUser((int)user.UserId);
                if (userlogin.AlternativeEmailID == null)
                {
                    var getdata = userLoginDetailsService.GetUserId((int)user.UserId);
                    userlogin.AlternativeEmailID = getdata.UserName;
                }
                //if (command == "Add")
                //{
                var votechecking = contestsService.GetAllVotes(long.Parse(id)).Where(m => m.Email == userlogin.AlternativeEmailID).Count();
                contestVote.ContestId = long.Parse(id);
                contestVote.Email = userlogin.AlternativeEmailID;
                contestVote.IPAddress = HttpContext.Request.UserHostAddress;
                contestVote.Name = userlogin.FirstName + " " + userlogin.LastName;
                contestVote.Type = "Facebook";
                if (votechecking == 0)
                { contestVote = contestsService.AddContestVote(contestVote); return Json("Voted", JsonRequestBehavior.AllowGet); }
                else
                    return Json("Already Voted", JsonRequestBehavior.AllowGet);
                //    contestVote = contestsService.AddContestVote(contestVote);
                //}
                //if (command == "Remove")
                //{
                //    int count = contestsService.RemoveContestVote(contestVote);
                //}

            }
            return Json("Please Login To Vote", JsonRequestBehavior.AllowGet);
        }

        //public JsonResult RemoveVote(string id)
        //{
        //    if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
        //    {
        //        var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
        //        var userlogin = userLoginDetailsService.GetUser((int)user.UserId);
        //        if (userlogin.AlternativeEmailID == null)
        //        {
        //            var getdata = userLoginDetailsService.GetUserId((int)user.UserId);
        //            userlogin.AlternativeEmailID = getdata.UserName;
        //        }
        //        var getVote = contestsService.GetAllVotes(long.Parse(id)).Where(m => m.Email == userlogin.AlternativeEmailID).FirstOrDefault();
        //        ContestVote contestVote = getVote;
        //        int count = contestsService.RemoveContestVote(contestVote);
        //    }
        //    return Json("UnVoted", JsonRequestBehavior.AllowGet);
        //}

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
                    //Adding Vote
                    //ContestVote contestVote = new ContestVote();
                    //contestVote.ContestId = long.Parse(Request.QueryString["id"]);
                    //contestVote.Email = email;
                    //contestVote.IPAddress = HttpContext.Request.UserHostAddress;
                    //contestVote.Name = name;
                    //contestVote.Type = "Facebook";
                    //contestVote = contestsService.AddContestVote(contestVote);
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

        public PartialViewResult ParticularEntryView(string id, string tcid)
        {
            if (id != null && tcid != null)
            {
                var AvailableContestEntries = contestsService.GetAllEntries(long.Parse(id));
                ViewBag.AvailableContestEntries = AvailableContestEntries.Where(m => m.ContestId == long.Parse(tcid)).FirstOrDefault();
                ViewBag.votecount = contestsService.GetAllVotes(long.Parse(tcid)).Where(m => m.Status == "Active").Count();
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    var userlogin = userLoginDetailsService.GetUser((int)user.UserId);
                    if (userlogin.AlternativeEmailID == null)
                    {
                        var getdata = userLoginDetailsService.GetUserId((int)user.UserId);
                        userlogin.AlternativeEmailID = getdata.UserName;
                    }
                    var getVote = contestsService.GetAllVotes(long.Parse(tcid)).Where(m => m.Email == userlogin.AlternativeEmailID && m.Status == "Active").Count();
                    if (getVote == 0) ViewBag.vote = "1";
                    else ViewBag.vote = "0";
                }
                ViewBag.display = "1";
                ViewBag.id = id;
                ViewBag.csid = tcid;
                //var fburl = "http://www.ahwanam.com/ParticularContest?id=id&csid=tcid";

                //ViewBag.fburl = "http://tinyurl.com/api-create.php?url=" + fburl;
            }
            else
            {
                ViewBag.display = "0";
            }

            return PartialView("ParticularEntryView");
        }
    }
}