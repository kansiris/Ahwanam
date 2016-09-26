﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class TestimonialformController : Controller
    {
        //
        // GET: /Testimonialform/
        public ActionResult Index()
        {
            int Uid = int.Parse(Request.QueryString["Uid"]);
            int Oid = int.Parse(Request.QueryString["Oid"]);
            TempData["Uid"] = Uid;
            TempData["Oid"] = Oid;
            return View();
        }

        public ActionResult Saveform(HttpPostedFileBase file, AdminTestimonial adminTestimonial)
        {
            
            AdminTestimonialPath adminTestimonialPath = new AdminTestimonialPath();
            TestmonialService testmonialService = new TestmonialService();
            adminTestimonial.UpdatedBy = (int)TempData["Uid"];
            adminTestimonial.Orderid = (int)TempData["Oid"];
            testmonialService.Savetestimonial(adminTestimonial);
            adminTestimonialPath.Id = adminTestimonial.Id;
            adminTestimonial.Status = "Pending";
            string fileName1 = "";
            string imagepath = @"/Testimonial/";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                int j = i + 1;
                var file1 = Request.Files[i];
                string path = System.IO.Path.GetExtension(file.FileName);
                var filename = "Testimonial_" + adminTestimonial.Id + "_" + j + path;
                fileName1 = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                file1.SaveAs(fileName1);
                adminTestimonialPath.ImagePath = filename;
                testmonialService.Savetestimonialpath(adminTestimonialPath);
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Submitted successfully!');location.href='" + @Url.Action("Index", "Index") + "'</script>");
        }
    }
}