using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Swerl.Referee.NerdDinnerSample.Models.ViewModels
{
    public class RolesIndexPageViewModel
    {
        public IList<ApplicationUser> Users { get; set; } 
    }
}