using UnityEngine;

public class CameraSwipe : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public RectTransform canvasRect; // Reference to the RectTransform of the canvas
    private Vector3 touchStart;
    private Vector3 velocity;
    private float deceleration = 0.95f; // Factor to decrease the velocity, closer to 1 means slower deceleration

    void Update ()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStart = mainCamera.ScreenToWorldPoint(touch.position);
                    velocity = Vector3.zero; // Reset velocity on new touch start
                    break;

                case TouchPhase.Moved:
                    Vector3 direction = touchStart - mainCamera.ScreenToWorldPoint(touch.position);
                    mainCamera.transform.position += direction;
                    velocity = direction / Time.deltaTime; // Calculate velocity
                    ClampCamera();
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // Do nothing here, let inertia handle the movement
                    break;
            }
        }
        else if (velocity.magnitude > 0.1f) // Continue moving with inertia
        {
            mainCamera.transform.position += velocity * Time.deltaTime;
            velocity *= deceleration; // Gradually decrease the velocity
            ClampCamera();
        }
    }

    void ClampCamera ()
    {
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        Vector3 canvasBottomLeft = canvasRect.TransformPoint(new Vector3(canvasRect.rect.xMin, canvasRect.rect.yMin, 0));
        Vector3 canvasTopRight = canvasRect.TransformPoint(new Vector3(canvasRect.rect.xMax, canvasRect.rect.yMax, 0));

        Vector3 clampedPosition = mainCamera.transform.position;

        if (bottomLeft.x < canvasBottomLeft.x)
            clampedPosition.x += canvasBottomLeft.x - bottomLeft.x;
        if (topRight.x > canvasTopRight.x)
            clampedPosition.x -= topRight.x - canvasTopRight.x;
        if (bottomLeft.y < canvasBottomLeft.y)
            clampedPosition.y += canvasBottomLeft.y - bottomLeft.y;
        if (topRight.y > canvasTopRight.y)
            clampedPosition.y -= topRight.y - canvasTopRight.y;

        mainCamera.transform.position = clampedPosition;

        // Stop the velocity if the camera is clamped to prevent shaking
        if (clampedPosition != mainCamera.transform.position)
        {
            velocity = Vector3.zero;
        }
    }
}
