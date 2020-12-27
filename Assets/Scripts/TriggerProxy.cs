using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerProxy : MonoBehaviour
{
    public List<string> requiredConditions;
    public bool[] requiredConditionsState = null;
    public UnityEvent TriggerEvent;
    bool firstCall = true;
    bool EventHasBeenTriggered = false;

    public void SetCondition(string condition)
    {
        if(EventHasBeenTriggered == true)
        {
            return;
        }
        if(firstCall == true)
        {
            requiredConditionsState = new bool[requiredConditions.Count];
            firstCall = false;
        }
        if(requiredConditions.Contains(condition))
        {
            requiredConditionsState[requiredConditions.IndexOf(condition)] = true;
            Debug.Log("condition " + condition + " set");
            //check all conditions if they now are true
            foreach(bool state in requiredConditionsState)
            {
                if(state == false)
                    return;
            }
            //all states are true
            TriggerEvent.Invoke();
            EventHasBeenTriggered = true;
        }
        else
        {
            Debug.Log("condition " + condition + " does not exit in requiredConditions");
        }
    }
}
