using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SubjectsManager;

public interface IQuiz
{
    void InitiateQuiz ();
    void SetQuizManager ( QuizManager quizManager );
    void SetQuestion ( Question question );
    void LoadCurrentQuestion ();
    void DeployQuestion ( ToriObject toriObject );
    void AnswerClicked ( bool isCorrect );
    void NextQuestion ();
    void DeployAnswers ();
    void CorrectAnswer ();
    void WrongAnswer ();
    void WrongFeedbackClicked ();
    void CompleteQuiz ();
}
