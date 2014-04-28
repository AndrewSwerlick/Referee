Referee
=======

Referee is an activity based authorization framework developed for ASP.NET MVC but designed to be used in any .NET scenario.

Why Referee
===========

Built in authorization mechanisms for ASP.NET MVC are limited to simple roles based authorization. When developers need to go beyond this, they often end up putting authorization logic directly into the body of their controller actions. 
This harms testability and maintainability of applicaitons, because authorization logic is mixed in with non-authorization code. Authorization logic has to be duplicated, to protect controller actions and to determine whether or not a user
should see links to those actions. Referee offers a framework that allows developers to encapsulate authorization logic in specific classes and to quickly configuration how authorization is applied to various controller actions.

Getting Started
===============
TODO. Look at the Referee.NerdDinnerSample for now