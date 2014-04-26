using System;
using System.Linq;
using System.Web.Mvc;

namespace Swerl.Referee.MVC
{
    public class AuthorizeActivity : IActionFilter
    {
        private readonly GlobalFilterCollection _filters;
        private readonly IMvcAuthorizationService _service;
        private readonly AuthorizationFailureManager _failureManager;

        public AuthorizeActivity(IMvcAuthorizationService service, GlobalFilterCollection filters)
        {
            _filters = filters;
            _service = service;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!Apply(filterContext))
                return;
            _service.Authorize(filterContext, filterContext.HttpContext.User, true);            
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        private bool Apply(ActionExecutingContext filterContext)
        {
            return _filters.Any(f => f.Instance.GetType() == this.GetType()) ||
                   filterContext.ActionDescriptor.GetCustomAttributes(typeof (AuthorizeActivityAttribute), false).Any();
        }
    }

    public class AuthorizeActivityAttribute : Attribute
    {
       
    }
}