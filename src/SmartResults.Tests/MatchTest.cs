using Xunit;

namespace SmartResults.Tests;

public class MatchTest
{
    [Fact]
    public void MatchSucceeded()
    {
        Result.Ok().Match(() => Assert.True(true), err => Assert.Fail());
    }

    [Fact]
    public void MatchFailed()
    {
        Result.Failed("Error").Match(() => Assert.Fail(), error => Assert.True(true));
    }

    [Fact]
    public void MatchValueSucceeded()
    {
        bool r = Result.Ok(123).Match(x => true, error => false);

        Assert.True(r);
    }

    [Fact]
    public void MatchValueFailed()
    {
        bool r = Result<int>.Failed("").Match(x => true, error => false);

        Assert.False(r);
    }
}
