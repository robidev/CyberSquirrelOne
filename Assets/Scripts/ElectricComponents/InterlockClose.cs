using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterlockClose : MonoBehaviour
{
    OperateDialog operateDialog;
    public UnityEvent TriggerEvent;
    public UnityEvent BlockEvent;
    public List<Switch> InputsTrue;
    public List<Switch> InputsFalse;
    public bool TestConditionOverride = false;

    void Start()
    {
        if(Resources.FindObjectsOfTypeAll<OperateDialog>().Length > 0)
            operateDialog = Resources.FindObjectsOfTypeAll<OperateDialog>()[0];
        else
            Debug.Log("cannot find operateDialog");
    }
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
            operateDialog.SetOperateResult(-1);          
        }
    }
    public void InterlockOverride()
    {
        TriggerEvent.Invoke();
        Debug.Log("Interlock override");
    }
}
