using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace WebUi.Filters
{
    public class AutorizeRolesFromConfigAttribute : AuthorizeAttribute
    {
        public AutorizeRolesFromConfigAttribute(String roleConfigKey)
        {
            base.Roles = WebConfigurationManager.AppSettings[roleConfigKey];
        }

        public AutorizeRolesFromConfigAttribute(params String[] rolesConfigKeys)
        {
            base.Roles = String.Join(",",
                rolesConfigKeys.Select(x => WebConfigurationManager.AppSettings[x]));
        }
    }
}