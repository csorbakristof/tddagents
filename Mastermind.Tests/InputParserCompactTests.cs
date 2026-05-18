using Mastermind;
using Xunit;

namespace Mastermind.Tests;

public class InputParserCompactTests
{
    private static readonly IReadOnlyDictionary<char, string> Map = new Dictionary<char, string>
    {
        ['R'] = "Red", ['B'] = "Blue", ['G'] = "Green", ['Y'] = "Yellow"
    };

    [Fact]
    public void ParseCompact_ValidInput_ReturnsSuccess()
    {
        var result = InputParser.ParseCompact("RBGY", Map, 4);

        var success = Assert.IsType<ParseResult.Success>(result);
        Assert.Equal(new[] { "Red", "Blue", "Green", "Yellow" }, success.Pegs);
    }

    [Fact]
    public void ParseCompact_LowercaseInput_Normalizes()
    {
        var result = InputParser.ParseCompact("rbgy", Map, 4);

        var success = Assert.IsType<ParseResult.Success>(result);
        Assert.Equal(new[] { "Red", "Blue", "Green", "Yellow" }, success.Pegs);
    }

    [Fact]
    public void ParseCompact_MixedCaseInput_Normalizes()
    {
        var result = InputParser.ParseCompact("RbGy", Map, 4);

        var success = Assert.IsType<ParseResult.Success>(result);
        Assert.Equal(new[] { "Red", "Blue", "Green", "Yellow" }, success.Pegs);
    }

    [Fact]
    public void ParseCompact_WrongLength_ReturnsFailure()
    {
        var result = InputParser.ParseCompact("RBG", Map, 4);

        Assert.IsType<ParseResult.Failure>(result);
    }

    [Fact]
    public void ParseCompact_InvalidLetter_ReturnsFailure()
    {
        var result = InputParser.ParseCompact("RBGX", Map, 4);

        var failure = Assert.IsType<ParseResult.Failure>(result);
        Assert.Contains("X", failure.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ParseCompact_EmptyInput_ReturnsFailure()
    {
        var result = InputParser.ParseCompact("", Map, 4);

        Assert.IsType<ParseResult.Failure>(result);
    }

    [Fact]
    public void ParseCompact_DuplicatesAllowed()
    {
        var result = InputParser.ParseCompact("RRGG", Map, 4);

        var success = Assert.IsType<ParseResult.Success>(result);
        Assert.Equal(new[] { "Red", "Red", "Green", "Green" }, success.Pegs);
    }
}
