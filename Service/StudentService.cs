using shoopdora_app.Dtos;
using shoopdora_app.Interface.IService;

namespace shoopdora_app.Service
{
    public class StudentService : IStudentService
    {

        public async Task<string> ValidateStudentAsync(CreateStudentDto createStudentDto)
        {
            if (string.IsNullOrEmpty(createStudentDto.StudentName))
            {
                return await Task.FromResult("Student name is required.");
            }

            if (string.IsNullOrEmpty(createStudentDto.FatherName))
            {
                return await Task.FromResult("Father name is required.");
            }

            if (string.IsNullOrEmpty(createStudentDto.Email))
            {
                return await Task.FromResult("Email is required.");
            }
            if (!IsValidEmail(createStudentDto.Email))
            {
                return await Task.FromResult("Valid email is required.");
            }

            if (createStudentDto.Age <= 0)
            {
                return await Task.FromResult("Age must be a positive integer.");
            }

            if (string.IsNullOrEmpty(createStudentDto.PhoneNumber.ToString()))
            {
                return await Task.FromResult("Phone number is required.");
            }

            if (!IsPhoneNumberValid(createStudentDto.PhoneNumber))
            {
                return await Task.FromResult("Phone number must start with '09' and be 9 digits long.");
            }

            if (createStudentDto.GradeId < 1 || createStudentDto.GradeId > 12)
            {
                return await Task.FromResult("GradeId must be between 1 and 12.");
            }

            return await Task.FromResult(string.Empty);
        }
        private bool IsValidEmail(string email)
        {
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            return phoneNumber.StartsWith("09") && phoneNumber.Length == 11;
        }
    }
}