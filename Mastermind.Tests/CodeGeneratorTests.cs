using Mastermind;

namespace Mastermind.Tests;

public class CodeGeneratorTests
{
    private static readonly string[] SixColors = { "Red", "Blue", "Green", "Yellow", "Orange", "Purple" };

    [Fact]
    public void Generate_ReturnsArrayOfCorrectLength()
    {
        var result = CodeGenerator.Generate(SixColors, 4);

        Assert.Equal(4, result.Length);
    }

    [Fact]
    public void Generate_AllPegsAreFromAvailableColors()
    {
        var allowed = new HashSet<string>(SixColors);

        var result = CodeGenerator.Generate(SixColors, 4);

        Assert.All(result, peg => Assert.Contains(peg, allowed));
    }

    [Fact]
    public void Generate_WithSingleColor_ReturnsAllSameColor()
    {
        var singleColor = new[] { "Red" };

        var result = CodeGenerator.Generate(singleColor, 4);

        Assert.All(result, peg => Assert.Equal("Red", peg));
    }

    [Fact]
    public void Generate_WithLengthOne_ReturnsSingleElement()
    {
        var result = CodeGenerator.Generate(SixColors, 1);

        Assert.Single(result);
        Assert.Contains(result[0], SixColors);
    }

    [Fact]
    public void Generate_ThrowsWhenColorsEmpty()
    {
        Assert.Throws<ArgumentException>(() => CodeGenerator.Generate(Array.Empty<string>(), 4));
    }
}
