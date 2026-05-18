namespace Mastermind;

internal class Program
{
    private static readonly IReadOnlyDictionary<char, string> ColorMap = new Dictionary<char, string>
    {
        ['R'] = "Red", ['B'] = "Blue", ['G'] = "Green",
        ['Y'] = "Yellow", ['P'] = "Purple", ['O'] = "Orange"
    };
    private const int CodeLength = 4;
    private const int MaxAttempts = 10;

    static void Main(string[] args)
    {
        var availableColors = ColorMap.Values.ToArray();
        var secret = CodeGenerator.Generate(availableColors, CodeLength);
        var game = new Game(secret, MaxAttempts);
        var io = new ConsoleIO();
        var runner = new GameRunner(io, game, ColorMap, CodeLength);
        runner.Run();
    }
}
