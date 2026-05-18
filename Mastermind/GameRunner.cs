namespace Mastermind;

public class GameRunner
{
    private readonly IGameIO _io;
    private readonly Game _game;
    private readonly string[] _validColors;
    private readonly int _codeLength;

    public GameRunner(IGameIO io, Game game, string[] validColors, int codeLength)
    {
        _io = io;
        _game = game;
        _validColors = validColors;
        _codeLength = codeLength;
    }

    public void Run()
    {
        _io.WriteLine($"Welcome to Mastermind! Guess the {_codeLength}-color code.");
        _io.WriteLine($"Available colors: {string.Join(", ", _validColors)}");

        while (true)
        {
            _io.WriteLine("Enter guess:");
            var line = _io.ReadLine() ?? string.Empty;
            var result = InputParser.Parse(line, _validColors, _codeLength);

            if (result is ParseResult.Failure failure)
            {
                _io.WriteLine(failure.ErrorMessage);
                continue;
            }

            var success = (ParseResult.Success)result;
            var guessResult = _game.MakeGuess(success.Pegs);

            _io.WriteLine($"Exact: {guessResult.Feedback.ExactMatches}, Color: {guessResult.Feedback.ColorMatches}");
            _io.WriteLine($"Attempts made: {guessResult.AttemptsMade}");

            if (guessResult.IsWon)
            {
                _io.WriteLine($"Congratulations, you won in {guessResult.AttemptsMade} attempt(s)!");
                break;
            }

            if (guessResult.IsGameOver)
            {
                _io.WriteLine($"Game over! You lost. The secret was: {string.Join(" ", _game.Secret)}");
                break;
            }
        }
    }
}
