using CatalogOnline.Data;
using CatalogOnline.Models.DBObjects;
using CatalogOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogOnline.Repository
{
    public class GradeRepository
    {
        private ApplicationDbContext dbContext;

        public GradeRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public GradeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<GradeModel> GetGrade()
        {
            List<GradeModel> gradeList = new List<GradeModel>();

            foreach (Grade dbGrade in dbContext.Grades)
            {
                gradeList.Add(MapDbObjectToModel(dbGrade));
            }

            foreach (GradeModel model in gradeList)
            {
                Student stud = new Student();
                stud = dbContext.Students.FirstOrDefault(x => x.IdStudent == model.IdStudent);
                if (stud != null)
                {
                    model.NameStud = stud.FirstName + " " + stud.LastName;
                }
            }

            foreach (GradeModel model in gradeList)
            {
                Teacher teach = new Teacher();
                teach = dbContext.Teachers.FirstOrDefault(x => x.IdTeacher == model.IdTeacher);
                if (teach != null)
                {
                    model.NameTeach = teach.FirstName + " " + teach.LastName;
                }
            }

            return gradeList;
        }

        public GradeModel GetGradeByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Grades.FirstOrDefault(x => x.IdGrade == ID));
        }

        public void InsertGrade(GradeModel gradeModel)
        {
            gradeModel.IdGrade = Guid.NewGuid();

            dbContext.Grades.Add(MapModelToDbObject(gradeModel));
            dbContext.SaveChanges();
        }

        public void UpdateGrade(GradeModel gradeModel)
        {
            Grade existingGrade = dbContext.Grades.FirstOrDefault(x => x.IdGrade == gradeModel.IdGrade);

            if (existingGrade != null)
            {
                existingGrade.IdGrade = gradeModel.IdGrade;
                existingGrade.IdStudent = gradeModel.IdStudent;
                existingGrade.IdTeacher = gradeModel.IdTeacher;
                existingGrade.Date = gradeModel.Date;
                existingGrade.Discipline = gradeModel.Discipline;
                existingGrade.Grade1 = gradeModel.Grade1;
                dbContext.SaveChanges();
            }
        }

        public void DeleteGrade(Guid ID)
        {
            Grade existingGrade = dbContext.Grades.FirstOrDefault(x => x.IdGrade == ID);

            if (existingGrade != null)
            {
                dbContext.Grades.Remove(existingGrade);
                dbContext.SaveChanges();
            }
        }

        private GradeModel MapDbObjectToModel(Grade dbGrade)
        {
            GradeModel gradeModel = new GradeModel();

            if (dbGrade != null)
            {
                gradeModel.IdGrade = dbGrade.IdGrade;
                gradeModel.IdStudent = dbGrade.IdStudent;
                gradeModel.IdTeacher = dbGrade.IdTeacher;
                gradeModel.Date = dbGrade.Date;
                gradeModel.Discipline = dbGrade.Discipline;
                gradeModel.Grade1 = dbGrade.Grade1;
            }
            return gradeModel;
        }

        private Grade MapModelToDbObject(GradeModel gradeModel)
        {
            Grade grade = new Grade();

            if (gradeModel != null)
            {
                grade.IdGrade = gradeModel.IdGrade;
                grade.IdStudent = gradeModel.IdStudent;
                grade.IdTeacher = gradeModel.IdTeacher;
                grade.Date = gradeModel.Date;
                grade.Discipline = gradeModel.Discipline;
                grade.Grade1 = gradeModel.Grade1;
            }
            return grade;
        }

        public GradeModel NameStudent(GradeModel model)
        {
            Student stud = new Student();
            stud = dbContext.Students.FirstOrDefault(x => x.IdStudent == model.IdStudent);

            if (stud != null)
            {
                model.NameStud = stud.FirstName + " " + stud.LastName;
            }

            return model;
        }

        public GradeModel NameTeacher(GradeModel model)
        {
            Teacher teach = new Teacher();
            teach = dbContext.Teachers.FirstOrDefault(x => x.IdTeacher == model.IdTeacher);

            if (teach != null)
            {
                model.NameTeach = teach.FirstName + " " + teach.LastName;
            }

            return model;
        }
    }
}
