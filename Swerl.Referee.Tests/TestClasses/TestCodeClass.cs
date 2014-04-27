using Swerl.Referee.Core.Configuration;

namespace Swerl.Referee.UnitTests.TestClasses
{
    public class TestCodeClass
    {
        public bool SomethingDone { get; set; }
        public string Param { get; set; }

        public void DoSomething(string param)
        {
            SomethingDone = true;
            Param = "test";
        }

        public void DoSomething2(string param)
        {
            SomethingDone = true;
            Param = "test";
        }
    }

    public class TestCodeClass2
    {
        [AuthorizationRegistration]
        public static void RegisterAuth(RefereeConfigurationBuilder configuration)
        {
            configuration.Register(a=> a.Method<TestCodeClass2>(c=> c.DoSomething(default(string))).AuthorizedBy<UnauthorizedAuthorizer>());
        }

        public bool SomethingDone { get; set; }
        public string Param { get; set; }

        public void DoSomething(string param)
        {
            SomethingDone = true;
            Param = "test";
        }
    }

    public class TestChildClass : TestCodeClass
    {
        
    }
}
