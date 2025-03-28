using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shoopdora_app.Dtos
{
    public class StudentWithDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public int Age { get; set;}
        public string PhoneNumber { get; set;}
        public string GradeName { get; set; }
        public List<string> SubjectNames { get; set; }
    }
}