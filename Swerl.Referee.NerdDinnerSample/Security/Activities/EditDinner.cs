using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.NerdDinnerSample.Models.EditModels;

namespace Swerl.Referee.NerdDinnerSample.Security.Activities
{
    public class EditDinner : TypedActivity
    {
        public int Id { get; private set; }

        public EditDinner(int id)
        {
            Id = id;
        }

        public EditDinner(DinnerEditModel model) : this(model.Id)
        {
            
        }
    }
}