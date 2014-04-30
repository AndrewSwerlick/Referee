using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using Swerl.Referee.Core;
using Swerl.Referee.Core.Activities;
using Swerl.Referee.Core.Authorizers;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.MVC.Configuration;
using Swerl.Referee.NerdDinnerSample.Models;
using Swerl.Referee.NerdDinnerSample.Models.EditModels;
using Swerl.Referee.NerdDinnerSample.Models.ViewModels;
using Swerl.Referee.NerdDinnerSample.Security.Activities;
using Swerl.Referee.NerdDinnerSample.Security.Authorizers;

namespace Swerl.Referee.NerdDinnerSample.Controllers
{
    public class DinnerController : Controller
    {
        [AuthorizationRegistration]
        public static void RegisterAuth(MVCRefereeConfigurationBuilder configuration)
        {
            //Ensure all actions on this controller require the user to be authenticated
            configuration.RegisterClassMethods<DinnerController>(a => a.AuthorizedBy<Authenticated>());

            //Ensure that the create action on this controller can only be called by users with the"Host" role
            configuration.RegisterEach<DinnerController>(c=> c.Create(),c=> c.Create(default(DinnerEditModel))).With(a=> a.AuthorizedBy<HasRoles>(r=> r.Roles("Host")));

            //Ensure that the delete action on this controller invokes a custom authorizer that checks in the database to see what roles
            //Are required for the activity named "Delete"
            configuration.Register(a => a.Method<DinnerController>(c => c.Delete(default(int))).Name("Delete").AuthorizedBy<RolesInDatabase>());

            configuration.RegisterEach<DinnerController>(a=> a.Edit(0), a=> a.Edit(default(DinnerEditModel)))
                .With(a=> a.As<EditDinner>().AuthorizedBy<EditDinnerAuthorizer>());
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

        [HttpPost]
        public ActionResult Create(DinnerEditModel model)
        {
            var dinner = Mapper.Map<Dinner>(model);
            _context.Dinners.Add(dinner);
            var user = _context.Users.Single(u => u.UserName == User.Identity.Name);
            dinner.Host = user;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult View(int id)
        {
            var dinner = _context.Dinners.Find(id);
            var vm = new DinnerViewModel
            {
                Data = Mapper.Map<DinnerEditModel>(dinner),
                CanEdit = _service.Authorize<DinnerController>(c => c.Edit(dinner.Id), User)
            };
            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var dinner = _context.Dinners.Find(id);
            return View(Mapper.Map<DinnerEditModel>(dinner));
        }

        [HttpPost]
        public ActionResult Edit(DinnerEditModel model)
        {
            var dinner = _context.Dinners.Find(model.Id);
            Mapper.Map(model, dinner);
            _context.SaveChanges();
            return RedirectToAction("View",new{id = model.Id});
        }

        [HttpPost]
        public ActionResult Delete(int dinnerId)
        {
            return View();
        }
    }
}