using Swerl.Referee.Activities;

namespace Swerl.Referee.UnitTests.TestClasses
{
    public class TestActivity2 : TypedActivity
    {
        public string Id { get; set; }

        public TestActivity2(string id)
        {
            Id = id;
        }
    }
}
