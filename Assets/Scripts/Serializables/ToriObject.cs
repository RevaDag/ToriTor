using UnityEngine;

[System.Serializable]
public class ToriObject
{
    public GameObject _objectPrefab;
    public GameObject _parallelObject;
    public string _subject;
    public Color _color;
    public AudioClip _clip;

    public GameObject objectPrefab { get { return _objectPrefab; } }
    public GameObject parallelObject { get { return _parallelObject; } }
    public string subject { get { return _subject; } }
    public Color color { get { return _color; } }
    public AudioClip clip { get { return _clip; } }
}