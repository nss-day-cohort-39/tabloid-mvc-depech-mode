using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
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
                return View("../Post/");
            }

            vm.Tags = _postTagRepo.GetPostTags(id); //list of all tags for this particular post
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

                return RedirectToAction("../Post/");
            }
            catch
            {
                return View(vm);
            }
        }
    }

}
