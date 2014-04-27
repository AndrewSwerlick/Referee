using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Swerl.Referee.Activities;

namespace Swerl.Referee.MVC
{
    public class AuthorizationFailureManager : IAuthorizationFailureManager
    {
        private readonly IList<MVCActivityRegistration> _activityRegistrations;

        public AuthorizationFailureManager(IList<MVCActivityRegistration> activityRegistrations)
        {
            _activityRegistrations = activityRegistrations.Where(a=> a.FailedResult != null).ToList();           
        }

        public void HandleFailedAuthorization(IActivity activity, ActionExecutingContext actionContext)
        {
            if (activity is MethodActivity && _activityRegistrations.Any(a => a.ActivityMethod == ((MethodActivity)activity).Info))
            {
                var registration = _activityRegistrations.Single(a => a.ActivityMethod == ((MethodActivity) activity).Info);
                actionContext.Result = registration.FailedResult;
            }
            else if (_activityRegistrations.Any(a => a.ActivityType == activity.GetType()))
            {
                actionContext.Result = _activityRegistrations.Single(a => a.ActivityType ==  activity.GetType()).FailedResult;
            }           
            else if (activity is NamedActivity && _activityRegistrations.Any(a => a.ActivityName == activity.Name))
                actionContext.Result = _activityRegistrations.Single(a => a.ActivityName == activity.Name).FailedResult;
            else
                actionContext.Result = new HttpUnauthorizedResult();
        }
    }
}
