using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shoopdora_app.Dtos
    {
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public T Data { get; }

        private Result(bool isSuccess, string errorMessage, T data)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public static Result<T> Success(T data) => new Result<T>(true, null, data);
        public static Result<T> Failure(string errorMessage) => new Result<T>(false, errorMessage, default);
    }

}