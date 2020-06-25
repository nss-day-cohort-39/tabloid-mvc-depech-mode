using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class TagsController : Controller
    {
        private readonly TagRepository _tagRepo;

        public TagsController(IConfiguration config)
        {
            _tagRepo = new TagRepository(config);
        }
            // GET: TagsController
        public ActionResult Index()
        {
            List<Tag> tags = _tagRepo.GetAll();

            return View(tags);
        }

        // GET: TagsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepo.Add(tag);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tag);
            }
        }

        // GET: TagsController/Edit/5
        public ActionResult Edit(int id)
        {
            var tag = _tagRepo.GetTagById(id);

            if (tag == null)
            {
                return RedirectToAction("Index");
            }

            return View(tag);
        }

        // POST: TagsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepo.Update(tag);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(tag);
            }
        }

        // GET: TagsController/Delete/5
        public ActionResult Delete(int id)
        {
            var tag = _tagRepo.GetTagById(id);

            if (tag == null)
            {
                return RedirectToAction("Index");
            }

            return View(tag);
        }

        // POST: TagsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepo.Delete(tag);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }
    }
}
