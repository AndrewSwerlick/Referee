using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.NerdDinnerSample.App_Start;

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
            var service = MVC.Referee.Configure((builder) => builder.InvokeStaticRegistrationMethods(typeof(MvcApplication).Assembly));
            AutofacConfig.Conifgure(service);
        }
    }
}
