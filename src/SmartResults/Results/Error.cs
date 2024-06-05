namespace SmartResults;

/// <summary>
/// Error
/// </summary>
public class Error : IError
{
    private static Dictionary<string, Type> _errorType = new Dictionary<string, Type>();

    public static void Register<T>(string? discriminator = null)
         where T : IError
    {
        Type type = typeof(T);

        if (discriminator == null)
        {
            discriminator = type.Name;
        }

        _errorType[discriminator] = type;
    }

    internal static (string Key, Type Type)[] GetErrors() => _errorType.Select(x => (x.Key, x.Value)).ToArray();

    static Error()
    {
        Register<Error>();
    }

    public Error(string message)
    {
        Message = message;
    }

    public static Error FromException(Exception ex)
    {
        return new Error(ex.Message) { Exception = ex };
    }

    /// <summary>
    /// Message
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Exception
    /// </summary>
    public Exception? Exception { get; init; }

    public override string ToString()
    {
        return Message;
    }
}
