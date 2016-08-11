using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
namespace MaaAahwanam.Web.Custom
{
    public class CustomPrincipal
    {
        public IIdentity Identity { get; private set; }
        public CustomPrincipal(string userId)
        {
            this.Identity = new GenericIdentity(userId);
        }

        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }       
        public string UserType { get; set; }      
    }
}