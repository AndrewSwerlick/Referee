using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Swerl.Referee.Core;
using Swerl.Referee.MVC;
using Swerl.Referee.NerdDinnerSample.Models;

namespace Swerl.Referee.NerdDinnerSample.App_Start
{
    public static class AutofacConfig
    {
        public static void Conifgure(IAuthorizationService authorizationService)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ApplicationDbContext>().InstancePerHttpRequest();
            builder.RegisterInstance(authorizationService).As<IAuthorizationService>();
            builder.RegisterControllers(typeof (AutofacConfig).Assembly);
            builder.RegisterAssemblyTypes(typeof (AutofacConfig).Assembly);
            builder.RegisterType<UserManager<ApplicationUser>>().InstancePerHttpRequest();
            builder.Register(c=> new UserStore<ApplicationUser>(c.Resolve<ApplicationDbContext>())).As<IUserStore<ApplicationUser>>().InstancePerHttpRequest();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}