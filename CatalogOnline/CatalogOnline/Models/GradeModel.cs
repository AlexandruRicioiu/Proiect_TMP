using CatalogOnline.Models.DBObjects;
using System.ComponentModel.DataAnnotations;

namespace CatalogOnline.Models
{
    public class GradeModel
    {
        public Guid IdGrade { get; set; }
        public Guid IdStudent { get; set; }
        public Guid IdTeacher { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;
        public string Discipline { get; set; } = null!;
        public int Grade1 { get; set; }

        public List<StudentModel> Students { get; set; } = new List<StudentModel>();
        public List<TeacherModel> Teachers { get; set; } = new List<TeacherModel>();

        public string NameStud { get; set; } = string.Empty;
        public string NameTeach { get; set; } = string.Empty;
    }
}
