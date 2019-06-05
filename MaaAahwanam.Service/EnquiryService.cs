using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class EnquiryService
    {
        public string SaveEnquiries(Enquiry enquiry)
        {
            string response = string.Empty;
            try
            {
                EnquiryRepository enquiryRepository = new EnquiryRepository();
                enquiryRepository.SaveEnquiries(enquiry);
                response = "Success";
            }
            catch (Exception ex)
            {
                response = "failure";
            }
            return response;
        }
        public Enquiry SaveallEnquiries(Enquiry enquiry)
        {
                EnquiryRepository enquiryRepository = new EnquiryRepository();
            return enquiryRepository.SaveEnquiries(enquiry);
        }

        public List<Enquiry> getallenquires()
        {
            EnquiryRepository enquiryRepository = new EnquiryRepository();
            return enquiryRepository.getallenquires();
        }

        public List<getallwishlistdetailsofusers_Result> Getwishlistdataforadmin()
        {
            EnquiryRepository enquiryRepository = new EnquiryRepository();
            return enquiryRepository.Getwishlistdataforadmin();
        }

        public List<getuserdetailsforadmin_Result> Getuserdataforadmin()
        {
            EnquiryRepository enquiryRepository = new EnquiryRepository();
            return enquiryRepository.Getuserdataforadmin();
        }
    }
}
