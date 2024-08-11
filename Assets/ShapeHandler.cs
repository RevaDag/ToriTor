using System.Collections.Generic;
using UnityEngine;
using VectorShapes;

public class ShapeHandler : MonoBehaviour
{
    public Shape shape; // Reference to the Shape component
    public int segments = 36; // Number of segments to generate
    public int completionThreshold = 2; // Number of points that can be missed

    private Vector3[] shapePoints;
    private int currentIndex = 0;

    private bool[] pointsCovered;
    private bool isDraggingComplete = false;

    public GameObject arrow; // Reference to the arrow object

    public void Initialize ()
    {
        if (shape != null)
        {
            ShapeData shapeData = shape.ShapeData;
            shapePoints = GenerateInterpolatedPoints(shapeData);
            pointsCovered = new bool[shapePoints.Length];
        }
        else
        {
            Debug.LogWarning("Shape is not assigned.");
        }
    }

    public Vector3[] GetShapePoints ()
    {
        return shapePoints;
    }

    private Vector3[] GenerateInterpolatedPoints ( ShapeData shapeData )
    {
        List<Vector3> interpolatedPoints = new List<Vector3>();

        if (shapeData.ShapeType == ShapeType.Circle)
        {
            // If the shape is a circle, generate points along the circumference
            Vector3 center = shapeData.ShapeOffset;
            float radius = shapeData.ShapeSize.x / 2f; // Assuming x and y are the same for a circle

            interpolatedPoints.AddRange(CreateCirclePoints(center, radius, segments));
        }
        else
        {
            // Assuming PolyPointPositions gives us the corners in order for non-circle shapes
            List<Vector3> corners = shapeData.GetPolyPointPositions();

            if (corners != null && corners.Count > 1)
            {
                for (int i = 0; i < corners.Count; i++)
                {
                    Vector3 start = corners[i];
                    Vector3 end = corners[(i + 1) % corners.Count]; // Wrap around to create a closed loop

                    // Generate points between start and end, excluding the start (corner) point
                    for (int j = 1; j <= segments; j++)  // Start j from 1 to exclude the corner point
                    {
                        float t = j / (float)segments;
                        Vector3 point = Vector3.Lerp(start, end, t);
                        interpolatedPoints.Add(point);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Not enough corners to generate interpolated points.");
            }
        }

        return interpolatedPoints.ToArray();
    }

    private Vector3[] CreateCirclePoints ( Vector3 center, float radius, int segments )
    {
        Vector3[] points = new Vector3[segments];
        float angleStep = 360.0f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = center.x + Mathf.Cos(angle) * radius;
            float y = center.y + Mathf.Sin(angle) * radius;
            points[i] = new Vector3(x, y, center.z);
        }

        return points;
    }


    public Vector3 GetClosestPoint ( Vector2 position )
    {
        Vector3 closestPoint = shapePoints[currentIndex];  // Start with the current index
        float closestDistanceSqr = (closestPoint - (Vector3)position).sqrMagnitude;

        // Iterate through the points to find the closest one
        for (int i = 0; i < shapePoints.Length; i++)
        {
            float dSqrToTarget = (shapePoints[i] - (Vector3)position).sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestPoint = shapePoints[i];
                currentIndex = i; // Update the current index to the closest match
            }
        }

        // Mark the current point as covered
        pointsCovered[currentIndex] = true;

        // Update the arrow direction to point to the next point in the array
        UpdateArrowDirection();

        CheckIfDragIsComplete();

        return closestPoint;
    }

    private void UpdateArrowDirection ()
    {
        if (arrow != null && shapePoints.Length > 0)
        {
            // Calculate the next point index
            int nextIndex = (currentIndex + 3) % shapePoints.Length;

            // Calculate the direction to the next point
            Vector3 directionToNextPoint = shapePoints[nextIndex] - shapePoints[currentIndex + 2];

            // Set the arrow's rotation to point towards the next point
            arrow.transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToNextPoint.normalized);
        }
    }


    private void CheckIfDragIsComplete ()
    {
        if (!isDraggingComplete)
        {
            // Check how many points are covered
            int uncoveredPoints = 0;
            foreach (bool covered in pointsCovered)
            {
                if (!covered)
                {
                    uncoveredPoints++;
                }
            }

            // Consider the drag complete if the number of uncovered points is within the threshold
            if (uncoveredPoints <= completionThreshold)
            {
                Debug.Log("Dragging complete. Player has covered enough points.");
                isDraggingComplete = true;
            }
        }
    }
}
