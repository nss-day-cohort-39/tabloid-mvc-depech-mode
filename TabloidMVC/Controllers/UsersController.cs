using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
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
        public ActionResult Index()
        {
            List<UserProfile> users = _userRepo.GetAll();

            return View(users);
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
    }
}
