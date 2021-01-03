using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disconnector : Switch
{
    private bool oldSwitchConducting;
    private float oldCurrent;
    private float OpenArcDamage = 510;
    private float CloseArcDamage = 500;
    private ConductingEquipment input;
    public override void Initialize(ConductingEquipment reference)
    {
        input = reference;
        oldSwitchConducting = SwitchConducting;
        foreach(ConductingEquipment output in outputs)
            output.Initialize(this);
    }
    public override void Step()
    {
        float tmpCurrent = 0;
        if(Destroyed)
        {
            voltage = 0;//not conducting anymore  
        }
        else
        {
            if(SwitchConducting == true)
                voltage = input.voltage;
            else
                voltage = 0;
        }
        
        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
            tmpCurrent += output.current;
        }

        current = tmpCurrent;
        if(oldSwitchConducting == false && SwitchConducting == true && current > 0.1) //if switch is closed this frame, and current is flowing
        {
            Debug.Log(name + ": close-arc");
            damage += CloseArcDamage;
        }
        if(oldSwitchConducting == true && SwitchConducting == false && oldCurrent > 0.1) //if switch is opened this frame, and current was flowing
        {
            Debug.Log(name + ": open-arc");
            damage += OpenArcDamage;
        }
        oldCurrent = current;
        oldSwitchConducting = SwitchConducting;
        
        base.Step();
    }
}
