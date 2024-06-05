using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestObject : MonoBehaviour
{
    [SerializeField] private float moveDistance = 200f;
    [SerializeField] private float moveDuration = .5f;
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup canvasGroup;
    private RectTransform canvasObject;
    private Chest chest;
    private AudioSource audioSource;
    private Button button;

    private void Awake ()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();
    }

    private void Start ()
    {
        canvasObject = GetComponent<RectTransform>();
        MoveAndFadeObject();
        button.interactable = true;
    }

    public void SetChest ( Chest chest )
    {
        this.chest = chest;
    }

    public void ReloadChest ()
    {
        if (button.interactable)
        {
            button.interactable = false;
            StartCoroutine(PlayAudioAndReloadChest());
        }
    }

    private IEnumerator PlayAudioAndReloadChest ()
    {
        audioSource.Play();
        yield return new WaitForSeconds(1);
        yield return ReverseMoveAndReposition();
        chest.ReloadChest();
        Destroy(this.gameObject);
    }

    public void MoveAndFadeObject ()
    {
        StartCoroutine(MoveAndRepositionCanvasObject());
    }

    private IEnumerator MoveAndRepositionCanvasObject ()
    {
        // Fade in the object
        yield return Fade(canvasGroup, fadeDuration, 0, 1);

        // Move the object up
        yield return MoveObject(canvasObject, moveDistance, moveDuration);

        // Change its position in the hierarchy to be the last
        canvasObject.SetAsLastSibling();

        // Move the object back down
        yield return MoveObject(canvasObject, -moveDistance, moveDuration);
    }

    private IEnumerator ReverseMoveAndReposition ()
    {
        // Move the object up
        yield return MoveObject(canvasObject, moveDistance, moveDuration);

        // Change its position in the hierarchy to be the last
        canvasObject.SetAsFirstSibling();

        // Move the object back down
        yield return MoveObject(canvasObject, -moveDistance, moveDuration);

        // Fade in the object
        yield return Fade(canvasGroup, fadeDuration, 1, 0);
    }

    private IEnumerator MoveObject ( RectTransform rectTransform, float distance, float duration )
    {
        Vector3 startPosition = rectTransform.localPosition;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + distance, startPosition.z);
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rectTransform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            yield return null;
        }

        rectTransform.localPosition = endPosition; // Ensure it ends at the exact target position
    }

    private IEnumerator Fade ( CanvasGroup canvasGroup, float duration, float start, float end )
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = end; // Ensure it ends at full opacity
    }

}
