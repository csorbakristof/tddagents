namespace Mastermind;

public record GuessResult(FeedbackResult Feedback, bool IsWon, bool IsGameOver, int AttemptsMade);
