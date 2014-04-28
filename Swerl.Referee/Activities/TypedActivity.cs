namespace Swerl.Referee.Core.Activities
{
    public class TypedActivity : IActivity
    {
        public TypedActivity()
        {
            Name = this.GetType().AssemblyQualifiedName;
        }

        public string Name { get; set; }
    }
}
