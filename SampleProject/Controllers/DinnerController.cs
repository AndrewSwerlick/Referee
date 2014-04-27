using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Swerl.Referee.Core;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.MVC.Configuration;
using Swerl.Referee.NerdDinnerSample.Models;
using Swerl.Referee.NerdDinnerSample.Models.ViewModels;
using Swerl.Referee.NerdDinnerSample.Security.Authorizers;

namespace Swerl.Referee.NerdDinnerSample.Controllers
{
    public class DinnerController : Controller
    {
        [AuthorizationRegistration]
        public static void RegisterAuth(MVCRefereeConfigurationBuilder configuration)
        {
            configuration.RegisterClassMethods<DinnerController>(a => a.AuthorizedBy<Authenticated>());
        }

        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _service;

        public DinnerController(ApplicationDbContext context, IAuthorizationService service)
        {
            _context = context;
            _service = service;
        }

        public ActionResult Index()
        {
            var dinners = _context.Dinners;
            return View(new DinnerIndexPageViewModel
            {
                Dinners = dinners.ToList(),
                CanCreateDinner = _service.Authorize<DinnerController>(c=> c.Create(), User)
            });
        }

        public ActionResult Create()
        {
            return View();
        }
    }
}