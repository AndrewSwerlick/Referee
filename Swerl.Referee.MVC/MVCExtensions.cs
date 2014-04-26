using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Swerl.Referee.Activities;
using Swerl.Referee.Configuration;
using Swerl.Referee.Resolvers;

namespace Swerl.Referee.MVC
{
    public static class MVCExtensions
    {              
        public static MVCActivityRegistration HandleFailureWith<T>(this ActivityRegistration registration) where T : ActionResult
        {
            var mvcRegistration = (MVCActivityRegistration) registration;
            mvcRegistration.FailedResult = Activator.CreateInstance<T>();
            return mvcRegistration;
        }

        public static ActivityRegistration HandleFailureWith(this ActivityRegistration registration,ActionResult result)
        {
            var mvcRegistration = (MVCActivityRegistration)registration;
            mvcRegistration.FailedResult = result;
            return mvcRegistration;
        }
    }
}
