namespace Swerl.Referee.Activities
{
    public class NamedActivity : IActivity
    {
        public string Name { get; private set; }
        public NamedActivity(string name)
        {
            Name = name;
        }
    }
}
