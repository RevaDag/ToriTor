using static GameManager;

public class QuizFactory : IQuizFactory
{
    public IQuiz CreateQuiz ( GameType gameType )
    {
        switch (gameType)
        {
            case GameType.Speech:
                return new SpeechQuiz();
            // Add other cases for different quiz types
            case GameType.Matching:
                return new MatchQuiz();
            default:
                throw new System.ArgumentException("Invalid game type");
        }
    }
}
