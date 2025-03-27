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

       public async Task<List<StudentWithDetailsDTO>> GetAllAsync(int limit)
            {
                IQueryable<Student> query = _context.Students
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
                        GradeName = s.Grade.GradeName,  
                        SubjectNames = s.StudentSubjects
                                        .Select(ss => ss.Subject.SubjectName) 
                                        .ToList()
                    })
                    .ToListAsync();

                return students;
            }


    }
}