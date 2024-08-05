public interface IQuiz
{
    void InitiateQuiz ();
    void SetQuizManager ( QuizManager quizManager );
    void CorrectAnswer ( Answer answer );
    void WrongAnswer ();
    void NextQuestion ();
}
