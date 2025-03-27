using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shoopdora_app.Models;



namespace shoopdora_app.Context
{
    public class ApplicationDbContext : DbContext 
    {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : base(options) 
    {
    }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<GradeSubject> GradeSubjects { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentSubject>()
            .HasKey(ss => new { ss.StudentId, ss.SubjectId });

        modelBuilder.Entity<StudentSubject>()
            .HasOne(ss => ss.Student)
            .WithMany(s => s.StudentSubjects)
            .HasForeignKey(ss => ss.StudentId);

        modelBuilder.Entity<StudentSubject>()
            .HasOne(ss => ss.Subject)
            .WithMany(s => s.StudentSubjects)
            .HasForeignKey(ss => ss.SubjectId);

        modelBuilder.Entity<GradeSubject>()
            .HasKey(gs => new { gs.GradeId, gs.SubjectId });

        modelBuilder.Entity<GradeSubject>()
            .HasOne(gs => gs.Grade)
            .WithMany(g => g.GradeSubjects)
            .HasForeignKey(gs => gs.GradeId);

        modelBuilder.Entity<GradeSubject>()
            .HasOne(gs => gs.Subject)
            .WithMany(s => s.GradeSubjects)
            .HasForeignKey(gs => gs.SubjectId);
    }
    }

}