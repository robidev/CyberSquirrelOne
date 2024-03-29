﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedCableObject : SerializedObject
{
    PowerlineObjectData data;
    Powerline powerline;
    // Start is called before the first frame update
    void Awake()
    {
        powerline = GetComponent<Powerline>();
        data = new PowerlineObjectData{ isPowered = powerline.isPowered }; //get/set
    }

    public override object getSaveData()
    {
        if(powerline == null || data == null)
            Awake();
            
        data.isPowered = powerline.isPowered;
        return data;
    }

    public override void setLoadData(object obj)
    {
        if(powerline == null)
            powerline = GetComponent<Powerline>();
        
        data = (PowerlineObjectData) obj;
        powerline.isPowered = data.isPowered;
        Debug.Log("data.ispowered:" + data.isPowered);
    }

    [System.Serializable]
    private class PowerlineObjectData
    {
        public bool isPowered;
    }
}
