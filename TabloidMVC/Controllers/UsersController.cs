using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserProfileRepository _userRepo;

        public UsersController(IConfiguration config)
        {
            _userRepo = new UserProfileRepository(config);
        }
        // GET: UsersController
        public ActionResult Index(bool deactivated)
        {
            UserListViewModel vm = new UserListViewModel();
            vm.Users = _userRepo.GetByActivation(deactivated);
            vm.Deactivated = deactivated;
            vm.User = new UserProfile();

            return View(vm);

        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            UserProfile user = _userRepo.GetById(id);

            if(user == null)
            {
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Deactivate Form
        public ActionResult Deactivate(int id)
        {
            UserProfile user = _userRepo.GetById(id);

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // POST: Deactivate User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id, UserProfile user)
        {
            try
            {
                _userRepo.DeactivateUser(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(user);
            }
        }

        public ActionResult Reactivate(int id)
        {
            _userRepo.ReactivateUser(id);

            return RedirectToAction("Index");
        }
    }
}
