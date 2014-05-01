Getting Started
================

To use and configure referee add the following code to your Global.asax file

MVC.Referee.Configure((builder) =>{
    //using the builder we can register differment methods in our controllers to be authorized by different IActivityAuthorizer classes, like the Authenticated class, which checks to see if a user is logged in
    builder.Register(a =>a.Method<AccountController>(c => c.Manage(default(ManageUserViewModel))).AuthorizedBy<Authenticated>());

    //We can also push our registration logic out to static methods in our other classes. This method calls all static methods in the defined assembly decorated with the "AuthorizationRegistration" attribute
    builder.InvokeStaticRegistrationMethods(typeof (MvcApplication).Assembly);
});

Replace the piece in the middle with your own authorization registration logic.

For more information refer to https://github.com/AndrewSwerlick/Referee