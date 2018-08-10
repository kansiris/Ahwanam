using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Service;
using MaaAahwanam.Models;
using MaaAahwanam.Utility;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace MaaAahwanam.Web.Areas.Admin.Controllers
{
    public class VendorsController : Controller
    {

        //VendorVenueService vendorVenueService = new VendorVenueService();
        //VendorImageService vendorImageService = new VendorImageService();
        VendorMasterService vendorMasterService = new VendorMasterService();
        VendorSetupService vendorSetupService = new VendorSetupService();
        UserLoginDetailsService userLoginDetailsService = new UserLoginDetailsService();
        public ActionResult AllVendors()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AllVendors(string dropstatus, string vid, string command, string id, string type, [Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster)
        {
            ViewBag.type = dropstatus;
            if (dropstatus != null && dropstatus != "")
            {
                ViewBag.VendorList = vendorSetupService.AllVendorList(dropstatus);
            }
            if (command == "Edit")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid });
            }
            if (command == "View")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "display" });
            }
            if (command == "Add New")
            {
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "add" });
            }
            if (command == "confirm")
            {
                TempData["confirm"] = 1;
                return RedirectToAction(dropstatus, "CreateVendor", new { id = id, vid = vid, op = "confirm" });
            }
            return View();
        }
        public ActionResult ActiveVendors()
        {
            return View();
        }
        public ActionResult PendingVendors()
        {
            return View();
        }
        public ActionResult SuspendedVendors()
        {
            return View();
        }
        public ActionResult VendorDetails(string id, [Bind(Prefix = "Item2")] VendorVenue vendorVenue, [Bind(Prefix = "Item1")] Vendormaster vendorMaster)
        {
            return View();
        }

        public ActionResult SearchVendor()
        {
            return View();
        }

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
      
    }
}