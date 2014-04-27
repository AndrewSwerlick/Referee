namespace Swerl.Referee.Core.Activities
{
    public class TypedActivity : IActivity
    {
        public string Name
        {
            get
            {
                return this.GetType().AssemblyQualifiedName;
            }
        }
    }
}
