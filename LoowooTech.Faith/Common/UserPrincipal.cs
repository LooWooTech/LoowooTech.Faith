using LoowooTech.Faith.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace LoowooTech.Faith.Common
{
    public class UserPrincipal:IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public UserPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

    }

    public class UserIdentity : IIdentity
    {
        public static readonly UserIdentity Guest = new UserIdentity();
        public int UserID { get; set; }
        public UserRole Role { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string TelPhone { get; set; }
        public string AuthenticationType { get { return "Web.Session"; } }
        public bool IsAuthenticated { get { return UserID > 0; } }
    }
}