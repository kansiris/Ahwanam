using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MaaAahwanam.Models;
using MaaAahwanam.Service;

namespace MaaAahwanam.Web.Controllers
{
    public class VendorSignUp1Controller : Controller
    {
        VenorVenueSignUpService vendorVenueSignUpService = new VenorVenueSignUpService();
        VendorMasterService vendorMasterService = new VendorMasterService();

        // GET: VendorSignUp1
        public ActionResult Index(string id, string vid, string type)
        {
            ViewBag.data = vendorMasterService.GetVendor(long.Parse(id));
            ViewBag.type = type;
            ViewBag.country = new SelectList(CountryList(), "Value", "Text");
            if (type == "Venue")
            {
                VendorVenueService vendorVenueService = new VendorVenueService();
                ViewBag.service = vendorVenueService.GetVendorVenue(long.Parse(id), long.Parse(vid));
                ViewBag.categorytype = ViewBag.service.VenueType;
                return View();
            }
            if (type == "Catering")
            {
                VendorCateringService vendorCateringService = new VendorCateringService();
                ViewBag.service = vendorCateringService.GetVendorCatering(long.Parse(id), long.Parse(vid));
                ViewBag.categorytype = ViewBag.service.CuisineType;
                return View();
            }
            if (type == "Photography")
            {
                VendorPhotographyService vendorPhotographyService = new VendorPhotographyService();
                ViewBag.service = vendorPhotographyService.GetVendorPhotography(long.Parse(id), long.Parse(vid));
                ViewBag.categorytype = ViewBag.service.PhotographyType;
                return View();
            }
            if (type == "Decorator")
            {
                VendorDecoratorService vendorDecoratorService = new VendorDecoratorService();
                ViewBag.service = vendorDecoratorService.GetVendorDecorator(long.Parse(id), long.Parse(vid));
                ViewBag.categorytype = ViewBag.service.DecorationType;
                return View();
            }
            if (type == "Other")
            {
                VendorOthersService vendorOthersService = new VendorOthersService();
                ViewBag.service = vendorOthersService.GetVendorOther(long.Parse(id), long.Parse(vid));
                ViewBag.categorytype = ViewBag.service.type;
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index([Bind(Prefix = "Item1")] Vendormaster vendorMaster, [Bind(Prefix = "Item2")] UserLogin userLogin, [Bind(Prefix = "Item3")]UserDetail userDetail, [Bind(Prefix = "Item4")]VendorVenue vendorVenue, string id, string vid, string type, string command)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string[] venueservices = { "Convention Hall", "Function Hall", "Banquet Hall", "Meeting Room", "Open Lawn", "Roof Top", "Hotel", "Resort" };
                string[] cateringservices = { "Indian", "Chinese", "Mexican", "South Indian", "Continental", "Multi Cuisine", "Chaat", "Fast Food", "Others" };
                string[] photographyservices = { "Wedding", "Candid", "Portfolio", "Fashion", "Toddler", "Videography", "Conventional", "Cinematography", "Others" };
                string[] decoratorservices = { "Florists", "TentHouse Decorators", "Others" };
                string[] otherservices = { "Mehendi" };
                List<string> matchingvenues = null; List<string> matchingcatering = null; List<string> matchingphotography = null; List<string> matchingdecorators = null;
                List<string> matchingothers = null;
                if (vendorVenue.VenueType != null)
                {
                    if (type == "Venue") //if (vendorMaster.ServicType.Split(',').Contains("Venue"))
                        matchingvenues = venueservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
                    if (type == "Catering") //if (vendorMaster.ServicType.Split(',').Contains("Catering"))
                        matchingcatering = cateringservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
                    if (type == "Photography") //if (vendorMaster.ServicType.Split(',').Contains("Photography"))
                        matchingphotography = photographyservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
                    if (type == "Decorator") //if (vendorMaster.ServicType.Split(',').Contains("Decorator"))
                        matchingdecorators = decoratorservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
                    if (type == "Other") //if (vendorMaster.ServicType.Split(',').Contains("Decorator"))
                        matchingothers = otherservices.Intersect(vendorVenue.VenueType.Split(',')).ToList();
                }
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('Select Atleat One Sub Category');location.href='/VendorSignUp1/Index?id=" + id + "&&vid=" + vid + "&&type=" + type + "'</script>");
                vendorMaster = vendorMasterService.UpdateVendorMaster(vendorMaster, long.Parse(id)); //Updating ServicType in Vendormaster Table

                #region Venue
                if (matchingvenues != null)  //if (vendorMaster.ServicType.Split(',').Contains("Venue"))
                {
                    var venuedata = vendorVenue;
                    vendorVenue = vendorVenueSignUpService.GetParticularVendorVenue(long.Parse(id), long.Parse(vid)); // Retrieving Particular Vendor Record
                    vendorVenue.Address = venuedata.Address;
                    vendorVenue.City = venuedata.City;
                    vendorVenue.State = venuedata.State;
                    vendorVenue.Landmark = venuedata.Landmark;
                    vendorVenue.ZipCode = venuedata.ZipCode;
                    vendorVenue.VendorMasterId = vendorMaster.Id;
                    //vendorVenue.tier = venuedata.tier;
                    if (command == "Save Info")
                    {
                        for (int a = 0; a < matchingvenues.Count(); a++)
                        {
                            if (vendorVenue.VenueType == "" || vendorVenue.VenueType == null)
                            {
                                vendorVenue.VenueType = matchingvenues[a];
                                vendorVenue.name = venuedata.name;
                                vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorVenue.VenueType = matchingvenues[a];
                                vendorVenue.name = null;
                                vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
                            }
                        }
                    }
                    
                    if (command == "Update Info")
                    {
                        for (int i = 0; i < matchingvenues.Count(); i++)
                        {
                            if (i == 0)
                            {
                                vendorVenue.VenueType = matchingvenues[i];
                                vendorVenue.name = venuedata.name;
                                vendorVenue = vendorVenueSignUpService.UpdateVenue(vendorVenue, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorVenue.VenueType = matchingvenues[i];
                                vendorVenue.name = null;
                                vendorVenue = vendorVenueSignUpService.AddVendorVenue(vendorVenue);
                            }
                        }
                    }
                }
                #endregion

                #region Catering

                if (matchingcatering != null)  //if (vendorMaster.ServicType.Split(',').Contains("Catering"))
                {
                    VendorCateringService vendorCateringService = new VendorCateringService();
                    VendorsCatering vendorsCatering = vendorVenueSignUpService.GetParticularVendorCatering(long.Parse(id), long.Parse(vid));
                    vendorsCatering.Address = vendorVenue.Address;
                    vendorsCatering.City = vendorVenue.City;
                    vendorsCatering.State = vendorVenue.State;
                    vendorsCatering.Landmark = vendorVenue.Landmark;
                    vendorsCatering.ZipCode = vendorVenue.ZipCode;
                    vendorsCatering.VendorMasterId = vendorMaster.Id;
                    //vendorsCatering.tier = vendorVenue.tier;
                    if (command == "Save Info")
                    {
                        for (int a = 0; a < matchingcatering.Count(); a++)
                        {
                            if (vendorsCatering.CuisineType == "" || vendorsCatering.CuisineType == null)
                            {
                                vendorsCatering.CuisineType = matchingcatering[a];
                                vendorsCatering.name = vendorVenue.name;
                                vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorsCatering.CuisineType = matchingcatering[a];
                                vendorsCatering.name = null;
                                vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                            }
                        }
                    }
                    if (command == "Update Info")
                    {
                        for (int i = 0; i < matchingcatering.Count(); i++)
                        {
                            if (i == 0)
                            {
                                vendorsCatering.CuisineType = matchingcatering[i];
                                vendorsCatering.name = vendorVenue.name;
                                vendorsCatering = vendorVenueSignUpService.UpdateCatering(vendorsCatering, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorsCatering.CuisineType = matchingcatering[i];
                                vendorsCatering.name = null;
                                vendorsCatering = vendorVenueSignUpService.AddVendorCatering(vendorsCatering);
                            }
                        }
                    }
                }

                #endregion

                #region Photography

                if (matchingphotography != null)  //if (vendorMaster.ServicType.Split(',').Contains("Photography"))
                {
                    VendorsPhotography vendorsPhotography = vendorVenueSignUpService.GetParticularVendorPhotography(long.Parse(id), long.Parse(vid));
                    vendorsPhotography.Address = vendorVenue.Address;
                    vendorsPhotography.City = vendorVenue.City;
                    vendorsPhotography.State = vendorVenue.State;
                    vendorsPhotography.Landmark = vendorVenue.Landmark;
                    vendorsPhotography.ZipCode = vendorVenue.ZipCode;
                    vendorsPhotography.VendorMasterId = vendorMaster.Id;
                    //vendorsPhotography.tier = vendorVenue.tier;
                    if (command == "Save Info")
                    {
                        for (int a = 0; a < matchingphotography.Count(); a++)
                        {
                            if (vendorsPhotography.PhotographyType == "" || vendorsPhotography.PhotographyType == null)
                            {
                                vendorsPhotography.PhotographyType = matchingphotography[a];
                                vendorsPhotography.name = vendorVenue.name;
                                vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorsPhotography.PhotographyType = matchingphotography[a];
                                vendorsPhotography.name = null;
                                vendorsPhotography = vendorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                            }
                        }
                    }

                    if (command == "Update Info")
                    {
                        for (int i = 0; i < matchingphotography.Count(); i++)
                        {
                            if (i == 0)
                            {
                                vendorsPhotography.PhotographyType = matchingphotography[i];
                                vendorsPhotography.name = vendorVenue.name;
                                vendorsPhotography = vendorVenueSignUpService.UpdatePhotography(vendorsPhotography, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorsPhotography.PhotographyType = matchingphotography[i];
                                vendorsPhotography.name = null;
                                vendorsPhotography = vendorVenueSignUpService.AddVendorPhotography(vendorsPhotography);
                            }
                        }
                    }
                }

                #endregion

                #region Decorator

                if (matchingdecorators != null)  //if (vendorMaster.ServicType.Split(',').Contains("Decorator"))
                {
                    VendorsDecorator vendorsDecorator = vendorVenueSignUpService.GetParticularVendorDecorator(long.Parse(id), long.Parse(vid));
                    vendorsDecorator.Address = vendorVenue.Address;
                    vendorsDecorator.City = vendorVenue.City;
                    vendorsDecorator.State = vendorVenue.State;
                    vendorsDecorator.Landmark = vendorVenue.Landmark;
                    vendorsDecorator.ZipCode = vendorVenue.ZipCode;
                    vendorsDecorator.VendorMasterId = vendorMaster.Id;
                    //vendorsDecorator.tier = vendorVenue.tier;
                    if (command == "Save Info")
                    {
                        for (int a = 0; a < matchingdecorators.Count(); a++)
                        {
                            if (vendorsDecorator.DecorationType == "" || vendorsDecorator.DecorationType == null)
                            {
                                vendorsDecorator.DecorationType = matchingdecorators[a];
                                vendorsDecorator.name = vendorVenue.name;
                                vendorsDecorator = vendorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorsDecorator.DecorationType = matchingdecorators[a];
                                vendorsDecorator.name = null;
                                vendorsDecorator = vendorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                            }
                        }
                    }
                    if (command == "Update Info")
                    {
                        for (int i = 0; i < matchingdecorators.Count(); i++)
                        {
                            if (i == 0)
                            {
                                vendorsDecorator.DecorationType = matchingdecorators[i];
                                vendorsDecorator.name = vendorVenue.name;
                                vendorsDecorator = vendorVenueSignUpService.UpdateDecorator(vendorsDecorator, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorsDecorator.DecorationType = matchingdecorators[i];
                                vendorsDecorator.name = null;
                                vendorsDecorator = vendorVenueSignUpService.AddVendorDecorator(vendorsDecorator);
                            }
                        }
                    }
                }

                #endregion

                #region Other

                if (matchingothers != null)  //if (vendorMaster.ServicType.Split(',').Contains("Decorator"))
                {
                    VendorsOther vendorsOther = vendorVenueSignUpService.GetParticularVendorOther(long.Parse(id), long.Parse(vid));
                    vendorsOther.Address = vendorVenue.Address;
                    vendorsOther.City = vendorVenue.City;
                    vendorsOther.State = vendorVenue.State;
                    vendorsOther.Landmark = vendorVenue.Landmark;
                    vendorsOther.ZipCode = vendorVenue.ZipCode;
                    vendorsOther.VendorMasterId = vendorMaster.Id;
                    //vendorsOther.tier = vendorVenue.tier;
                    if (command == "Save Info")
                    {
                        for (int a = 0; a < matchingothers.Count(); a++)
                        {
                            if (vendorsOther.type == "" || vendorsOther.type == null)
                            {
                                vendorsOther.type = matchingothers[a];
                                vendorsOther.name = vendorVenue.name;
                                vendorsOther = vendorVenueSignUpService.UpdateOther(vendorsOther, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorsOther.type = matchingothers[a];
                                vendorsOther.name = null;
                                vendorsOther = vendorVenueSignUpService.AddVendorOther(vendorsOther);
                            }
                        }
                    }
                    if (command == "Update Info")
                    {
                        for (int i = 0; i < matchingothers.Count(); i++)
                        {
                            if (i == 0)
                            {
                                vendorsOther.type = matchingothers[i];
                                vendorsOther.name = vendorVenue.name;
                                vendorsOther = vendorVenueSignUpService.UpdateOther(vendorsOther, vendorMaster, long.Parse(id), long.Parse(vid));
                            }
                            else
                            {
                                vendorsOther.type = matchingothers[i];
                                vendorsOther.name = null;
                                vendorsOther = vendorVenueSignUpService.AddVendorOther(vendorsOther);
                            }
                        }
                    }
                }

                #endregion

                return Content("<script language='javascript' type='text/javascript'>alert('General Information Registered Successfully');location.href='" + @Url.Action("Index", "AvailableServices", new { id = vendorMaster.Id }) + "'</script>");
            }
            else
            {
                return RedirectToAction("Index", "HomePage");
            }
        }

        private List<SelectListItem> CountryList()
        {
            List<SelectListItem> cultureList = new List<SelectListItem>();
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            if (getCultureInfo.Count() > 0)
            {
                foreach (CultureInfo cultureInfo in getCultureInfo)
                {
                    RegionInfo getRegionInfo = new RegionInfo(cultureInfo.LCID);
                    var newitem = new SelectListItem { Text = getRegionInfo.EnglishName, Value = getRegionInfo.EnglishName };
                    cultureList.Add(newitem);
                }
            }
            return cultureList;
        }

        public JsonResult checkemail(string emailid)
        {
            VendorMasterService vendorMasterService = new VendorMasterService();
            int query = vendorMasterService.checkemail(emailid);
            if (query != 0)
            {
                return Json("exists", JsonRequestBehavior.AllowGet);
            }
            return Json("valid", JsonRequestBehavior.AllowGet);
        }

    }
}