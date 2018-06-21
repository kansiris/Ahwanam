using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using System.IO;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class ContestsManagementController : Controller
    {
        // GET: Admin/ContestsManagement
        ContestsService contestsService = new ContestsService();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        public ActionResult Index(string type, string id)
        {
            var contests = contestsService.GetAllContests().Where(m => m.Status == "Active");
            ViewBag.contests = contests;
            if (type == "New") ViewBag.display = "1";
            if (id != "0" && id != null)
            {
                ViewBag.display = "1";
                ViewBag.contestname = contests.Where(m => m.ContentMasterID == long.Parse(id)).FirstOrDefault().ContestName;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string command, ContestMaster contestMaster,string id)
        {
            if (command == "Add")
            {
                DateTime date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                contestMaster.CreatedDate = contestMaster.UpdatedDate = date;
                contestMaster.Status = "Active";
                contestMaster = contestsService.AddNewContest(contestMaster);
                if (contestMaster.ContentMasterID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Contest Added Successfully');location.href='" + @Url.Action("Index", "ContestsManagement") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed To Add Contest');location.href='" + @Url.Action("Index", "ContestsManagement") + "'</script>");
                }
            }
            if (command == "Update")
            {
                contestMaster.ContentMasterID = long.Parse(id);
                int count = contestsService.UpdateContestName(contestMaster);
                if (count != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Contest Updated Successfully');location.href='" + @Url.Action("Index", "ContestsManagement") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed To Update Contest');location.href='" + @Url.Action("Index", "ContestsManagement") + "'</script>");
                }
            }
            return View();
        }

        public ActionResult CancelContest()
        {
            return RedirectToAction("Index", "ContestsManagement");
        }

        public ActionResult RemoveContest(string id)
        {
            int count = contestsService.RemoveContest(long.Parse(id));
            if (count != 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Contest Removed Successfully');location.href='" + @Url.Action("Index", "ContestsManagement") + "'</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Failed To Remove Contest');location.href='" + @Url.Action("Index", "ContestsManagement") + "'</script>");
            }
        }

        public ActionResult AllEnteredContestes(string selectedcontest, string id ,string command, string selectedcontest1)
        {
            var records = contestsService.GetAllContests();
            ViewBag.records = records;
            if (selectedcontest != null && selectedcontest != "Select Contest" && selectedcontest1 == null)
            {
                ViewBag.contests = contestsService.GetAllEntries(long.Parse(selectedcontest));
                ViewBag.selectedcontest = selectedcontest;
            }
            if (id != "0" && id != null && command == null && selectedcontest != null && selectedcontest != "Select Contest")
            {
                ViewBag.selectedcontest = selectedcontest;

                ViewBag.display =  id;
                var contestdetails = contestsService.GetAllEntries(long.Parse(selectedcontest));
                ViewBag.contestdetails = contestdetails.Where(m => m.ContestId == long.Parse(id)).FirstOrDefault();
            }
            if (id != "0" && id != null && command != null && selectedcontest1 != null)
            {

                ViewBag.display = id;
                var contestdetails = contestsService.GetAllEntries(long.Parse(selectedcontest1));
                var cont1 = contestdetails.Where(m => m.ContestId == long.Parse(id) ).FirstOrDefault();
                ViewBag.selectedcontest = cont1;

                var username = cont1.Name;
                var typeid = cont1.UserLoginID;
                username = Capitalise(username);

                UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();

                var userlogin = userLoginDetailsService.GetUserId(Convert.ToInt32(typeid));
                var emailid = userlogin.UserName;
                EmailSendingUtility emailSendingUtility = new EmailSendingUtility();

                FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/Contest.html"));
                string readFile = File.OpenText().ReadToEnd();
                var txtone = " Your contest is activated";
                readFile = readFile.Replace("[Message]", txtone);
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Contests";

                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", username);
                string txtmessage = readFile;//readFile + body;

                string subj = "Attention required";

                emailSendingUtility.Email_maaaahwanam(emailid, txtmessage, subj);

                var email1 = "prabodh.dasari@xsilica.com";
                var txtmes = "Contestant is activated";
                readFile = readFile.Replace("[Message]", txtmes);
                readFile = readFile.Replace("[ActivationLink]", url);
                readFile = readFile.Replace("[name]", username);
                string txtmessage1 = readFile;//readFile + body;

                string subj1 = "Contestant is activated";

                emailSendingUtility.Email_maaaahwanam(email1, txtmessage1, subj1);


                contestsService.Activationcontest(cont1,command );
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + command + "');location.href='" + @Url.Action("AllEnteredContestes", "ContestsManagement") + "'</script>");

            }

            if (id != "0" && id != null && selectedcontest1 != null)
            {
                ViewBag.selectedcont1 = selectedcontest1;
                ViewBag.display = id;
                var contestdetails = contestsService.GetAllEntries(long.Parse(selectedcontest1));
                var cont1 = contestdetails.Where(m => m.ContestId == long.Parse(id)).ToList();
                ViewBag.selectedcontest1 = cont1;

               // contestsService.Activationcontest(cont1, command);
               // return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + command + "');location.href='" + @Url.Action("AllEnteredContestes", "ContestsManagement") + "'</script>");

            }
            return View();
        }


        public string Capitalise(string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }

        [HttpPost]
        public ActionResult submitquery(string emailid, string txtone, string cid ,string selectedcontest)
        {
            var contestdetails = contestsService.GetAllEntries(long.Parse(selectedcontest));
            var cont1 = contestdetails.Where(m => m.ContestId == long.Parse(cid)).FirstOrDefault();
            //   var userdetails = userLoginDetailsService.GetUser(id);
            var typeid = cont1.UserLoginID;

            UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();

            var userlogin = userLoginDetailsService.GetUserId(Convert.ToInt32(typeid));
            var username = cont1.Name;

            username = Capitalise(username);

            emailid = userlogin.UserName;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();

            FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/Contest.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[Message]", txtone);
            string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Contests";

            readFile = readFile.Replace("[ActivationLink]", url);

            readFile = readFile.Replace("[name]", username);
            string txtmessage = readFile;//readFile + body;

            string subj = "Attention required";

            emailSendingUtility.Email_maaaahwanam(emailid, txtmessage, subj);



         
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult submitquery1(string emailid, string txtone, string cid, string selectedcontest)
        {
            var contestdetails = contestsService.GetAllEntries(long.Parse(selectedcontest));
            var cont1 = contestdetails.Where(m => m.ContestId == long.Parse(cid)).FirstOrDefault();
            //   var userdetails = userLoginDetailsService.GetUser(id);
            var typeid = cont1.UserLoginID;
            var username = cont1.Name;
            UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
            username = Capitalise(username);

            var userlogin = userLoginDetailsService.GetUserId(Convert.ToInt32(typeid));

            emailid = userlogin.UserName;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Contests";

            FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/Contest.html"));
            string readFile = File.OpenText().ReadToEnd();
            readFile = readFile.Replace("[Message]", txtone);
            readFile = readFile.Replace("[ActivationLink]", url);

            readFile = readFile.Replace("[name]", username);
            string txtmessage = readFile;//readFile + body;

            string subj = "Contestant Is declined";

            emailSendingUtility.Email_maaaahwanam(emailid, txtmessage, subj);


            ViewBag.display = cid;
            var contestdetails1 = contestsService.GetAllEntries(long.Parse(selectedcontest));
            var cont11 = contestdetails.Where(m => m.ContestId == long.Parse(cid)).FirstOrDefault();
            ViewBag.selectedcontest = cont1;
            string command = "InActive";
            contestsService.Activationcontest(cont1, command);

            return Json("success", JsonRequestBehavior.AllowGet);
        }
    }
}