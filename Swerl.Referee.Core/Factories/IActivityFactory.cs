using System;
using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Core.Factories
{
    public interface IActivityFactory
    {
        IActivity BuildDefaultActivity(string name);
        IActivity BuildActivity<T>(params object[] parameters) where T : IActivity;
        IActivity BuildActivity(Type type, params object[] parameters);
    }
}
