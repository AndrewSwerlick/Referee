using System;
using System.Diagnostics;
using System.Reflection;

namespace Swerl.Referee.Core.Activities
{
    public class MethodActivity : IActivity
    {
        public MethodInfo Info { get; private set; }

        public MethodActivity(MethodInfo info)
        {
            if(info.DeclaringType == null)
                throw new ArgumentException("Cannot create a method activity from a method not declared on a concrete type","info");

            Info = info;
            Name = info.DeclaringType.FullName + "-" + Info.Name;
        }

        public string Name { get; set; }
    }
}
