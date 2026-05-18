namespace Mastermind;

public class CodeGenerator
{
    public static string[] Generate(string[] availableColors, int codeLength)
    {
        if (availableColors == null || availableColors.Length == 0)
            throw new ArgumentException("availableColors must not be empty.", nameof(availableColors));

        return Enumerable.Range(0, codeLength)
            .Select(_ => availableColors[Random.Shared.Next(availableColors.Length)])
            .ToArray();
    }
}
