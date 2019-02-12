using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Repository.db;
using MaaAahwanam.Models;

namespace MaaAahwanam.Service
{
    public class RatingsServices
    {
        RatingsRepository ratingsRepository = new RatingsRepository();
        public List<Rating> GetRatings()
        {
            return ratingsRepository.GetRatings();
        }
    }
}
