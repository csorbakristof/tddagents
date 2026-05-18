namespace Mastermind;

public class CodeGenerator
{
    public static string[] Generate(string[] availableColors, int codeLength)
    {
        if (availableColors == null || availableColors.Length == 0)
            throw new ArgumentException("availableColors must not be empty.", nameof(availableColors));

        var result = new string[codeLength];
        for (int i = 0; i < codeLength; i++)
            result[i] = availableColors[Random.Shared.Next(availableColors.Length)];
        return result;
    }
}
