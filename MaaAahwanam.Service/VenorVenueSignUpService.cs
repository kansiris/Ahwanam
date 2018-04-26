using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Utility;

namespace MaaAahwanam.Service
{
    public class VenorVenueSignUpService
    {
        VendorVenueSignUpRepository vendorVenueSignUpRepository = new VendorVenueSignUpRepository();
        VendorVenueRepository vendorVenueRepository = new VendorVenueRepository();
        VendorCateringRepository vendorCateringRepository = new VendorCateringRepository();
        VendorsPhotographyRepository vendorsPhotographyRepository = new VendorsPhotographyRepository();
        VendorsDecoratorRepository vendorsDecoratorRepository = new VendorsDecoratorRepository();
        VendorOthersRepository vendorOthersRepository = new VendorOthersRepository();
        VendorEventOrganiserRepository vendorEventOrganiserRepository = new VendorEventOrganiserRepository();

        string updateddate = DateTime.UtcNow.ToShortDateString();
        
        public UserLogin AddUserLogin(UserLogin userLogin)
        {
            //userLogin.Password = randomPassword.GenerateString();
            userLogin.UserType = "Vendor";
            userLogin.UpdatedBy = 2;
            userLogin.Status = "InActive";
            userLogin.RegDate = Convert.ToDateTime(updateddate);
            userLogin.UpdatedDate = Convert.ToDateTime(updateddate);
            return vendorVenueSignUpRepository.AddUserLogin(userLogin);
        }

        public UserLogin GetUserLogin(UserLogin userLogin)
        {
            return vendorVenueSignUpRepository.GetUserLogin(userLogin);
        }

        public Vendormaster AddvendorMaster(Vendormaster vendormaster)
        {
            vendormaster.Status = "InActive";
            vendormaster.UpdatedDate = Convert.ToDateTime(updateddate);
            //vendormaster.ServicType = "Venue";
            return vendorVenueSignUpRepository.AddVendormaster(vendormaster);
        }

        public UserDetail AddUserDetail(UserDetail userDetail, Vendormaster vendormaster)
        {
            userDetail.FirstName = vendormaster.ContactPerson;
            userDetail.UserPhone = vendormaster.ContactNumber;
            userDetail.Url = vendormaster.Url;
            userDetail.Address = vendormaster.Address;
            userDetail.City = vendormaster.City;
            userDetail.State = vendormaster.State;
            userDetail.ZipCode = vendormaster.ZipCode;
            userDetail.Status = "InActive";
            userDetail.UpdatedBy = 2;
            userDetail.UpdatedDate = Convert.ToDateTime(updateddate);
            userDetail.AlternativeEmailID = vendormaster.EmailId;
            userDetail.Landmark = vendormaster.Landmark;
            return vendorVenueSignUpRepository.AddUserDetail(userDetail);
        }

        //Venue Area

        public VendorVenue AddVendorVenue(VendorVenue VendorVenue)
        {
            VendorVenue.UpdatedDate = Convert.ToDateTime(updateddate);
            return vendorVenueSignUpRepository.AddVendorVenue(VendorVenue);
        }

        public VendorVenue UpdateVenue(VendorVenue vendorVenue, Vendormaster vendorMaster, long masterid, long vid)
        {
            //vendorVenue.Status = "InActive";
            vendorVenue.UpdatedDate = Convert.ToDateTime(updateddate);
            //vendorMaster.Status = "InActive";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            //vendorMaster.ServicType = "Venue";
            vendorVenue = vendorVenueRepository.UpdateVenue(vendorVenue, masterid, vid);
            return vendorVenue;
        }

        public List<VendorVenue> GetVendorVenue(long id)
        {
            return vendorVenueSignUpRepository.GetVendorVenue(id);
        }
        

        public VendorVenue GetParticularVendorVenue(long id, long vid)
        {
            return vendorVenueRepository.GetVendorVenue(id, vid);
        }

        public Package addpack(Package package)
        {
            return vendorVenueSignUpRepository.Addpackage(package);
        }

        public Package updatepack(string id ,Package package)
        {
            return vendorVenueSignUpRepository.updatepackage(long.Parse(id),package);
        }

        public NDeals adddeal(NDeals deals)
        {
            return vendorVenueSignUpRepository.Adddeals(deals);
        }
        //Catering Area

        public VendorsCatering AddVendorCatering(VendorsCatering vendorsCatering)
        {
            vendorsCatering.UpdatedDate = Convert.ToDateTime(updateddate);
            return vendorVenueSignUpRepository.AddVendorCatering(vendorsCatering);
        }

        public VendorsCatering UpdateCatering(VendorsCatering vendorsCatering, Vendormaster vendorMaster, long masterid, long vid)
        {
            //vendorsCatering.Status = "InActive";
            vendorsCatering.UpdatedDate = Convert.ToDateTime(updateddate);
            //vendorMaster.Status = "InActive";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            //vendorMaster.ServicType = "Venue";
            vendorsCatering = vendorCateringRepository.UpdatesCatering(vendorsCatering, masterid, vid);
            return vendorsCatering;
        }

        public List<VendorsCatering> GetVendorCatering(long id)
        {
            return vendorVenueSignUpRepository.GetVendorCatering(id);
        }

        public VendorsCatering GetParticularVendorCatering(long id, long vid)
        {
            return vendorCateringRepository.GetVendorsCatering(id, vid);
        }

        //Photography Area

        public VendorsPhotography AddVendorPhotography(VendorsPhotography vendorsPhotography)
        {
            vendorsPhotography.UpdatedDate = Convert.ToDateTime(updateddate);
            return vendorVenueSignUpRepository.AddVendorPhotography(vendorsPhotography);
        }

        public VendorsPhotography UpdatePhotography(VendorsPhotography vendorsPhotography, Vendormaster vendorMaster, long masterid, long vid)
        {
            //vendorsPhotography.Status = "InActive";
            vendorsPhotography.UpdatedDate = Convert.ToDateTime(updateddate);
            //vendorMaster.Status = "InActive";
            vendorMaster.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsPhotography = vendorsPhotographyRepository.UpdatesPhotography(vendorsPhotography, masterid, vid);
            return vendorsPhotography;
        }

        public List<VendorsPhotography> GetVendorPhotography(long id)
        {
            return vendorVenueSignUpRepository.GetVendorPhotography(id);
        }

        public VendorsPhotography GetParticularVendorPhotography(long id, long vid)
        {
            return vendorsPhotographyRepository.GetVendorsPhotography(id, vid);
        }

        //Decorator Area

        public VendorsDecorator AddVendorDecorator(VendorsDecorator vendorsDecorator)
        {
            vendorsDecorator.UpdatedDate = Convert.ToDateTime(updateddate);
            return vendorVenueSignUpRepository.AddVendorDecorator(vendorsDecorator);
        }

        public List<VendorsDecorator> GetVendorDecorator(long id)
        {
            return vendorVenueSignUpRepository.GetVendorDecorator(id);
        }

        public VendorsDecorator UpdateDecorator(VendorsDecorator vendorsDecorator, Vendormaster vendorMaster, long masterid, long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            //vendorsDecorator.Status = "InActive";
            vendorsDecorator.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsDecorator = vendorsDecoratorRepository.UpdateDecorator(vendorsDecorator, masterid, vid);
            return vendorsDecorator;
        }

        public VendorsDecorator GetParticularVendorDecorator(long id, long vid)
        {
            return vendorsDecoratorRepository.GetVendorDecorator(id, vid);
        }


        //Event Organiser Area

        public VendorsEventOrganiser AddVendorEventOrganiser(VendorsEventOrganiser vendorsEventOrganiser)
        {
            vendorsEventOrganiser.UpdatedDate = Convert.ToDateTime(updateddate);
            return vendorEventOrganiserRepository.AddEventOrganiser(vendorsEventOrganiser);
        }

        public List<VendorsEventOrganiser> GetVendorEventOrganiser(long id)
        {
            return vendorVenueSignUpRepository.GetVendorEventOrganiser(id);
        }

        public VendorsEventOrganiser UpdateEventOrganiser(VendorsEventOrganiser vendorsEventOrganiser, Vendormaster vendorMaster, long masterid, long vid)
        {
            string updateddate = DateTime.UtcNow.ToShortDateString();
            //vendorsDecorator.Status = "InActive";
            vendorsEventOrganiser.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsEventOrganiser = vendorEventOrganiserRepository.UpdateEventOrganiser(vendorsEventOrganiser, masterid, vid);
            return vendorsEventOrganiser;
        }

        public VendorsEventOrganiser GetParticularVendorEventOrganiser(long id, long vid)
        {
            return vendorEventOrganiserRepository.GetVendorEventOrganiser(id, vid);
        }

        //Others Area

        public VendorsOther AddVendorOther(VendorsOther vendorsOther)
        {
            vendorsOther.UpdatedDate = Convert.ToDateTime(updateddate);
            return vendorVenueSignUpRepository.AddVendorOther(vendorsOther);
        }

        public List<VendorsOther> GetVendorOther(long id)
        {
            return vendorVenueSignUpRepository.GetVendorOther(id);
        }

        public VendorsOther UpdateOther(VendorsOther vendorsOther, Vendormaster vendorMaster, long masterid, long vid)
        {
            //string updateddate = DateTime.UtcNow.ToShortDateString();
            //vendorsDecorator.Status = "InActive";
            vendorsOther.UpdatedDate = Convert.ToDateTime(updateddate);
            vendorsOther = vendorOthersRepository.UpdateOthers(vendorsOther, masterid, vid);
            return vendorsOther;
        }

        public VendorsOther GetParticularVendorOther(long id, long vid)
        {
            return vendorOthersRepository.GetVendorOthers(id, vid);
        }

        // Discount% updation in every service
        public long DiscountUpdate(string type,string id,string vid,string discount)
        {
            long masterid = 0;
            if(type == "Venue")
            {
                VendorVenue vendorVenue = vendorVenueRepository.GetVendorVenue(long.Parse(id), long.Parse(vid));
                vendorVenue.discount = discount;
                vendorVenue = vendorVenueRepository.UpdateVenue(vendorVenue, long.Parse(id), long.Parse(vid));
                masterid = vendorVenue.Id;
            }
            if (type == "Catering")
            {
                VendorsCatering vendorsCatering = vendorCateringRepository.GetVendorsCatering(long.Parse(id), long.Parse(vid));
                vendorsCatering.discount = discount;
                vendorsCatering = vendorCateringRepository.UpdatesCatering(vendorsCatering, long.Parse(id), long.Parse(vid));
                masterid = vendorsCatering.Id;
            }
            if (type == "Decorator")
            {
                VendorsDecorator vendorsDecorator = vendorsDecoratorRepository.GetVendorDecorator(long.Parse(id), long.Parse(vid));
                vendorsDecorator.discount = discount;
                vendorsDecorator = vendorsDecoratorRepository.UpdateDecorator(vendorsDecorator, long.Parse(id), long.Parse(vid));
                masterid = vendorsDecorator.Id;
            }
            if (type == "Photography")
            {
                VendorsPhotography vendorsPhotography = vendorsPhotographyRepository.GetVendorsPhotography(long.Parse(id), long.Parse(vid));
                vendorsPhotography.discount = discount;
                vendorsPhotography = vendorsPhotographyRepository.UpdatesPhotography(vendorsPhotography, long.Parse(id), long.Parse(vid));
                masterid = vendorsPhotography.Id;
            }
            if (type == "Event")
            {
                VendorsEventOrganiser vendorsEventOrganiser = vendorEventOrganiserRepository.GetVendorEventOrganiser(long.Parse(id), long.Parse(vid));
                vendorsEventOrganiser.discount = discount;
                vendorsEventOrganiser = vendorEventOrganiserRepository.UpdateEventOrganiser(vendorsEventOrganiser, long.Parse(id), long.Parse(vid));
                masterid = vendorsEventOrganiser.Id;
            }
            if (type == "Other")
            {
                VendorsOther vendorsOther = vendorOthersRepository.GetVendorOthers(long.Parse(id), long.Parse(vid));
                vendorsOther.discount = discount;
                vendorsOther = vendorOthersRepository.UpdateOthers(vendorsOther, long.Parse(id), long.Parse(vid));
                masterid = vendorsOther.Id;
            }
            return masterid;
        }

        // Removing Vendor Service Record
        public string RemoveVendorService(string id,string type)
        {
            return vendorVenueSignUpRepository.RemoveVendorService(id,type);
        }

        //updating vendor Service Record
        public long UpdateVendorService(string id, string vid, string type)
        {
            long masterid = 0;
            if (type == "Venue")
            {
                VendorVenue vendorVenue = vendorVenueRepository.GetVendorVenue(long.Parse(id), long.Parse(vid));
                vendorVenue.VenueType = "";
                vendorVenue = vendorVenueRepository.UpdateVenue(vendorVenue, long.Parse(id), long.Parse(vid));
                masterid = vendorVenue.Id;
            }
            if (type == "Catering")
            {
                VendorsCatering vendorsCatering = vendorCateringRepository.GetVendorsCatering(long.Parse(id), long.Parse(vid));
                vendorsCatering.CuisineType = "";
                vendorsCatering = vendorCateringRepository.UpdatesCatering(vendorsCatering, long.Parse(id), long.Parse(vid));
                masterid = vendorsCatering.Id;
            }
            if (type == "Decorator")
            {
                VendorsDecorator vendorsDecorator = vendorsDecoratorRepository.GetVendorDecorator(long.Parse(id), long.Parse(vid));
                vendorsDecorator.DecorationType = "";
                vendorsDecorator = vendorsDecoratorRepository.UpdateDecorator(vendorsDecorator, long.Parse(id), long.Parse(vid));
                masterid = vendorsDecorator.Id;
            }
            if (type == "Photography")
            {
                VendorsPhotography vendorsPhotography = vendorsPhotographyRepository.GetVendorsPhotography(long.Parse(id), long.Parse(vid));
                vendorsPhotography.PhotographyType = "";
                vendorsPhotography = vendorsPhotographyRepository.UpdatesPhotography(vendorsPhotography, long.Parse(id), long.Parse(vid));
                masterid = vendorsPhotography.Id;
            }
            if (type == "Event")
            {
                VendorsEventOrganiser vendorsEventOrganiser = vendorEventOrganiserRepository.GetVendorEventOrganiser(long.Parse(id), long.Parse(vid));
                vendorsEventOrganiser.type = "";
                vendorsEventOrganiser = vendorEventOrganiserRepository.UpdateEventOrganiser(vendorsEventOrganiser, long.Parse(id), long.Parse(vid));
                masterid = vendorsEventOrganiser.Id;
            }
            if (type == "Other")
            {
                VendorsOther vendorsOther = vendorOthersRepository.GetVendorOthers(long.Parse(id), long.Parse(vid));
                vendorsOther.type = "";
                vendorsOther = vendorOthersRepository.UpdateOthers(vendorsOther, long.Parse(id), long.Parse(vid));
                masterid = vendorsOther.Id;
            }
            return masterid;
        }
    }
}
