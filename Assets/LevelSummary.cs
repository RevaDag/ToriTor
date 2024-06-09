using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSummary : MonoBehaviour
{
    [SerializeField] private Fader SummaryCanvasFader;
    [SerializeField] private Transform SummaryObjectsParent;
    [SerializeField] private GameObject SummaryObjectPrefab;
    [SerializeField] private TMP_Text levelSubject;

    private List<ToriObject> toriObjects = new List<ToriObject>();
    private List<SummaryObject> summaryObjects = new List<SummaryObject>();

    private void Start ()
    {
        GetCurrentLevelSubject();
        GetLevelObjects();
    }

    private void GetLevelObjects ()
    {
        if (GameManager.Instance != null)
        {
            SetToriObjectList(GameManager.Instance.currentLevelObjects);
        }
    }

    public void SetToriObjectList ( List<ToriObject> _toriObjects )
    {
        toriObjects.Clear();

        toriObjects = _toriObjects;

        CreateSummaryObjects();
    }


    private void CreateSummaryObjects ()
    {
        foreach (ToriObject obj in toriObjects)
        {
            GameObject summaryObjectInstance = Instantiate(SummaryObjectPrefab, SummaryObjectsParent);
            SummaryObject summaryObject = summaryObjectInstance.GetComponent<SummaryObject>();
            summaryObject.SetToriObject(obj);
            summaryObjects.Add(summaryObject);
            Debug.Log($"Created SummaryObject for {obj.objectName}");
        }
    }


    public void AssignToriToSummaries ()
    {
        for (int i = 0; i < toriObjects.Count && i < summaryObjects.Count; i++)
        {
            summaryObjects[i].SetToriObject(toriObjects[i]);
            Debug.Log($"Assigned {toriObjects[i].objectName} to {summaryObjects[i].name}");
        }
    }

    public void GetCurrentLevelSubject ()
    {
        levelSubject.text = GameManager.Instance.currentLevel.Subject;
    }

    public void ShowSummary ()
    {
        SummaryCanvasFader.FadeIn();
    }
}
