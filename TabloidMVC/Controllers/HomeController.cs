using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostRepository _postRepo;
        private readonly PostTagRepository _postTagRepo;

        public HomeController(IConfiguration config)
        {
            _postRepo = new PostRepository(config);
            _postTagRepo = new PostTagRepository(config);
        }

            public IActionResult Index()
        {
            int? currentUserId = GetCurrentUserProfileId();
                if (currentUserId == null)
            {
                return View();
            } else
            {
                List<Post> posts = _postRepo.GetSubscribedPostByUserId(currentUserId);
                foreach (Post post in posts)
                {
                    List<Tag> tags = _postTagRepo.GetPostTags(post.Id);
                    post.Tags = tags;

                }
                HomeViewModel vm = new HomeViewModel()
                {
                    CurrentUserId = currentUserId,
                    Posts = posts
                };
                return View(vm);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private int? GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != null)
            {
                return int.Parse(id);
            } else
            {
                return null;
            }
            
        }
    }
}
