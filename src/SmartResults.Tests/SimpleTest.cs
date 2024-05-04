using Xunit;

namespace SmartResults.Tests;

public class SimpleTest
{
    [Fact]
    public void IsSucceeded()
    {
        Result result = Result.Ok();

        Assert.True(result.IsSucceeded);
        Assert.False(result.IsFailed);
    }

    [Fact]
    public void IsFailed()
    {
        Result result = Result.Failed("");

        Assert.False(result.IsSucceeded);
        Assert.True(result.IsFailed);
    }

    [Fact]
    public void IsValueSucceeded()
    {
        Result<int> result = Result.Ok(123);

        Assert.True(result.IsSucceeded);
        Assert.False(result.IsFailed);
        Assert.Equal(123, result.Value);
    }

    [Fact]
    public void IsValueFailed()
    {
        Result<int> result = Result<int>.Failed("");

        Assert.False(result.IsSucceeded);
        Assert.True(result.IsFailed);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
