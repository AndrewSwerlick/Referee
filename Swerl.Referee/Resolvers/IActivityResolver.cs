using System;
using System.Linq.Expressions;
using Swerl.Referee.Core.Activities;

namespace Swerl.Referee.Core.Resolvers
{
    public interface IActivityResolver
    {
        IActivity GetActivity(string name);
        IActivity GetActivity(Type type, params object[] constructorParameters);
        IActivity GetActivity(LambdaExpression expression);
    }
}