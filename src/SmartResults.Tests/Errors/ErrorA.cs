
namespace SmartResults.Tests;

public class ErrorA : Error
{
    public ErrorA(string message) : base(message)
    {

    }

    public string Value { get; set; }
}
