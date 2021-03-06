﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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
        private readonly CommentRepository _commentRepository;
        private readonly PostTagRepository _postTagRepository;
        private readonly PostTagRepository _postTagRepo;
        private readonly SubscriptionRepository _subRepo;

        public PostController(IConfiguration config)
        {
            _postRepository = new PostRepository(config);
            _categoryRepository = new CategoryRepository(config);
            _commentRepository = new CommentRepository(config);
            _postTagRepository = new PostTagRepository(config);
            _postTagRepo = new PostTagRepository(config);
            _subRepo = new SubscriptionRepository(config);
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
            int userId = GetCurrentUserProfileId();
            if (post == null)
            {
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            post.Tags = _postTagRepo.GetPostTags(id);
            post.IsSubscribed = _subRepo.IsSubscribed(new SubscribeViewModel() { SubscriberUserId = userId, ProviderUserId = post.UserProfileId });
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
            int userId = GetCurrentUserProfileId();
            vm.Post = post;

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            int currentUserId = GetCurrentUserProfileId();
            string UsersRole = GetCurrentUserRole();
            if(UsersRole == "Author" && post.UserProfileId != currentUserId)
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
            var post = _postRepository.GetPostById(id);

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            int currentUserId = GetCurrentUserProfileId();
            string UsersRole = GetCurrentUserRole();
            if (UsersRole == "Author" && post.UserProfileId != currentUserId)
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
        private string GetCurrentUserRole()
        {
            return User.FindFirstValue(ClaimTypes.Role);
        }
    }

}
