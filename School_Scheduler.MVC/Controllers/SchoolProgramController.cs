using Microsoft.AspNet.Identity;
using School_Scheduler.MVC.Filters;
using School_Scheduler.MVC.Helpers;
using School_Scheduler.MVC.Models;
using School_Scheduler.MVC.Models.Domain;
using School_Scheduler.MVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School_Scheduler.MVC.Controllers
{

    // TODO: You must check the user's discriminator (Instructor)
    public class SchoolProgramController : Controller
    {
        private ApplicationDbContext DbContext;
        public SchoolProgramController()
        {
            DbContext = new ApplicationDbContext();
        }

        [EnsureDiscriminatorClaim(Discriminator.Instructor, Discriminator.Student)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult CreateProgram()
        {
            // TODO: You must check the user's discriminator (Instructor)
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult CreateProgram(CreateEditSchoolProgramViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Name = model.Name.Trim();
            if (DbContext.SchoolPrograms.Any(p =>
            p.Name.ToLower() == model.Name.ToLower()))
            {
                ModelState.AddModelError(nameof(CreateEditSchoolProgramViewModel.Name),
                    "Program Name should be unique");
                return View(model);
            }

            SchoolProgram program;
            program = new SchoolProgram();
            program.Name = model.Name;

            DbContext.SchoolPrograms.Add(program);
            DbContext.SaveChanges();
            return View();
        }

        //[HttpGet]
        //public ActionResult EditProgram(string name)
        //{
        //    var program = DbContext.SchoolPrograms.FirstOrDefault(
        //        p => p.Name == name);
        //    if (program == null)
        //    {
        //        return RedirectToAction(nameof(SchoolProgramController.Index));
        //    }
        //    var model = new CreateEditSchoolProgramViewModel();
        //    model.Name = program.Name;
        //    return View(model);
        //}

        [HttpGet]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult EditProgram(Guid id)
        {
            var program = DbContext.SchoolPrograms.FirstOrDefault(
                p => p.Id == id);
            if (program == null)
            {
                ModelState.AddModelError("", "School Program not found...");
                return View("Error");
            }
            string userId = User.Identity.GetUserId();
            if (!program.Instructors.Any(p => p.Id == userId))
            {
                ModelState.AddModelError("", "You can't edit a School Program that you don't belong to");
                return View("Error");
            }

            var model = new CreateEditSchoolProgramViewModel
            {
                Name = program.Name
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult EditProgram(Guid id, CreateEditSchoolProgramViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Name = model.Name.Trim();
            if (DbContext.SchoolPrograms.Any(p =>
            p.Name.ToLower() == model.Name.ToLower()))
            {
                ModelState.AddModelError(nameof(CreateEditSchoolProgramViewModel.Name),
                    "Program Name should be unique");
                return View(model);
            }

            SchoolProgram program;
            program = new SchoolProgram();
            program.Name = model.Name;
            return View();
        }

        [EnsureDiscriminatorClaim(Discriminator.Instructor, Discriminator.Student)]
        public ActionResult Details()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult Delete(string name)
        {
            var program = DbContext.SchoolPrograms.FirstOrDefault(
                p => p.Name == name);

            if (program == null)
            {
                ModelState.AddModelError("", "School Program not found...");
                return View("Error");
            }
            string userId = User.Identity.GetUserId();
            if (!program.Instructors.Any(p => p.Id == userId))
            {
                ModelState.AddModelError("", "You can't edit a School Program that you don't belong to");
                return View("Error");
            }

            DbContext.SchoolPrograms.Remove(program);
            DbContext.SaveChanges();

            return RedirectToAction(nameof(SchoolProgramController.Index));
        }
    }
}