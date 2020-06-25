﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class AccountController : Controller
    {
        UserProfileRepository _userProfileRepository;

        public AccountController(IConfiguration configuration)
        {
            _userProfileRepository = new UserProfileRepository(configuration);
        }

        public IActionResult Register()
        {
            var newUser = new UserProfile();
            return View(newUser);
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserProfile newUser)
        {
            if (_userProfileRepository.GetByEmail(newUser.Email) == null)
            {
                try
                {
                    newUser.CreateDateTime = DateAndTime.Now;
                    newUser.UserTypeId = 2;
                    _userProfileRepository.Add(newUser);

                }
                catch
                {
                    return View();
                }

                var registeredUser = _userProfileRepository.GetByEmail(newUser.Email);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, registeredUser.Id.ToString()),
                new Claim(ClaimTypes.Email, registeredUser.Email),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            return View();
            
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var userProfile = _userProfileRepository.GetByEmail(credentials.Email);

            if (userProfile == null)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
                new Claim(ClaimTypes.Role, userProfile.UserType.Name)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
