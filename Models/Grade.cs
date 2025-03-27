using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shoopdora_app.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public string GradeName { get; set; }

        public List<Student> Students { get; set; } = new();
        public List<GradeSubject> GradeSubjects { get; set; } = new();
    }
}