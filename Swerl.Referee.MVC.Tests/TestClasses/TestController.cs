﻿using System.Web.Mvc;

namespace Swerl.Referee.MVC.Tests.TestClasses
{
    class TestController : Controller
    {
        public ActionResult ControllerAction(string id)
        {
            return new EmptyResult();
        }
    }
}
