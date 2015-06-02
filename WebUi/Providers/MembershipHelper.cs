using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebUi.Providers
{
    public static class MembershipHelper
    {

        public static  int GetCurrentUserId(String userName)
        {
            MembershipUser membershipUser = Membership.GetUser(userName);
            if (membershipUser != null)
            {
                object providerUserKey = membershipUser.ProviderUserKey;
                if (providerUserKey != null)
                {
                    return (int)providerUserKey;
                }
            }
            return 1;
        }
    }
}