using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Swerl.Referee.NerdDinnerSample.Models.EditModels
{
    public class RolesEditModel
    {
        public string Id { get; set; }
        public IList<SelectListItem> RoleOptions { get; set; }
        public IList<string> SelectedRoles { get; set; }
        public string UserName { get; set; }
    }
}