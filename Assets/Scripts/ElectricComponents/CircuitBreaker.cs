using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CircuitBreaker : ConductingEquipment
{
    public bool SwitchConducting;
    private bool oldSwitchConducting;
    private ConductingEquipment input;
    public List<ConductingEquipment> outputs;
    public ConductingEquipment CT;
    public ConductingEquipment VT;
    public float OverVoltage = 1000;
    public float OverCurrentTreshold = 50;
    public float OverCurrentTime = 2;//2 sec
    public float OverCurrentImmediate = 100;
    public UnityEvent OnTrip;
    private float OC = 0;
    public override void Initialize(ConductingEquipment reference)
    {
        input = reference;
        oldSwitchConducting = SwitchConducting;
        foreach(ConductingEquipment output in outputs)
            output.Initialize(this);
    }
    public override void Step()
    {
        current = 0;
        if(Destroyed)
        {
            voltage = 0;//not conducting anymore

        }
        else
        {
            PerformProtectionFunction();
            if(SwitchConducting == true)
                voltage = input.voltage;
            else
                voltage = 0;
        }
        
        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
            current += output.current;
        }

        if(oldSwitchConducting == false && SwitchConducting == true) //if switch is closed this frame
        {
            Debug.Log(name + ": Bang close");
        }
        if(oldSwitchConducting == true && SwitchConducting == false) //if switch is opened this frame
        {
            Debug.Log(name + ": Bang open");
        }

        oldSwitchConducting = SwitchConducting;
        base.Step();
    }

    void PerformProtectionFunction()
    {
        if(SwitchConducting == false)
            return;
        //overvoltage
        if(VT.voltage > OverVoltage)
        {
            SwitchConducting = false;
            OnTrip.Invoke();
            Debug.Log(name + ": overvoltage trip");
            return;
        }
        //overcurrent
        if(CT.current > OverCurrentImmediate)
        {
            SwitchConducting = false;
            OnTrip.Invoke();
            Debug.Log(name + ": overcurrent immediate trip");  
            return;          
        }
        
        //time overcurrent
        if(CT.current > OverCurrentTreshold)
        {
            float delta = (OverCurrentImmediate - OverCurrentTreshold) / OverCurrentTime;
            OC += (CT.current - OverCurrentTreshold) * Time.deltaTime;
            if(OC > delta)
            {
                SwitchConducting = false;
                OnTrip.Invoke();
                Debug.Log(name + ": time overcurrent trip"); 
            }
        }
        else
        {
            if(OC > 0)
            {
                OC -= (OverCurrentTreshold - CT.current) * Time.deltaTime; //subtract current with similar falloff rate
            }
            else //ensure we clamp negative values to 0
            {
                OC = 0;
            }
        }
    }
    public void SetSwitchConducting(bool value)
    {
        SwitchConducting = value;
    }
}
