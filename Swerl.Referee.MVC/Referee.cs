using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swerl.Referee.Core;
using Swerl.Referee.Core.Factories;
using Swerl.Referee.MVC.Configuration;
using Swerl.Referee.MVC.Factories;

namespace Swerl.Referee.MVC
{
    public static class Referee
    {
        public static IAuthorizationService Configure(MVCRefereeConfigurationBuilder builder)
        {
            var configuration = builder.Build();
            var service = new MvcAuthorizationService(configuration.AuthorizerResolver, configuration.ActivityResolver,
                new AuthorizationFailureManager(builder.ActivityRegistrations));
            var filter = new AuthorizeActivity(service, GlobalFilters.Filters);

            GlobalFilters.Filters.Add(filter);
            CurrentAuthorizationService = service;
            return service;
        }

        public static IAuthorizationService Configure(Action<MVCRefereeConfigurationBuilder> builderConfiguration)
        {
            var builder = new MVCRefereeConfigurationBuilder(new AuthorizerFactory(), new ActivityFactory());
            builderConfiguration.Invoke(builder);            
            return Configure(builder);
        }

        public static IAuthorizationService CurrentAuthorizationService { get; private set; }
    }
}
