using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swerl.Referee.NerdDinnerSample.Models.EditModels;

namespace Swerl.Referee.NerdDinnerSample.Models.ViewModels
{
    public class DinnerViewModel
    {
        public DinnerEditModel Data { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}