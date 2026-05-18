namespace Mastermind;

public class ConsoleIO : IGameIO
{
    public void WriteLine(string message) => Console.WriteLine(message);
    public string? ReadLine() => Console.ReadLine();
}
