using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Swerl.Referee.NerdDinnerSample.Models
{
    public class ActivityPermission
    {       
        public string Name { get; set; }
        public virtual IList<IdentityRole> Roles { get; set; } 
    }
}