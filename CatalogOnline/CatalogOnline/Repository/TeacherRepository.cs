using CatalogOnline.Data;
using CatalogOnline.Models.DBObjects;
using CatalogOnline.Models;

namespace CatalogOnline.Repository
{
    public class TeacherRepository
    {
        private ApplicationDbContext dbContext;

        public TeacherRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public TeacherRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<TeacherModel> GetTeacher()
        {
            List<TeacherModel> teacherList = new List<TeacherModel>();

            foreach (Teacher dbTeacher in dbContext.Teachers)
            {
                teacherList.Add(MapDbObjectToModel(dbTeacher));
            }
            return teacherList;
        }

        public TeacherModel GetTeacherByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Teachers.FirstOrDefault(x => x.IdTeacher == ID));
        }

        public void InsertTeacher(TeacherModel teacherModel)
        {
            teacherModel.IdTeacher = Guid.NewGuid();

            dbContext.Teachers.Add(MapModelToDbObject(teacherModel));
            dbContext.SaveChanges();
        }

        public void UpdateTeacher(TeacherModel teacherModel)
        {
            Teacher existingTeacher = dbContext.Teachers.FirstOrDefault(x => x.IdTeacher == teacherModel.IdTeacher);

            if (existingTeacher != null)
            {
                existingTeacher.IdTeacher = teacherModel.IdTeacher;
                existingTeacher.FirstName = teacherModel.FirstName;
                existingTeacher.LastName = teacherModel.LastName;
                existingTeacher.Email = teacherModel.Email;
                existingTeacher.PhoneNr = teacherModel.PhoneNr;
                existingTeacher.Discipline = teacherModel.Discipline;
                dbContext.SaveChanges();
            }
        }

        public void DeleteTeacher(Guid ID)
        {
            Teacher existingTeacher = dbContext.Teachers.FirstOrDefault(x => x.IdTeacher == ID);

            if (existingTeacher != null)
            {
                dbContext.Teachers.Remove(existingTeacher);
                dbContext.SaveChanges();
            }
        }

        private TeacherModel MapDbObjectToModel(Teacher dbTeacher)
        {
            TeacherModel teacherModel = new TeacherModel();

            if (dbTeacher != null)
            {
                teacherModel.IdTeacher = dbTeacher.IdTeacher;
                teacherModel.FirstName = dbTeacher.FirstName;
                teacherModel.LastName = dbTeacher.LastName;
                teacherModel.Email = dbTeacher.Email;
                teacherModel.PhoneNr = dbTeacher.PhoneNr;
                teacherModel.Discipline = dbTeacher.Discipline;
            }
            return teacherModel;
        }

        private Teacher MapModelToDbObject(TeacherModel teacherModel)
        {
            Teacher teacher = new Teacher();

            if (teacherModel != null)
            {
                teacher.IdTeacher = teacherModel.IdTeacher;
                teacher.FirstName = teacherModel.FirstName;
                teacher.LastName = teacherModel.LastName;
                teacher.Email = teacherModel.Email;
                teacher.PhoneNr = teacherModel.PhoneNr;
                teacher.Discipline = teacherModel.Discipline;
            }
            return teacher;
        }
    }
}
