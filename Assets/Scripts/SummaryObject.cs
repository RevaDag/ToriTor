using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummaryObject : MonoBehaviour
{
    [SerializeField] private Image objectImage;
    [SerializeField] private TMP_Text objectText;
    private AudioSource audioSource;

    public ToriObject toriObject { get; private set; }

    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetToriObject ( ToriObject toriObject )
    {
        this.toriObject = toriObject;
        UpdateObject();
    }

    private void UpdateObject ()
    {
        if (toriObject != null)
        {
            objectImage.sprite = toriObject.summarySprite;
            objectText.text = toriObject.objectName;
            audioSource.clip = toriObject.clip;

        }
    }

}
