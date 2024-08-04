using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SubjectsManager;

public interface IQuiz
{
    void InitiateQuiz ();
    void SetQuizManager ( QuizManager quizManager );
    void CorrectAnswer ( Answer answer );
    void WrongAnswer ();
    void CompleteQuiz ();
}
