using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shoopdora_app.Models;

namespace shoopdora_app.Context
{
    public class DataSeeder
    {
         public static void SeedData(ApplicationDbContext context)
        {
            if (!context.Grades.Any())
            {
                context.Grades.AddRange(
                Enumerable.Range(1, 12).Select(i => new Grade { GradeName = $"Grade {i}" })
            );
            }

            if (!context.Subjects.Any())
            {
                context.Subjects.AddRange(
                    new Subject { SubjectName = "Myanmar" },
                    new Subject { SubjectName = "English" },
                    new Subject { SubjectName = "Math" },
                    new Subject { SubjectName = "Science" },
                    new Subject { SubjectName = "Geography" },
                    new Subject { SubjectName = "History" },
                    new Subject { SubjectName = "Physics" },
                    new Subject { SubjectName = "Chemistry" },
                    new Subject { SubjectName = "Biology" }
                );
            }

            context.SaveChanges();

             // 3. Pre-fill GradeSubject based on grade level
        if (!context.GradeSubjects.Any())
        {
            var grades = context.Grades.ToList();
            var subjects = context.Subjects.ToList();
            var gradeSubjects = new List<GradeSubject>();

            foreach (var grade in grades)
            {
                var assignedSubjects = new List<Subject>();

                if (grade.GradeId >= 1)
                {
                    assignedSubjects.AddRange(subjects.Where(s =>
                        s.SubjectName == "Myanmar" ||
                        s.SubjectName == "English" ||
                        s.SubjectName == "Math" ||
                        s.SubjectName == "Science"));
                }

                if (grade.GradeId >= 5)
                {
                    assignedSubjects.AddRange(subjects.Where(s =>
                        s.SubjectName == "Geography" ||
                        s.SubjectName == "History"));
                }

                if (grade.GradeId >= 10)
                {
                    assignedSubjects.AddRange(subjects.Where(s =>
                        s.SubjectName == "Physics" ||
                        s.SubjectName == "Chemistry" ||
                        s.SubjectName == "Biology"));
                }

                foreach (var subject in assignedSubjects)
                {
                    gradeSubjects.Add(new GradeSubject
                    {
                        GradeId = grade.GradeId,
                        SubjectId = subject.SubjectId
                    });
                }
            }

            context.GradeSubjects.AddRange(gradeSubjects);
        }

            context.SaveChanges();
        }
    }
}