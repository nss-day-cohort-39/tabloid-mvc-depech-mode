using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostTagController : Controller
    {

        private readonly PostRepository _postRepo;
        private readonly TagRepository _tagRepo;
        private readonly PostTagRepository _postTagRepo;

        public PostTagController(IConfiguration config)
        {
            _postRepo = new PostRepository(config);
            _tagRepo = new TagRepository(config);
            _postTagRepo = new PostTagRepository(config);
        }

        // GET: PostTagController
        public ActionResult Manage(int id)
        {
            var vm = new PostTagViewModel();

            var post = _postRepo.GetPostById(id);
            vm.Post = post;

            if (post == null)
            {
                return RedirectToAction("Index", "MyPosts");
            }

            int currentUserId = GetCurrentUserProfileId();
            string UsersRole = GetCurrentUserRole();
            if (UsersRole == "Author" && post.UserProfileId != currentUserId)
            {
                return RedirectToAction("Index", "MyPosts");
            }

            List<Tag> postTags = _postTagRepo.GetPostTags(id); //list of all tags for this particular post
            vm.Tags = postTags;
            
            //create a comma separated string of the tags
            if (postTags.Count > 0)
            {
                string tagString = "";

                for (int i = 0; i < postTags.Count; i++)
                {
                    tagString += postTags[i].Name;
                    if (i != (postTags.Count-1))
                    {
                        tagString += ",";
                    }
                }
                vm.TagString = tagString;
            }
            vm.TagList = _tagRepo.GetAll(); //list of all possible tags

            return View(vm);
        }

        // POST: PostTagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(int id, PostTagViewModel vm)
        {
            try
            {

                var post = _postRepo.GetPostById(id);
                vm.Post = post;
                vm.TagList = _tagRepo.GetAll(); //list of all possible tags

                _postTagRepo.UpdateTags(vm);

                if (post.UserProfileId == GetCurrentUserProfileId())
                {
                    return RedirectToAction("Details", "MyPosts", new { id = vm.Post.Id });
                } else
                {
                    return RedirectToAction("Details", "Post", new { id = vm.Post.Id });
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        private string GetCurrentUserRole()
        {
            return User.FindFirstValue(ClaimTypes.Role);
        }
    }

}
