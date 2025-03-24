using SmartResults.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SmartResults;

/// <summary>
/// Result
/// </summary>
[JsonConverter(typeof(ResultJsonConverter))]
public readonly struct Result : IResult<Result>, IEquatable<Result>
{
    private Result(IError error)
    {
        _error = error;
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
    public bool IsFailed => _error != null;

    /// <summary>
    /// Is result is succeeded?
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSucceeded => _error == null;

    public override bool Equals(object? obj)
    {
        return obj is Result result && Equals(result);
    }

    public bool Equals(Result other)
    {
        if (IsSucceeded)
        {
            if (other.IsSucceeded)
            {
                return true;
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
        return HashCode.Combine(IsSucceeded, Error);
    }

    /// <summary>
    /// ToString
    /// </summary>
    public override string ToString()
    {
        if (IsSucceeded)
        {
            return "Succeeded";
        }
        else
        {
            return $"Failed: {Error}";
        }
    }

    /// <summary>
    /// Match
    /// </summary>
    public void Match(Action succeeded, Action<IError> failed)
    {
        if (IsSucceeded)
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
    public TOut Match<TOut>(Func<TOut> succeeded, Func<IError, TOut> failed)
    {
        if (IsSucceeded)
        {
            return succeeded();
        }
        else
        {
            return failed(Error);
        }
    }

    /// <summary>
    /// Creates a succeeded result.
    /// </summary>
    public static Result Ok()
    {
        return new Result();
    }

    /// <summary>
    /// Creates a succeeded result with <typeparamref name="T"/> value.
    /// </summary>
    public static Result<T> Ok<T>(T value = default!)
    {
        return Result<T>.Ok(value);
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static Result Failed(string message)
    {
        return Failed(new Error(message));
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static Result Failed(IError error)
    {
        return new Result(error);
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static Result<T> Failed<T>(string message)
    {
        return Result<T>.Failed(message);
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    public static Result<T> Failed<T>(IError error)
    {
        return Result<T>.Failed(error);
    }

    public static Result Try(Action action)
    {
        try
        {
            action();

            return Ok();
        }
        catch (Exception ex)
        {
            return Failed(SmartResults.Error.FromException(ex));
        }
    }

    public static async Task<Result> TryAsync(Func<Task> action)
    {
        try
        {
            await action().ConfigureAwait(false);

            return Ok();
        }
        catch (Exception ex)
        {
            return Failed(SmartResults.Error.FromException(ex));
        }
    }

    public static Result OkIf(bool isSucceeded, Func<IError> error)
    {
        return isSucceeded ? Ok() : Failed(error());
    }

    public static Result FailedIf(bool isFailed, Func<IError> error)
    {
        return isFailed ? Failed(error()) : Ok();
    }   

    public static bool operator ==(Result left, Result right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result left, Result right)
    {
        return left.Equals(right) == false;
    }    
}
