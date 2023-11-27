using CatalogOnline.Data;
using CatalogOnline.Models;
using CatalogOnline.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogOnline.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class GradeController : Controller
    {
        private Repository.GradeRepository _gradeRepository;
        private Repository.StudentRepository _studentRepository;
        private Repository.TeacherRepository _teacherRepository;

        public GradeController(ApplicationDbContext dbContext)
        {
            _gradeRepository = new Repository.GradeRepository(dbContext);
            _studentRepository = new Repository.StudentRepository(dbContext);
            _teacherRepository = new Repository.TeacherRepository(dbContext);
        }

        [Authorize(Roles = "Teacher, Student")]
        // GET: GradeController
        public ActionResult Index()
        {
            var grades = _gradeRepository.GetGrade();
            return View("Index", grades);
        }

        [Authorize(Roles = "Teacher")]
        // GET: GradeController/Details/5
        public ActionResult Details(Guid id)
        {
            var model = _gradeRepository.GetGradeByID(id);
            model = _gradeRepository.NameStudent(model);
            model = _gradeRepository.NameTeacher(model);
            return View("Details", model);
        }

        [Authorize(Roles = "Teacher")]
        // GET: GradeController/Create
        public ActionResult Create()
        {
            var students = _studentRepository.GetStudent();
            var teachers = _teacherRepository.GetTeacher();

            var model = new Models.GradeModel
            {
                Students = students,
                Teachers = teachers

            };
            return View("Create", model);
        }

        // POST: GradeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.GradeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _gradeRepository.InsertGrade(model);

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
        // GET: GradeController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var students = _studentRepository.GetStudent();
            var teachers = _teacherRepository.GetTeacher();

            var model = _gradeRepository.GetGradeByID(id);

            if (model != null)
            {
                model.Students = students;
                model.Teachers = teachers;

                return View("Edit", model);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Teacher")]
        // POST: GradeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
        {
            try
            {
                var model = new GradeModel();

                var task = TryUpdateModelAsync(model);
                task.Wait();
                if (task.Result)
                {
                    _gradeRepository.UpdateGrade(model);
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
        // GET: GradeController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var model = _gradeRepository.GetGradeByID(id);
            model = _gradeRepository.NameStudent(model);
            model = _gradeRepository.NameTeacher(model);
            return View("Delete", model);
        }

        // POST: GradeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                _gradeRepository.DeleteGrade(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Delete", id);
            }
        }
    }
}
