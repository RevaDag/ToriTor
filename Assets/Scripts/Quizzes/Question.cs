using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;

    public RectTransform target;

    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Text tmpText;

    public void SetAudioClip ( AudioClip _clip )
    {
        audioSource.clip = _clip;
    }

    public void SetImage ( ToriObject toriObject )
    {
        image.sprite = toriObject.parallelObjectSprite;
        image.SetNativeSize();
    }

    public void SetDialogLine ( ToriObject toriObject )
    {
        List<Line> lines = new List<Line>();
        Line line = new Line();

        line.text = toriObject.objectName;
        line.audioClip = toriObject.clip;
        line.type = Line.Type.Question;

        lines.Add(line);

        dialogManager.SetLinesAndPlay(lines);
    }
}
