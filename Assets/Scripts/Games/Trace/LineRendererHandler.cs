using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererHandler : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private int pointIndex = 0;
    private Coroutine fadeCoroutine;


    void Awake ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }


    public void StartLine ( Vector3 startPosition )
    {
        // If there are no points yet, start the line
        if (lineRenderer.positionCount == 0)
        {
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, startPosition);
            pointIndex = 1;
            //Debug.Log($"Starting new line at position: {startPosition}");
        }
        // If there are already points, continue from the last point
        else
        {
            //Debug.Log($"Continuing line from existing points. Current point count: {lineRenderer.positionCount}");
        }
    }

    public void AddPoint ( Vector3 position )
    {
        // If the Canvas is in World Space, ensure the position matches
        Vector3 worldPosition = position; // Already in world space
        lineRenderer.positionCount = pointIndex + 1;
        lineRenderer.SetPosition(pointIndex, worldPosition);
        pointIndex++;
        //Debug.Log($"Adding point at world position: {worldPosition}");
    }

    public void FadeIn ( float duration )
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeLine(0.0f, 1.0f, duration));
    }

    public void FadeOut ( float duration )
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeLine(1.0f, 0.0f, duration));
    }

    private IEnumerator FadeLine ( float startAlpha, float endAlpha, float duration )
    {
        float elapsedTime = 0.0f;

        // Get the initial color of the LineRenderer
        Color initialColor = lineRenderer.startColor;

        // Gradually change the alpha value of the LineRenderer over the specified duration
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            Color currentColor = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            lineRenderer.startColor = currentColor;
            lineRenderer.endColor = currentColor;

            yield return null;
        }

        // Ensure the final alpha value is set
        Color finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, endAlpha);
        lineRenderer.startColor = finalColor;
        lineRenderer.endColor = finalColor;
    }

    public void ClearLine ()
    {
        lineRenderer.positionCount = 0;
        pointIndex = 0;
    }
}
