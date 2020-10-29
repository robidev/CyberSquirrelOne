using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineairLoad : ConductingEquipment
{
    public float resistance = 10f;
    private ConductingEquipment input;
    public override void Initialize(ConductingEquipment reference)
    {
        input = reference;
    }
    public override void Step()
    {
        if(Destroyed)
        {
            voltage = 0;//not conducting anymore
            current = 0;
            return;
        }
        voltage = input.voltage;
        current = voltage / resistance;
        base.Step();
    }
}