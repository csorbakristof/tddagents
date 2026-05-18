namespace Mastermind;

public class Game
{
    private readonly string[] _secret;
    private readonly int _maxAttempts;
    private int _attemptsMade;
    private bool _isGameOver;

    public Game(string[] secret, int maxAttempts)
    {
        _secret = secret;
        _maxAttempts = maxAttempts;
    }

    public string[] Secret => _secret;

    public GuessResult MakeGuess(string[] guess)
    {
        if (_isGameOver)
            throw new InvalidOperationException("The game is already over.");

        var feedback = FeedbackEvaluator.Evaluate(_secret, guess);
        _attemptsMade++;
        bool isWon = feedback.ExactMatches == _secret.Length;
        bool isGameOver = isWon || _attemptsMade >= _maxAttempts;
        _isGameOver = isGameOver;

        return new GuessResult(feedback, isWon, isGameOver, _attemptsMade);
    }
}
