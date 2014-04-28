using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Tests.TestClasses
{
    public class TestActivity : TypedActivity
    {
        public string Id { get; set; }

        public TestActivity()
        {

        }

        public TestActivity(string id)
        {
            Id = id;
        }

    }
}
