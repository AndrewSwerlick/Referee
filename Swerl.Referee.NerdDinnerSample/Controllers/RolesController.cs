using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Swerl.Referee.Core.Authorizers;
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
        private readonly UserManager<ApplicationUser> _userManager;

        [AuthorizationRegistration]
        public static void RegisterAuth(MVCRefereeConfigurationBuilder builder)
        {
            builder.RegisterClassMethods<RolesController>(a=> a.AuthorizedBy<Authenticated>());
        }

        public RolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
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
                    Value = r.Name,
                }).ToList(),
                SelectedRoles = _userManager.GetRoles(id).ToList(),
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

            foreach (var role in model.RoleOptions)
            {
                _userManager.RemoveFromRole(model.Id, role.Value);
            }

            foreach (var role in model.SelectedRoles)
            {
                _userManager.AddToRole(model.Id, role);
            }
            
            _context.SaveChanges();

            //force logout login to refresh the claims cookie. Otherwise new roles won't take immediate effect
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);

            return RedirectToAction("EditRoles",new {id=model.Id});
        }
    }
}