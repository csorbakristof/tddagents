namespace Mastermind;

public abstract record ParseResult
{
    public record Success(string[] Pegs) : ParseResult;
    public record Failure(string ErrorMessage) : ParseResult;
}
