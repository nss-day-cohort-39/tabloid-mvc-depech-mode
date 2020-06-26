using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly PostTagRepository _postTagRepo;

        public PostController(IConfiguration config)
        {
            _postRepository = new PostRepository(config);
            _categoryRepository = new CategoryRepository(config);
            _postTagRepo = new PostTagRepository(config);
        }

        public IActionResult Index()
        {
            var vm = new PostIndexViewModel();
            vm.Posts = _postRepository.GetAllPublishedPosts();
            vm.UserId = GetCurrentUserProfileId();
            vm.PostModel = new Post();
            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            post.Tags = _postTagRepo.GetPostTags(id);
            var vm = new PostIndexViewModel();
            vm.PostModel = post;
            vm.UserId = GetCurrentUserProfileId();
            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        public ActionResult Edit(int id)
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            var post = _postRepository.GetPostById(id);
            vm.Post = post;

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
                _postRepository.Update(vm.Post);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        public ActionResult Delete(int id)
        {
            int userId = GetCurrentUserProfileId();
            var post = _postRepository.GetPostById(id);

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.Delete(post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

    }

}
