using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Swerl.Referee.Activities
{
    class MethodActivity : IActivity
    {
        private readonly MethodInfo _info;

        public MethodActivity(MethodInfo info)
        {
            if(info.DeclaringType == null)
                throw new ArgumentException("Cannot create a method activity from a method not declared on a concrete type","info");

            _info = info;
        }

        public string Name
        {
            get
            {
                Debug.Assert(_info.DeclaringType != null, "_info.DeclaringType != null");
                return _info.DeclaringType.FullName + "-" + _info.Name;
            }
        }
    }
}
