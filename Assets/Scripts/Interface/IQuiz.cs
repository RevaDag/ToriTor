using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public interface IQuiz
{
    void InitiateQuiz ();
    void SetQuizManager ( QuizManager quizManager );
    void SetQuestion ( Question question );
    void SetAnswers ( List<Answer> answerList );
    void LoadCurrentQuestion ();
    void DeployQuestion ( ToriObject toriObject );
    void NextQuestion ();
    void DeployAnswers ();
    void CorrectAnswer ();
    void WrongAnswer ();
    void CorrectFeedbackClicked ();
    void WrongFeedbackClicked ();
}
