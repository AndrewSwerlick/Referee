using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
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
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}