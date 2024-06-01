using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectLoader : MonoBehaviour
{
    private string subjectTitle;

    public void SetSubjectTitle ( string _subjectTitle )
    {
        subjectTitle = _subjectTitle;
    }

    public void LoadSubject ( string subject )
    {
        if (ObjectCollection.Instance != null)
        {
            ObjectCollection.Instance.SetSubjectToLoad(subject, subjectTitle);
        }
    }
}
