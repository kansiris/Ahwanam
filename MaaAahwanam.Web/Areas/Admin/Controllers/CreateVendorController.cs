using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.IO;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class CreateVendorController : Controller
    {
        VendorImageService vendorImageService = new VendorImageService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        const string imagepath = @"/vendorimages/";
        //public CreateVendorController(IAccommodationService accommodationService)
        //{ }
        public ActionResult Images()
        {
            return View();
        }

        public ActionResult BeautyServices(string id, [Bind(Prefix = "Item2")] VendorsBeautyService vendorsBeautyService, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorBeautyServicesService vendorBeautyServicesService = new VendorBeautyServicesService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("BeautyService", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("BeautyService", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsBeautyService = vendorBeautyServicesService.GetVendorBeautyService(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsBeautyService>(vendorMaster, vendorsBeautyService);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult BeautyServices([Bind(Prefix = "Item2")] VendorsBeautyService vendorsBeautyService, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id,string vid)
        {
            string fileName = string.Empty;
            VendorBeautyServicesService vendorBeautyServicesService = new VendorBeautyServicesService();
            vendorsBeautyService.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsBeautyService = vendorBeautyServicesService.AddBeautyService(vendorsBeautyService, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsBeautyService.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "BeautyService_" + vendorsBeautyService.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsBeautyService.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("BeautyServices", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("BeautyServices", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsBeautyService.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsBeautyService.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsBeautyService = vendorBeautyServicesService.UpdatesBeautyService(vendorsBeautyService, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsBeautyService.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "BeautyService_" + vendorsBeautyService.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsBeautyService.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "BeautyService" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public ActionResult Catering(string id, [Bind(Prefix = "Item2")] VendorsCatering vendorsCatering, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorCateringService vendorCateringService = new VendorCateringService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Catering", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Catering", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsCatering = vendorCateringService.GetVendorCatering(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsCatering>(vendorMaster, vendorsCatering);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Catering([Bind(Prefix = "Item2")] VendorsCatering vendorsCatering, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file,string Command, string id,string vid)
        {
            string fileName = string.Empty;
            VendorCateringService vendorCateringService = new VendorCateringService();
            vendorsCatering.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsCatering = vendorCateringService.AddCatering(vendorsCatering, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsCatering.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Catering_" + vendorsCatering.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsCatering.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Catering", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Catering", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsCatering.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsCatering.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsCatering = vendorCateringService.UpdatesCatering(vendorsCatering, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsCatering.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Catering_" + vendorsCatering.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsCatering.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Catering" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public ActionResult Decorator(string id, [Bind(Prefix = "Item2")] VendorsDecorator vendorsDecorator, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Decorator", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Decorator", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsDecorator = vendorDecoratorService.GetVendorDecorator(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsDecorator>(vendorMaster, vendorsDecorator);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Decorator([Bind(Prefix = "Item2")] VendorsDecorator vendorsDecorator, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id,string vid)
        {
            string fileName = string.Empty;
            VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
            vendorsDecorator.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command=="Save")
            {
                vendorsDecorator = vendorDecoratorService.AddDecorator(vendorsDecorator, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsDecorator.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Decorator_" + vendorsDecorator.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsDecorator.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Decorator", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Decorator", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsDecorator.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsDecorator.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsDecorator = vendorDecoratorService.UpdateDecorator(vendorsDecorator, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsDecorator.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Decorator_" + vendorsDecorator.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsDecorator.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Decorator" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }


            return View();
        }
        public ActionResult Entertainment(string id, [Bind(Prefix = "Item2")] VendorsEntertainment vendorsEntertainment, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorEntertainmentService vendorEntertainmentService = new VendorEntertainmentService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Entertainment", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Entertainment", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsEntertainment = vendorEntertainmentService.GetVendorEntertainment(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsEntertainment>(vendorMaster, vendorsEntertainment);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Entertainment([Bind(Prefix = "Item2")] VendorsEntertainment vendorsEntertainment, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id,string vid)
        {
            string fileName = string.Empty;
            VendorEntertainmentService vendorEntertainmentService = new VendorEntertainmentService();
            vendorsEntertainment.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsEntertainment = vendorEntertainmentService.AddEntertainment(vendorsEntertainment, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsEntertainment.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Entertainment_" + vendorsEntertainment.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsEntertainment.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Entertainment", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Entertainment", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsEntertainment.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsEntertainment.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsEntertainment = vendorEntertainmentService.UpdateEntertainment(vendorsEntertainment, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsEntertainment.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Entertainment_" + vendorsEntertainment.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsEntertainment.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Entertainment" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public ActionResult EventOrganisers(string id, [Bind(Prefix = "Item2")] VendorsEventOrganiser vendorsEventOrganiser, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorEventOrganiserService vendorEventOrganiserService = new VendorEventOrganiserService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Entertainment", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Entertainment", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsEventOrganiser = vendorEventOrganiserService.GetVendorEventOrganiser(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsEventOrganiser>(vendorMaster, vendorsEventOrganiser);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult EventOrganisers([Bind(Prefix = "Item2")] VendorsEventOrganiser vendorsEventOrganisers, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid)
        {
            string fileName = string.Empty;
            VendorEventOrganiserService vendorEventOrganiserService = new VendorEventOrganiserService();
            vendorsEventOrganisers.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsEventOrganisers = vendorEventOrganiserService.AddEventOrganiser(vendorsEventOrganisers, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsEventOrganisers.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "EventOrganisers_" + vendorsEventOrganisers.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsEventOrganisers.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("EventOrganisers", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("EventOrganisers", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsEventOrganisers.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsEventOrganisers.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsEventOrganisers = vendorEventOrganiserService.UpdateEventOrganiser(vendorsEventOrganisers, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsEventOrganisers.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "EventOrganiser_" + vendorsEventOrganisers.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsEventOrganisers.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "EventOrganisers" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public ActionResult Gifts(string id, [Bind(Prefix = "Item2")] VendorsGift vendorsGift, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorGiftService vendorGiftService = new VendorGiftService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Gifts", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Gifts", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsGift = vendorGiftService.GetVendorGift(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsGift>(vendorMaster, vendorsGift);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Gifts([Bind(Prefix = "Item2")] VendorsGift vendorsGift, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid)
        {
            string fileName = string.Empty;
            VendorGiftService vendorGiftService = new VendorGiftService();
            vendorsGift.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsGift = vendorGiftService.AddGift(vendorsGift, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsGift.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Gift_" + vendorsGift.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsGift.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Gifts", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Gifts", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsGift.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsGift.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsGift = vendorGiftService.UpdatesGift(vendorsGift, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsGift.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Gift_" + vendorsGift.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsGift.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "InvitationCards" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public ActionResult InvitationCards(string id, [Bind(Prefix = "Item2")] VendorsInvitationCard vendorsInvitationCard, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorInvitationCardsService vendorInvitationCardsService = new VendorInvitationCardsService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("InvitationCards", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("InvitationCards", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsInvitationCard = vendorInvitationCardsService.GetVendorInvitationCard(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsInvitationCard>(vendorMaster, vendorsInvitationCard);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult InvitationCards([Bind(Prefix = "Item2")] VendorsInvitationCard vendorsInvitationCard, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid)
        {
            string fileName = string.Empty;
            VendorInvitationCardsService vendorInvitationCardsService = new VendorInvitationCardsService();
            vendorsInvitationCard.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsInvitationCard = vendorInvitationCardsService.AddInvitationCard(vendorsInvitationCard, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsInvitationCard.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "InvitationCard_" + vendorsInvitationCard.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsInvitationCard.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("InvitationCards", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("InvitationCards", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsInvitationCard.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsInvitationCard.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsInvitationCard = vendorInvitationCardsService.UpdatesInvitationCard(vendorsInvitationCard, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsInvitationCard.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "InvitationCard_" + vendorsInvitationCard.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsInvitationCard.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "InvitationCards" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public ActionResult Photography(string id, [Bind(Prefix = "Item2")] VendorsPhotography vendorsPhotography, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Photography", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Photography", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsPhotography = vendorPhotographyService.GetVendorPhotography(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsPhotography>(vendorMaster, vendorsPhotography);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Photography([Bind(Prefix = "Item2")] VendorsPhotography vendorsPhotography, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid)
        {
            string fileName = string.Empty;
            VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
            vendorsPhotography.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsPhotography = vendorPhotographyService.AddPhotography(vendorsPhotography, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsPhotography.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();

                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Photography_" + vendorsPhotography.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsPhotography.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Photography", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Photography", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsPhotography.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsPhotography.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsPhotography = vendorPhotographyService.UpdatesPhotography(vendorsPhotography, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsPhotography.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Photography_" + vendorsPhotography.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsPhotography.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Photography" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public ActionResult TravelAccomodation(string id, [Bind(Prefix = "Item2")] VendorsTravelandAccomodation vendorsTravelandAccomodation, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorTravelAndAccomadationService vendorTravelandAccomodationsService = new VendorTravelAndAccomadationService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("TravelAccomodation", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("TravelAccomodation", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsTravelandAccomodation = vendorTravelandAccomodationsService.GetVendorTravelandAccomodation(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsTravelandAccomodation>(vendorMaster, vendorsTravelandAccomodation);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult TravelAccomodation([Bind(Prefix = "Item2")] VendorsTravelandAccomodation vendorsTravelandAccomodation, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid)
        {
            string fileName = string.Empty;
            VendorTravelAndAccomadationService vendorTravelAndAccomadationService = new VendorTravelAndAccomadationService();
            vendorsTravelandAccomodation.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsTravelandAccomodation = vendorTravelAndAccomadationService.AddTravelAndAccomadation(vendorsTravelandAccomodation, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsTravelandAccomodation.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "TravelandAccomodation_" + vendorsTravelandAccomodation.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsTravelandAccomodation.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("TravelAccomodation", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("TravelAccomodation", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                //long masterid = vendorsTravelandAccomodation.VendorMasterId = vendorMaster.Id = long.Parse(id);
                //vendorMaster.UpdatedBy = vendorsTravelandAccomodation.UpdatedBy = ValidUserUtility.ValidUser();
                long masterid =  long.Parse(id);
                vendorsTravelandAccomodation.VendorMasterId = masterid;
                vendorMaster.Id = masterid;
                vendorsTravelandAccomodation = vendorTravelAndAccomadationService.UpdateTravelandAccomodation(vendorsTravelandAccomodation, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsTravelandAccomodation.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "TravelandAccomodation_" + vendorsTravelandAccomodation.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsTravelandAccomodation.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public ActionResult WeddingCollection(string id, [Bind(Prefix = "Item2")] VendorsWeddingCollection vendorsWeddingCollection, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorWeddingCollectionService vendorWeddingCollectionsService = new VendorWeddingCollectionService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("WeddingCollection", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("WeddingCollection", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsWeddingCollection = vendorWeddingCollectionsService.GetVendorWeddingCollection(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsWeddingCollection>(vendorMaster, vendorsWeddingCollection);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult WeddingCollection([Bind(Prefix = "Item2")] VendorsWeddingCollection vendorsWeddingCollection, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid)
        {
            string fileName = string.Empty;
            VendorWeddingCollectionService vendorWeddingCollectionService = new VendorWeddingCollectionService();
            vendorsWeddingCollection.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsWeddingCollection = vendorWeddingCollectionService.AddWeddingCollection(vendorsWeddingCollection, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsWeddingCollection.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "WeddingCollection_" + vendorsWeddingCollection.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsWeddingCollection.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("WeddingCollection", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("WeddingCollection", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsWeddingCollection.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsWeddingCollection.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsWeddingCollection = vendorWeddingCollectionService.UpdateWeddingCollection(vendorsWeddingCollection, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsWeddingCollection.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "WeddingCollection_" + vendorsWeddingCollection.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsWeddingCollection.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "WeddingCollection" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public ActionResult Venue(string id, [Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster,string src,string op,string vid)
        {
            VendorVenueService vendorVenueService = new VendorVenueService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("venue", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("venue", "createvendor") + "'</script>");
                }
            }
            if (id!=null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id),long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorVenue = vendorVenueService.GetVendorVenue(long.Parse(id),long.Parse(vid)); 
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id)); 
                var a = new Tuple<Vendormaster, VendorVenue>(vendorMaster, vendorVenue);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Venue([Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file,string Command,string id, string vid)
        {
            string fileName = string.Empty;
            VendorVenueService vendorVenueService = new VendorVenueService();
            vendorVenue.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorVenue = vendorVenueService.AddVenue(vendorVenue, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorVenue.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Venue_" + vendorVenue.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorVenue.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Venue", "CreateVendor",new { id=vendorVenue.Id}) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Venue", "CreateVendor") + "'</script>");
                }
            }
            if (Command=="update")
            {
                long masterid = vendorVenue.VendorMasterId  = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorVenue.UpdatedBy = ValidUserUtility.ValidUser();
                vendorVenue = vendorVenueService.UpdateVenue(vendorVenue, vendorMaster, masterid,long.Parse(vid));
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorVenue.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_','.');
                        imageno = int.Parse(y[2]);
                    }
                    
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Venue_" + vendorVenue.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorVenue.Id != 0 || vendorImage.ImageId != 0) 
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            if (Command == "add")
            {
                vendorMaster.Id = long.Parse(id);
                vendorVenue = vendorVenueService.AddNewVenue(vendorVenue, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorVenue.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Venue_" + vendorVenue.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorVenue.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Venue", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Venue", "CreateVendor") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult Others(string id, [Bind(Prefix = "Item2")] VendorsOther vendorsOther, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op,string vid)
        {
            VendorOthersService vendorOthersService = new VendorOthersService();
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src);
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Others", "createvendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Others", "createvendor") + "'</script>");
                }
            }
            if (id != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsOther = vendorOthersService.GetVendorOther(long.Parse(id));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsOther>(vendorMaster, vendorsOther);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                }
                return View(a);
            }
            else
            {
                ViewBag.operation = 1;
                ViewBag.imagescount = 10;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Others([Bind(Prefix = "Item2")] VendorsOther vendorsOther, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid)
        {
            string fileName = string.Empty;
            string ImagesURL = string.Empty;
            VendorOthersService vendorOthersService = new VendorOthersService();
            vendorsOther.UpdatedBy = ValidUserUtility.ValidUser();
            vendorMaster.UpdatedBy = ValidUserUtility.ValidUser();
            if (Command == "Save")
            {
                vendorsOther = vendorOthersService.AddOther(vendorsOther, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsOther.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //const string imagepath = @"/vendorimages";
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Others_" + vendorsOther.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsOther.Id != 0 && vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Others", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Others", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                long masterid = vendorsOther.VendorMasterId = vendorMaster.Id = long.Parse(id);
                vendorMaster.UpdatedBy = vendorsOther.UpdatedBy = ValidUserUtility.ValidUser();
                vendorsOther = vendorOthersService.UpdateOther(vendorsOther, vendorMaster, masterid);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsOther.Id;
                int imagecount = 10;
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    int imageno = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string x = list[i].ToString();
                        string[] y = x.Split('_', '.');
                        imageno = int.Parse(y[2]);
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Other_" + vendorsOther.VendorMasterId + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }
                    }
                    if (vendorsOther.Id != 0 || vendorImage.ImageId != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                    }
                }
                else
                {
                    ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                    ViewBag.imagescount = imagecount - list.Count;
                    ViewData["error"] = "You Have Crossed Images Limit";
                }
            }
            return View();
        }
        public JsonResult checkemail(string emailid)
        {
            int query = vendorMasterService.checkemail(emailid);
            if (query != 0)
            {
                return Json("exists", JsonRequestBehavior.AllowGet);
            }
            return Json("valid", JsonRequestBehavior.AllowGet);
        }

        //public ActionResult images(string ids)
        //{
        //    var images = vendorImageService.GetVendorImagesService(long.Parse(ids));
        //    return Json(images);
        //}

        //public JsonResult DeleteImage(string imgsrc, string ids)
        //{
            //var images = entities.TempVenueDetails.Where(s => s.ID == id).SingleOrDefault();
            //string path = images.Images.ToString().TrimEnd(',') + ",";
            //images.Images = path.Replace(imgsrc + ",", "");
            //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Server.MapPath(@"~/CSSWeb/Uploadedimages"));
            //FileInfo oFileInfo = new FileInfo(Server.MapPath(@"~/CSSWeb/Uploadedimages/" + imgsrc));
            //oFileInfo.Delete();
            //oFileInfo.Refresh();
            //entities.SaveChanges();
        //    return Json("success");
        //}
    }
}