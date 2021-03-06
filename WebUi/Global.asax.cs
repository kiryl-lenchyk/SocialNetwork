﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SocialNetwork.Logger.Interface;

namespace WebUi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ILogger logger = (ILogger)DependencyResolver.Current.GetService(typeof(ILogger));

            Exception exception = Server.GetLastError();
            Server.ClearError();
            
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                logger.Log(LogLevel.Warn,httpException.Message);
                Response.Redirect("~/404");
            }
            else
            {
                logger.Log(LogLevel.Fatal,exception.ToString());
                Response.Redirect("~/Error");
            }
        }
    }
}