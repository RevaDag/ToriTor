using UnityEngine;
using System.Collections.Generic;

public class WigglyGameObjects : MonoBehaviour
{
    [SerializeField] private List<GameObject> gameObjects; // List of GameObjects to animate
    [SerializeField] private float amplitude = 1f; // Amplitude of the wiggle
    [SerializeField] private float frequency = 1f; // Frequency of the wiggle
    [SerializeField] private float phaseShift = 0.5f; // Phase shift for the wiggly animation

    private Vector3[] originalPositions; // Store original positions of the GameObjects

    void Start ()
    {
        // Store the original positions of the GameObjects
        originalPositions = new Vector3[gameObjects.Count];
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] != null)
            {
                originalPositions[i] = gameObjects[i].transform.localPosition;
            }
        }
    }

    void Update ()
    {
        AnimateWiggle();
    }

    void AnimateWiggle ()
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] != null)
            {
                // Calculate the new vertical position with a phase shift
                float newY = originalPositions[i].y + Mathf.Sin(Time.time * frequency + i * phaseShift) * amplitude;

                // Apply the new position
                gameObjects[i].transform.localPosition = new Vector3(originalPositions[i].x, newY, originalPositions[i].z);
            }
        }
    }
}
