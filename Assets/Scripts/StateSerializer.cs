using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TigerForge;

public class StateSerializer : MonoBehaviour
{
    EasyFileSave myFile;

    void Start()
    {
        myFile = new EasyFileSave();
        Debug.Log("Filename:" + myFile.GetFileName());
        Save();
        Load();
    }
    void Load()
    {
        if(!myFile.Load())
        {
            Debug.Log("load issue");
            return;
        }

        object[] obj = Resources.FindObjectsOfTypeAll<SerializedObject>(); //this ensures disabled object are also considered
        //object[] obj = GameObject.FindObjectsOfType(typeof (SerializedObject)); //this ignores disabled objects
        foreach (object o in obj)
        {
            SerializedObject g = (SerializedObject) o;
            if(g.UUID == "")
                continue;

            var objData = myFile.GetBinary(g.UUID);
            if(objData != null)
            {
                g.setLoadData(objData);
                Debug.Log(g.UUID + " is loaded");
            }
        }
        Debug.Log("load done");
    }

    void Save()
    {
        object[] obj = Resources.FindObjectsOfTypeAll<SerializedObject>(); //this ensures disabled object are also considered
        //object[] obj = Resources.FindObjectsOfType(typeof (SerializedObject)); //this ignores disabled objects
        foreach (object o in obj)
        {
            SerializedObject g = (SerializedObject) o;
            if(g.UUID == "")
                continue;
                
            myFile.AddBinary(g.UUID, g.getSaveData());
            Debug.Log(g.UUID + " is saved");
        }
        myFile.Save();
        Debug.Log("saving done");
    }
}
