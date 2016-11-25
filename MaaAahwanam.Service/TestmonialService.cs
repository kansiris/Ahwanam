using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;
using AutoMapper;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Service
{
    public class TestmonialService
    {
        AdminTestimonialRepository testimonialRepository = new AdminTestimonialRepository();
        AdminTestimonialPathRepository testimonialpathRepository = new AdminTestimonialPathRepository();

        public List<SP_GetTestimonials_Result> TestmonialServiceList()
        {
            List<SP_GetTestimonials_Result> l1 = testimonialRepository.AdminTestimonialList();
            return l1;
        }
        public AdminTestimonial Savetestimonial(AdminTestimonial adminTestimonial)
        {
            testimonialRepository.SaveAdminTestimonial(adminTestimonial);
            return adminTestimonial;
        }
        public void Savetestimonialpath(AdminTestimonialPath adminTestimonialPath)
        {
            testimonialpathRepository.SaveAdminTestimonial(adminTestimonialPath);
        }

        public List<AdminTestimonial> GetOrderid(long id)
        {
            return testimonialRepository.GetOrderid(id);
        }
    }
}
