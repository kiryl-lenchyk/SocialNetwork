using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace WebUi.Filters
{
    /// <summary>
    /// AuthorizeAttribute with role control, but roles names loaded from web.confir 
    /// </summary>
    public class AutorizeRolesFromConfigAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Create attribute with one role to control acsses
        /// </summary>
        /// <param name="roleConfigKey">config key for role name</param>
        public AutorizeRolesFromConfigAttribute(String roleConfigKey)
        {
            base.Roles = WebConfigurationManager.AppSettings[roleConfigKey];
        }

        /// <summary>
        /// Create attribute with many roles to control acsses
        /// </summary>
        /// <param name="rolesConfigKeys">config keys for roles names</param>
        public AutorizeRolesFromConfigAttribute(params String[] rolesConfigKeys)
        {
            base.Roles = String.Join(",",
                rolesConfigKeys.Select(x => WebConfigurationManager.AppSettings[x]));
        }
    }
}