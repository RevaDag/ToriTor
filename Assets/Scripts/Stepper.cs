using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stepper : MonoBehaviour
{
    [SerializeField] private GameObject stepPrefab;
    [SerializeField] private Sprite activeStepSprite;

    public List<GameObject> steps { get; private set; }
    public int currentStep { get; private set; }

    private void Awake ()
    {
        steps = new List<GameObject>();
    }

    private void Start ()
    {
        GetSteps();
        //activateNextStep();
    }

    private void GetSteps ()
    {
        // Assuming `GameManager.Instance.currentLevel.StepsNumber` returns an integer representing the number of steps
        int stepsNumber = GameManager.Instance.currentLevel.StepsNumber;

        for (int i = 0; i < stepsNumber; i++)
        {
            steps.Add(Instantiate(stepPrefab, transform));
        }
    }

    public void activateNextStep ()
    {
        if (currentStep >= steps.Count) return;

        steps[currentStep].GetComponent<Image>().sprite = activeStepSprite;
        currentStep++;
    }
}
