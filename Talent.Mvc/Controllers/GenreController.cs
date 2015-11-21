using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talent.DataAccess.Ado;
using Talent.Domain;

namespace Talent.Mvc.Controllers
{
    public class GenreController : Controller
    {
        private GenreRepository _repo = new GenreRepository();

        // List Genres
        public ActionResult Index()
        {
            var model = _repo.Fetch();
            return View(model);
        }

        public ActionResult Create()
        {
            Genre model = new Genre();
            return View("Edit", model); // Create and Edit can share same view??
        }

        [HttpPost]
        public ActionResult Create(Genre model)
        {
            try
            {
                _repo.Persist(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Edit", model);
            }
        }

        public ActionResult Edit(int id)
        {
            var model = _repo.Fetch(id).FirstOrDefault();
            if (model == null) return HttpNotFound();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Genre vm)
        {
            if( ModelState.IsValid)
            { 
                try
                {
                    vm.IsDirty = true;
                    _repo.Persist(vm);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var model = _repo.Fetch(id).FirstOrDefault();
            if (model == null) return HttpNotFound();
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var model = _repo.Fetch(id).FirstOrDefault();
                if (model == null) return HttpNotFound();
                model.IsMarkedForDeletion = true;
                _repo.Persist(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
