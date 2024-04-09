using Xunit;

namespace KaizenChallenge.Tests;

public class ScrollWheel
{
    public int ShortestPath(TimeSpan current, TimeSpan target)
    {
        return new Day().ShortestPath(current.Hours, target.Hours)
            + new Hour().ShortestPath(current.Minutes, target.Minutes);
    }
}

internal abstract class Period(int duration)
{
    public int Duration { get; } = duration;
    public int MidPoint => Duration / 2;

    public int ShortestPath(int current, int target)
    {
        var diff = Math.Abs(current - target);

        return (diff > MidPoint) ? Duration - diff : diff;
    }
}

internal class Day : Period
{
    public Day() : base(24)
    {
    }
}

internal class Hour : Period
{
    public Hour() : base(60)
    {
    }
}

public class ScrollWheelTest
{
    [Theory]
    [InlineData("03:15", "21:55", 26)]
    [InlineData("00:00", "00:01", 1)]
    [InlineData("00:00", "01:00", 1)]
    [InlineData("00:00", "01:01", 2)]
    [InlineData("00:00", "00:30", 30)]
    [InlineData("00:00", "00:31", 29)]
    [InlineData("00:00", "12:31", 41)]
    [InlineData("00:00", "13:31", 40)]
    public void ShortestPathIsCorrect(string Current, string Target, int ExpectedResult)
    {
        var sut = new ScrollWheel();

        Assert.True(ExpectedResult == sut.ShortestPath(TimeSpan.Parse(Current), TimeSpan.Parse(Target)));

        // The order of the arguments is irrelevant given it is a circular period
        Assert.True(ExpectedResult == sut.ShortestPath(TimeSpan.Parse(Target), TimeSpan.Parse(Current)));
    }

    [Theory]
    [InlineData("abbcccddddx", true)]
    [InlineData("abbcccddz", false)]
    public void BeautifulStringAssessmentIsCorrect(string input, bool expectedResult){
        var store = input
            .GroupBy(item => item)
            .ToDictionary(item => item.Key, item => item.Count());

        var isBeautiful = store.All(item => {
            var precedingChar = item.Key; 
            precedingChar--;

            if(store.TryGetValue(precedingChar, out var count)){
                return item.Value > count;
            }
            
            return item.Value > 0;            
        });

        Assert.True(expectedResult == isBeautiful);
    }
}