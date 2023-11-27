using System;
using System.Collections.Generic;

namespace CatalogOnline.Models.DBObjects
{
    public partial class Grade
    {
        public Guid IdGrade { get; set; }
        public Guid IdStudent { get; set; }
        public Guid IdTeacher { get; set; }
        public DateTime Date { get; set; }
        public string Discipline { get; set; } = null!;
        public int Grade1 { get; set; }
    }
}
