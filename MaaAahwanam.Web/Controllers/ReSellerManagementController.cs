using MaaAahwanam.Models;
using MaaAahwanam.Repository;
using MaaAahwanam.Service;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MaaAahwanam.Web.Controllers
{
    public class ReSellerManagementController : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorVenueService vendorVenueService = new VendorVenueService();
        viewservicesservice viewservicesss = new viewservicesservice();
        newmanageuser newmanageuse = new newmanageuser();
        Vendormaster vendorMaster = new Vendormaster();
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        PartnerService partnerservice = new PartnerService();
        const string imagepath = @"/partnerdocs/";


        // GET: ReSellerManagement
        public ActionResult Index(string partid)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                string uid = user.UserId.ToString();
                string vemail = newmanageuse.Getusername(long.Parse(uid));
                vendorMaster = newmanageuse.GetVendorByEmail(vemail);
                string VendorId = vendorMaster.Id.ToString();
                string vid = vendorMaster.Id.ToString();

                ViewBag.masterid = VendorId;
                var resellers = partnerservice.GetPartners(VendorId);
                var resellerspack = partnerservice.getPartnerPackage(VendorId);
                var pkgs = viewservicesss.getvendorpkgs(VendorId).ToList();
                List<string> pppl = new List<string>();
                List<PartnerPackage> p = new List<PartnerPackage>();
                List<SPGETNpkg_Result> p1 = new List<SPGETNpkg_Result>();
                if (partid != "" && partid != null)
                {
                    var resellerspacklist = resellerspack.Where(m => m.PartnerID == long.Parse(partid)).ToList();
                    var pkglist = resellerspacklist.Select(m => m.packageid).ToList();
                    foreach (var item in pkgs)
                    {
                        if (pkglist.Contains(item.PackageID.ToString()))
                            p.AddRange(resellerspack.Where(m => m.packageid == item.PackageID.ToString()).ToList());
                        else
                            p1.AddRange(pkgs.Where(m=>m.PackageID == item.PackageID));
                    }
                    ViewBag.resellerpkg = p;
                    ViewBag.pkg = p1;
                    ViewBag.resellers = resellers.Where(m => m.PartnerID == long.Parse(partid)).FirstOrDefault();
                    ViewBag.resellersfiles = partnerservice.GetFiles(vid, partid);
                }
                ViewBag.package = pkgs;
                var venues = vendorVenueSignUpService.GetVendorVenue(long.Parse(vid)).ToList();
                var catering = vendorVenueSignUpService.GetVendorCatering(long.Parse(vid)).ToList();
                var photography = vendorVenueSignUpService.GetVendorPhotography(long.Parse(vid));
                var decorators = vendorVenueSignUpService.GetVendorDecorator(long.Parse(vid));
                var others = vendorVenueSignUpService.GetVendorOther(long.Parse(vid));
                ViewBag.venues = venues;
                ViewBag.catering = catering;
                ViewBag.photography = photography;
                ViewBag.decorators = decorators;
                ViewBag.others = others;
                ViewBag.partid = partid;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public JsonResult Index(Partner partner, string command, string partid)
        {

            partner.RegisteredDate = DateTime.Now.Date;
            partner.UpdatedDate = DateTime.Now.Date;
            partner.ExpiryDate = DateTime.Now.Date;
            if (command == "save") { partner = partnerservice.AddPartner(partner); }
            else if (command == "Update") { partner = partnerservice.UpdatePartner(partner, partid); }
            else if (command == "Update1") { partner = partnerservice.UpdatePartner(partner, partid); }
            var emailid = partner.emailid;
            var getpartner = partnerservice.getPartner(emailid);
            return Json(getpartner, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PartnerPackage(PartnerPackage partnerPackage, string command, string partid)
        {
            partnerPackage.RegisteredDate = DateTime.Now.Date;
            partnerPackage.UpdatedDate = DateTime.Now.Date;
            
            if (command == "save") { partnerPackage = partnerservice.addPartnerPackage(partnerPackage); }
            //else if (command == "Update") { partnerPackage = partnerservice.UpdatepartnerPackage(partnerPackage, partid); }

            //var emailid = partner.emailid;
            //var getpartner = partnerservice.getPartner(emailid);
            return Json(partnerPackage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Contacts(PartnerContact Partnercontact, string command, string partid)
        {
            Partnercontact.RegisteredDate = DateTime.Now;
            Partnercontact.UpdatedDate = DateTime.Now;
            if (command == "Update") { Partnercontact = partnerservice.UpdatePartnercontact(Partnercontact); }
            //else if (command == "Update") { partnerPackage = partnerservice.UpdatepartnerPackage(partnerPackage, partid); }

            //var emailid = partner.emailid;
            //var getpartner = partnerservice.getPartner(emailid);
            return Json(Partnercontact, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult UploadContractdoc(HttpPostedFileBase helpSectionImages, string command, string partid,string vid)
        {

            string fileName = string.Empty;
            string filename = string.Empty;
            //VendorImage vendorImage = new VendorImage();
            //Vendormaster vendorMaster = new Vendormaster();
            PartnerFile partnerFile = new PartnerFile();
            if (helpSectionImages != null)
            {
                string path = System.IO.Path.GetExtension(helpSectionImages.FileName);
                int imageno = 0;
                int imagecount = 2;
                var list = partnerservice.GetFiles(vid, partid);
                var resellers = partnerservice.GetPartners(vid);
                var reellers1 = resellers.Where(m => m.PartnerID == long.Parse(partid)).FirstOrDefault();

                if (list.Count <= imagecount && Request.Files.Count <= imagecount - list.Count)
                {
                    //getting max imageno
                    if (list.Count != 0)
                    {
                        string lastimage = list.OrderByDescending(m => m.FileName).FirstOrDefault().FileName;
                        var splitimage = lastimage.Split('_', '.');
                        imageno = int.Parse(splitimage[3]);
                    }
                    //Uploading images in db & folder
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + i + 1;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            filename = reellers1.PartnerName + "_" + vid + "_" + partid + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            partnerFile.FileName = filename;
                            //vendorImage.ImageType = type;//"Slider";
                            partnerFile.VendorID = vid;
                            partnerFile.PartnerID = partid;
                            partnerFile.UpdatedDate = DateTime.Now;
                            partnerFile.RegisteredDate = DateTime.Now;

                            // vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                            partnerFile = partnerservice.addPartnerfile(partnerFile);
                        }
                    }
                }
            }
            return Json(filename, JsonRequestBehavior.AllowGet);

            //Partnercontact.RegisteredDate = DateTime.Now;
            //Partnercontact.UpdatedDate = DateTime.Now;
            //if (command == "Update") { Partnercontact = partnerservice.UpdatePartnercontact(Partnercontact); }
            ////else if (command == "Update") { partnerPackage = partnerservice.UpdatepartnerPackage(partnerPackage, partid); }

            ////var emailid = partner.emailid;
            ////var getpartner = partnerservice.getPartner(emailid);
            //return Json(Partnercontact, JsonRequestBehavior.AllowGet);
        }

       
    }
}