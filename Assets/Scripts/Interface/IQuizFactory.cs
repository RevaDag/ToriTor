using static GameManager;

public interface IQuizFactory
{
    IQuiz CreateQuiz ( GameType gameType );
}
