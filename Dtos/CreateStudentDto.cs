using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shoopdora_app.Dtos
{
    public class CreateStudentDto
    {
        public string StudentName { get; set; }

        public string FatherName { get; set; }

        public string Email { get; set; }

        public int Age { get; set; }

        public string  PhoneNumber { get; set; }

        public int GradeId { get; set; }
    }
}