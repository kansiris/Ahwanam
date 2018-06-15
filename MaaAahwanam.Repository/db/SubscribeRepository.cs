using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaaAahwanam.Models;

namespace MaaAahwanam.Repository.db
{
    public class SubscribeRepository
    {
        readonly ApiContext _dbContext = new ApiContext();
        public void SavingSubscriptionEmailId(Subscription subscription)
        {
            _dbContext.Subscription.Add(subscription);
            _dbContext.SaveChanges();
        }
        public long checkmail(string email)
        {
            var count = _dbContext.Subscription.Where(m => m.EmailId == email).FirstOrDefault();
            if (count != null)
                return count.SubscriptionId;
            else
                //count.UserLoginId = 0;
                return 0;
        }
    }
}
