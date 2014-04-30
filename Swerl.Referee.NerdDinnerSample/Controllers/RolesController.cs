using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Swerl.Referee.Core.Configuration;
using Swerl.Referee.MVC.Configuration;
using Swerl.Referee.NerdDinnerSample.Models;
using Swerl.Referee.NerdDinnerSample.Models.EditModels;
using Swerl.Referee.NerdDinnerSample.Models.ViewModels;
using Swerl.Referee.NerdDinnerSample.Security.Authorizers;

namespace Swerl.Referee.NerdDinnerSample.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        [AuthorizationRegistration]
        public static void RegisterAuth(MVCRefereeConfigurationBuilder builder)
        {
            builder.RegisterClassMethods<RolesController>(a=> a.AuthorizedBy<Authenticated>());
        }

        public RolesController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View(new RolesIndexPageViewModel
            {
                Users = _context.Users.ToList()
            });
        }

        public ActionResult EditRoles(string id)
        {
            var user = _context.Users.Find(id);
            var em = new RolesEditModel
            {
                Id = id,
                RoleOptions = _context.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id,
                }).ToList(),
                SelectedRoles = user.Roles.Select(r => r.RoleId).ToList(),
                UserName = user.UserName
            };
            return View(em);
        }

        public ActionResult SaveRoles(RolesEditModel model)
        {
            var user = _context.Users.Find(model.Id);
            model.RoleOptions = _context.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
            }).ToList();

            foreach (var role in model.SelectedRoles)
            {
                _userManager.AddToRole(model.Id, role);
            }
            
            _context.SaveChanges();

            return RedirectToAction("EditRoles",new {id=model.Id});
        }
    }
}