using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChestKey : MonoBehaviour
{
    public Answer answer;

    private Chest chest;
    private RectTransform target;


    public void SetChest ( Chest chest )
    {
        this.chest = chest;
    }

    public void SetTarget ( RectTransform target )
    {
        this.target = target;
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
        answer.audioSource.Play();
        chest.OpenChest();
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



}
