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

        var firstInvalid = tokens.FirstOrDefault(t => !lookup.ContainsKey(t));
        if (firstInvalid != null)
            return new ParseResult.Failure($"'{firstInvalid}' is not a valid color.");

        return new ParseResult.Success(tokens.Select(t => lookup[t]).ToArray());
    }

    public static ParseResult ParseCompact(string input, IReadOnlyDictionary<char, string> colorMap, int codeLength)
    {
        if (input.Length != codeLength)
            return new ParseResult.Failure($"Expected {codeLength} letters, got {input.Length}.");

        var upper = input.ToUpperInvariant();
        var firstInvalid = upper.FirstOrDefault(c => !colorMap.ContainsKey(c));
        if (firstInvalid != default)
            return new ParseResult.Failure($"'{firstInvalid}' is not a valid color letter.");

        return new ParseResult.Success(upper.Select(c => colorMap[c]).ToArray());
    }
}
