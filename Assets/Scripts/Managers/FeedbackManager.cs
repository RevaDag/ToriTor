using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private ToriTheCat toriTheCat;

    [SerializeField] private List<Line> positiveFeedback;
    [SerializeField] private List<Line> encouragementFeedback;

    private int previousFeedbackIndex = -1;


    private Line RandomFeedback ( List<Line> feedbackList )
    {
        if (feedbackList.Count == 0)
        {
            Debug.LogWarning("Feedback list is empty.");
            return null; // Handle the case where the list is empty
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, feedbackList.Count);
        } while (newIndex == previousFeedbackIndex && feedbackList.Count > 1);

        //Debug.Log($"Selected feedback index: {newIndex} (previous: {previousFeedbackIndex})");

        previousFeedbackIndex = newIndex;

        feedbackList[newIndex].type = Line.Type.Feedback;

        return feedbackList[newIndex];
    }

    public void SendFeedback ( int feedbackType )
    {
        Line feedback = null;

        switch (feedbackType)
        {
            case 0:
                feedback = RandomFeedback(positiveFeedback);
                toriTheCat.SetEmotion("Like");
                break;
            case 1:
                feedback = RandomFeedback(encouragementFeedback);
                toriTheCat.SetEmotion("Default");
                break;
            case 2:
                feedback = RandomFeedback(positiveFeedback);
                toriTheCat.SetEmotion("Celebrate");
                break;

            default:
                Debug.LogError("Invalid feedback type");
                return;
        }

        if (feedback != null)
        {
            List<Line> lineList = new List<Line> { feedback };
            dialogManager.SetLinesAndPlay(lineList);
        }
    }

}
