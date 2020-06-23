using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class MyPostsController : Controller
    {
        private readonly PostRepository _postRepo;

        public MyPostsController(IConfiguration config)
        {
            _postRepo = new PostRepository(config);
        }

        // GET: MyPosts
        public ActionResult Index()
        {
            //Get the user ID
            int userId = GetCurrentUserId();

            List<Post> userPosts = _postRepo.GetAllUserPosts(userId);

            return View(userPosts);
        }

        // GET: MyPosts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MyPosts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MyPosts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MyPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MyPosts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MyPosts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
