using System.Diagnostics.CodeAnalysis;

namespace SmartResults;

/// <summary>
/// Result
/// </summary>
public readonly struct Result : IResult<Result>, IEquatable<Result>
{
    private Result(IError error)
    {
        _error = error;
    }

    private readonly IError _error = default!;

    /// <summary>
    /// Errors
    /// </summary>
    public IError? Error
    {
        get
        {
            if (IsOk)
            {
                throw new InvalidOperationException();
            }

            return _error;
        }
    }

    /// <summary>
    /// IsFailed
    /// </summary>
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailed => Error != null;

    /// <summary>
    /// IsSucceeded
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsOk => Error == null;

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Result result && Equals(result);
    }

    public bool Equals(Result other)
    {
        return IsOk == other.IsOk;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(typeof(Result), IsOk);
    }

    /// <summary>
    /// ToString
    /// </summary>
    public override string ToString()
    {
        if (IsFailed)
        {
            return $"Error: {Error}";
        }
        else
        {
            return $"";
        }        
    }

    /// <summary>
    /// Match
    /// </summary>
    public void Match(Action succeeded, Action<IError> failed)
    {
        if (IsOk)
        {
            succeeded();
        }
        else
        {
            failed(Error);
        }
    }

    /// <summary>
    /// Match
    /// </summary>
    public TResult Match<TResult>(Func<TResult> succeeded, Func<IError, TResult> failed)
    {
        if (IsOk)
        {
            return succeeded();
        }
        else
        {
            return failed(Error);
        }
    }

    /// <summary>
    /// Ok
    /// </summary>
    public static Result Ok()
    {
        return new Result();
    }

    /// <summary>
    /// Ok
    /// </summary>
    public static Result<T> Ok<T>()
    {
        return Result<T>.Ok();
    }

    /// <summary>
    /// Ok
    /// </summary>
    public static Result<T> Ok<T>(T value)
    {
        return Result<T>.Ok(value);
    }

    /// <summary>
    /// Failed
    /// </summary>
    public static Result Failed(string message)
    {
        return Failed(new Error(message));
    }

    /// <summary>
    /// Failed
    /// </summary>
    public static Result Failed(Exception exception)
    {
        return Failed(new Error(exception.Message, exception));
    }

    /// <summary>
    /// Failed
    /// </summary>
    public static Result Failed(IError error)
    {
        return new Result(error);
    }

    public static TResult Try<TResult>(Func<TResult> action)
        where TResult : IResult<TResult>
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            return TResult.Failed(SmartResults.Error.FromException(ex));
        }
    }

    public static async Task<TResult> TryAsync<TResult>(Func<Task<TResult>> action)
        where TResult : IResult<TResult>
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return TResult.Failed(SmartResults.Error.FromException(ex));
        }
    }

    public static Result OkIf(bool isSucceded, IError error)
    {
        return isSucceded ? Ok() : Failed(error);
    }

    public static Result FailedIf(bool isFailed, string error)
    {
        return isFailed ? Failed(error) : Ok();
    }

    public static bool operator ==(Result left, Result right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result left, Result right)
    {
        return !(left == right);
    }    
}
