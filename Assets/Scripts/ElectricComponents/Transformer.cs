using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : ConductingEquipment
{
    public float ratio = 2/1;
    private ConductingEquipment input;
    public float outputCurrentMax = 1000;
    public float PowerMax = 1000000;
    private float power = 0;
    public List<ConductingEquipment> outputs;
    public override void Initialize(ConductingEquipment reference)
    {
        input = reference;
        foreach(ConductingEquipment output in outputs)
            output.Initialize(this);
    }
    public override void Step()
    {
        if(Destroyed)
        {
            voltage = 0;//not transforming anymore
        }
        else
        {
            voltage = input.voltage / ratio;
        }

        float output_current = 0;
        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
            output_current += output.current;
        }

        if(Destroyed)
        {
            current = input.voltage / 0.0001f;//shorted
        }
        else
        {
            current = output_current / ratio;
            power = output_current * voltage;

            if(output_current > outputCurrentMax)
            {
                damage += ( output_current - outputCurrentMax );
            }
            if(power > PowerMax)
            {
                damage += ( power - PowerMax );
            }
        }
        //Debug.Log(name + " input: " + current + " A, " + input.voltage + " V");
        //Debug.Log(name + " output: " + output_current + " A, " + voltage + " V");
    }
}