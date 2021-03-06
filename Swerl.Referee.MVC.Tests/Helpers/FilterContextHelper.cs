﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Moq;
using Swerl.Referee.Core.Extensions;
using Swerl.Referee.MVC.Tests.TestClasses;
using Swerl.Referee.Tests.TestClasses;

namespace Swerl.Referee.MVC.Tests.Helpers
{
    public class FilterContextHelper
    {
        public static ActionExecutingContext ContextFromExpression<T>(Expression<Action<T>> expression)
        {
            var httpcontext = Mock.Of<HttpContextBase>();
            Mock.Get(httpcontext).Setup(x => x.User).Returns(new TestPrincipal());

            var info = expression.GetMethodInfo();
            var methodExp = expression.Body as MethodCallExpression;
            var dictionary = new Dictionary<string, object>();
            var arguments =
                methodExp.Arguments.Select(c => Expression.Lambda(c).Compile().DynamicInvoke()).ToArray();

            var parameters = info.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var name = parameters[i].Name;
                var argument = arguments[i];
                dictionary.Add(name,argument);
            }

            var context = new ActionExecutingContext(new ControllerContext
            {
                Controller = new TestController(),
                HttpContext = httpcontext
            },
                new ReflectedActionDescriptor(info.DeclaringType.GetMethod(expression.GetMethodName()), expression.GetMethodName(),
                    new ReflectedAsyncControllerDescriptor(info.DeclaringType)),
                dictionary);

            return context;
        }
    }
}
