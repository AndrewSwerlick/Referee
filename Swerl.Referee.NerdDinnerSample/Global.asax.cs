using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.NerdDinnerSample.App_Start;
using Swerl.Referee.NerdDinnerSample.Controllers;
using Swerl.Referee.NerdDinnerSample.Models;
using Swerl.Referee.NerdDinnerSample.Security.Authorizers;

namespace Swerl.Referee.NerdDinnerSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var service = MVC.Referee.Configure((builder) =>
            {
                //using the builder we can register differment methods in our controllers to be authorized by different IActivityAuthorizer classes, like the Authenticated class, which checks to see if a user is logged in
                builder.Register(a =>a.Method<AccountController>(c => c.Manage(default(ManageUserViewModel))).AuthorizedBy<Authenticated>());

                //We can also push our registration logic out to static methods in our other classes. This method calls all static methods in the defined assembly decorated with the "AuthorizationRegistration" attribute
                builder.InvokeStaticRegistrationMethods(typeof (MvcApplication).Assembly);
            });
            AutofacConfig.Conifgure(service);  
            AutomapConfig.Configure();
        }
    }
}
