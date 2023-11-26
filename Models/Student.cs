using System;
using System.Collections.Generic;

namespace newRelicTestingApplication.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Gender { get; set; }

    }
}
