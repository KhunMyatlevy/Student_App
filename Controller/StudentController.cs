using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById ([FromRoute] int id)
        {
            var student = await _studentRepo.GetByIdAsync(id);
            if (student == null)
            {
                return BadRequest("Student does not exist.");
            }

            return Ok(student);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateById ([FromRoute] int id, [FromBody] CreateStudentDto updateStudentDto)
        {
            var request = await _studentService.ValidateStudentAsync(updateStudentDto);
            
            if (!string.IsNullOrEmpty(request))
            {
                return BadRequest(new { message = request });
            }

            var updatedStudent = await _studentRepo.UpdateStudentGradeAsync(id, updateStudentDto);
            if (updatedStudent == null)
            {
                return BadRequest("Student does not found is here.");
            }

            var returnStudent = new NewStudentDto
            {
                StudentId = updatedStudent.StudentId,
                StudentName = updatedStudent.StudentName,
                FatherName = updatedStudent.FatherName,
                Email = updatedStudent.Email,
                Age = updatedStudent.Age,
                PhoneNumber = updatedStudent.PhoneNumber,
                GradeId = updatedStudent.GradeId,
                GradeName = updatedStudent.GradeId >= 1 && updatedStudent.GradeId <= 12
                ? $"Grade {updatedStudent.GradeId}"
                : null
            };

            return Ok(returnStudent);
        }
        [HttpPatch("{id}/soft-delete")]
        public async Task<IActionResult> SoftDeleteById ([FromRoute] int id)
        {
            var deleteRecord = await _studentRepo.SoftDeleteByIdAsync(id);
            if (deleteRecord == null)
            {
                return BadRequest("Student does not found.");
            }
            return Ok(deleteRecord);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById ([FromRoute] int id)
        {
            var existStudent = await _studentRepo.DeleteById(id);
            if (existStudent == null)
            {
                return BadRequest("Student does not found.");
            }

            return NoContent();
        }
    }
}