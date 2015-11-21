using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talent.DataAccess.Ado;
using Talent.Domain;
using Talent.Mvc.Models;

namespace Talent.Mvc.Controllers
{
    public class ShowController : Controller
    {
        private ShowRepository _repo = new ShowRepository();

        // GET: /Show/
        public ActionResult Index()
        {
            var model = new ShowsViewModel();
            model.Shows = _repo.Fetch();
            return View(model);
        }

        // GET: /Show/Create
        public ActionResult Create()
        {
            Show show = new Show();
            var model = new ShowViewModel{ShowModel = show};
            return View("Edit", model);
        }

        // POST: /Show/Create
        [HttpPost]
        public ActionResult Create(ShowViewModel vm)
        {
            try
            {
                _repo.Persist(vm.ShowModel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Edit", vm);
            }
        }

        // GET: /Show/Edit/5
        public ActionResult Edit(int id)
        {
            var show = _repo.Fetch(id).FirstOrDefault();
            var model = new ShowViewModel { ShowModel = show };
            if (model == null) return HttpNotFound();
            return View(model);
        }

        // POST: /Show/Edit/5
        [HttpPost]
        public ActionResult Edit(ShowViewModel vm)
        {
            try
            {
                //var model = _repo.Fetch(vm.ShowModel.ShowId).FirstOrDefault();
                //if (model == null) return HttpNotFound();
                //model.Title = vm.ShowModel.Title;
                //model.LengthInMinutes = vm.ShowModel.LengthInMinutes;
                //model.TheatricalReleaseDate = vm.ShowModel.TheatricalReleaseDate;
                //model.DvdReleaseDate = vm.ShowModel.DvdReleaseDate;
                //model.MpaaRatingId = vm.ShowModel.MpaaRatingId;
                vm.ShowModel.IsDirty = true;
                _repo.Persist(vm.ShowModel);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: /Show/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _repo.Fetch(id).FirstOrDefault();
            if (model == null) return HttpNotFound();
            return View(model);
        }

        //
        // POST: /Show/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Show show = _repo.Fetch(id).FirstOrDefault();
                show.IsMarkedForDeletion = true;
                _repo.Persist(show);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
