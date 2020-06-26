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
            int providerUserId = post.UserProfileId;
            UserProfile userProfile = _userRepo.GetById(providerUserId);
            int currentUserId = GetCurrentUserProfileId();

            if (userProfile == null)
            {
                return RedirectToAction("Index", "Post");
            } else
            {
                SubscribeViewModel vm = new SubscribeViewModel()
                {
                    SubscriberUserId = currentUserId,
                    ProviderUserId = providerUserId,
                    PostId = post.Id,
                    ProviderUserProfile = userProfile
                };

                return View(vm);
            }

            
        }

        // POST: SubscriptionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int id, SubscribeViewModel vm)
        {
            try
            {
                int currentUserId = GetCurrentUserProfileId();
                Subscription sub = new Subscription()
                {
                    SubscriberUserProfileId = currentUserId,
                    ProviderUserProfileId = vm.ProviderUserId
                };
                _subRepo.Add(sub);
                return RedirectToAction("Details", "Post", new { id = vm.PostId });
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: SubscriptionController/Delete/5
        public ActionResult Delete(int id)
        {
            Post post = _postRepo.GetPostById(id);
            int providerUserId = post.UserProfileId;
            UserProfile userProfile = _userRepo.GetById(providerUserId);

            if (userProfile == null)
            {
                return RedirectToAction("Index", "Post");
            }
            else
            {
                int currentUserId = GetCurrentUserProfileId();
                SubscribeViewModel vm = new SubscribeViewModel()
                {
                    SubscriberUserId = currentUserId,
                    ProviderUserId = providerUserId,
                    PostId = post.Id,
                    ProviderUserProfile = userProfile
                };

                return View(vm);
            }
        }

        // POST: SubscriptionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, SubscribeViewModel vm)
        {
            try
            {
                int currentUserId = GetCurrentUserProfileId();
                vm.SubscriberUserId = currentUserId;
                Subscription sub = _subRepo.GetSubscription(vm);
                _subRepo.Delete(sub);
                return RedirectToAction("Details", "Post", new { id = vm.PostId });
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
    }
}
