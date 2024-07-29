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
            case GameType.Chest:
                return new ChestQuiz();
            case GameType.Catch:
                return new CatchQuiz();
            case GameType.Find:
                return new FindQuiz();
            default:
                throw new System.ArgumentException("Invalid game type");
        }
    }
}
