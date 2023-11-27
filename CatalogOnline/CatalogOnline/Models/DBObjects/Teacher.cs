using System;
using System.Collections.Generic;

namespace CatalogOnline.Models.DBObjects
{
    public partial class Teacher
    {
        public Guid IdTeacher { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNr { get; set; } = null!;
        public string Discipline { get; set; } = null!;
    }
}
