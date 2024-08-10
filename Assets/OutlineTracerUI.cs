using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class OutlineTracerUI : MonoBehaviour
{
    public Image targetImage;
    public float outlineThreshold = 0.05f;  // Distance from edge considered as part of outline
    public float minTraceDistance = 0.01f;  // Minimum distance between points in the trace

    private RectTransform imageRectTransform;
    private Vector2 imageSize;
    private List<Vector2> tracedPoints = new List<Vector2>();
    private LineRenderer lineRenderer;

    private Vector2 lastLocalPoint;  // Store the last local point for Gizmos
    private Vector2 lastNormalizedPoint;  // Store the last normalized point for Gizmos

    void Start ()
    {
        if (targetImage == null)
        {
            Debug.LogError("Target Image is not assigned!");
            return;
        }

        imageRectTransform = targetImage.rectTransform;
        imageSize = imageRectTransform.rect.size;
        lineRenderer = GetComponent<LineRenderer>();

        // Set up LineRenderer
        lineRenderer.startWidth = 0.05f;  // Adjust the width as needed
        lineRenderer.endWidth = 0.05f;
        lineRenderer.useWorldSpace = false;  // Important: this allows the positions to be relative to the Image
        lineRenderer.positionCount = 0;

        // Assign a simple material (with a color) to the LineRenderer
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }

    void Update ()
    {
        if (targetImage == null)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(imageRectTransform, touch.position, null, out localPoint))
                {
                    // Store the local point for Gizmos
                    lastLocalPoint = localPoint;

                    // Adjust localPoint so that it is relative to the bottom-left corner of the RectTransform
                    localPoint += imageRectTransform.rect.size * imageRectTransform.pivot;

                    // Convert the local point to normalized coordinates
                    Vector2 normalizedPoint = new Vector2(
                        localPoint.x / imageRectTransform.rect.width,
                        localPoint.y / imageRectTransform.rect.height
                    );

                    // Store the normalized point for Gizmos
                    lastNormalizedPoint = normalizedPoint;

                    Debug.Log("Normalized Point: " + normalizedPoint);

                    if (IsPointOnOutline(normalizedPoint))
                    {
                        AddTracedPoint(normalizedPoint);
                        UpdateLineRenderer();
                    }
                    else
                    {
                        Debug.Log("Point not on outline: " + normalizedPoint);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                CheckTraceCompletion();
            }
        }
    }

    bool IsPointOnOutline ( Vector2 point )
    {
        // Ensure the point is within the bounds of the normalized coordinates
        if (point.x < 0 || point.x > 1 || point.y < 0 || point.y > 1)
            return false;

        // Check the distance to the edges
        float distanceToEdge = Mathf.Min(
            point.x,
            point.y,
            1 - point.x,
            1 - point.y
        );

        Debug.Log("Distance to edge: " + distanceToEdge + " (Threshold: " + outlineThreshold + ")");
        return distanceToEdge <= outlineThreshold;
    }

    void AddTracedPoint ( Vector2 point )
    {
        // Avoid adding points too close to each other
        if (tracedPoints.Count == 0 || Vector2.Distance(point, tracedPoints[tracedPoints.Count - 1]) >= minTraceDistance)
        {
            tracedPoints.Add(point);
            Debug.Log("Added traced point: " + point + " | Total points: " + tracedPoints.Count);
        }
        else
        {
            Debug.Log("Point too close to previous one, not added: " + point);
        }
    }

    void UpdateLineRenderer ()
    {
        lineRenderer.positionCount = tracedPoints.Count;
        for (int i = 0; i < tracedPoints.Count; i++)
        {
            // Convert normalized points to local space
            Vector3 localPoint = new Vector3(
                tracedPoints[i].x * imageSize.x - (imageSize.x / 2),
                tracedPoints[i].y * imageSize.y - (imageSize.y / 2),
                0
            );
            lineRenderer.SetPosition(i, localPoint);
        }
    }

    void CheckTraceCompletion ()
    {
        // Enhanced check: the number of points and the distance covered by the traced points
        if (tracedPoints.Count > 20)
        {
            Debug.Log("Outline tracing complete!");
        }
        else
        {
            Debug.Log("Incomplete trace. Try again.");
        }

        tracedPoints.Clear();
        lineRenderer.positionCount = 0;
    }

    void OnDrawGizmos ()
    {
        if (targetImage == null || imageRectTransform == null)
            return;

        // Draw the local point in yellow
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(imageRectTransform.TransformPoint(lastLocalPoint), 10f);

        // Draw the normalized point in red (relative to the RectTransform's position)
        Vector2 normalizedPositionInWorld = new Vector2(
            lastNormalizedPoint.x * imageRectTransform.rect.width,
            lastNormalizedPoint.y * imageRectTransform.rect.height
        );
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(imageRectTransform.TransformPoint(normalizedPositionInWorld), 10f);

        // Optionally, draw the bounds of the RectTransform in green
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(imageRectTransform.position, imageRectTransform.rect.size);
    }
}
