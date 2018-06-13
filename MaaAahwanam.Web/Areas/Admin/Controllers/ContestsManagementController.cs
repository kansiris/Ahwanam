using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;

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
        public ActionResult Index(string command, ContestMaster contestMaster, string id)
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

        public ActionResult AllEnteredContestes(string selectedcontest)
        {
            var records = contestsService.GetAllContests();
            ViewBag.records = records;
            if (selectedcontest != null)
            {
                ViewBag.contests = contestsService.GetAllEntries(long.Parse(selectedcontest));
            }
            return View();
        }
    }
}