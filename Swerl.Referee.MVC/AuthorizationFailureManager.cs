using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Swerl.Referee.Activities;

namespace Swerl.Referee.MVC
{
    public class AuthorizationFailureManager : IAuthorizationFailureManager
    {
        private readonly IDictionary<string, ActionResult> _resultsForNamedActivities;
        private readonly IDictionary<Type, ActionResult> _resultsForTypeActivities;

        public AuthorizationFailureManager(IList<MVCActivityRegistration> activityRegistrations)
        {
            _resultsForNamedActivities = activityRegistrations.ToDictionary(a => a.ActivityName, a => a.FailedResult);
            _resultsForTypeActivities = activityRegistrations.ToDictionary(a => a.ActivityType, a => a.FailedResult);
        }

        public void HandleFailedAuthorization(IActivity activity, ActionExecutingContext actionContext)
        {
            if (_resultsForTypeActivities.ContainsKey(activity.GetType()))
            {
                actionContext.Result = _resultsForTypeActivities[activity.GetType()];
            }
            else if (_resultsForNamedActivities.ContainsKey(activity.Name))
            {
                actionContext.Result = _resultsForNamedActivities[activity.Name];
            }
            else
                actionContext.Result = new HttpUnauthorizedResult();
        }
    }
}
