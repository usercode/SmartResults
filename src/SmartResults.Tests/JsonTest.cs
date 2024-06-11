using System.Text.Json;
using Xunit;

namespace SmartResults.Tests;

public class JsonTest
{
    public JsonTest()
    {
        Error.Register<ErrorA>();
    }

    [Fact]
    public void IsSucceeded()
    {
        Result result = Result.Ok();

        string json = JsonSerializer.Serialize(result);

        Result result2 = JsonSerializer.Deserialize<Result>(json);

        Assert.True(result.IsSucceeded);
    }

    [Fact]
    public void IsFailed()
    {
        Result result = Result.Failed("Something is wrong!");

        string json = JsonSerializer.Serialize(result);

        Result result2 = JsonSerializer.Deserialize<Result>(json);

        Assert.True(result2.IsFailed);
        Assert.Equal(result.Error.Message, result2.Error.Message);
    }

    [Fact]
    public void IsSucceededWithValue()
    {
        Result<int> result = Result.Ok(123);

        string json = JsonSerializer.Serialize(result);

        Result<int> result2 = JsonSerializer.Deserialize<Result<int>>(json);

        Assert.True(result.IsSucceeded);
        Assert.Equal(result.Value, result2.Value);
    }

    [Fact]
    public void IsFailedWithValue()
    {
        Result<int> result = Result.Failed<int>("Something is wrong!");

        string json = JsonSerializer.Serialize(result);

        Result<int> result2 = JsonSerializer.Deserialize<Result<int>>(json);

        Assert.True(result2.IsFailed);
        Assert.Equal(result.Error.Message, result2.Error.Message);
    }

    [Fact]
    public void IsSucceededWithComplexValue()
    {
        Result<ComplexValue> result = Result.Ok(new ComplexValue() { Name = "Ok", Value = 123 });

        string json = JsonSerializer.Serialize(result);

        Result<ComplexValue> result2 = JsonSerializer.Deserialize<Result<ComplexValue>>(json);

        Assert.True(result.IsSucceeded);
        Assert.Equal(result.Value.Name, result2.Value.Name);
        Assert.Equal(result.Value.Value, result2.Value.Value);
    }

    [Fact]
    public void PolymorphicError()
    {
        Result result = Result.Failed(new ErrorA("Test A") { Value = "123" });

        string json = JsonSerializer.Serialize(result);

        Result result2 = JsonSerializer.Deserialize<Result>(json);

        Assert.True(result2.IsFailed);
        Assert.IsType<ErrorA>(result2.Error);
        Assert.Equal(result.Error.Message, result2.Error.Message);
        Assert.Equal(((ErrorA)result.Error).Value, ((ErrorA)result2.Error).Value);
    }

    [Fact]
    public void PolymorphicErrorWithValue()
    {
        Result<int> result = Result<int>.Failed(new ErrorA("Error") { Value = "123" });

        string json = JsonSerializer.Serialize(result);

        Result<int> result2 = JsonSerializer.Deserialize<Result<int>>(json);

        Assert.True(result2.IsFailed);
        Assert.IsType<ErrorA>(result2.Error);
        Assert.Equal(result.Error.Message, result2.Error.Message);
        Assert.Equal(((ErrorA)result.Error).Value, ((ErrorA)result2.Error).Value);
    }
}
