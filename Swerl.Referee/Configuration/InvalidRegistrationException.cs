using System;

namespace Swerl.Referee.Core.Configuration
{
    public class InvalidRegistrationException : Exception
    {
        public InvalidRegistrationException(string message) : base(message)
        {
            
        }
    }
}
