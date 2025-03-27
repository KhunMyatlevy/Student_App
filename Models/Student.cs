using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shoopdora_app.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        public int GradeId { get; set; }
        public Grade Grade { get; set; }
        public List<StudentSubject> StudentSubjects { get; set; } = new();
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}