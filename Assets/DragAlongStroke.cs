using UnityEngine;
using VectorShapes;
using System.Collections.Generic;

public class DragAlongShape : MonoBehaviour
{
    public Shape shape; // Reference to the Shape component
    public RectTransform draggableObject; // The object to drag along the shape, using RectTransform since it's on a Canvas
    public Canvas canvas; // Reference to the Canvas
    public int circleSegments = 36; // Number of segments to represent the circle


    private Vector3[] shapePoints; // Array to hold points on the shape's stroke

    void Start ()
    {
        // Extract the points from the shape
        ShapeData shapeData = shape.ShapeData;
        shapePoints = ExtractPointsFromShapeData(shapeData);
    }

    void Update ()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Convert touch position to world space
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0;

                // Find the closest point on the shape stroke
                Vector3 closestPoint = GetClosestPointOnShape(touchPosition);

                // Convert the closest point to the canvas space
                Vector2 canvasPosition = WorldToCanvasPosition(canvas, closestPoint);

                // Update the position of the draggable object
                draggableObject.anchoredPosition = canvasPosition;
            }
        }
    }

    private Vector3[] ExtractPointsFromShapeData ( ShapeData shapeData )
    {
        if (shapeData.ShapeType == ShapeType.Circle)
        {
            // If it's a circle, create points manually
            return CreateCirclePoints(shapeData.ShapeOffset, shapeData.ShapeSize.x / 2, circleSegments);
        }
        else
        {
            // Otherwise, get the polygon points (for other shapes)
            List<Vector3> polyPoints = shapeData.GetPolyPointPositions();

            // Convert the List to an array if needed
            return polyPoints.ToArray();
        }
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


    private Vector3 GetClosestPointOnShape ( Vector3 position )
    {
        Vector3 closestPoint = Vector3.zero;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Vector3 point in shapePoints)
        {
            Vector3 directionToTarget = point - position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    private Vector2 WorldToCanvasPosition ( Canvas canvas, Vector3 worldPosition )
    {
        // Convert the world position to screen space
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);

        // Convert the screen space to canvas space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, canvas.worldCamera, out Vector2 canvasPosition);

        Debug.Log($"World Position: {worldPosition}, Screen Position: {screenPosition}, Canvas Position: {canvasPosition}");

        return canvasPosition;
    }

}
