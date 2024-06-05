using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestKey : MonoBehaviour
{
    private Chest chest;
    public GameObject parallelObject;
    public Image keyImage;

    private RectTransform target;
    private AudioSource audioSource;

    private void Awake ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetChest ( Chest chest )
    {
        this.chest = chest;
    }

    public void SetTarget ( RectTransform target )
    {
        this.target = target;
    }

    public void SetParallelObject ( GameObject parallelObject )
    {
        this.parallelObject = parallelObject;
    }

    public void CheckTarget ( PointerEventData eventData )
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(target, Input.mousePosition, null))
        {
            this.transform.SetParent(target);
            this.transform.localPosition = Vector3.zero;
            OpenLock();
        }
    }

    private void OpenLock ()
    {
        chest.OpenChest();
        PlayAudioSource();
        DisableDrag();
    }

    private void DisableDrag ()
    {
        GetComponent<Draggable>().DisableDrag();
    }

    public void EnableDrag ()
    {
        GetComponent<Draggable>().EnableDrag();

    }

    public void SetAudioClip ( AudioClip clip )
    {
        audioSource.clip = clip;
    }

    private void PlayAudioSource ()
    {
        audioSource.Play();
    }

}
