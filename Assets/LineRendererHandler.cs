using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererHandler : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private int pointIndex = 0;

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
            Debug.Log($"Starting new line at position: {startPosition}");
        }
        // If there are already points, continue from the last point
        else
        {
            Debug.Log($"Continuing line from existing points. Current point count: {lineRenderer.positionCount}");
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



    public void ClearLine ()
    {
        lineRenderer.positionCount = 0;
        pointIndex = 0;
    }
}
