using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> RightFeedback;
    [SerializeField] private List<AudioClip> WrongFeedback;

    [SerializeField] private List<ParticleSystem> particleSystems;

    private AudioSource audioSource;

    private int previousFeedbackIndex = -1;

    public enum FeedbackType
    {
        Right,
        Wrong,
        Celebrate
    }

    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomFeedback ( List<AudioClip> feedbackList )
    {
        if (feedbackList == null || feedbackList.Count == 0)
        {
            Debug.LogError("Feedback list is empty or null.");
            return null;
        }

        if (feedbackList.Count == 1)
        {
            previousFeedbackIndex = 0;
            return feedbackList[0];
        }

        int newIndex;
        do
        {
            newIndex = Random.Range(0, feedbackList.Count);
        } while (newIndex == previousFeedbackIndex);

        previousFeedbackIndex = newIndex;
        return feedbackList[newIndex];
    }


    // Sends feedback based on the feedback type.
    public void SetFeedback ( FeedbackType feedbackType )
    {
        AudioClip feedback = null;

        switch (feedbackType)
        {
            case FeedbackType.Right:
                feedback = GetRandomFeedback(RightFeedback);
                PlayConfetti();
                break;
            case FeedbackType.Wrong:
                feedback = GetRandomFeedback(WrongFeedback);
                break;
            case FeedbackType.Celebrate:
                feedback = GetRandomFeedback(RightFeedback);
                break;
            default:
                Debug.LogError("Invalid feedback type");
                return;
        }

        audioSource.clip = feedback;
        //audioSource.Play();
    }

    public void PlayConfetti ()
    {
        foreach (ParticleSystem particleSystem in particleSystems)
        {
            particleSystem.Play();
        }
    }
}
