namespace ConcesionarioDDD.SharedKernel
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Fail(string message) => new Result(false, message);
    }

    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value => _value;

        protected Result(T value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        public static Result<T> Ok(T value) => new Result<T>(value, true, string.Empty);
        public new static Result<T> Fail(string message) => new Result<T>(default!, false, message);
    }
}