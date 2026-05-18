namespace Mastermind;

public record FeedbackResult(int ExactMatches, int ColorMatches);

public class FeedbackEvaluator
{
    public static FeedbackResult Evaluate(string[] secret, string[] guess)
    {
        var pairs = secret.Zip(guess);

        int exactMatches = pairs.Count(p => p.First == p.Second);

        var unmatched = pairs.Where(p => p.First != p.Second).ToList();
        var secretFreq = CountFrequencies(unmatched.Select(p => p.First));
        var guessFreq = CountFrequencies(unmatched.Select(p => p.Second));

        int colorMatches = guessFreq.Sum(kv =>
            secretFreq.TryGetValue(kv.Key, out int sCount) ? Math.Min(sCount, kv.Value) : 0);

        return new FeedbackResult(exactMatches, colorMatches);
    }

    private static Dictionary<string, int> CountFrequencies(IEnumerable<string> items) =>
        items.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
}
