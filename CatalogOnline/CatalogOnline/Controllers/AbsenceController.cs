using CatalogOnline.Data;
using CatalogOnline.Models;
using CatalogOnline.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogOnline.Controllers
{
    public class AbsenceController : Controller
    {
        private Repository.AbsenceRepository _absenceRepository;
        private Repository.StudentRepository _studentRepository;
        private Repository.TeacherRepository _teacherRepository;

        public AbsenceController(ApplicationDbContext dbContext)
        {
            _absenceRepository = new Repository.AbsenceRepository(dbContext);
            _studentRepository = new Repository.StudentRepository(dbContext);
            _teacherRepository = new Repository.TeacherRepository(dbContext);
        }

        [Authorize(Roles = "Teacher, Student")]
        // GET: AbsenceController
        public ActionResult Index()
        {
            var absences = _absenceRepository.GetAbsence();
            return View("Index", absences);
        }

        [Authorize(Roles = "Teacher, Student")]
        // GET: AbsenceController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _absenceRepository.GetAbsenceByID(id);
            model = _absenceRepository.NameStudent(model);
            model = _absenceRepository.NameTeacher(model);
            return View("Details", model);
        }

        [Authorize(Roles = "Teacher")]
        // GET: AbsenceController/Create
        public ActionResult Create()
        {
            var students = _studentRepository.GetStudent();
            var teachers = _teacherRepository.GetTeacher();

            var model = new Models.AbsenceModel
            {
                Students = students,
                Teachers = teachers

            };
            return View("Create", model);
        }

        [Authorize(Roles = "Teacher")]
        // POST: AbsenceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.AbsenceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _absenceRepository.InsertAbsence(model);

                    return RedirectToAction("Index");
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }

                model.Students = _studentRepository.GetStudent();
                model.Teachers = _teacherRepository.GetTeacher();

                return View("Create", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View("Create", model);
            }
        }

        [Authorize(Roles = "Teacher")]
        // GET: AbsenceController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var students = _studentRepository.GetStudent();
            var teachers = _teacherRepository.GetTeacher();

            var model = _absenceRepository.GetAbsenceByID(id);

            if (model != null)
            {
                model.Students = students;
                model.Teachers = teachers;

                return View("Edit", model);
            }

            return RedirectToAction("Index");
        }

        // POST: AbsenceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new AbsenceModel();

                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _absenceRepository.UpdateAbsence(model);
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

        [Authorize(Roles = "Teacher")]
        // GET: AbsenceController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _absenceRepository.GetAbsenceByID(id);
            model = _absenceRepository.NameStudent(model);
            model = _absenceRepository.NameTeacher(model);
            return View("Delete", model);
        }

        // POST: AbsenceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _absenceRepository.DeleteAbsence(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Delete", id);
            }
        }
    }
}
