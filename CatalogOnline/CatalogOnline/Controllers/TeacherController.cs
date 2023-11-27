using CatalogOnline.Data;
using CatalogOnline.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CatalogOnline.Repository;

namespace CatalogOnline.Controllers
{
    public class TeacherController : Controller
    {
        private Repository.TeacherRepository _teacherRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public TeacherController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _teacherRepository = new Repository.TeacherRepository(dbContext);
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [Authorize(Roles = "Admin")]
        // GET: TeacherController
        public ActionResult Index()
        {
            var teachers = _teacherRepository.GetTeacher();
            return View("Index", teachers);
        }

        [Authorize(Roles = "Admin")]
        // GET: TeacherController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _teacherRepository.GetTeacherByID(id);
            return View("Details", model);
        }

        [Authorize(Roles = "Admin")]
        // GET: TeacherController/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: TeacherController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                Models.TeacherModel model = new Models.TeacherModel();

                var task = TryUpdateModelAsync(model);
                task.Wait();

                if (task.Result)
                {
                    var userName = model.Email;

                    var user = new IdentityUser { UserName = userName, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, "Qwerty123!");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Teacher");

                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

                        if (confirmResult.Succeeded)
                        {
                            _teacherRepository.InsertTeacher(model);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error.");
                            return View("Create");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Eroare.");
                        return View("Create");
                    }
                }

                return View("Create");
            }
            catch
            {
                return View("Create");
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: TeacherController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var model = _teacherRepository.GetTeacherByID(id);
            return View("Edit", model);
        }

        // POST: TeacherController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new TeacherModel();

                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _teacherRepository.UpdateTeacher(model);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", id);
                }

            }
            catch
            {
                return RedirectToAction("Index", id);
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: TeacherController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _teacherRepository.GetTeacherByID(id);
            return View("Delete", model);
        }

        // POST: TeacherController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _teacherRepository.DeleteTeacher(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Delete", id);
            }
        }
    }
}
