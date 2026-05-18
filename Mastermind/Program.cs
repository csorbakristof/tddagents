namespace Mastermind;

internal class Program
{
    private static readonly string[] AvailableColors = { "Red", "Blue", "Green", "Yellow", "Purple", "Orange" };
    private const int CodeLength = 4;
    private const int MaxAttempts = 10;

    static void Main(string[] args)
    {
        var secret = CodeGenerator.Generate(AvailableColors, CodeLength);
        var game = new Game(secret, MaxAttempts);
        var io = new ConsoleIO();
        var runner = new GameRunner(io, game, AvailableColors, CodeLength);
        runner.Run();
    }
}
