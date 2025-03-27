using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shoopdora_app.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        public List<StudentSubject> StudentSubjects { get; set; } = new();
        public List<GradeSubject> GradeSubjects { get; set; } = new();
    }
}