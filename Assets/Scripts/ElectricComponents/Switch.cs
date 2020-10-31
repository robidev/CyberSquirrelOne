using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : ConductingEquipment
{
    public bool SwitchConducting;
    public List<ConductingEquipment> outputs;

    public void SetSwitchConducting(bool value)
    {
        SwitchConducting = value;
    }
}
