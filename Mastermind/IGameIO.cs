namespace Mastermind;

public interface IGameIO
{
    void WriteLine(string message);
    string? ReadLine();
}
