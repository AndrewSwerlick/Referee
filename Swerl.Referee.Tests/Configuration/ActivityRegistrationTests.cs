using NUnit.Framework;
using Swerl.Referee.Authorizers;
using Swerl.Referee.Configuration;

namespace Swerl.Referee.UnitTests.Configuration
{
    public class ActivityRegistrationTests
    {
        [Test]
        public void Ensure_We_Can_Fluently_Register_An_Authorizer_For_An_Activity_Registration()
        {
            var registration = new ActivityRegistration().AuthorizedBy<DefaultAuthorizer>();
            Assert.That(registration.AuthorizerType, Is.EqualTo(typeof (DefaultAuthorizer)));
        }
    }
}
