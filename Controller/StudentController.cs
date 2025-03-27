using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shoopdora_app.Dtos;
using shoopdora_app.Interface.IRepository;
using shoopdora_app.Interface.IService;
using shoopdora_app.Models;

namespace shoopdora_app.Controller
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IStudentIRepository _studentRepo;
        public StudentController(IStudentService studentService, IStudentIRepository studentRepo)
        {
            _studentService = studentService;
            _studentRepo = studentRepo;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto createStudentDto)
        {
            var request = await _studentService.ValidateStudentAsync(createStudentDto);

            if (!string.IsNullOrEmpty(request))
            {
                return BadRequest(new { message = request });
            }

            var student = new Student
            {
                StudentName = createStudentDto.StudentName,
                FatherName = createStudentDto.FatherName,
                Email = createStudentDto.Email,
                Age = createStudentDto.Age,
                PhoneNumber = createStudentDto.PhoneNumber,
                GradeId = createStudentDto.GradeId,
            };
            var adduser = await _studentRepo.CreateAsync(student);

            var newstudent = new NewStudentDto
            {
                StudentId = adduser.StudentId,
                StudentName = adduser.StudentName,
                FatherName = adduser.FatherName,
                Email = adduser.Email,
                Age = adduser.Age,
                PhoneNumber = adduser.PhoneNumber,
                GradeId = adduser.GradeId,
                GradeName = adduser.Grade.GradeName,
                CreatedAt = adduser.CreatedAt
            };

            return Ok(newstudent);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll ([FromQuery] int limit)
        {
            var students = await _studentRepo.GetAllAsync(limit);
            if (students == null)
            {
                return BadRequest("No students were found.");
            }
            return Ok(students);
        }
    }
}