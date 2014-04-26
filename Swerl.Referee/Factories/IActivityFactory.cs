using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swerl.Referee.Activities;

namespace Swerl.Referee.Factories
{
    public interface IActivityFactory
    {
        IActivity BuildDefaultActivity(string name);
        IActivity BuildActivity<T>(params object[] parameters) where T : IActivity;
        IActivity BuildActivity(Type type, params object[] parameters);
    }
}
