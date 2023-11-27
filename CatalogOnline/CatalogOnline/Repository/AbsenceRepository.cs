 using CatalogOnline.Data;
using CatalogOnline.Models.DBObjects;
using CatalogOnline.Models;
using Humanizer.Localisation;
using System.Diagnostics;

namespace CatalogOnline.Repository
{
    public class AbsenceRepository
    {
        private ApplicationDbContext dbContext;

        public AbsenceRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public AbsenceRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<AbsenceModel> GetAbsence()
        {
            List<AbsenceModel> absenceList = new List<AbsenceModel>();

            foreach (Absence dbAbsence in dbContext.Absences)
            {
                absenceList.Add(MapDbObjectToModel(dbAbsence));
            }

            foreach (AbsenceModel model in absenceList)
            {
                Student stud = new Student();
                stud = dbContext.Students.FirstOrDefault(x => x.IdStudent == model.IdStudent);
                if(stud != null)
                {
                    model.NameStud = stud.FirstName + " " + stud.LastName;
                }
            }

            foreach (AbsenceModel model in absenceList)
            {
                Teacher teach = new Teacher();
                teach = dbContext.Teachers.FirstOrDefault(x => x.IdTeacher == model.IdTeacher);
                if (teach != null)
                {
                    model.NameTeach = teach.FirstName + " " + teach.LastName;
                }
            }

            return absenceList;
        }

        public AbsenceModel GetAbsenceByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Absences.FirstOrDefault(x => x.IdAbsence == ID));
        }

        public void InsertAbsence(AbsenceModel absenceModel)
        {
            absenceModel.IdAbsence = Guid.NewGuid();

            dbContext.Absences.Add(MapModelToDbObject(absenceModel));
            dbContext.SaveChanges();
        }

        public void UpdateAbsence(AbsenceModel absenceModel)
        {
            Absence existingAbsence = dbContext.Absences.FirstOrDefault(x => x.IdAbsence == absenceModel.IdAbsence);

            if (existingAbsence != null)
            {
                existingAbsence.IdAbsence = absenceModel.IdAbsence;
                existingAbsence.IdStudent = absenceModel.IdStudent;
                existingAbsence.IdTeacher = absenceModel.IdTeacher;
                existingAbsence.Date = absenceModel.Date;
                existingAbsence.Discipline = absenceModel.Discipline;
                dbContext.SaveChanges();
            }
        }

        public void DeleteAbsence(Guid ID)
        {
            Absence existingAbsence = dbContext.Absences.FirstOrDefault(x => x.IdAbsence == ID);

            if (existingAbsence != null)
            {
                dbContext.Absences.Remove(existingAbsence);
                dbContext.SaveChanges();
            }
        }

        private AbsenceModel MapDbObjectToModel(Absence dbAbsence)
        {
            AbsenceModel absenceModel = new AbsenceModel();

            if (dbAbsence != null)
            {
                absenceModel.IdAbsence = dbAbsence.IdAbsence;
                absenceModel.IdStudent = dbAbsence.IdStudent;
                absenceModel.IdTeacher = dbAbsence.IdTeacher;
                absenceModel.Date = dbAbsence.Date;
                absenceModel.Discipline = dbAbsence.Discipline;
            }
            return absenceModel;
        }

        private Absence MapModelToDbObject(AbsenceModel absenceModel)
        {
            Absence absence = new Absence();

            if (absenceModel != null)
            {
                absence.IdAbsence = absenceModel.IdAbsence;
                absence.IdStudent = absenceModel.IdStudent;
                absence.IdTeacher = absenceModel.IdTeacher;
                absence.Date = absenceModel.Date;
                absence.Discipline = absenceModel.Discipline;
            }
            return absence;
        }

        public AbsenceModel NameStudent(AbsenceModel model)
        {
            Student stud = new Student();
            stud = dbContext.Students.FirstOrDefault(x => x.IdStudent == model.IdStudent);

            if(stud != null)
            {
                model.NameStud = stud.FirstName + " " + stud.LastName;
            }

            return model;
        }

        public AbsenceModel NameTeacher(AbsenceModel model)
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
