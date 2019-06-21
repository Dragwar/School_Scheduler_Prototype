using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using School_Scheduler.MVC.Filters;
using School_Scheduler.MVC.Helpers;
using School_Scheduler.MVC.Models;
using School_Scheduler.MVC.Models.Domain;
using School_Scheduler.MVC.Models.ViewModels;
using System.Collections.Generic;

namespace School_Scheduler.MVC.Controllers
{
    public class SchoolProgramController : Controller
    {
        private ApplicationDbContext DbContext { get; } = new ApplicationDbContext();

        [HttpGet]
        [EnsureDiscriminatorClaim(Discriminator.Instructor, Discriminator.Student)]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            Discriminator userDiscriminator = User.Identity.GetUserDiscriminator();
            ApplicationUser foundUser = DbContext.Users.FirstOrDefault(user => userId == user.Id);

            // this check isn't really needed because [EnsureDiscriminatorClaim]
            if (foundUser == null)
            {
                ModelState.AddModelError("", "You must be logged in. (If you already are contact an admin for assistance)");
                return View("Error");
            }

            List<SchoolProgram> usersSchoolPrograms;
            switch (foundUser)
            {
                case Instructor _: usersSchoolPrograms = DbContext.SchoolPrograms.Where(sp => sp.Instructors.Any(i => i.Id == userId)).ToList(); break;
                case Student _: usersSchoolPrograms = DbContext.SchoolPrograms.Where(sp => sp.EnrolledStudents.Any(i => i.Id == userId)).ToList(); break;

                // these checks aren't really needed because [EnsureDiscriminatorClaim]
                case ApplicationUser _: ModelState.AddModelError("", "You must be a Student or Instructor for viewing courses"); return View("Error");
                default: ModelState.AddModelError("", "You must be logged in. (If you already are contact an admin for assistance)"); return View("Error");
            }

            ViewBag.SchoolPrograms = usersSchoolPrograms;

            return View();
        }

        [HttpGet]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult Create(CreateEditSchoolProgramViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Name = model.Name.Trim();

            if (DbContext.SchoolPrograms.Any(p => p.Name.ToLower() == model.Name.ToLower()))
            {
                ModelState.AddModelError(nameof(CreateEditSchoolProgramViewModel.Name), "Program Name should be unique");
                return View(model);
            }

            string userId = User.Identity.GetUserId();
            if (DbContext.Users.FirstOrDefault(user => user.Id == userId) is Instructor foundInstructor)
            {
                SchoolProgram schoolProgram = new SchoolProgram
                {
                    Name = model.Name,
                };
                foundInstructor.SchoolProgram = schoolProgram;

                DbContext.SchoolPrograms.Add(schoolProgram);
                DbContext.SaveChanges();
                return RedirectToAction(nameof(Details), new { schoolProgram.Id });
            }

            ModelState.AddModelError("", "You must be a Instructor to be able to create School Programs");
            return View("Error");
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
        public ActionResult Edit(Guid id)
        {
            SchoolProgram program = DbContext.SchoolPrograms.FirstOrDefault(sp => sp.Id == id);
            if (program == null)
            {
                ModelState.AddModelError("", "School Program not found...");
                return View("Error");
            }
            string userId = User.Identity.GetUserId();
            if (!program.Instructors.Any(i => i.Id == userId))
            {
                ModelState.AddModelError("", "You can't edit a School Program that you don't belong to");
                return View("Error");
            }

            CreateEditSchoolProgramViewModel model = new CreateEditSchoolProgramViewModel
            {
                Name = program.Name
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, CreateEditSchoolProgramViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            SchoolProgram foundSchoolProgram = DbContext.SchoolPrograms.FirstOrDefault(sp => sp.Id == id);
            if (foundSchoolProgram == null)
            {
                ModelState.AddModelError("", "School Program was not found");
                return View("Error");
            }

            model.Name = model.Name.Trim();

            if (foundSchoolProgram.Name.ToLower() != model.Name.ToLower())
            {
                if (DbContext.SchoolPrograms.Any(sp => sp.Name.ToLower() == model.Name.ToLower()))
                {
                    ModelState.AddModelError(nameof(CreateEditSchoolProgramViewModel.Name), "Program Name should be unique");
                    return View(model);
                }
            }

            string userId = User.Identity.GetUserId();


            if (!foundSchoolProgram.Instructors.Any(i => i.Id == userId))
            {
                ModelState.AddModelError("", "You aren't part of this School Program therefore you can't edit it");
                return View("Error");
            }

            foundSchoolProgram.Name = model.Name;

            DbContext.SaveChanges();

            return RedirectToAction(nameof(Details), new { foundSchoolProgram.Id });
        }

        [HttpGet]
        [EnsureDiscriminatorClaim(Discriminator.Student, Discriminator.Instructor)]
        public ActionResult Details(Guid id)
        {
            string userId = User.Identity.GetUserId();

            SchoolProgram foundSchoolProgram = DbContext.SchoolPrograms
                .Include(course => course.EnrolledStudents)
                .Include(course => course.Courses)
                .Include(course => course.Instructors)
                .FirstOrDefault(sp => sp.Id == id);

            if (foundSchoolProgram == null)
            {
                ModelState.AddModelError("", "School Program was not found");
                return View("Error");
            }

            if (foundSchoolProgram.EnrolledStudents.Any(s => s.Id == userId) || foundSchoolProgram.Instructors.Any(i => i.Id == userId))
            {
                ViewBag.SchoolProgram = foundSchoolProgram;
                return View();
            }

            ModelState.AddModelError("", "You aren't part of this School Program therefore you can't view it");
            return View("Error");
        }


        //[HttpPost]
        //public ActionResult Delete(string name)
        //{
        //    var program = DbContext.SchoolPrograms.FirstOrDefault(
        //        p => p.Name == name);

        //    if (program == null)
        //    {
        //        ModelState.AddModelError("", "School Program not found...");
        //        return View("Error");
        //    }
        //    string userId = User.Identity.GetUserId();
        //    if (!program.Instructors.Any(p => p.Id == userId))
        //    {
        //        ModelState.AddModelError("", "You can't edit a School Program that you don't belong to");
        //        return View("Error");
        //    }

        //    DbContext.SchoolPrograms.Remove(program);
        //    DbContext.SaveChanges();

        //    return RedirectToAction(nameof(SchoolProgramController.Index));
        //}
    }
}