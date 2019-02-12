using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class RatingsRepository
    {
        readonly ApiContext _dbContext = new ApiContext();

        public List<Rating> GetRatings()
        {
            return _dbContext.Rating.ToList();
        }
    }
}
