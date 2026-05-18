namespace Mastermind;

public class InputParser
{
    public static ParseResult Parse(string input, string[] validColors, int expectedLength)
    {
        var tokens = (input ?? string.Empty)
            .Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (tokens.Length != expectedLength)
            return new ParseResult.Failure($"Expected {expectedLength} colors, got {tokens.Length}.");

        var lookup = validColors.ToDictionary(c => c, c => c, StringComparer.OrdinalIgnoreCase);

        var pegs = new string[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
        {
            if (!lookup.TryGetValue(tokens[i], out var canonical))
                return new ParseResult.Failure($"'{tokens[i]}' is not a valid color.");
            pegs[i] = canonical;
        }

        return new ParseResult.Success(pegs);
    }
}
