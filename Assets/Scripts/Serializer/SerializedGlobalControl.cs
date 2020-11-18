﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedGlobalControl : SerializedObject
{
    GlobalControlData data;
    GlobalControl control;
    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<GlobalControl>();
        data = new GlobalControlData(); //get/set
    }

    public override object getSaveData()
    {
        if(control == null || data == null)
            Start();

        data.enabledPlayers = control.characterEnabledList;
        data.currentPlayer = control.selected;
        return data;
    }

    public override void setLoadData(object obj)
    {
        if(control == null)
            control = GetComponent<GlobalControl>();

        data = (GlobalControlData) obj;
        control.characterEnabledList = data.enabledPlayers;
        control.selected = data.currentPlayer;
        Debug.Log("data.enabledPlayers:" + data.enabledPlayers);
        Debug.Log("data.currentPlayer:" + data.currentPlayer);
    }

    [System.Serializable]
    private class GlobalControlData
    {
        public int currentPlayer;
        public List<bool> enabledPlayers;
    }
}
