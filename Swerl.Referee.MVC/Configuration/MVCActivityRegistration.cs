using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swerl.Referee.Configuration;

namespace Swerl.Referee.MVC
{
    public class MVCActivityRegistration : ActivityRegistration<MVCActivityRegistration>
    {
        public ActionResult FailedResult { get; set; }

        public MVCActivityRegistration HandleFailureWith<T>() where T : ActionResult
        {
            FailedResult = Activator.CreateInstance<T>();
            return this;
        }

        public MVCActivityRegistration HandleFailureWith(ActionResult result)
        {
            FailedResult = result;
            return this;
        }
    }
}
