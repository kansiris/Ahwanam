using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;

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
            if (selectedcontest != null && selectedcontest != "Select Contest")
            {
                ViewBag.contests = contestsService.GetAllEntries(long.Parse(selectedcontest));
                ViewBag.selectedcontest = selectedcontest;
            }
            if (id != "0" && id != null && command == null && selectedcontest != null)
            {
                ViewBag.selectedcontest = selectedcontest;

                ViewBag.display =  id;
                var contestdetails = contestsService.GetAllEntries(long.Parse(selectedcontest));
                ViewBag.contestdetails = contestdetails.Where(m => m.ContestId == long.Parse(id)).FirstOrDefault();
            }
            if (id != "0" && id != null && command != null && selectedcontest1 != null)
            {
                ViewBag.selectedcontest = selectedcontest;

                ViewBag.display = id;
                var contestdetails = contestsService.GetAllEntries(long.Parse(selectedcontest1));
                var cont1 = contestdetails.Where(m => m.ContestId == long.Parse(id) ).FirstOrDefault();
                
                contestsService.Activationcontest(cont1,command );
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + command + "');location.href='" + @Url.Action("AllEnteredContestes", "ContestsManagement") + "'</script>");

            }
            return View();
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
            emailid = userlogin.UserName;
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(emailid, txtone, "Attention required");
            return Json("success", JsonRequestBehavior.AllowGet);
        }


    }
}