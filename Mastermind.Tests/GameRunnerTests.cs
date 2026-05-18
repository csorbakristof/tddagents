using Mastermind;

namespace Mastermind.Tests;

internal class FakeIO : IGameIO
{
    private readonly Queue<string> _inputs;
    public List<string> Output { get; } = new();

    public FakeIO(params string[] inputs) => _inputs = new Queue<string>(inputs);

    public void WriteLine(string message) => Output.Add(message);
    public string? ReadLine() => _inputs.Count > 0 ? _inputs.Dequeue() : null;
}

public class GameRunnerTests
{
    private static readonly string[] ValidColors = { "Red", "Blue", "Green", "Yellow", "Purple", "Orange" };
    private const int CodeLength = 4;

    [Fact]
    public void Run_PlayerWinsOnFirstGuess_PrintsWinMessage()
    {
        var secret = new[] { "Red", "Blue", "Green", "Yellow" };
        var game = new Game(secret, maxAttempts: 10);
        var io = new FakeIO("Red Blue Green Yellow");
        var runner = new GameRunner(io, game, ValidColors, CodeLength);

        runner.Run();

        Assert.Contains(io.Output, line =>
            line.Contains("won", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("correct", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Run_PlayerExhaustsAttempts_PrintsLoseMessage()
    {
        var secret = new[] { "Red", "Red", "Red", "Red" };
        var game = new Game(secret, maxAttempts: 2);
        var io = new FakeIO("Blue Blue Blue Blue", "Blue Blue Blue Blue");
        var runner = new GameRunner(io, game, ValidColors, CodeLength);

        runner.Run();

        Assert.Contains(io.Output, line =>
            line.Contains("lost", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("game over", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("secret", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Run_InvalidInput_PromptsAgainAndEventuallyWins()
    {
        var secret = new[] { "Red", "Blue", "Green", "Yellow" };
        var game = new Game(secret, maxAttempts: 10);
        var io = new FakeIO("NotAColor Blue Green Yellow", "Red Blue Green Yellow");
        var runner = new GameRunner(io, game, ValidColors, CodeLength);

        runner.Run();

        // An error message was printed for the invalid input
        Assert.Contains(io.Output, line =>
            line.Contains("not a valid color", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("invalid", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("error", StringComparison.OrdinalIgnoreCase));

        // And the player eventually won (only 1 successful guess registered)
        Assert.Contains(io.Output, line =>
            line.Contains("won", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("correct", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Run_PrintsFeedbackAfterEachGuess()
    {
        var secret = new[] { "Red", "Blue", "Green", "Yellow" };
        var game = new Game(secret, maxAttempts: 10);
        // Wrong guess first, then correct
        var io = new FakeIO("Red Blue Yellow Green", "Red Blue Green Yellow");
        var runner = new GameRunner(io, game, ValidColors, CodeLength);

        runner.Run();

        // After the wrong guess, output should contain feedback (exact and color matches)
        Assert.Contains(io.Output, line =>
            (line.Contains("exact", StringComparison.OrdinalIgnoreCase) ||
             line.Contains("black", StringComparison.OrdinalIgnoreCase)) &&
            (line.Contains("color", StringComparison.OrdinalIgnoreCase) ||
             line.Contains("white", StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public void Run_ShowsRemainingAttemptsAfterEachGuess()
    {
        var secret = new[] { "Red", "Blue", "Green", "Yellow" };
        var game = new Game(secret, maxAttempts: 10);
        // Wrong guess first, then correct
        var io = new FakeIO("Red Blue Yellow Green", "Red Blue Green Yellow");
        var runner = new GameRunner(io, game, ValidColors, CodeLength);

        runner.Run();

        // Output should mention remaining attempts or attempt number
        Assert.Contains(io.Output, line =>
            line.Contains("attempt", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("remaining", StringComparison.OrdinalIgnoreCase) ||
            line.Contains("left", StringComparison.OrdinalIgnoreCase));
    }
}
