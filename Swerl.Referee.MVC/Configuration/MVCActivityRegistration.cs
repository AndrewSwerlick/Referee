using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swerl.Referee.Configuration;

namespace Swerl.Referee.MVC
{
    public class MVCActivityRegistration : ActivityRegistration
    {
        public ActionResult FailedResult { get; set; }
    }
}
