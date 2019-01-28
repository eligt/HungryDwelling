using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsControl : MonoBehaviour
{
    public float timeMultiplier = 1.0f;
    
    private InstructionsStep[] stepList;
    private float currentTime;

    // Use this for initialization
    void Start ()
    {
        stepList = GetComponentsInChildren<InstructionsStep>(true);
        currentTime = 0.0f;

		foreach (InstructionsStep step in stepList)
        {
            step.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentTime += Time.deltaTime * timeMultiplier;

        foreach (InstructionsStep step in stepList)
        {
            if (step.startTime <= currentTime && (step.startedTime == 0.0f || step.startedTime + step.duration > currentTime))
            {
                // Visible
                if (!step.shown && step.meetsCondition())
                {
                    step.shown = true;
                    step.startedTime = currentTime;

                    ShowStep(step, true);
                }
            }
            else
            {
                // Not visible
                if (step.shown)
                {
                    step.shown = false;

                    ShowStep(step, false);
                }
            }
        }
    }
    
    void ShowStep(InstructionsStep step, bool show)
    {
        step.Show(show);
    }
}
