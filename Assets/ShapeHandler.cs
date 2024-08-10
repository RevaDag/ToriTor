using System.Collections.Generic;
using UnityEngine;
using VectorShapes;

public class ShapeHandler : MonoBehaviour
{
    public Shape shape; // Reference to the Shape component
    public int segments = 36; // Number of segments to generate

    private Vector3[] shapePoints;
    private int currentIndex = 0;


    public void Initialize ()
    {
        if (shape != null)
        {
            ShapeData shapeData = shape.ShapeData;
            shapePoints = GenerateInterpolatedPoints(shapeData);
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

                    // Generate points between start and end
                    for (int j = 0; j <= segments; j++)
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

    /*    public Vector3 GetClosestPoint ( Vector2 position )
        {
            Vector3 closestPoint = shapePoints[currentIndex];  // Start with the current index
            float closestDistanceSqr = (closestPoint - (Vector3)position).sqrMagnitude;

            // Iterate through the points starting from the next index
            for (int i = currentIndex + 1; i < shapePoints.Length; i++)
            {
                float dSqrToTarget = (shapePoints[i] - (Vector3)position).sqrMagnitude;

                // If the distance increases, break the loop to avoid skipping points
                if (dSqrToTarget > closestDistanceSqr)
                {
                    break;
                }

                // Update the closest point and index if this point is closer
                closestDistanceSqr = dSqrToTarget;
                closestPoint = shapePoints[i];
                currentIndex = i;
            }

            return closestPoint;
        }*/

    public Vector3 GetClosestPoint ( Vector2 position )
    {
        Vector3 closestPoint = shapePoints[currentIndex];  // Start with the current index
        float closestDistanceSqr = (closestPoint - (Vector3)position).sqrMagnitude;

        // Iterate through the points starting from the current index and wrapping around if necessary
        for (int i = 0; i < shapePoints.Length; i++)
        {
            int indexToCheck = (currentIndex + i) % shapePoints.Length;
            float dSqrToTarget = (shapePoints[indexToCheck] - (Vector3)position).sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestPoint = shapePoints[indexToCheck];
                currentIndex = indexToCheck; // Update the current index to the best match
            }
        }

        return closestPoint;
    }



}
