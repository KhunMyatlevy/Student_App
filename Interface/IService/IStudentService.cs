using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shoopdora_app.Dtos;

namespace shoopdora_app.Interface.IService
{
    public interface IStudentService
    {
        Task<string> ValidateStudentAsync(CreateStudentDto createStudentDto);
    }
}