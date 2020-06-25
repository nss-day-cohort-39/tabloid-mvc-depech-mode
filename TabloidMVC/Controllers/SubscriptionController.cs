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
    public class SubscriptionController : Controller
    {

        private readonly SubscriptionRepository _subRepo;
        private readonly UserProfileRepository _userRepo;
        private readonly PostRepository _postRepo;

        public SubscriptionController(IConfiguration config)
        {
            _subRepo = new SubscriptionRepository(config);
            _userRepo = new UserProfileRepository(config);
            _postRepo = new PostRepository(config);
        }


        // GET: SubscriptionController/Create
        public ActionResult Index(int id)
        {
            Post post = _postRepo.GetPostById(id);
            int subscriberUserId = post.UserProfileId;
            UserProfile userProfile = _userRepo.GetById(subscriberUserId);

            if (userProfile == null)
            {
                return RedirectToAction("Index", "Post");
            } else
            {
                SubscribeViewModel vm = new SubscribeViewModel()
                {
                    ProviderUserId = GetCurrentUserProfileId(),
                    SubscriberUserId = subscriberUserId,
                    PostId = post.Id,
                    ProviderUserProfile = userProfile
                };

                return View(vm);
            }

            
        }

        // POST: SubscriptionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IFormCollection collection)
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

        // GET: SubscriptionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubscriptionController/Delete/5
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

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
