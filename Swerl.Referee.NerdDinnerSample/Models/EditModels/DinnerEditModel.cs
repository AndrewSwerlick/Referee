using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Swerl.Referee.NerdDinnerSample.Models.EditModels
{
    public class DinnerEditModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}