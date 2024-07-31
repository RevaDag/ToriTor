using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class DraggingTutorial : MonoBehaviour
{
    [SerializeField] private RectTransform answerRect;
    [SerializeField] private RectTransform targetRect;
    [SerializeField] private RectTransform draggingObject;
    [SerializeField] private Canvas canvas;

    [SerializeField] private Fader fader;

    private bool isMoving = false;
    private Coroutine moveCoroutine;

    public async Task SetAnswerAndPlay ( RectTransform answer )
    {
        answerRect = answer;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        isMoving = true;
        await Task.Delay(2000);
        moveCoroutine = StartCoroutine(MoveObjectToTarget());
    }

    public void StopMoving ()
    {
        isMoving = false;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        fader.FadeOut();
    }

    private IEnumerator MoveObjectToTarget ()
    {
        while (isMoving)
        {
            Vector3 startPosition = answerRect.position;
            Vector3 endPosition = targetRect.position;
            float duration = 2.0f; // duration in seconds
            float elapsed = 0.0f;

            fader.FadeIn();

            while (elapsed < duration)
            {
                draggingObject.position = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            draggingObject.position = endPosition;

            fader.FadeOut();

            // Optional: Add a delay between loops
            yield return new WaitForSeconds(1.0f);

            // Reset position
            draggingObject.position = answerRect.position;
        }
    }
}
