using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorSignUp2Controller : Controller
    {
        const string imagepath = @"/vendorimages/";
        VendorImageService vendorImageService = new VendorImageService();
        // GET: VendorSignUp2
        public ActionResult Index(string id,string vid)
        {
            ViewBag.id = id;
            ViewBag.vid = vid;
            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
            return View();
        }

        [HttpPost]
        public ActionResult Index(string id, string vid, HttpPostedFileBase file,string removedimages,string type)
        {
            string fileName = string.Empty;
            VendorImage vendorImage = new VendorImage();
            Vendormaster vendorMaster = new Vendormaster();
            vendorMaster.Id = long.Parse(id);
            vendorImage.VendorId = long.Parse(vid);
            if (Request.Files.Count <= 6)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    int j = i + 1;
                    var file1 = Request.Files[i];
                    if (removedimages.Contains(file1.FileName)) { }
                    else
                    {
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = type +"_" + id + "_" + vid + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Photo gallery Uploaded');location.href='AvailableServices/Index?id=" + id + "&&vid=" + vid + "'</script>");
            }
            //return RedirectToAction("Index", "VendorSignUp3", new { id = id, vid = vid });
            //return Content("<script language='javascript' type='text/javascript'>alert('Photo gallery Uploaded');location.href='" + @Url.Action("Index", "AvailableServices", new { id = vid, vid = id }) + "'</script>");
            return View();
        }

        //public JsonResult Removeimage(string src, string id,string vid)
        //{
        //    VendorImageService vendorImageService = new VendorImageService();
        //    var vendorImage = vendorImageService.GetImageId(src, long.Parse(vid));
        //    string delete = vendorImageService.DeleteImage(vendorImage);
        //    if (delete == "success")
        //    {
        //        string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
        //        System.IO.File.Delete(fileName);
        //    }
        //    return Json(delete, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Removeimage(string src, string id, string vid)
        {
            var vendorImage = vendorImageService.GetImageId(src, long.Parse(vid));
            string delete = vendorImageService.DeleteImage(vendorImage);
            if (delete == "success")
            {
                string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                System.IO.File.Delete(fileName);
                return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Index", "VendorSignUp2", new { id = id, vid = vid }) + "'</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Index", "VendorSignUp2", new { id = id, vid = vid }) + "'</script>");
            }
        }
    }
}