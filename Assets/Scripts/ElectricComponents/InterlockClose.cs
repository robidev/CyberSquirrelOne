using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterlockClose : MonoBehaviour
{
    public UnityEvent TriggerEvent;
    public UnityEvent BlockEvent;
    public List<Switch> InputsTrue;
    public List<Switch> InputsFalse;
    public bool TestConditionOverride = false;

    public void InterlockCloseTest()
    {
        bool TestCondition = true;
        foreach(Switch inputTrue in InputsTrue)
            TestCondition &= inputTrue.SwitchConducting;

        foreach(Switch inputFalse in InputsFalse)
            TestCondition &= !inputFalse.SwitchConducting;

        if(TestCondition || TestConditionOverride)
        {
            TriggerEvent.Invoke();
            Debug.Log("InterlockTest passed");
        }
        else
        {
            BlockEvent.Invoke();  
            Debug.Log("InterlockTest blocked");          
        }
    }
    public void InterlockOverride()
    {
        TriggerEvent.Invoke();
        Debug.Log("Interlock override");
    }
}
