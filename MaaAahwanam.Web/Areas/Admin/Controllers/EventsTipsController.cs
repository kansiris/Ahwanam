using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.IO;
using MaaAahwanam.Web.Custom;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class EventsTipsController : Controller
    {
        EventsandtipsService eventsandtipsService = new EventsandtipsService();
        const string imagepath = @"/EventsAndTipsimages/";
        //
        // GET: /Admin/EventsTips/
        public ActionResult Index(string id, string src)
        {
            //Retrieves Avaliable Recordes List
            var list = eventsandtipsService.EventsandTipsList();
            ViewBag.EventsandTipsList = list;
            if (id!=null)
            {
                //Gets particular Record
                var particularevent = eventsandtipsService.GetEventandTip(long.Parse(id));
                //Assigns images list to a variable
                string x = particularevent.Image.ToString();
                if (x != "" && x != null)
                {
                    ViewBag.images = x.Split(',');
                }
                return View(particularevent);
            }
            if (src!=null)
            {

            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(EventsandTip eventAndTip,HttpPostedFileBase file,string Command,string id)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            EventsandtipsService eventsAndTipsService = new EventsandtipsService();
            long value = eventsAndTipsService.EventIDCount();
            string fileName = string.Empty;
            string ImagesURL = string.Empty;
            eventAndTip.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            var filename = string.Empty;
                            string path = System.IO.Path.GetExtension(file.FileName);
                            if (eventAndTip.Type == "Event")
                            {
                                filename = eventAndTip.Type + "_" + value + "_" + j + path;
                            }
                            else if (eventAndTip.Type == "Beauty Tips")
                            {
                                filename = "Beauty_" + value + "_" + j + path;
                            }
                            else if (eventAndTip.Type == "Health Tips")
                            {
                                filename = "Health_" + value + "_" + j + path;
                            }
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            ImagesURL += filename + ",";
                        }
                    }
                }
                eventAndTip.Image = ImagesURL.TrimEnd(',');
                eventAndTip = eventsAndTipsService.AddEventandTip(eventAndTip);
                if (eventAndTip.EventId != 0)
                return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Index", "EventsTips") + "'</script>");
                return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed!!!');location.href='" + @Url.Action("Index", "EventsTips") + "'</script>");
                }

            if (Command == "Update")
            {
                var particularevent = eventsandtipsService.GetEventandTip(long.Parse(id));
                string x = particularevent.Image.ToString();
                ViewBag.images = x.Split(',');
                int count = Enumerable.Count(ViewBag.images);
                ViewBag.imagescount = count;
                int imageno = 0;
                string x1 = ViewBag.images[0].ToString();
                string[] y = x1.Split('_', '.');
                if (y[0]!= "" && y != null)
                {
                     imageno = int.Parse(y[1]);
                }
               
                if (Request.Files.Count <= 10 - count)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = count + 1+i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            var filename = string.Empty;
                            string path = System.IO.Path.GetExtension(file.FileName);
                            if (eventAndTip.Type == "Event")
                            {
                                filename = eventAndTip.Type + "_" + imageno + "_" + j + path;
                            }
                            else if (eventAndTip.Type == "Beauty Tips")
                            {
                                filename = "Beauty_" + imageno + "_" + j + path;
                            }
                            else if (eventAndTip.Type == "Health Tips")
                            {
                                filename = "Health_" + imageno + "_" + j + path;
                            }
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            ImagesURL += filename + ",";
                        }
                    }
                    eventAndTip.Image = particularevent.Image + "," + ImagesURL.TrimEnd(',');
                    eventAndTip = eventsAndTipsService.UpdateEventandTip(eventAndTip, long.Parse(id));
                    if (eventAndTip.EventId != 0)
                    return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllRecords", "EventsTips") + "'</script>");
                    return Content("<script language='javascript' type='text/javascript'>alert('Update Failed!!!');location.href='" + @Url.Action("AllRecords", "EventsTips") + "'</script>");
                }
                else
                {
                    if (file != null)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('You have Crossed Images Limit');location.href='" + @Url.Action("Index", "EventsTips", new { id = id }) + "'</script>");
                    }
                    else
                    {
                        eventAndTip.Image = particularevent.Image;
                        eventAndTip = eventsAndTipsService.UpdateEventandTip(eventAndTip, long.Parse(id));
                        if (eventAndTip.EventId != 0)
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllRecords", "EventsTips") + "'</script>");
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed!!!');location.href='" + @Url.Action("AllRecords", "EventsTips") + "'</script>");
                    }
                }
                
            }
            return View();
        }

        public ActionResult AllRecords()
        {
            var list = eventsandtipsService.EventsandTipsList().OrderByDescending(m => m.EventId);
            ViewBag.EventsandTipsList = list;
            return View();
        }
    }
	}
