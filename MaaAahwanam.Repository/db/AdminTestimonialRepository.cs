using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;
using MaaAahwanam.Repository;

namespace MaaAahwanam.Repository.db
{
    public class AdminTestimonialRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        MaaAahwanamEntities maaAahwanamEntities = new MaaAahwanamEntities();
        public List<SP_GetTestimonials_Result> AdminTestimonialList()
        {
            return maaAahwanamEntities.SP_GetTestimonials().ToList();
        }
        public AdminTestimonial SaveAdminTestimonial(AdminTestimonial adminTestimonial)
        {
            _dbContext.AdminTesimonial.Add(adminTestimonial);
            _dbContext.SaveChanges();
            return adminTestimonial;
        }

        public List<AdminTestimonial> GetOrderid(long id)
        {
            return _dbContext.AdminTesimonial.Where(m=>m.Orderid == id).ToList();
        }
    }
}
