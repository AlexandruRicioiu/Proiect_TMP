using CatalogOnline.Data;
using CatalogOnline.Models;
using CatalogOnline.Models.DBObjects;
using Microsoft.AspNetCore.Authorization;

namespace CatalogOnline.Repository
{

    public class StudentRepository
    {
        private ApplicationDbContext dbContext;

        public StudentRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public StudentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<StudentModel> GetStudent()
        {
            List<StudentModel> studentList = new List<StudentModel>();

            foreach(Student dbStudent in dbContext.Students)
            {
                studentList.Add(MapDbObjectToModel(dbStudent));
            }
            return studentList;
        }

        public StudentModel GetStudentByID(Guid ID)
        {
            return MapDbObjectToModel(dbContext.Students.FirstOrDefault(x => x.IdStudent == ID));
        }

        public void InsertStudent(StudentModel studentModel)
        {
            studentModel.IdStudent = Guid.NewGuid();

            dbContext.Students.Add(MapModelToDbObject(studentModel));
            dbContext.SaveChanges();
        }

        public void UpdateStudent(StudentModel studentModel)
        {
            Student existingStudent = dbContext.Students.FirstOrDefault(x => x.IdStudent == studentModel.IdStudent);

            if (existingStudent != null)
            {
                existingStudent.IdStudent = studentModel.IdStudent;
                existingStudent.FirstName = studentModel.FirstName;
                existingStudent.LastName = studentModel.LastName;
                existingStudent.Email = studentModel.Email;
                existingStudent.PhoneNr = studentModel.PhoneNr;
                existingStudent.Class = studentModel.Class;
                dbContext.SaveChanges();
            }
        }

        public void DeleteStudent(Guid ID) 
        {
            Student existingStudent = dbContext.Students.FirstOrDefault(x => x.IdStudent == ID);

            if(existingStudent != null)
            {
                dbContext.Students.Remove(existingStudent);
                dbContext.SaveChanges();
            }
        }

        private StudentModel MapDbObjectToModel(Student dbStudent)
        {
            StudentModel studentModel = new StudentModel();

            if(dbStudent != null)
            {
                studentModel.IdStudent = dbStudent.IdStudent;
                studentModel.FirstName = dbStudent.FirstName;
                studentModel.LastName = dbStudent.LastName;
                studentModel.Email = dbStudent.Email;
                studentModel.PhoneNr = dbStudent.PhoneNr;
                studentModel.Class = dbStudent.Class;
            }
            return studentModel;
        }

        private Student MapModelToDbObject(StudentModel studentModel)
        {
            Student student = new Student();

            if(studentModel != null)
            {
                student.IdStudent = studentModel.IdStudent;
                student.FirstName = studentModel.FirstName;
                student.LastName = studentModel.LastName;
                student.Email = studentModel.Email;
                student.PhoneNr = studentModel.PhoneNr;
                student.Class = studentModel.Class;
            }
            return student;
        }

    }
}
