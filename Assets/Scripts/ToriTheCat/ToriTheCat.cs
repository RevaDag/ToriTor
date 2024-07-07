using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToriTheCat : MonoBehaviour
{
    [SerializeField] private Image toriImage;
    [SerializeField] private Animator animator;
    [SerializeField] private Fader toriFader;

    [SerializeField] private List<ToriEmotion> toriEmotions;

    public void FadeIn ()
    {
        toriFader.FadeIn();
    }

    public void FadeOut ()
    {
        toriFader.FadeOut();
    }

    public void SlideUp ()
    {
        animator.SetTrigger("SlideUp");
    }

    public void SlideDown ()
    {
        animator.SetTrigger("SlideDown");
    }

  

    public void SetEmotion ( string emotionName )
    {
        ToriEmotion emotion = toriEmotions.Find(e => e.EmotionName == emotionName);
        if (emotion != null)
        {
            toriImage.sprite = emotion.Sprite;
            toriImage.SetNativeSize();

            // Adjust the RectTransform based on the new pivot
            RectTransform rectTransform = toriImage.GetComponent<RectTransform>();
            Vector2 pivot = emotion.Sprite.pivot / emotion.Sprite.rect.size;
            rectTransform.pivot = pivot;

            // Optionally, set the anchored position to maintain the correct positioning
            rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            Debug.LogWarning("Emotion not found: " + emotionName);
        }
    }
}
