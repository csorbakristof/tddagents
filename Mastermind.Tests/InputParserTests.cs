using Mastermind;

namespace Mastermind.Tests;

public class InputParserTests
{
    private static readonly string[] ValidColors = ["Red", "Blue", "Green", "Yellow"];

    [Fact]
    public void Parse_ValidInput_ReturnsSuccess()
    {
        var result = InputParser.Parse("Red Blue Green Yellow", ValidColors, 4);

        Assert.IsType<ParseResult.Success>(result);
    }

    [Fact]
    public void Parse_CaseInsensitive_NormalizesToValidColorName()
    {
        var result = InputParser.Parse("red BLUE green yellow", ValidColors, 4);

        var success = Assert.IsType<ParseResult.Success>(result);
        Assert.Equal(["Red", "Blue", "Green", "Yellow"], success.Pegs);
    }

    [Fact]
    public void Parse_TooFewTokens_ReturnsFailure()
    {
        var result = InputParser.Parse("Red Blue Green", ValidColors, 4);

        Assert.IsType<ParseResult.Failure>(result);
    }

    [Fact]
    public void Parse_TooManyTokens_ReturnsFailure()
    {
        var result = InputParser.Parse("Red Blue Green Yellow Red", ValidColors, 4);

        Assert.IsType<ParseResult.Failure>(result);
    }

    [Fact]
    public void Parse_InvalidColor_ReturnsFailure()
    {
        var result = InputParser.Parse("Red Blue Green Purple", ValidColors, 4);

        Assert.IsType<ParseResult.Failure>(result);
    }

    [Fact]
    public void Parse_EmptyInput_ReturnsFailure()
    {
        var result = InputParser.Parse("   ", ValidColors, 4);

        Assert.IsType<ParseResult.Failure>(result);
    }

    [Fact]
    public void Parse_ValidInput_PegsMatchExact()
    {
        var result = InputParser.Parse("red BLUE green YELLOW", ValidColors, 4);

        var success = Assert.IsType<ParseResult.Success>(result);
        Assert.Equal(["Red", "Blue", "Green", "Yellow"], success.Pegs);
    }
}
