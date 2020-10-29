using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : ConductingEquipment
{
    private ConductingEquipment input;
    public List<ConductingEquipment> outputs;
    public override void Initialize(ConductingEquipment reference)
    {
        input = reference;
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
            voltage = input.voltage;
        }

        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
            current += output.current;
        }
        if(Destroyed)
        {
            current = 0;//not conducting anymore
        }
        base.Step();
    }
}