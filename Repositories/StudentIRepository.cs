using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shoopdora_app.Context;
using shoopdora_app.Interface.IRepository;
using shoopdora_app.Models;
using Microsoft.EntityFrameworkCore;
using shoopdora_app.Dtos;


namespace shoopdora_app.Repositories
{
    public class StudentIRepository : IStudentIRepository
    {
        private readonly ApplicationDbContext _context;
        public StudentIRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Student> CreateAsync(Student student)
        {
            // Check if the student name already exists
            string originalStudentName = student.StudentName;
            int count = await _context.Students
                                    .Where(s => s.StudentName.StartsWith(originalStudentName))
                                    .CountAsync();

            if (count > 0)
            {
                student.StudentName = $"{originalStudentName}{count + 1}";
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var studentWithGrade = await _context.Students
                                                .Where(s => s.StudentId == student.StudentId)
                                                .Include(s => s.Grade)
                                                .FirstOrDefaultAsync();

            if (studentWithGrade?.Grade != null)
            {
                var predefinedSubjects = await _context.GradeSubjects
                                                        .Where(gs => gs.GradeId == studentWithGrade.GradeId)
                                                        .Select(gs => gs.SubjectId)
                                                        .ToListAsync();

                var studentSubjects = predefinedSubjects.Select(subjectId => new StudentSubject
                {
                    StudentId = student.StudentId,
                    SubjectId = subjectId
                }).ToList();

                await _context.StudentSubjects.AddRangeAsync(studentSubjects);
                await _context.SaveChangesAsync();
            }

            var studentWithGradeAndSubjects = await _context.Students
                                                            .Where(s => s.StudentId == student.StudentId)
                                                            .Include(s => s.Grade)
                                                            .Include(s => s.StudentSubjects)
                                                            .ThenInclude(ss => ss.Subject)
                                                            .FirstOrDefaultAsync();

            return studentWithGradeAndSubjects;
        }

        public async Task<Student> DeleteById(int id)
        {
            var existStudent = await _context.Students.FirstOrDefaultAsync(u => u.StudentId == id);
            if (existStudent == null)
            {
                return null;
            }

            _context.Students.Remove(existStudent);
            await _context.SaveChangesAsync();
            return existStudent;
        }

        public async Task<List<StudentWithDetailsDTO>> GetAllAsync(int limit)
        {
            IQueryable<Student> query = _context.Students
                                                .Where(s => !s.IsDeleted)
                                                .Include(s => s.Grade)
                                                .Include(s => s.StudentSubjects)
                                                .ThenInclude(ss => ss.Subject);

            if (limit == 0)
            {
                var allStudents = await query
                    .Select(s => new StudentWithDetailsDTO
                    {
                        Id = s.StudentId,
                        Name = s.StudentName,
                        FatherName = s.FatherName,
                        Email = s.Email,
                        Age = s.Age,
                        PhoneNumber = s.PhoneNumber,
                        GradeName = s.Grade.GradeName,
                        SubjectNames = s.StudentSubjects
                                            .Select(ss => ss.Subject.SubjectName)
                                            .ToList()
                    })
                    .ToListAsync();

                    return allStudents;
                }

                var students = await query
                    .Take(limit)
                    .Select(s => new StudentWithDetailsDTO
                    {
                        Id = s.StudentId,
                        Name = s.StudentName,
                        FatherName = s.FatherName,
                        Email = s.Email,
                        Age = s.Age,
                        PhoneNumber = s.PhoneNumber,
                        GradeName = s.Grade.GradeName,  
                        SubjectNames = s.StudentSubjects
                                        .Select(ss => ss.Subject.SubjectName) 
                                        .ToList()
                    })
                    .ToListAsync();

                return students;
            }

        public async Task<StudentWithDetailsDTO> GetByIdAsync(int id)
        {
            var student = await _context.Students
                                        .Where(s => !s.IsDeleted)
                                        .Include(s => s.Grade)
                                        .Include(s => s.StudentSubjects)
                                        .ThenInclude(ss => ss.Subject)
                                        .FirstOrDefaultAsync(u => u.StudentId == id);

            if (student == null)
            {
                return null;
            }

            var returnStudent = new StudentWithDetailsDTO

            {
                Id = student.StudentId,
                Name = student.StudentName,
                FatherName = student.FatherName,
                Email = student.Email,
                Age = student.Age,
                PhoneNumber = student.PhoneNumber,
                GradeName = student.Grade.GradeName,
                SubjectNames = student.StudentSubjects
                                    .Select(ss => ss.Subject.SubjectName)
                                    .ToList()
            };

            return returnStudent;
        }

        public async Task<Student> SoftDeleteByIdAsync(int id)
        {
            var existingStudent = await _context.Students.FirstOrDefaultAsync(u => u.StudentId == id);
            if (existingStudent == null)
            {
                return null;
            } 
            existingStudent.IsDeleted = true;
            await _context.SaveChangesAsync();
            return existingStudent;
        }

        public async Task<Student> UpdateStudentGradeAsync(int id, CreateStudentDto updateStudentDto)
        {
            var student = await _context.Students.FirstOrDefaultAsync(u => u.StudentId == id);

            if (student == null)
                return null;

            if (student.IsDeleted == true)
            {
                return null;
            }

            if (student.GradeId != updateStudentDto.GradeId)
            {

            var currentSubjects = await _context.StudentSubjects
                .Where(ss => ss.StudentId == id)
                .ToListAsync();

            _context.StudentSubjects.RemoveRange(currentSubjects);

            var newSubjects = await _context.GradeSubjects
                .Where(gs => gs.GradeId == updateStudentDto.GradeId)
                .Select(gs => gs.SubjectId)
                .ToListAsync();

            foreach (var subjectId in newSubjects)
            {
                _context.StudentSubjects.Add(new StudentSubject
                {
                    StudentId = id,
                    SubjectId = subjectId
                });
            }

            student.GradeId = updateStudentDto.GradeId;
            }

            student.Age = updateStudentDto.Age;
            student.StudentName = updateStudentDto.StudentName;
            student.FatherName = updateStudentDto.FatherName;
            student.PhoneNumber = updateStudentDto.PhoneNumber;
            student.Email = updateStudentDto.Email;
            student.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return student;
        }

    }
}