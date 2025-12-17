namespace API_de_Reservas.Models
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success()
            => new Result(true, string.Empty);

        public static Result Failure(string error)
            => new Result(false, error);
    }
    public class Result<T> : Result
    {
        public T Value { get; }

        protected Result(T value)
            : base(true, string.Empty)
        {
            Value = value;
        }

        protected Result(string error)
            : base(false, error)
        {
            Value = default!;
        }

        public static Result<T> Success(T value)
            => new Result<T>(value);

        public static new Result<T> Failure(string error)
            => new Result<T>(error);
    }
}
