using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.Core.Factories;
using Swerl.Referee.MVC.Factories;

namespace Swerl.Referee.MVC.Configuration
{
    public class MVCRefereeConfigurationBuilder : AbstractRefereeConfigurationBuilder<MVCActivityRegistration>
    {
        public MVCRefereeConfigurationBuilder(IAuthorizerFactory authorizerFactory, IActivityFactory activityFactory)
            : base(authorizerFactory, activityFactory)
        {

        }

        public override MVCActivityRegistration BuildRegistration()
        {
            return new MVCActivityRegistration();
        }


    }
}
