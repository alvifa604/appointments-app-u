namespace Application.Core
{
    public class Result<T>
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public bool Found { get; set; }
        public bool Authorized { get; set; }
        public string Error { get; set; }
        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Found = true, Value = value }; //Se usa para respuestas 200
        public static Result<T> NotFound(string error) => new Result<T> { IsSuccess = true, Found = false, Error = error }; //Se usa para respuestas 404
        public static Result<T> Failure(string error) => new Result<T> { IsSuccess = false, Found = false, Error = error }; //Se usa para respuestas400 
        public static Result<T> Unauthorized(string error) => new Result<T> { IsSuccess = true, Found = false, Authorized = false, Error = error }; //Se usa para respuestas 401
    }
}