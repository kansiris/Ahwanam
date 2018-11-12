using MaaAahwanam.Models;
using MaaAahwanam.Service;
using MaaAahwanam.Utility;
using MaaAahwanam.Web.Custom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class vendorController : Controller
    {
        VendorImageService vendorImageService = new VendorImageService();
        VendorSetupService vendorSetupService = new VendorSetupService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        DealService dealService = new DealService();
        const string imagepath = @"/vendorimages/";
        static string vendorid = "";

        // GET: Admin/vendor
        public ActionResult Index( string type)
        {
            //if (type == null) {
            //    ViewBag.VendorList = null;
            //}
            //else
            //{
            ViewBag.type = type;
            ViewBag.VendorList = vendorSetupService.AllVendorList(type);
            //}
            return View();
        }
        
        //public ActionResult AllVendors(string dropstatus, string vid, string command, string id, string type, [Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster)
        //{
        //    ViewBag.type = dropstatus;
        //    if (dropstatus != null && dropstatus != "")
        //    {
        //        ViewBag.VendorList = vendorSetupService.AllVendorList(dropstatus);
        //    }
        //    if (command == "Edit")
        //    {
        //        return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid });
        //    }
        //    if (command == "View")
        //    {
        //        return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "display" });
        //    }
        //    if (command == "Add New")
        //    {
        //        return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "add" });
        //    }
        //    if (command == "confirm")
        //    {
        //        TempData["confirm"] = 1;
        //        return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "confirm" });
        //    }
        //    return View();
        //}
        [HttpPost]
        public ActionResult SearchVendor(string searchvendor, string command, Vendormaster vendormaster, string pemail, string id)
        {
            UserLogin userlogin = new UserLogin();
            UserDetail userdetails = new UserDetail();
            //var details = vendorMasterService.SearchVendors();
            //var particularvendor = details.Where(m => m.BusinessName.Contains(searchvendor)).FirstOrDefault();
            //var VendorList = vendorMasterService.SearchVendors().Where(m => m.BusinessName.ToLower() == searchvendor.TrimEnd()).FirstOrDefault();
            var VendorList = vendorMasterService.SearchVendors().Where(m => m.BusinessName.ToLower().Contains(searchvendor.ToLower().TrimEnd())).FirstOrDefault();

            ViewBag.VendorList = VendorList;
            if (command == "Update")
            {
                if (pemail != vendormaster.EmailId)
                {
                    int query = vendorMasterService.checkemail(vendormaster.EmailId);
                    if (query != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Email ID Already Taken');location.href='/admin/Vendors/SearchVendor'</script>");
                    }
                }
                var updatedetails = vendormaster;
                VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
                vendormaster = vendorMasterService.GetVendor(long.Parse(id));
                vendormaster.EmailId = updatedetails.EmailId;
                vendormaster.ContactNumber = updatedetails.ContactNumber;
                vendormaster.ContactPerson = updatedetails.ContactPerson;
                vendormaster = vendorMasterService.UpdateVendorDetails(vendormaster, long.Parse(id)); // Updating Email ID in Vendor Master Table
                //userlogin.UserName = pemail;
                //userlogin = venorVenueSignUpService.GetUserLogdetails(userlogin);
                userlogin = venorVenueSignUpService.GetParticularUserdetails(pemail);
                userlogin.UserName = vendormaster.EmailId;
                userlogin = userLoginDetailsService.UpdateUserName(userlogin, pemail); // Updating Email ID in User Login Table
                //userdetails = userLoginDetailsService.GetUserDetailsByEmail(pemail);
                userdetails = userLoginDetailsService.GetUser(int.Parse(userlogin.UserLoginId.ToString()));
                userdetails.AlternativeEmailID = vendormaster.EmailId;
                userdetails.FirstName = vendormaster.ContactPerson;
                userdetails.UserPhone = vendormaster.ContactNumber;
                //userdetails = userLoginDetailsService.UpdateUserDetailEmail(userdetails, pemail); // Updating Email ID in User Detail Table
                userdetails = userLoginDetailsService.UpdateUserdetails(userdetails, userlogin.UserLoginId);
                return Content("<script language='javascript' type='text/javascript'>alert('Info Updated');location.href='/admin/Vendors/SearchVendor'</script>");
            }
            if (command == "Email")
            {
                VenorVenueSignUpService venorVenueSignUpService = new VenorVenueSignUpService();
                userlogin.UserName = vendormaster.EmailId;
                var userResponse = venorVenueSignUpService.GetParticularUserdetails(pemail);
                if (userResponse != null)
                {
                    string emailid = userlogin.UserName;
                    if (userResponse.ActivationCode == null)
                    {
                        userlogin = userResponse;
                        userlogin.ActivationCode = Guid.NewGuid().ToString();
                        userlogin = userLoginDetailsService.UpdateUserName(userlogin, emailid);
                        userResponse.ActivationCode = userlogin.ActivationCode;
                    }

                    string activationcode = userResponse.ActivationCode;
                    int userid = Convert.ToInt32(userResponse.UserLoginId);
                    var userdetail = userLoginDetailsService.GetUser(userid);
                    string name = userdetail.FirstName;

                    // vendor mail activation  begin

                    string mailid = userlogin.UserName;
                    var userR = userResponse;//venorVenueSignUpService.GetUserdetails(mailid);
                    string pas1 = userR.Password;
                    string url = Request.Url.Scheme + "://" + Request.Url.Authority + "/NUserRegistration/ActivateEmail1?ActivationCode=" + activationcode + "&&Email=" + emailid;
                    FileInfo File = new FileInfo(Server.MapPath("/mailtemplate/WelcomeMessage.html"));
                    string readFile = File.OpenText().ReadToEnd();
                    readFile = readFile.Replace("[ActivationLink]", url);
                    readFile = readFile.Replace("[name]", name);
                    readFile = readFile.Replace("[username]", mailid);
                    readFile = readFile.Replace("[pass1]", pas1);
                    string txtto = userlogin.UserName;
                    string txtmessage = readFile;//readFile + body;
                    string subj = "Welcome to Ahwanam";

                    // vendor mail activation  end

                    EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
                    emailSendingUtility.Email_maaaahwanam(txtto, txtmessage, subj);
                    return Content("<script language='javascript' type='text/javascript'>alert('Invitation Sent to " + txtto + "');location.href='/admin/Vendors/SearchVendor'</script>");
                }
            }
            return View();
        }

        //excel download
        public ActionResult download(string dropstatus)
        {

            if (dropstatus != null)
            {
                var modelCust1 = vendorSetupService.AllVendorList(dropstatus);
                var gv = new GridView();
                gv.DataSource = modelCust1;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=ahwanam.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
                return Content("<script>alert('Excel sheet is downloaded');location.href='/Admin/Vendors/AllVendors'</script>");
            }


            return Content("<script> alert('please the vendortype')</script>");

        }



        public ActionResult BeautyServices(string id, [Bind(Prefix = "Item2")] VendorsBeautyService vendorsBeautyService, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorBeautyServicesService vendorBeautyServicesService = new VendorBeautyServicesService();
            if (vid != null)
            {
                vendorid = vid;
            }
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("BeautyServices", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("BeautyServices", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null & vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsBeautyService = vendorBeautyServicesService.GetVendorBeautyService(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsBeautyService, Deal>(vendorMaster, vendorsBeautyService, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
        public ActionResult BeautyServices([Bind(Prefix = "Item2")] VendorsBeautyService vendorsBeautyService, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorBeautyServicesService vendorBeautyServicesService = new VendorBeautyServicesService();
            //vendorsBeautyService.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsBeautyService.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
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
                            var filename = "BeautyService_" + vendorsBeautyService.VendorMasterId + "_" + vendorsBeautyService.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsBeautyService.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("BeautyServices", "CreateVendor", new { id = vendorsBeautyService.Id }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("BeautyServices", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                }
                else
                {
                    vendorsBeautyService.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    //var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
                    long masterid = vendorsBeautyService.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorsBeautyService.UpdatedBy = ValidUserUtility.ValidUser();//user.UserId;
                    vendorsBeautyService = vendorBeautyServicesService.UpdatesBeautyService(vendorsBeautyService, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "BeautyService_" + vendorsBeautyService.VendorMasterId + "_" + vendorsBeautyService.Id + "_" + j + path;
                                fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                                file1.SaveAs(fileName);
                                vendorImage.ImageName = filename;
                                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();//user.UserId;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsBeautyService.Id != 0 || vendorImage.ImageId != 0)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "InvitationCards" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }

            if (Command == "Active")
            {
                vendorsBeautyService.Status = vendorMaster.Status = Command;
                vendorsBeautyService = vendorBeautyServicesService.ActivationBeautyService(vendorsBeautyService, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsBeautyService.Status = vendorMaster.Status = Command;
                vendorsBeautyService = vendorBeautyServicesService.ActivationBeautyService(vendorsBeautyService, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "add")
            {
                vendorsBeautyService.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorsBeautyService = vendorBeautyServicesService.AddNewBeautyService(vendorsBeautyService, vendorMaster);
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
                            var filename = "BeautyService_" + vendorsBeautyService.VendorMasterId + "_" + vendorsBeautyService.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsBeautyService.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "BeautyServices";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult Catering(string id, [Bind(Prefix = "Item2")] VendorsCatering vendorsCatering, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {

            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorCateringService vendorCateringService = new VendorCateringService();
            if (vid != null)
            {
                vendorid = vid;
            }
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Catering", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Catering", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsCatering = vendorCateringService.GetVendorCatering(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsCatering, Deal>(vendorMaster, vendorsCatering, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
        public ActionResult Catering([Bind(Prefix = "Item2")] VendorsCatering vendorsCatering, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorCateringService vendorCateringService = new VendorCateringService();
            //vendorsCatering.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsCatering.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
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
                            var filename = "Catering_" + vendorsCatering.VendorMasterId + "_" + vendorsCatering.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsCatering.Id != 0 || vendorImage.ImageId != 0)
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
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                }
                else
                {
                    vendorsCatering.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    long masterid = vendorsCatering.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorsCatering.UpdatedBy = ValidUserUtility.ValidUser();
                    vendorsCatering = vendorCateringService.UpdatesCatering(vendorsCatering, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "Catering_" + vendorsCatering.VendorMasterId + "_" + vendorsCatering.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsCatering.Id != 0 || vendorImage.ImageId != 0)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "InvitationCards" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }

            if (Command == "Active")
            {
                vendorsCatering.Status = vendorMaster.Status = Command;
                vendorsCatering = vendorCateringService.activeCatering(vendorsCatering, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsCatering.Status = vendorMaster.Status = Command;
                vendorsCatering = vendorCateringService.InactiveCatering(vendorsCatering, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "add")
            {
                vendorsCatering.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorsCatering = vendorCateringService.AddNewCatering(vendorsCatering, vendorMaster);
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
                            var filename = "Catering_" + vendorsCatering.VendorMasterId + "_" + vendorsCatering.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsCatering.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "Catering";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult Decorator(string id, [Bind(Prefix = "Item2")] VendorsDecorator vendorsDecorator, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
            if (vid != null)
            {
                vendorid = vid;
            }

            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Decorator", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Decorator", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsDecorator = vendorDecoratorService.GetVendorDecorator(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsDecorator, Deal>(vendorMaster, vendorsDecorator, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
        public ActionResult Decorator([Bind(Prefix = "Item2")] VendorsDecorator vendorsDecorator, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
            //vendorsDecorator.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsDecorator.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
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
                            var filename = "Decorator_" + vendorsDecorator.VendorMasterId + "_" + vendorsDecorator.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsDecorator.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Decorator", "CreateVendor") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Decorator", "CreateVendor") + "'</script>");
                }
            }

            if (Command == "Active")
            {
                vendorsDecorator.Status = vendorMaster.Status = Command;
                vendorsDecorator = vendorDecoratorService.activeDecorator(vendorsDecorator, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsDecorator.Status = vendorMaster.Status = Command;
                vendorsDecorator = vendorDecoratorService.InactiveDecorator(vendorsDecorator, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "update")
            {
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                }
                else
                {
                    vendorsDecorator.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    long masterid = vendorsDecorator.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorsDecorator.UpdatedBy = ValidUserUtility.ValidUser();
                    vendorsDecorator = vendorDecoratorService.UpdateDecorator(vendorsDecorator, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "Decorator_" + vendorsDecorator.VendorMasterId + "_" + vendorsDecorator.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsDecorator.Id != 0 || vendorImage.ImageId != 0)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "InvitationCards" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }
            if (Command == "add")
            {
                vendorsDecorator.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorsDecorator = vendorDecoratorService.AddNewDecorator(vendorsDecorator, vendorMaster);
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
                            var filename = "Decorator_" + vendorsDecorator.VendorMasterId + "_" + vendorsDecorator.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsDecorator.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "Decorator";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult Entertainment(string id, [Bind(Prefix = "Item2")] VendorsEntertainment vendorsEntertainment, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorEntertainmentService vendorEntertainmentService = new VendorEntertainmentService();
            if (vid != null)
            {
                vendorid = vid;
            }
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Entertainment", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Entertainment", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsEntertainment = vendorEntertainmentService.GetVendorEntertainment(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsEntertainment, Deal>(vendorMaster, vendorsEntertainment, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
        public ActionResult Entertainment([Bind(Prefix = "Item2")] VendorsEntertainment vendorsEntertainment, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorEntertainmentService vendorEntertainmentService = new VendorEntertainmentService();
            //vendorsEntertainment.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsEntertainment.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
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
                            var filename = "Entertainment_" + vendorsEntertainment.VendorMasterId + "_" + vendorsEntertainment.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsEntertainment.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Entertainment", "CreateVendor", new { id = vendorsEntertainment.Id }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Entertainment", "CreateVendor") + "'</script>");
                }
            }

            if (Command == "Active")
            {
                vendorsEntertainment.Status = vendorMaster.Status = Command;
                vendorsEntertainment = vendorEntertainmentService.activationEntertainment(vendorsEntertainment, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsEntertainment.Status = vendorMaster.Status = Command;
                vendorsEntertainment = vendorEntertainmentService.activationEntertainment(vendorsEntertainment, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }


            if (Command == "update")
            {
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                }
                else
                {
                    vendorsEntertainment.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    long masterid = vendorsEntertainment.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorsEntertainment.UpdatedBy = ValidUserUtility.ValidUser();
                    vendorsEntertainment = vendorEntertainmentService.UpdateEntertainment(vendorsEntertainment, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "Entertainment_" + vendorsEntertainment.VendorMasterId + "_" + vendorsEntertainment.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsEntertainment.Id != 0 || vendorImage.ImageId != 0)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "InvitationCards" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }
            if (Command == "add")
            {
                vendorsEntertainment.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorsEntertainment = vendorEntertainmentService.AddNewEntertainment(vendorsEntertainment, vendorMaster);
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
                            var filename = "Entertainment_" + vendorsEntertainment.VendorMasterId + "_" + vendorsEntertainment.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsEntertainment.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "Entertainment";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult EventOrganisers(string id, [Bind(Prefix = "Item2")] VendorsEventOrganiser vendorsEventOrganiser, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorEventOrganiserService vendorEventOrganiserService = new VendorEventOrganiserService();
            if (vid != null)
            {
                vendorid = vid;
            }

            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("EventOrganisers", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("EventOrganisers", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsEventOrganiser = vendorEventOrganiserService.GetVendorEventOrganiser(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsEventOrganiser, Deal>(vendorMaster, vendorsEventOrganiser, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
        public ActionResult EventOrganisers([Bind(Prefix = "Item2")] VendorsEventOrganiser vendorsEventOrganisers, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorEventOrganiserService vendorEventOrganiserService = new VendorEventOrganiserService();
            //vendorsEventOrganisers.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsEventOrganisers.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
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
                            var filename = "EventOrganisers_" + vendorsEventOrganisers.VendorMasterId + "_" + vendorsEventOrganisers.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsEventOrganisers.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("EventOrganisers", "CreateVendor", new { id = vendorsEventOrganisers.Id }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("EventOrganisers", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                }
                else
                {
                    vendorsEventOrganisers.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    long masterid = vendorsEventOrganisers.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorsEventOrganisers.UpdatedBy = ValidUserUtility.ValidUser();
                    vendorsEventOrganisers = vendorEventOrganiserService.UpdateEventOrganiser(vendorsEventOrganisers, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "EventOrganiser_" + vendorsEventOrganisers.VendorMasterId + "_" + vendorsEventOrganisers.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsEventOrganisers.Id != 0 || vendorImage.ImageId != 0)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "InvitationCards" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }
            if (Command == "Active")
            {
                vendorsEventOrganisers.Status = vendorMaster.Status = Command;
                vendorsEventOrganisers = vendorEventOrganiserService.activationOrganiser(vendorsEventOrganisers, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsEventOrganisers.Status = vendorMaster.Status = Command;
                vendorsEventOrganisers = vendorEventOrganiserService.activationOrganiser(vendorsEventOrganisers, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }
            if (Command == "add")
            {
                vendorsEventOrganisers.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorsEventOrganisers = vendorEventOrganiserService.AddNewEventOrganiser(vendorsEventOrganisers, vendorMaster);
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
                            var filename = "EventOrganiser_" + vendorsEventOrganisers.VendorMasterId + "_" + vendorsEventOrganisers.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsEventOrganisers.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "EventOrganisers";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult Gifts(string id, [Bind(Prefix = "Item2")] VendorsGift vendorsGift, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorGiftService vendorGiftService = new VendorGiftService();
            if (vid != null)
            {
                vendorid = vid;
            }
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Gifts", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Gifts", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsGift = vendorGiftService.GetVendorGift(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsGift, Deal>(vendorMaster, vendorsGift, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
        public ActionResult Gifts([Bind(Prefix = "Item2")] VendorsGift vendorsGift, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorGiftService vendorGiftService = new VendorGiftService();
            //vendorsGift.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsGift.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
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
                            var filename = "Gift_" + vendorsGift.VendorMasterId + "_" + vendorsGift.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsGift.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Gifts", "CreateVendor", new { id = vendorsGift.Id }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Gifts", "CreateVendor") + "'</script>");
                }
            }

            if (Command == "Active")
            {
                vendorsGift.Status = vendorMaster.Status = Command;
                vendorsGift = vendorGiftService.activationGift(vendorsGift, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsGift.Status = vendorMaster.Status = Command;
                vendorsGift = vendorGiftService.activationGift(vendorsGift, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "update")
            {
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                }
                else
                {
                    vendorsGift.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    long masterid = vendorsGift.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorsGift.UpdatedBy = ValidUserUtility.ValidUser();
                    vendorsGift = vendorGiftService.UpdatesGift(vendorsGift, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "Gift_" + vendorsGift.VendorMasterId + "_" + vendorsGift.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsGift.Id != 0 || vendorImage.ImageId != 0)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "InvitationCards" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }
            if (Command == "add")
            {
                vendorsGift.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorsGift = vendorGiftService.AddNewGift(vendorsGift, vendorMaster);
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
                            var filename = "Gift_" + vendorsGift.VendorMasterId + "_" + vendorsGift.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsGift.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "Gifts";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult InvitationCards(string id, [Bind(Prefix = "Item2")] VendorsInvitationCard vendorsInvitationCard, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorInvitationCardsService vendorInvitationCardsService = new VendorInvitationCardsService();
            if (vid != null)
            {
                vendorid = vid;
            }
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("InvitationCards", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("InvitationCards", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsInvitationCard = vendorInvitationCardsService.GetVendorInvitationCard(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsInvitationCard, Deal>(vendorMaster, vendorsInvitationCard, deal);
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
        public ActionResult InvitationCards([Bind(Prefix = "Item2")] VendorsInvitationCard vendorsInvitationCard, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorInvitationCardsService vendorInvitationCardsService = new VendorInvitationCardsService();
            //vendorsInvitationCard.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsInvitationCard.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
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
                            var filename = "InvitationCard_" + vendorsInvitationCard.VendorMasterId + "_" + vendorsInvitationCard.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsInvitationCard.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("InvitationCards", "CreateVendor", new { id = vendorsInvitationCard.Id }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("InvitationCards", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                    return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                }
                else
                {
                    vendorsInvitationCard.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    long masterid = vendorsInvitationCard.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorsInvitationCard.UpdatedBy = ValidUserUtility.ValidUser();
                    vendorsInvitationCard = vendorInvitationCardsService.UpdatesInvitationCard(vendorsInvitationCard, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "InvitationCard_" + vendorsInvitationCard.VendorMasterId + "_" + vendorsInvitationCard.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsInvitationCard.Id != 0 || vendorImage.ImageId != 0)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "InvitationCards" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }

            }
            if (Command == "add")
            {
                vendorMaster.Id = long.Parse(id);
                vendorsInvitationCard = vendorInvitationCardsService.AddNewInvitationCard(vendorsInvitationCard, vendorMaster);
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
                            var filename = "InvitationCard_" + vendorsInvitationCard.VendorMasterId + "_" + vendorsInvitationCard.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsInvitationCard.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }

            if (Command == "Active")
            {
                vendorsInvitationCard.Status = vendorsInvitationCard.Status = Command;
                vendorsInvitationCard = vendorInvitationCardsService.activationInvitationCard(vendorsInvitationCard, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsInvitationCard.Status = vendorMaster.Status = Command;
                vendorsInvitationCard = vendorInvitationCardsService.activationInvitationCard(vendorsInvitationCard, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "InvitationCards";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult Photography(string id, [Bind(Prefix = "Item2")] VendorsPhotography vendorsPhotography, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {

            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
            if (vid != null)
            {
                vendorid = vid;
            }
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Photography", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Photography", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsPhotography = vendorPhotographyService.GetVendorPhotography(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsPhotography, Deal>(vendorMaster, vendorsPhotography, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
        public ActionResult Photography([Bind(Prefix = "Item2")] VendorsPhotography vendorsPhotography, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
            //vendorsPhotography.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsPhotography.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorsPhotography = vendorPhotographyService.AddPhotography(vendorsPhotography, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsPhotography.Id;
                vendorImage.UpdatedBy = user.UserId;
                //if (deal != null)
                //{
                //    deal.VendorType = vendorVenue.VenueType;
                //    deal.VendorId = vendorMaster.Id;
                //    deal.VendorSubId = vendorVenue.Id;
                //    deal = dealService.AddDealService(deal);
                //}
                if (Request.Files.Count <= 10)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = i + 1;

                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "Photography_" + vendorsPhotography.VendorMasterId + "_" + vendorsPhotography.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsPhotography.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Photography", "CreateVendor", new { id = vendorsPhotography.Id }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Photography", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {

                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                    return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                }
                else
                {
                    long masterid = vendorsPhotography.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorsPhotography.UpdatedBy = user.UserId;
                    vendorsPhotography = vendorPhotographyService.UpdatesPhotography(vendorsPhotography, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "Photography_" + vendorsPhotography.VendorMasterId + "_" + vendorsPhotography.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsPhotography.Id != 0 || vendorImage.ImageId != 0)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Photography" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }
            if (Command == "add")
            {
                vendorsPhotography.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorsPhotography = vendorPhotographyService.AddNewPhotography(vendorsPhotography, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsPhotography.Id;
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
                            var filename = "Photography_" + vendorsPhotography.VendorMasterId + "_" + vendorsPhotography.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsPhotography.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Active")
            {
                vendorsPhotography.Status = vendorMaster.Status = Command;
                vendorsPhotography = vendorPhotographyService.ActivePhotography(vendorsPhotography, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsPhotography.Status = vendorMaster.Status = Command;
                vendorsPhotography = vendorPhotographyService.InActivePhotography(vendorsPhotography, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "Add Deal")
            {
                deal.VendorType = "Photography";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult TravelAccomodation(string id, [Bind(Prefix = "Item2")] VendorsTravelandAccomodation vendorsTravelandAccomodation, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorTravelAndAccomadationService vendorTravelandAccomodationsService = new VendorTravelAndAccomadationService();
            if (vid != null)
            {
                vendorid = vid;
            }

            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("TravelAccomodation", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("TravelAccomodation", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsTravelandAccomodation = vendorTravelandAccomodationsService.GetVendorTravelandAccomodation(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsTravelandAccomodation, Deal>(vendorMaster, vendorsTravelandAccomodation, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
        public ActionResult TravelAccomodation([Bind(Prefix = "Item2")] VendorsTravelandAccomodation vendorsTravelandAccomodation, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, HttpPostedFileBase file, string Command, string id, string vid, string d, [Bind(Prefix = "Item3")]Deal deal)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorTravelAndAccomadationService vendorTravelAndAccomadationService = new VendorTravelAndAccomadationService();
            //vendorsTravelandAccomodation.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsTravelandAccomodation.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorsTravelandAccomodation = vendorTravelAndAccomadationService.AddTravelAndAccomadation(vendorsTravelandAccomodation, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsTravelandAccomodation.Id;
                vendorImage.UpdatedBy = ValidUserUtility.ValidUser();
                //if (deal != null)
                //{
                //    deal.VendorType = vendorVenue.VenueType;
                //    deal.VendorId = vendorMaster.Id;
                //    deal.VendorSubId = vendorVenue.Id;
                //    deal = dealService.AddDealService(deal);
                //}
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
                            var filename = "TravelandAccomodation_" + vendorsTravelandAccomodation.VendorMasterId + "_" + vendorsTravelandAccomodation.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsTravelandAccomodation.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("TravelAccomodation", "CreateVendor", new { id = vendorsTravelandAccomodation.Id }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("TravelAccomodation", "CreateVendor") + "'</script>");
                }
            }

            if (Command == "Active")
            {
                vendorsTravelandAccomodation.Status = vendorMaster.Status = Command;
                vendorsTravelandAccomodation = vendorTravelAndAccomadationService.activationTravelandAccomodation(vendorsTravelandAccomodation, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsTravelandAccomodation.Status = vendorMaster.Status = Command;
                vendorsTravelandAccomodation = vendorTravelAndAccomadationService.activationTravelandAccomodation(vendorsTravelandAccomodation, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }


            if (Command == "update")
            {
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                    return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                }
                else
                {
                    vendorsTravelandAccomodation.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    //long masterid = vendorsTravelandAccomodation.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    //vendorMaster.UpdatedBy = vendorsTravelandAccomodation.UpdatedBy = ValidUserUtility.ValidUser();
                    long masterid = long.Parse(id);
                    vendorsTravelandAccomodation.VendorMasterId = masterid;
                    vendorMaster.Id = masterid;
                    vendorsTravelandAccomodation = vendorTravelAndAccomadationService.UpdateTravelandAccomodation(vendorsTravelandAccomodation, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "TravelandAccomodation_" + vendorsTravelandAccomodation.VendorMasterId + "_" + vendorsTravelandAccomodation.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsTravelandAccomodation.Id != 0 || vendorImage.ImageId != 0)
                            {

                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Venue" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }
            if (Command == "add")
            {
                vendorsTravelandAccomodation.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorsTravelandAccomodation = vendorTravelAndAccomadationService.AddNewTravelandAccomodation(vendorsTravelandAccomodation, vendorMaster);
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
                            var filename = "TravelandAccomodation_" + vendorsTravelandAccomodation.VendorMasterId + "_" + vendorsTravelandAccomodation.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsTravelandAccomodation.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "TravelAccomodation";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult WeddingCollection(string id, [Bind(Prefix = "Item2")] VendorsWeddingCollection vendorsWeddingCollection, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, string src, string op, string vid)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorWeddingCollectionService vendorWeddingCollectionsService = new VendorWeddingCollectionService();
            if (vid != null)
            {
                vendorid = vid;
            }
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("WeddingCollection", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("WeddingCollection", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsWeddingCollection = vendorWeddingCollectionsService.GetVendorWeddingCollection(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                var a = new Tuple<Vendormaster, VendorsWeddingCollection>(vendorMaster, vendorsWeddingCollection);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
                            var filename = "WeddingCollection_" + vendorsWeddingCollection.VendorMasterId + "_" + vendorsWeddingCollection.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsWeddingCollection.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("WeddingCollection", "CreateVendor", new { id = vendorsWeddingCollection.Id }) + "'</script>");
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
                vendorsWeddingCollection = vendorWeddingCollectionService.UpdateWeddingCollection(vendorsWeddingCollection, vendorMaster, masterid, long.Parse(vid));
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
                        if (y[3] == "jpg")
                        {
                            imageno = int.Parse(y[2]);
                        }
                        else
                        {
                            imageno = int.Parse(y[3]);
                        }
                    }

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        int j = imageno + 1 + i;
                        var file1 = Request.Files[i];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            string path = System.IO.Path.GetExtension(file.FileName);
                            var filename = "WeddingCollection_" + vendorsWeddingCollection.VendorMasterId + "_" + vendorsWeddingCollection.Id + "_" + j + path;
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
                    if (file != null)
                    {
                        ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                        ViewBag.imagescount = imagecount - list.Count;
                        ViewData["error"] = "You Have Crossed Images Limit";
                    }
                    else
                    {
                        if (vendorsWeddingCollection.Id != 0 || vendorImage.ImageId != 0)
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "WeddingCollection" }
                        }
                        else
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                        }
                    }
                }
            }

            if (Command == "Active")
            {
                vendorsWeddingCollection.Status = vendorMaster.Status = Command;
                vendorsWeddingCollection = vendorWeddingCollectionService.activateWeddingCollection(vendorsWeddingCollection, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsWeddingCollection.Status = vendorMaster.Status = Command;
                vendorsWeddingCollection = vendorWeddingCollectionService.activateWeddingCollection(vendorsWeddingCollection, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "add")
            {
                vendorMaster.Id = long.Parse(id);
                vendorsWeddingCollection = vendorWeddingCollectionService.AddNewWeddingCollection(vendorsWeddingCollection, vendorMaster);
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
                            var filename = "WeddingCollection_" + vendorsWeddingCollection.VendorMasterId + "_" + vendorsWeddingCollection.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsWeddingCollection.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            return View();
        }
        public ActionResult Venue(string id, [Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, [Bind(Prefix = "Item3")]Deal deal, string src, string op, string vid, string d)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorVenueService vendorVenueService = new VendorVenueService();
            if (vid != null)
            {
                vendorid = vid;
            }
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("venue", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("venue", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorVenue = vendorVenueService.GetVendorVenue(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorVenue, Deal>(vendorMaster, vendorVenue, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }

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
        public ActionResult Venue([Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, [Bind(Prefix = "Item3")]Deal deal, HttpPostedFileBase file, string Command, string id, string vid, string d)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            VendorVenueService vendorVenueService = new VendorVenueService();
            //vendorVenue.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorVenue.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorVenue = vendorVenueService.AddVenue(vendorVenue, vendorMaster);
                //if (deal != null)
                //{
                //    deal.VendorType = vendorVenue.VenueType;
                //    deal.VendorId = vendorMaster.Id;
                //    deal.VendorSubId = vendorVenue.Id;
                //    deal = dealService.AddDealService(deal);
                //}
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorVenue.Id;
                vendorImage.UpdatedBy = user.UserId;
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
                            var filename = "Venue_" + vendorVenue.VendorMasterId + "_" + vendorVenue.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorVenue.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Venue", "CreateVendor", new { id = vendorVenue.Id }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Venue", "CreateVendor") + "'</script>");
                }
            }
            //if (Command == "Active" || Command == "InActive")
            //{
            //    vendorVenue.Status = vendorMaster.Status = Command;
            //    vendorVenue = vendorVenueService.UpdateVenue(vendorVenue, vendorMaster, long.Parse(id), long.Parse(vid));
            //    userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
            //    return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            //}

            if (Command == "Active")
            {
                vendorVenue.Status = vendorMaster.Status = Command;
                vendorVenue = vendorVenueService.activeVenue(vendorVenue, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorVenue.Status = vendorMaster.Status = Command;
                vendorVenue = vendorVenueService.inactiveVenue(vendorVenue, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "Submit")
            {
                //vendorVenue.Status = vendorMaster.Status = Command;
                //vendorVenue = vendorVenueService.inactiveVenue(vendorVenue, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                //return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }
            if (Command == "cancel")
            {

                return Content("<script language='javascript' type='text/javascript'>location.href='" + @Url.Action("Venue", "CreateVendors") + "'</script>");
            }

            if (Command == "update")
            {
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>"); //, new { dropdown = "Venue" }
                    }
                }
                else
                {
                    vendorVenue.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    long masterid = vendorVenue.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorVenue.UpdatedBy = ValidUserUtility.ValidUser();
                    vendorVenue = vendorVenueService.UpdateVenue(vendorVenue, vendorMaster, masterid, long.Parse(vid));
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
                            string[] y = x.Split('_', '.');
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }

                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "Venue_" + vendorVenue.VendorMasterId + "_" + vendorVenue.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorVenue.Id != 0 || vendorImage.ImageId != 0)
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Venue" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }
            if (Command == "Add New")
            {
                vendorVenue.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorVenue = vendorVenueService.AddNewVenue(vendorVenue, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorVenue.Id;
                vendorImage.UpdatedBy = user.UserId;
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
                            var filename = "Venue_" + vendorVenue.VendorMasterId + "_" + vendorVenue.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorVenue.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "Venue";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }

        public ActionResult Others(string id, [Bind(Prefix = "Item2")] VendorsOther vendorsOther, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, [Bind(Prefix = "Item3")]Deal deal, string src, string op, string vid, string d)
        {
            if (op == "confirm") { ViewBag.confirm = TempData["confirm"]; }
            VendorOthersService vendorOthersService = new VendorOthersService();
            if (vid != null)
            {
                vendorid = vid;
            }
            if (src != null)
            {
                var vendorImage = vendorImageService.GetImageId(src, long.Parse(vendorid));
                string delete = vendorImageService.DeleteImage(vendorImage);
                if (delete == "success")
                {
                    string fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + src));
                    System.IO.File.Delete(fileName);
                    return Content("<script language='javascript' type='text/javascript'>alert('Image deleted successfully!');location.href='" + @Url.Action("Others", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!');location.href='" + @Url.Action("Others", "createvendor", new { id = id, vid = vendorid }) + "'</script>");
                }
            }
            if (id != null && vid != null)
            {
                var list = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.imagescount = 10 - list.Count;
                vendorsOther = vendorOthersService.GetVendorOther(long.Parse(id), long.Parse(vid));
                vendorMaster = vendorMasterService.GetVendor(long.Parse(id));
                //deal = dealService.GetDealService(int.Parse(d));
                if (d != null)
                {
                    deal = dealService.GetDealService(int.Parse(d));
                    if (deal != null)
                    {
                        ViewBag.dealslist = 1;
                    }
                }
                var a = new Tuple<Vendormaster, VendorsOther, Deal>(vendorMaster, vendorsOther, deal);
                ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                ViewBag.masterid = id;
                if (op != null)
                {
                    ViewBag.displaydata = "enable";
                    if (op == "add")
                    {
                        ViewBag.images = null;
                        ViewBag.imagescount = 10;
                    }
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
        public ActionResult Others([Bind(Prefix = "Item2")] VendorsOther vendorsOther, [Bind(Prefix = "Item1")] Vendormaster vendorMaster, [Bind(Prefix = "Item3")]Deal deal, HttpPostedFileBase file, string Command, string id, string vid, string d)
        {
            var user = (CustomPrincipal)System.Web.HttpContext.Current.User;
            string fileName = string.Empty;
            string ImagesURL = string.Empty;
            VendorOthersService vendorOthersService = new VendorOthersService();
            //vendorsOther.UpdatedBy = user.UserId;
            //vendorMaster.UpdatedBy = user.UserId;
            if (Command == "Save")
            {
                vendorsOther.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorsOther = vendorOthersService.AddOther(vendorsOther, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsOther.Id;
                vendorImage.UpdatedBy = user.UserId;
                //if (deal != null)
                //{
                //    deal.VendorType = vendorVenue.VenueType;
                //    deal.VendorId = vendorMaster.Id;
                //    deal.VendorSubId = vendorVenue.Id;
                //    deal = dealService.AddDealService(deal);
                //}
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
                            var filename = "Others_" + vendorsOther.VendorMasterId + "_" + vendorsOther.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsOther.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("Others", "CreateVendor", new { id = vendorsOther.Id }) + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("Others", "CreateVendor") + "'</script>");
                }
            }
            if (Command == "update")
            {
                if (d != null)
                {
                    deal = dealService.UpdateDealService(deal, int.Parse(d));
                    if (deal.DealID != 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                    }
                    return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    vendorsOther.UpdatedBy = user.UserId;
                    vendorMaster.UpdatedBy = user.UserId;
                    long masterid = vendorsOther.VendorMasterId = vendorMaster.Id = long.Parse(id);
                    vendorMaster.UpdatedBy = vendorsOther.UpdatedBy = user.UserId;
                    vendorsOther = vendorOthersService.UpdateOther(vendorsOther, vendorMaster, masterid, long.Parse(vid));
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
                            if (y[3] == "jpg")
                            {
                                imageno = int.Parse(y[2]);
                            }
                            else
                            {
                                imageno = int.Parse(y[3]);
                            }
                        }

                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            int j = imageno + 1 + i;
                            var file1 = Request.Files[i];
                            if (file1 != null && file1.ContentLength > 0)
                            {
                                string path = System.IO.Path.GetExtension(file.FileName);
                                var filename = "Other_" + vendorsOther.VendorMasterId + "_" + vendorsOther.Id + "_" + j + path;
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
                        if (file != null)
                        {
                            ViewBag.images = vendorImageService.GetVendorImagesService(long.Parse(id), long.Parse(vid));
                            ViewBag.imagescount = imagecount - list.Count;
                            ViewData["error"] = "You Have Crossed Images Limit";
                        }
                        else
                        {
                            if (vendorsOther.Id != 0 || vendorImage.ImageId != 0)
                            {

                                return Content("<script language='javascript' type='text/javascript'>alert('Updated Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>"); //, new { dropdown = "Venue" }
                            }
                            else
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Update Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                            }
                        }
                    }
                }
            }

            if (Command == "Active")
            {
                vendorsOther.Status = vendorMaster.Status = Command;
                vendorsOther = vendorOthersService.activationOther(vendorsOther, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "InActive")
            {
                vendorsOther.Status = vendorMaster.Status = Command;
                vendorsOther = vendorOthersService.activationOther(vendorsOther, vendorMaster, long.Parse(id), long.Parse(vid));
                //userLoginDetailsService.Updatestatus(vendorMaster.EmailId, Command);
                return Content("<script language='javascript' type='text/javascript'>alert('Vendor is " + Command + "');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
            }

            if (Command == "add")
            {
                vendorsOther.UpdatedBy = user.UserId;
                vendorMaster.UpdatedBy = user.UserId;
                vendorMaster.Id = long.Parse(id);
                vendorsOther = vendorOthersService.AddNewOther(vendorsOther, vendorMaster);
                VendorImage vendorImage = new VendorImage();
                vendorImage.VendorId = vendorsOther.Id;
                vendorImage.UpdatedBy = user.UserId;
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
                            var filename = "Others_" + vendorsOther.VendorMasterId + "_" + vendorsOther.Id + "_" + j + path;
                            fileName = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(imagepath + filename));
                            file1.SaveAs(fileName);
                            vendorImage.ImageName = filename;
                            vendorImage = vendorImageService.AddVendorImage(vendorImage, vendorMaster);
                        }

                    }
                }
                if (vendorsOther.Id != 0 || vendorImage.ImageId != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registered Successfully');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Registration Failed');location.href='" + @Url.Action("AllVendors", "Vendors") + "'</script>");
                }
            }
            if (Command == "Add Deal")
            {
                deal.VendorType = "Others";
                deal.VendorId = long.Parse(id);
                deal.VendorSubId = long.Parse(vid);
                deal = dealService.AddDealService(deal);
                if (deal.DealID != 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Deal Added');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Failed!!!');location.href='" + @Url.Action("AllDeals", "Deals") + "'</script>");
                }
            }
            return View();
        }


        [HttpPost]
        public ActionResult submitquery(string emailid, string txtone)
        {
            EmailSendingUtility emailSendingUtility = new EmailSendingUtility();
            emailSendingUtility.Email_maaaahwanam(emailid, txtone, "Attention required");
            return Json("success", JsonRequestBehavior.AllowGet);
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


    }
}