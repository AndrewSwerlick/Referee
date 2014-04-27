using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Referee.Activities
{
    public class MethodActivity : IActivity
    {
        public MethodInfo Info { get; private set; }

        public MethodActivity(MethodInfo info)
        {
            if(info.DeclaringType == null)
                throw new ArgumentException("Cannot create a method activity from a method not declared on a concrete type","info");

            Info = info;
        }

        public string Name
        {
            get
            {
                Debug.Assert(Info.DeclaringType != null, "_info.DeclaringType != null");
                return Info.DeclaringType.FullName + "-" + Info.Name;
            }
        }
    }
}
