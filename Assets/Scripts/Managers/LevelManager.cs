using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int world;
    private Level level;
    [SerializeField] private QuizSummary levelSummary;
    public Stepper stepper;

    /*
        private void Start ()
        {
            level = GameManager.Instance.currentLevel;
        }*/

    public void NextStep ()
    {
        stepper?.activateNextStep();
    }

    public void CompleteLevel ()
    {
        // GameManager.Instance.SaveCompletedLevel(level);
        if (levelSummary != null)
        {
            levelSummary.ShowSummary();
        }
    }

    public bool IsLastStep ()
    {
        if (stepper.currentStep == stepper.steps.Count)
            return true;
        else
            return false;
    }
}
