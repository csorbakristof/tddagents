namespace Mastermind;

public record FeedbackResult(int ExactMatches, int ColorMatches);

public class FeedbackEvaluator
{
    public static FeedbackResult Evaluate(string[] secret, string[] guess)
    {
        int exactMatches = 0;
        var unmatchedSecret = new List<string>();
        var unmatchedGuess = new List<string>();

        for (int i = 0; i < secret.Length; i++)
        {
            if (secret[i] == guess[i])
            {
                exactMatches++;
            }
            else
            {
                unmatchedSecret.Add(secret[i]);
                unmatchedGuess.Add(guess[i]);
            }
        }

        var secretFreq = CountFrequencies(unmatchedSecret);
        var guessFreq = CountFrequencies(unmatchedGuess);

        int colorMatches = 0;
        foreach (var key in guessFreq.Keys)
        {
            if (secretFreq.TryGetValue(key, out int sCount))
                colorMatches += Math.Min(sCount, guessFreq[key]);
        }

        return new FeedbackResult(exactMatches, colorMatches);
    }

    private static Dictionary<string, int> CountFrequencies(IEnumerable<string> items)
    {
        var freq = new Dictionary<string, int>();
        foreach (var item in items)
            freq[item] = freq.GetValueOrDefault(item) + 1;
        return freq;
    }
}
