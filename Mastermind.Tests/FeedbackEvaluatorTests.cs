using Mastermind;

namespace Mastermind.Tests;

public class FeedbackEvaluatorTests
{
    [Fact]
    public void AllCorrect_Returns4ExactAnd0Color()
    {
        var secret = new[] { "Red", "Blue", "Green", "Yellow" };
        var guess   = new[] { "Red", "Blue", "Green", "Yellow" };

        var result = FeedbackEvaluator.Evaluate(secret, guess);

        Assert.Equal(4, result.ExactMatches);
        Assert.Equal(0, result.ColorMatches);
    }

    [Fact]
    public void AllWrong_Returns0And0()
    {
        var secret = new[] { "Red",    "Red",    "Red",    "Red"    };
        var guess   = new[] { "Blue",   "Blue",   "Blue",   "Blue"   };

        var result = FeedbackEvaluator.Evaluate(secret, guess);

        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(0, result.ColorMatches);
    }

    [Fact]
    public void AllColorOnlyMatches_Returns0ExactAnd4Color()
    {
        // Every color present but rotated one position
        var secret = new[] { "Red", "Blue", "Green", "Yellow" };
        var guess   = new[] { "Blue", "Green", "Yellow", "Red" };

        var result = FeedbackEvaluator.Evaluate(secret, guess);

        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(4, result.ColorMatches);
    }

    [Fact]
    public void MixedResult_Returns2ExactAnd1Color()
    {
        // Positions 0 and 1 exact; position 2 has Yellow which is in secret at position 3; position 3 wrong
        var secret = new[] { "Red",  "Blue",  "Green",  "Yellow" };
        var guess   = new[] { "Red",  "Blue",  "Yellow", "Purple" };

        var result = FeedbackEvaluator.Evaluate(secret, guess);

        Assert.Equal(2, result.ExactMatches);
        Assert.Equal(1, result.ColorMatches);
    }

    [Fact]
    public void DuplicatesInSecret_NoDoubleCount()
    {
        // Secret has Red twice; guess has Red once in wrong position → color match = 1, not 2
        var secret = new[] { "Red",  "Red",   "Blue",  "Green" };
        var guess   = new[] { "Blue", "Green", "Green", "Red"   };

        var result = FeedbackEvaluator.Evaluate(secret, guess);

        Assert.Equal(0, result.ExactMatches);
        Assert.Equal(3, result.ColorMatches); // Red×1, Blue×1, Green×1
    }

    [Fact]
    public void DuplicatesInGuess_NoDoubleCount()
    {
        // Secret has Red once; guess has Red twice → color match for Red should be 1, not 2
        var secret = new[] { "Red",  "Blue",  "Green", "Yellow" };
        var guess   = new[] { "Red",  "Red",   "Red",   "Purple" };

        var result = FeedbackEvaluator.Evaluate(secret, guess);

        Assert.Equal(1, result.ExactMatches); // position 0 exact
        Assert.Equal(0, result.ColorMatches); // no remaining unmatched Reds in secret
    }
}
