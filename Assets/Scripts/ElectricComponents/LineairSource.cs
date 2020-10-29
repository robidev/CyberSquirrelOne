using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineairSource : ConductingEquipment
{
    public float Sourcevoltage = 1000;
    public float MaxSourceCurrent = 1000;
    public float Response = 1;
    public List<ConductingEquipment> outputs;
    void Start()
    {
        Initialize(null);
    }

    void Update()
    {
        Step();
    }


    public override void Initialize(ConductingEquipment reference)
    {
        voltage = Sourcevoltage;
        foreach(ConductingEquipment output in outputs)
            output.Initialize(this);
    }
    public override void Step()
    {
        if(Destroyed)
        {
            voltage = 0;//not conducting anymore
        }
        current = 0;
        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
            current += output.current;
            base.Step();
        }

        if(Destroyed)
        {
            current = 0;
        }
        else
        {
            if(current > MaxSourceCurrent)//drop voltage
            {
                if(voltage > 0)
                    voltage -= Response;
                else
                    voltage = 0;
            }
            else if(voltage < Sourcevoltage)
            {
                voltage += Response;
            }
        }
    }
} 