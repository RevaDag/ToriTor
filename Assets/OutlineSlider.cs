using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineSlider : MonoBehaviour
{
    public RectTransform handle; // The UI handle of the slider
    public List<RectTransform> outlinePoints; // The points that define the outline
    public Slider slider; // The slider component
    public float movementSpeed = 5f; // Control the speed of the handle movement
    private Vector2 targetPosition; // The target position for the handle

    void Start ()
    {
        // Initialize the handle position to the first outline point
        if (outlinePoints.Count > 0)
        {
            handle.anchoredPosition = outlinePoints[0].position;
        }
    }

    void Update ()
    {
        // Ensure the slider value is between 0 and 1
        float t = Mathf.Clamp01(slider.value);
        // Get the total number of segments in the outline
        int numSegments = outlinePoints.Count - 1;
        // Calculate which segment the slider is on
        int segmentIndex = Mathf.FloorToInt(t * numSegments);
        // Calculate the position along the segment
        float segmentT = (t * numSegments) - segmentIndex;

        if (segmentIndex < numSegments)
        {
            // Linear interpolation between the points
            Vector2 newPos = Vector2.Lerp(outlinePoints[segmentIndex].position, outlinePoints[segmentIndex + 1].position, segmentT);
            handle.anchoredPosition = newPos;
        }

        handle.anchoredPosition = Vector2.Lerp(handle.anchoredPosition, targetPosition, Time.deltaTime * movementSpeed);

    }
}
