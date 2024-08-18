using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomSpriteAnimator : MonoBehaviour
{
    public Image image;  // Reference to the Image component
    public Sprite[] sprites;  // Array of sprites to animate through
    public float frameRate = 10f;  // Frames per second

    private int currentFrame;
    private Coroutine animationCoroutine;

    void Start ()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (sprites.Length > 0)
        {
            currentFrame = 0;
            SetSprite(currentFrame);
        }
    }

    // This method sets the sprite and adjusts the image size
    void SetSprite ( int index )
    {
        image.sprite = sprites[index];
        image.SetNativeSize();
    }

    // This method starts the animation coroutine
    public void StartAnimation ()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(AnimateSprites());
    }

    // This method stops the animation coroutine
    public void StopAnimation ()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        currentFrame = 0;
    }

    // Coroutine that handles the sprite animation
    private IEnumerator AnimateSprites ()
    {
        while (true)  // Loop until manually stopped
        {
            yield return new WaitForSeconds(1f / frameRate);

            currentFrame++;
            if (currentFrame >= sprites.Length)
            {
                StopAnimation();
                yield break;
            }

            SetSprite(currentFrame);
        }
    }
}
