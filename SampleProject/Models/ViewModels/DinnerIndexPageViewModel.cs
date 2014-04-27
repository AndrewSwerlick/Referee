using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Swerl.Referee.NerdDinnerSample.Models.ViewModels
{
    public class DinnerIndexPageViewModel
    {
        public IList<Dinner> Dinners { get; set; }
        public bool CanCreateDinner { get; set; }
    }
}