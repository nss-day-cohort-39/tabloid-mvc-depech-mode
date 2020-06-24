using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class MyPostsController : Controller
    {
        private readonly PostRepository _postRepo;
        private readonly CategoryRepository _categoryRepository;

        public MyPostsController(IConfiguration config)
        {
            _postRepo = new PostRepository(config);
            _categoryRepository = new CategoryRepository(config);
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
            var post = _postRepo.GetPublisedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepo.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }

            return View("../Post/Details", post);
        }

        // GET: MyPosts/Create
        public ActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View("../Post/Create", vm);
            
        }

        // POST: MyPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepo.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View("../Post/Create", vm);
            }
        }

        // GET: MyPosts/Edit/5
        public ActionResult Edit(int id)
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            int userId = GetCurrentUserProfileId();
            var post = _postRepo.GetUserPostById(id, userId);
            vm.Post = post;

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            return View("../Post/Edit", vm);
        }

        // POST: MyPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
                _postRepo.Update(vm.Post);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("../Post/Edit", vm);
            }
        }

        // GET: MyPosts/Delete/5
        public ActionResult Delete(int id)
        {
            int userId = GetCurrentUserProfileId();
            var post = _postRepo.GetUserPostById(id, userId);

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            return View("../Post/Delete", post);
        }

        // POST: MyPosts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepo.Delete(post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("../Post/Delete", post);
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
