using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shoopdora_app.Dtos;
using shoopdora_app.Models;

namespace shoopdora_app.Interface.IRepository
{
    public interface IStudentIRepository
    {
        Task<Student> CreateAsync(Student student);
        Task<List<StudentWithDetailsDTO>> GetAllAsync (int limit);
        Task<StudentWithDetailsDTO> GetByIdAsync (int id);
        Task<Student> UpdateStudentGradeAsync(int studentId, CreateStudentDto updateStudentDto);
        Task<Student> SoftDeleteByIdAsync(int id);
        Task<Student> DeleteById (int id);
    }
}