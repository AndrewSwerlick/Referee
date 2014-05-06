Referee
=======

Referee is an activity based authorization framework developed for ASP.NET MVC but designed to be used in any .NET scenario.

Why Referee
===========

Built in authorization mechanisms for ASP.NET MVC are limited to simple roles based authorization. When developers need to go beyond this, they often end up putting authorization logic directly into the body of their controller actions. 
This harms testability and maintainability of applicaitons, because authorization logic is mixed in with non-authorization code. Authorization logic has to be duplicated, to protect controller actions and to determine whether or not a user
should see links to those actions. Referee offers a framework that allows developers to encapsulate authorization logic in specific classes and to quickly configure how authorization is applied to various controller actions.

Getting Started
===============
To start working with Referee install the nuget package available at this feed https://www.myget.org/feed/Packages/swerl. For MVC apps install the package Swerl.Referee.MVC. 

Once the package is installed, you'll need to configure Referee to run during application startup. The easiest way to do that is to add these lines to your Global.asax file

	MVC.Referee.Configure((builder) =>
	{
		//Your registration code here
	});

In the middle section, you add registration code specific to your app. In the regisistration code you tell Referee how specific controller actions should be authorized. 

For example, let's say you have a Controller called MyController with an action called Foo. To make it so that only authenticated users can hit the Foo action, you'd write a line like this.

	builder.Register(a =>a.Method<MyController>(c => c.Foo()).AuthorizedBy<Authenticated>());

If you want all controller actions in MyController to require authentication you'd write this

	builder.RegisterClassMethods<MyController>(a =>a.AuthorizedBy<Authenticated>());

What's actually going on here?
------------------------------
Basically in each case, you're telling Referee that some method in your application is supposed to be authorized by the Authenticated class. The Authenticated class is a special class defined by Referee that implements the type IActivityAuthorizer.

Aside from the Authenticated class, Referee exposes one other authorizer called HasRoles. You use it in a similar way, like this. 

	builder.Register(a =>a.Method<MyController>(c => c.Foo()).AuthorizedBy<HasRoles>(r=> r.Roles("Bar"));

However the real power in referee is not using the built in IActivityAuthorizer classes but creating your own

Custom Authorizers
-------------------
To create a custom authorizer, all you have to do is inherit from the class IAuthorizer. For example let's create an authorizer that only allows users who's name starts with "A" to perform the action.

	public class StartsWithA : IActivityAuthorizer
    {
        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return user.Identity.Name.StartsWith("A");
        }
    }

Now we can tell Referee to use this authorizer in our application with the same syntax as above

	builder.Register(a =>a.Method<MyController>(c => c.Foo()).AuthorizedBy<StartsWithA>();







