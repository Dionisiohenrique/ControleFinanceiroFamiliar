namespace ControleFinanceiroFamiliar.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public IReadOnlyList<string> Errors { get; }
    protected Result(bool success, IReadOnlyList<string> errors) { IsSuccess = success; Errors = errors; }
    public static Result Ok() => new(true, Array.Empty<string>());
    public static Result Fail(params string[] errors) => new(false, errors);
    public static Result Fail(IEnumerable<string> errors) => new(false, errors.ToList());
}

public class Result<T> : Result
{
    public T? Value { get; }
    private Result(bool success, T? value, IReadOnlyList<string> errors) : base(success, errors) { Value = value; }
    public static Result<T> Ok(T value) => new(true, value, Array.Empty<string>());
    public static new Result<T> Fail(params string[] errors) => new(false, default, errors);
    public static new Result<T> Fail(IEnumerable<string> errors) => new(false, default, errors.ToList());
}