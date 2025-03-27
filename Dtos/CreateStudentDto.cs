using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shoopdora_app.Dtos
{
    public class CreateStudentDto
    {
        [Required]
        public string StudentName { get; set; }

        [Required]
        public string FatherName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [Phone]
        public string  PhoneNumber { get; set; }

        [Required]
        public int GradeId { get; set; }
    }
}