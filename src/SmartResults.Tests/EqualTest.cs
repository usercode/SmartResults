using Xunit;

namespace SmartResults.Tests;

public class EqualTest
{
    [Fact]
    public void EqualSucceeded()
    {
        Assert.Equal(Result.Ok(), Result.Ok());
        Assert.NotEqual(Result.Ok(), Result.Failed(""));
    }

    [Fact]
    public void EqualFailed()
    {
        Error error = new Error("");

        Result resultA = Result.Failed(error);
        Result resultB = Result.Failed(error);

        Assert.Equal(resultA, resultB);
    }

    [Fact]
    public void EqualSucceededWithValue()
    {
        Assert.Equal(Result.Ok(100), Result.Ok(100));
        Assert.NotEqual(Result.Ok(100), Result.Ok(200));
    }

    [Fact]
    public void EqualFailedWithValue()
    {
        Error error = new Error("");

        Result<int> resultA = Result<int>.Failed(error);
        Result<int> resultB = Result<int>.Failed(error);

        Assert.Equal(resultA, resultB);
    }
}
