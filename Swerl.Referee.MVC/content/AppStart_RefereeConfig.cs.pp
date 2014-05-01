using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Infrastructure;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.MVC;

[assembly: WebActivatorEx.PostApplicationStartMethod(typeof($rootnamespace$.AppStart_RefereeConfig), "Start")]
 
namespace $rootnamespace$ {
    public static class AppStart_RefereeConfig {
        public static void Start() {
            // Sets up the referee authorization framework. Allows you to configure desired authorization behavior. 
            //For information see https://github.com/AndrewSwerlick/Referee
            Referee.Configure(b =>
            {
				//Place your register logic here 
                //b.Register(a => a.Method<MyController>(c => c.MyAction("").AuthorizedBy<Authenticated>));

				//or call
                //b.InvokeStaticRegistrationMethods(typeof (rootnamespace.AppStart_RefereeConfig).Assembly);
				//and place registration logic in other classes by creating a static method and decorate it with the AuthorizationRegistration attribute
            });
        }     
    }
}