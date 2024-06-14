using UnityEngine;

[System.Serializable]
public class Line
{
    public enum Type
    {
        Dialog,
        Feedback,
        Question
    }

    public Type type;
    public string text;
    public AudioClip audioClip;
}

