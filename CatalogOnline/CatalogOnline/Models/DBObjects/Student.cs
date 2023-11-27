using System;
using System.Collections.Generic;

namespace CatalogOnline.Models.DBObjects
{
    public partial class Student
    {
        public Guid IdStudent { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNr { get; set; } = null!;
        public string Class { get; set; } = null!;
    }
}
