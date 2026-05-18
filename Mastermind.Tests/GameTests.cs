using Mastermind;

namespace Mastermind.Tests;

public class GameTests
{
    private static readonly string[] Secret = { "Red", "Blue", "Green", "Yellow" };

    [Fact]
    public void MakeGuess_CorrectGuess_ReturnsIsWonTrue()
    {
        var game = new Game(Secret, maxAttempts: 10);

        var result = game.MakeGuess(new[] { "Red", "Blue", "Green", "Yellow" });

        Assert.True(result.IsWon);
        Assert.Equal(Secret.Length, result.Feedback.ExactMatches);
    }

    [Fact]
    public void MakeGuess_WrongGuess_DoesNotWin()
    {
        var game = new Game(Secret, maxAttempts: 10);

        var result = game.MakeGuess(new[] { "Purple", "Purple", "Purple", "Purple" });

        Assert.False(result.IsWon);
    }

    [Fact]
    public void MakeGuess_TracksAttemptCount()
    {
        var game = new Game(Secret, maxAttempts: 10);
        var wrongGuess = new[] { "Purple", "Purple", "Purple", "Purple" };

        game.MakeGuess(wrongGuess);
        var result = game.MakeGuess(wrongGuess);

        Assert.Equal(2, result.AttemptsMade);
    }

    [Fact]
    public void MakeGuess_LastAttemptAndWrong_IsGameOverTrue()
    {
        var game = new Game(Secret, maxAttempts: 3);
        var wrongGuess = new[] { "Purple", "Purple", "Purple", "Purple" };

        game.MakeGuess(wrongGuess);
        game.MakeGuess(wrongGuess);
        var result = game.MakeGuess(wrongGuess);

        Assert.True(result.IsGameOver);
        Assert.False(result.IsWon);
    }

    [Fact]
    public void MakeGuess_WinOnLastAttempt_IsWonAndGameOver()
    {
        var game = new Game(Secret, maxAttempts: 3);
        var wrongGuess = new[] { "Purple", "Purple", "Purple", "Purple" };
        var correctGuess = new[] { "Red", "Blue", "Green", "Yellow" };

        game.MakeGuess(wrongGuess);
        game.MakeGuess(wrongGuess);
        var result = game.MakeGuess(correctGuess);

        Assert.True(result.IsWon);
        Assert.True(result.IsGameOver);
    }

    [Fact]
    public void MakeGuess_AfterGameOver_ThrowsInvalidOperationException()
    {
        var game = new Game(Secret, maxAttempts: 1);
        var wrongGuess = new[] { "Purple", "Purple", "Purple", "Purple" };

        game.MakeGuess(wrongGuess); // exhausts attempts

        Assert.Throws<InvalidOperationException>(() => game.MakeGuess(wrongGuess));
    }

    [Fact]
    public void MakeGuess_FeedbackIsCorrect()
    {
        // Secret: Red Blue Green Yellow
        // Guess:  Red Blue Yellow Purple
        // Expected: 2 exact (Red, Blue), 1 color (Yellow is in secret but wrong position)
        var game = new Game(Secret, maxAttempts: 10);

        var result = game.MakeGuess(new[] { "Red", "Blue", "Yellow", "Purple" });

        Assert.Equal(2, result.Feedback.ExactMatches);
        Assert.Equal(1, result.Feedback.ColorMatches);
    }
}
