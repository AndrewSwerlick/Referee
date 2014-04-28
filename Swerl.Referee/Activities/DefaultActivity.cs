namespace Swerl.Referee.Core.Activities
{
    public class NamedActivity : IActivity
    {
        public string Name { get; set; }
        public NamedActivity(string name)
        {
            Name = name;
        }
    }
}
