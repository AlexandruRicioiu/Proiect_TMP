using CatalogOnline.Data;
using CatalogOnline.Models;
using CatalogOnline.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CatalogOnline.Controllers
{
    
    public class StudentController : Controller
    {
        private Repository.StudentRepository _studentRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public StudentController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _studentRepository = new Repository.StudentRepository(dbContext);
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [Authorize(Roles = "Admin, Teacher")]
        // GET: StudentController
        public ActionResult Index()
        {
            var students = _studentRepository.GetStudent();
            return View("Index", students);
        }

        // GET: StudentController/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(Guid id)
        {
            var model = _studentRepository.GetStudentByID(id);
            return View("Details", model);
        }

        [Authorize(Roles = "Admin")]
        // GET: StudentController/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                Models.StudentModel model = new Models.StudentModel();

                var task = TryUpdateModelAsync(model);
                task.Wait();

                if (task.Result)
                {
                    var userName = model.Email;

                    var user = new IdentityUser { UserName = userName, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, "Qwerty123!");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Student");

                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

                        if (confirmResult.Succeeded)
                        {
                            _studentRepository.InsertStudent(model);
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
        // GET: StudentController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var model = _studentRepository.GetStudentByID(id);
            return View("Edit", model);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new StudentModel();

                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _studentRepository.UpdateStudent(model);
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
        // GET: StudentController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _studentRepository.GetStudentByID(id);
            return View("Delete", model);
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _studentRepository.DeleteStudent(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Delete", id);
            }
        }
    }
}
