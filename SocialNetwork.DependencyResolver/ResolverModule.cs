﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Web.Common;
using SocialNetwork.Bll.Interface.Services;
using SocialNetwork.Bll.Service;
using SocialNetwork.Dal;
using SocialNetwork.Dal.Interface;
using SocialNetwork.Dal.Interface.Repository;
using SocialNetwork.Dal.Repository;
using SocialNetwork.Orm;

namespace SocialNetwork.DependencyResolver
{
    public static class ResolverConfig
    {
        public static void ConfigurateResolver(this IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<DbContext>().To<SocialNetworkDatabase>().InRequestScope();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
        }
    }
}
