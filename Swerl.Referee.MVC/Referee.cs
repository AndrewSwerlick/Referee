using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swerl.Referee.Core.Factories;
using Swerl.Referee.MVC.Configuration;
using Swerl.Referee.MVC.Factories;

namespace Swerl.Referee.MVC
{
    public static class Referee
    {
        public static void Configure(MVCRefereeConfigurationBuilder builder)
        {
            var configuration = builder.Build();
            var filter = new AuthorizeActivity(
                new MvcAuthorizationService(configuration.AuthorizerResolver, configuration.ActivityResolver,new AuthorizationFailureManager(builder.ActivityRegistrations)), GlobalFilters.Filters);

            GlobalFilters.Filters.Add(filter);
        }

        public static void Configure(Action<MVCRefereeConfigurationBuilder> builderConfiguration)
        {
            var builder = new MVCRefereeConfigurationBuilder(new AuthorizerFactory(), new ActivityFactory());
            builderConfiguration.Invoke(builder);
            Configure(builder);
        }
    }
}
