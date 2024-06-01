using SmartResults.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SmartResults;

/// <summary>
/// Result
/// </summary>
[JsonConverter(typeof(ResultJsonConverterFactory))]
public readonly struct Result<T> : IResult<Result<T>>, IEquatable<Result<T>>
{
    private Result(T value)
    {
        _value = value;
    }

    private Result(IError error)
    {
        _error = error;
    }

    private readonly T _value = default!;

    /// <summary>
    /// Gets the result value.
    /// </summary>
    public T Value
    {
        get
        {
            if (IsFailed)
            {
                throw new InvalidOperationException();
            }

            return _value;
        }
    }

    private readonly IError? _error;

    /// <summary>
    /// Error
    /// </summary>
    public IError? Error
    {
        get
        {
            if (IsSucceeded)
            {
                throw new InvalidOperationException();
            }

            return _error;
        }
    }

    /// <summary>
    /// Is result failed?
    /// </summary>
    [MemberNotNullWhen(true, nameof(Error))]    
    public bool IsFailed => _error is not null;

    /// <summary>
    /// Is result succeeded?
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSucceeded => _error is null;

    public override bool Equals(object? obj)
    {
        return obj is Result<T> result && Equals(result);
    }

    public bool Equals(Result<T> other)
    {
        if (IsSucceeded)
        {
            if (other.IsSucceeded)
            {
                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }
        }
        else
        {
            if (other.IsFailed)
            {
                return EqualityComparer<IError>.Default.Equals(Error, other.Error);
            }
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IsSucceeded, Value, Error);
    }

    /// <summary>
    /// ToString
    /// </summary>
    public override string ToString()
    {
        if (IsSucceeded)
        {
            return $"Succeeded: {Value}";
        }
        else
        {
            return $"Failed: {Error}";
        }
    }

    /// <summary>
    /// Match
    /// </summary>
    public void Match(Action<T> succeeded, Action<IError> failed)
    {
        if (IsSucceeded)
        {
            succeeded(Value);
        }
        else
        {
            failed(Error);
        }
    }

    /// <summary>
    /// Match
    /// </summary>
    public TOut Match<TOut>(Func<T, TOut> succeeded, Func<IError, TOut> failed)
    {
        if (IsSucceeded)
        {
            return succeeded(Value);
        }
        else
        {
            return failed(Error);
        }
    }

    /// <summary>
    /// Converts value from <typeparamref name="T"/> to <typeparamref name="TOut"/>.
    /// </summary>
    public Result<TOut> ToResult<TOut>(Func<T, TOut> convert)
    {
        if (IsSucceeded)
        {
            return Result<TOut>.Ok(convert(Value));
        }
        else
        {
            return Result<TOut>.Failed(Error);
        }
    }

    /// <summary>
    /// Creates a succeeded result.
    /// </summary>
    public static Result<T> Ok()
    {
        return new Result<T>();
    }

    /// <summary>
    /// Creates a succeeded result with value.
    /// </summary>
    public static Result<T> Ok(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static Result<T> Failed(string message)
    {
        return new Result<T>(new Error(message));
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static Result<T> Failed(Exception exception)
    {
        return new Result<T>(new Error(exception.Message, exception));
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static Result<T> Failed(IError error)
    {
        return new Result<T>(error);
    }    

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static implicit operator T(Result<T> result)
    {
        if (result.IsFailed)
        {
            throw new Exception(result.Error.Message);
        }

        return result.Value;
    }

    public static implicit operator Result(Result<T> result)
    {
        return result.IsSucceeded ? Result.Ok() : Result.Failed(result.Error);
    }

    public static bool operator ==(Result<T> left, Result<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result<T> left, Result<T> right)
    {
        return left.Equals(right) == false;
    }    
}
