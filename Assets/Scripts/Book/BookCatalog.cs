using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCatalog : MonoBehaviour
{
    public List<SubjectButton> subjectButtons = new List<SubjectButton>();

    public void ShowHideSubjectButtons ()
    {
        if (ObjectCollection.Instance == null) return;

        foreach (var pair in subjectButtons)
        {
            if (!ObjectCollection.Instance.collection.ContainsKey(pair.subject))
            {
                pair.subjectImage.gameObject.SetActive(false);
                pair.lockImage.gameObject.SetActive(true);
            }
        }

    }


}
