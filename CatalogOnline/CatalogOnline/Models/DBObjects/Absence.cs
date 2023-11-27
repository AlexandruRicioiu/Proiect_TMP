using System;
using System.Collections.Generic;

namespace CatalogOnline.Models.DBObjects
{
    public partial class Absence
    {
        public Guid IdAbsence { get; set; }
        public Guid IdStudent { get; set; }
        public Guid IdTeacher { get; set; }
        public DateTime Date { get; set; }
        public string Discipline { get; set; } = null!;
    }
}
