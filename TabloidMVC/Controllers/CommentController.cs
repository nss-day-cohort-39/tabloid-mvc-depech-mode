﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly CommentRepository _commentRepository;

        public CommentController(IConfiguration config)
        {
            _postRepository = new PostRepository(config);
            _commentRepository = new CommentRepository(config);
        }
        // GET: CommentController
        public ActionResult Index(int PostId)
        {
            var vm = new CommentViewModel();
            List<Comment> comments = _commentRepository.GetCommentsByPostId(PostId);
            vm.Comments = comments;
            Post post = _postRepository.GetPostByIdForComment(PostId);
            vm.Post = post;
            return View(vm);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create(int PostId)
        { 
            var comm = new Comment();
            comm.PostId = PostId;
            comm.UserProfileId = GetCurrentUserProfileId();
            return View(comm);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            try
            {
                comment.CreateDateTime = DateTime.Now;
                _commentRepository.AddComment(comment);

                return RedirectToAction("Index", new { PostId = comment.PostId });
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return View(comment);
            }
        }

        //GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        //POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepository.UpdateComment(comment);

                return RedirectToAction("Index", new { PostId = comment.PostId } );
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);
            return View(comment);
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment)
        {
            try
            {
                _commentRepository.DeleteComment(comment.Id);
                return RedirectToAction("Index", new { PostId = comment.PostId });
            }
            catch (Exception ex)
            {  
                return View(comment);
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
