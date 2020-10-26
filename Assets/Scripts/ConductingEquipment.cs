using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConductingEquipment
{
    public string name;
    public float voltage = 0;
    public float current = 0;
    public virtual void Initialize(ConductingEquipment reference)
    {

    }
    public virtual void Step()
    {

    }
}

public class LineairSource : ConductingEquipment
{
    public float Sourcevoltage = 1000;
    public List<ConductingEquipment> outputs;
    public override void Initialize(ConductingEquipment reference)
    {
        voltage = Sourcevoltage;
        foreach(ConductingEquipment output in outputs)
            output.Initialize(this);
    }
    public override void Step()
    {
        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
        }
        Debug.Log(name + " source: " + current + " A, " + voltage + " V");
    }
} 

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
        voltage = input.voltage;
        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
            current += output.current;
        }
        input.current = current;
        Debug.Log(name + " wire: " + current + " A, " + voltage + " V");
    }
}

public class Switch : ConductingEquipment
{
    public bool SwitchConducting;
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
        if(SwitchConducting == true)
            voltage = input.voltage;
        else
            voltage = 0;
        
        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
            current += output.current;
        }
        input.current = current;
        Debug.Log(name + " switch: " + current + " A, " + voltage + " V");
    }
}

public class Transformer : ConductingEquipment
{
    public int ratio = 2/1;
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
        voltage = input.voltage / ratio;

        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
            current += output.current;
        }
        input.current = current / ratio;
        Debug.Log(name + " input: " + current + " A, " + voltage + " V");
        Debug.Log(name + " output: " + current + " A, " + voltage + " V");
    }
}

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
        voltage = input.voltage;
        current = voltage / resistance;
        input.current = current;
        Debug.Log(name + " load: " + current + " A, " + voltage + " V");
    }
}