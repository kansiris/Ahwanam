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
            if (Request.Files.Count <= 4)
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
            }
            //return RedirectToAction("Index", "VendorSignUp3", new { id = id, vid = vid });
            //return Content("<script language='javascript' type='text/javascript'>alert('Photo gallery Uploaded');location.href='" + @Url.Action("Index", "AvailableServices", new { id = vid, vid = id }) + "'</script>");
            return Content("<script language='javascript' type='text/javascript'>alert('Photo gallery Uploaded');location.href='AvailableServices/Index?id=" + id + "&&vid=" + vid + "'</script>");
        }
    }
}