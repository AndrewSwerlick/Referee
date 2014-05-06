Referee
=======

Referee is an activity based authorization framework developed for ASP.NET MVC but designed to be used in any .NET scenario.

Why Referee
===========

Built in authorization mechanisms for ASP.NET MVC are limited to simple roles based authorization. When developers need to go beyond this, they often end up putting authorization logic directly into the body of their controller actions. 
This harms testability and maintainability of applications, because authorization logic is mixed in with non-authorization code. Authorization logic has to be duplicated, to protect controller actions and to determine whether or not a user
should see links to those actions. Referee offers a framework that allows developers to encapsulate authorization logic in specific classes and to quickly configure how authorization is applied to various controller actions.

Getting Started
===============
To start working with Referee install the nuget package available at this feed https://www.myget.org/feed/Packages/swerl. For MVC apps install the package Swerl.Referee.MVC. 

Once the package is installed, you'll need to configure Referee to run during application startup. The easiest way to do that is to add these lines to your Global.asax.cs file

	Swerl.Referee.MVC.Referee.Configure((builder) =>
	{
		//Your registration code here
	});

In the middle section, you add registration code specific to tell Referee how specific controller actions should be authorized. 

For example, imagine you have a Controller called MyController with an action called Foo. To make it so that only authenticated users can hit the Foo action, you'd write a line like this.

	builder.Register(a =>a.Method<MyController>(c => c.Foo()).AuthorizedBy<Authenticated>());

If you want all controller actions in MyController to require authentication you'd write this

	builder.RegisterClassMethods<MyController>(a => a.AuthorizedBy<Authenticated>());

What's actually going on here?
------------------------------
You're telling Referee that some method in your application is supposed to be authorized by the Authenticated class. The Authenticated class is a special class defined by Referee that implements the type IActivityAuthorizer.

Aside from the Authenticated class, Referee exposes one other authorizer called HasRoles. You use it in a similar way, like this. 

	builder.Register(a =>a.Method<MyController>(c => c.Foo())
						  .AuthorizedBy<HasRoles>(r=> r.Roles("Bar"));

However the real power in Referee is not using the built in IActivityAuthorizer classes but creating your own.

Custom Authorizers
-------------------
To create a custom authorizer, all you have to do is inherit from the class IAuthorizer. For example here is an authorizer that only allows users whose name starts with "A" to perform the action.

	public class StartsWithA : IActivityAuthorizer
    {
        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return user.Identity.Name.StartsWith("A");
        }
    }

Now you can tell Referee to use this authorizer in your application with the same syntax as above

	builder.Register(a =>a.Method<MyController>(c => c.Foo()).AuthorizedBy<StartsWithA>());

You can also tell referee to run configuration logic on the authorizer after it's built. If you want a more generic IActivityAuthorizer that will work with any letter you can write that like this.

	public class StartsWith : IActivityAuthorizer
    {
		public string StartingString {get;set;}

        public bool Authorize(IActivity activity, IPrincipal user)
        {
            return user.Identity.Name.StartsWith(StartingString);
        }
    }

Then you can register it like this.

	builder.Register(a => a.Method<MyController>(c => c.Foo())
						  .AuthorizedBy<StartsWithA>(a=> a.StartingString = "A"));

Consuming Method Parameters
---------------------------
What if your method MyController.Foo takes a parameter though? Like an integer id for a specific record in the database? In this case, Referee allows you to create another class that will help you pass that that data into your authorizer. 
That class should implement IActivity or inherit from one of the abstract classes built into Referee that implements IActivity (TypedActivity,NameActivity,MethodActivity). For your Foo method it would be something like this.

	public class FooActivity : TypedActivity
    {
        public int Id { get; private set; }

        public FooActivity(int id)
        {
            Id = id;
        }        
    }

Then you can create an IActivityAuthorizer like this
	
	public class EditFooAuthorizer : AbstractActivityAuthorizer<FooActivity>
    {       
        public override bool Authorize(FooActivity activity, IPrincipal user)
        {
            return activity.Id > 5 || user.IsInRole("Admin");
        }
    }


Finally you tie this all together with the registration

	builder.Register(a =>a.Method<MyController>(c => c.Foo(0))
						  .As<FooActivity>()
						  .AuthorizedBy<EditFooAuthorizer>());

Now users can only edit Foos if they're an Admin, or the Foo has an id less than 5.

You can reuse the same FooActivity class with multiple methods. For example, imagine there are two versions of the MyController.Foo class, one that take in an int as a parameter, and one that takes in a string. 
All you have to do to support the string verion is add a constructor to FooActivity that takes a string.

	public class FooActivity : TypedActivity
    {
        public int Id { get; private set; }

        public FooActivity(int id)
        {
            Id = id;
        }  
		
		public FooActivity(string id)
		{
			Id = int.Parse(id);
		}      
    }

If your method takes multiple parameters, you just need to ensure that there's a constructor with the a matching list of parameters

Using Referee In Your Controller Actions
----------------------------------------
Part of the reason we built referee is to make it easy to use your authorization logic inside your controller actions, for doing things like creating conditional links. To this end, Referee exposes an IAuthorizationService logic which you can call to quickly authorizer users.
You can get an instance of this object through dependency injection, or by accessing the static Referee.CurrentAuthorizationService object.

Let's say you're in the index method of your controller "MyController" and you want to check that the current user is authorized to run the "CreateFoo" method. You can simply do this.
	
	service.Authorize<MyController>(c=> c.CreateFoo(), User)

The method will return true or false depending on whether or not the user is authorized.

You can also pass method parameters into the method you're authorizing. For example, let's say you want to see if the current user is allowed to edit the Foo with Id 6.

	service.Authorize<MyController>(c=> c.EditFoo(6), User)

Again, you'll get a true false response you can use to decide if you want to render the link.

Using Dependency Injection
--------------------------
Referee is built with dependency injection in mind, and you can use dependency injection in your authorizers, and to get access to the IAuthorizationService object.

Out of the box all authorizers are built using calls to the current MVC DependencyResolver. So whatever Dependency injection framework you use, Referee should play nicely with it so long as you using DependencyResolver.SetCurrent() to wire up your dependency resolution.

If this behavior does not suit your needs you can create your own implementation of the IAuthorizerFactory class for building up IActivityAuthorizers. 
Then instead of using the static configuration code, you'll need to manaully build up an instance of the MVCRefereeConfigurationBuilder class, passing in your own IAuthorizerFactory instance.
Once you've called all the configuration logic against that instance of the MVCRefereeConfigurationBuilder class, you can call the Swerl.Referee.MVC.Referee.Configure and pass in the builder instance.

To register the current authorization service with your dependency injection framework, simply store the return value of the Swerl.Referee.MVC.Referee.Configure method. 
This method returns the configured IAuthorizationService instance, which you can register with your DI framework as a single application instance.














