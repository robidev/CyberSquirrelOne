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
        {
            operateDialog = Resources.FindObjectsOfTypeAll<OperateDialog>()[0];
        }
        else
        {
            Debug.Log("cannot find operateDialog");
        }
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
            Debug.Log("InterlockTest passed");
            AlarmWindow.instance.SetMessage("Sub_S42",gameObject.name,this.GetType().Name,"InterlockTest passed","INFO");   
            TriggerEvent.Invoke();
        }
        else
        {
            BlockEvent.Invoke();  
            Debug.Log("InterlockTest blocked"); 
            operateDialog.SetOperateResult(-1); 
            AlarmWindow.instance.SetMessage("Sub_S42",gameObject.name,this.GetType().Name,"InterlockTest blocked","ERROR");     
        }
    }
    public void InterlockOverride()
    {
        Debug.Log("Interlock override");
        AlarmWindow.instance.SetMessage("Sub_S42",gameObject.name,this.GetType().Name,"Interlock override","INFO"); 
        TriggerEvent.Invoke();
    }
}
